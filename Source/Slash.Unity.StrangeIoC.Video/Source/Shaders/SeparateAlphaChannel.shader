// This shader takes a texture that contains the normal image and a separate one which contains the alpha channel
// and merges them to have a way to get videos with transparent areas (e.g. for green screens)

Shader "Slash/Video/Separate Alpha Channel" {

	Properties {
		_MainTex("Color (RGB)", 2D) = "white"
		[Toggle] _FlipY("Flip Y", Float) = 0
		_CellSize("Cell Size", Float) = 0
		[Toggle] _AlphaBeside("Alpha is next to image (default: below)", Float) = 0
	}

	SubShader{

		Tags{ "Queue" = "Transparent" "RenderType" = "Transparent" }

		ZWrite Off
		Lighting Off
		Blend SrcAlpha OneMinusSrcAlpha

		Pass {
			CGPROGRAM

			#pragma vertex vert
			#pragma fragment frag

			#include "UnityCG.cginc"

			sampler2D _MainTex;
			float4 _MainTex_ST;
			float _FlipY;
			float _CellSize;
			float _AlphaBeside;

			struct appdata
			{
				float4 vertex : POSITION;
				float2 texcoord : TEXCOORD0;
			};

			struct v2f
			{
				float4 vertex : SV_POSITION;
				half2 texcoord : TEXCOORD0;
				half2 texcoordAlpha : TEXCOORD1;
			};

			v2f vert(appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);

				float2 texcoord = TRANSFORM_TEX(v.texcoord, _MainTex);

				if (_FlipY == 1)
				{
					texcoord.y = 1 - texcoord.y;
				}

				if (_AlphaBeside == 1)
				{
					texcoord.x = 1 - texcoord.x;

					// Limit area to first half.
					texcoord.x = texcoord.x / 2 + 0.5;

					// Main texture is considered to be on the right, alpha texture on the left.
					o.texcoord = texcoord;
					o.texcoordAlpha = float2(texcoord.x - 0.5, texcoord.y);
				}
				else
				{
					// Limit area to first half.
					texcoord.y = texcoord.y / 2 + 0.5;

					// Main texture is considered to be on top, alpha texture on bottom.
					o.texcoord = texcoord;
					o.texcoordAlpha = float2(texcoord.x, texcoord.y - 0.5);
				}

				return o;
			}

			float4 frag(v2f IN) : SV_Target
			{
				if (_CellSize > 0)
				{
					float cellSize = _CellSize / 100.0;

					float2 steppedUV = IN.texcoord.xy;
					if (cellSize > 0.001)
					{
						steppedUV /= cellSize;
						steppedUV = round(steppedUV);
						steppedUV *= cellSize;
					}

					float2 steppedUVAlpha = IN.texcoordAlpha.xy;
					if (cellSize > 0.001)
					{
						steppedUVAlpha /= cellSize;
						steppedUVAlpha = round(steppedUVAlpha);
						steppedUVAlpha *= cellSize;
					}

					// Take color from main texture and alpha from alpha texture.
					float3 col = tex2D(_MainTex, steppedUV).rgb;
					float alpha = tex2D(_MainTex, steppedUVAlpha).r;
					return float4(col, alpha);
				}
				else
				{
					// Take color from main texture and alpha from alpha texture.
					float3 col = tex2D(_MainTex, IN.texcoord.xy).rgb;
					float alpha = tex2D(_MainTex, IN.texcoordAlpha.xy).r;
					return float4(col, alpha);
				}
			}

			ENDCG
		}
	}
}