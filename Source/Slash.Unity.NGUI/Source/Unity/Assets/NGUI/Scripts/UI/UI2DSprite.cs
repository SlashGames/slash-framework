//----------------------------------------------
//            NGUI: Next-Gen UI kit
// Copyright © 2011-2014 Tasharen Entertainment
//----------------------------------------------

#if !UNITY_3_5 && !UNITY_4_0 && !UNITY_4_1 && !UNITY_4_2

using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// 2D Sprite is capable of drawing sprites added in Unity 4.3. When importing your textures,
/// import them as Sprites and you will be able to draw them with this widget.
/// If you provide a Packing Tag in your import settings, your sprites will get automatically
/// packed into an atlas for you, so creating an atlas beforehand is not necessary.
/// </summary>

[ExecuteInEditMode]
[AddComponentMenu("NGUI/UI/NGUI Unity2D Sprite")]
public class UI2DSprite : UIWidget
{
	[HideInInspector][SerializeField] UnityEngine.Sprite mSprite;
	[HideInInspector][SerializeField] Material mMat;
	[HideInInspector][SerializeField] Shader mShader;

	/// <summary>
	/// To be used with animations.
	/// </summary>

	public UnityEngine.Sprite nextSprite;

	int mPMA = -1;

	/// <summary>
	/// UnityEngine.Sprite drawn by this widget.
	/// </summary>

	public UnityEngine.Sprite sprite2D
	{
		get
		{
			return mSprite;
		}
		set
		{
			if (mSprite != value)
			{
				RemoveFromPanel();
				mSprite = value;
				nextSprite = null;
				MarkAsChanged();
			}
		}
	}

	/// <summary>
	/// Material used by the widget.
	/// </summary>

	public override Material material
	{
		get
		{
			return mMat;
		}
		set
		{
			if (mMat != value)
			{
				RemoveFromPanel();
				mMat = value;
				mPMA = -1;
				MarkAsChanged();
			}
		}
	}

	/// <summary>
	/// Shader used by the texture when creating a dynamic material (when the texture was specified, but the material was not).
	/// </summary>

	public override Shader shader
	{
		get
		{
			if (mMat != null) return mMat.shader;
			if (mShader == null) mShader = Shader.Find("Unlit/Transparent Colored");
			return mShader;
		}
		set
		{
			if (mShader != value)
			{
				RemoveFromPanel();
				mShader = value;

				if (mMat == null)
				{
					mPMA = -1;
					MarkAsChanged();
				}
			}
		}
	}
	
	/// <summary>
	/// Texture used by the UITexture. You can set it directly, without the need to specify a material.
	/// </summary>

	public override Texture mainTexture
	{
		get
		{
			if (mSprite != null) return mSprite.texture;
			if (mMat != null) return mMat.mainTexture;
			return null;
		}
	}

	/// <summary>
	/// Whether the texture is using a premultiplied alpha material.
	/// </summary>

	public bool premultipliedAlpha
	{
		get
		{
			if (mPMA == -1)
			{
				Shader sh = shader;
				mPMA = (sh != null && sh.name.Contains("Premultiplied")) ? 1 : 0;
			}
			return (mPMA == 1);
		}
	}

	/// <summary>
	/// Widget's dimensions used for drawing. X = left, Y = bottom, Z = right, W = top.
	/// This function automatically adds 1 pixel on the edge if the texture's dimensions are not even.
	/// It's used to achieve pixel-perfect sprites even when an odd dimension widget happens to be centered.
	/// </summary>

	public override Vector4 drawingDimensions
	{
		get
		{
			Vector2 offset = pivotOffset;

			float x0 = -offset.x * mWidth;
			float y0 = -offset.y * mHeight;
			float x1 = x0 + mWidth;
			float y1 = y0 + mHeight;

			int w = (mSprite != null) ? Mathf.RoundToInt(mSprite.textureRect.width) : mWidth;
			int h = (mSprite != null) ? Mathf.RoundToInt(mSprite.textureRect.height) : mHeight;

			if ((w & 1) != 0) x1 -= (1f / w) * mWidth;
			if ((h & 1) != 0) y1 -= (1f / h) * mHeight;

			return new Vector4(
				mDrawRegion.x == 0f ? x0 : Mathf.Lerp(x0, x1, mDrawRegion.x),
				mDrawRegion.y == 0f ? y0 : Mathf.Lerp(y0, y1, mDrawRegion.y),
				mDrawRegion.z == 1f ? x1 : Mathf.Lerp(x0, x1, mDrawRegion.z),
				mDrawRegion.w == 1f ? y1 : Mathf.Lerp(y0, y1, mDrawRegion.w));
		}
	}

	/// <summary>
	/// Texture rectangle.
	/// </summary>

	public Rect uvRect
	{
		get
		{
			Texture tex = mainTexture;

			if (tex != null)
			{
				Rect rect = mSprite.textureRect;

				rect.xMin /= tex.width;
				rect.xMax /= tex.width;
				rect.yMin /= tex.height;
				rect.yMax /= tex.height;

				return rect;
			}
			return new Rect(0f, 0f, 1f, 1f);
		}
	}

	/// <summary>
	/// Update the sprite in case it was animated.
	/// </summary>

	protected override void OnUpdate ()
	{
		if (nextSprite != null)
		{
			if (nextSprite != mSprite)
				sprite2D = nextSprite;
			nextSprite = null;
		}
		base.OnUpdate();
	}

	/// <summary>
	/// Adjust the scale of the widget to make it pixel-perfect.
	/// </summary>

	public override void MakePixelPerfect ()
	{
		if (mSprite != null)
		{
			Rect rect = mSprite.textureRect;
			int w = Mathf.RoundToInt(rect.width);
			int h = Mathf.RoundToInt(rect.height);

			if ((w & 1) == 1) ++w;
			if ((h & 1) == 1) ++h;

			width = w;
			height = h;
		}
		base.MakePixelPerfect();
	}

	/// <summary>
	/// Virtual function called by the UIPanel that fills the buffers.
	/// </summary>

	public override void OnFill (BetterList<Vector3> verts, BetterList<Vector2> uvs, BetterList<Color32> cols)
	{
		Color colF = color;
		colF.a = finalAlpha;
		Color32 col = premultipliedAlpha ? NGUITools.ApplyPMA(colF) : colF;
		Vector4 v = drawingDimensions;
		Rect rect = uvRect;

		verts.Add(new Vector3(v.x, v.y));
		verts.Add(new Vector3(v.x, v.w));
		verts.Add(new Vector3(v.z, v.w));
		verts.Add(new Vector3(v.z, v.y));

		uvs.Add(new Vector2(rect.xMin, rect.yMin));
		uvs.Add(new Vector2(rect.xMin, rect.yMax));
		uvs.Add(new Vector2(rect.xMax, rect.yMax));
		uvs.Add(new Vector2(rect.xMax, rect.yMin));

		cols.Add(col);
		cols.Add(col);
		cols.Add(col);
		cols.Add(col);
	}
}
#endif
