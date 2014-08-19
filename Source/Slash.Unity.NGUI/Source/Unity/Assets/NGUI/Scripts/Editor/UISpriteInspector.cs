//----------------------------------------------
//            NGUI: Next-Gen UI kit
// Copyright © 2011-2014 Tasharen Entertainment
//----------------------------------------------

using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

/// <summary>
/// Inspector class used to edit UISprites.
/// </summary>

[CanEditMultipleObjects]
#if UNITY_3_5
[CustomEditor(typeof(UISprite))]
#else
[CustomEditor(typeof(UISprite), true)]
#endif
public class UISpriteInspector : UIWidgetInspector
{
	/// <summary>
	/// Atlas selection callback.
	/// </summary>

	void OnSelectAtlas (Object obj)
	{
		serializedObject.Update();
		SerializedProperty sp = serializedObject.FindProperty("mAtlas");
		sp.objectReferenceValue = obj;
		serializedObject.ApplyModifiedProperties();
		NGUISettings.atlas = obj as UIAtlas;
	}

	/// <summary>
	/// Sprite selection callback function.
	/// </summary>

	void SelectSprite (string spriteName)
	{
		serializedObject.Update();
		SerializedProperty sp = serializedObject.FindProperty("mSpriteName");
		sp.stringValue = spriteName;
		serializedObject.ApplyModifiedProperties();
		NGUISettings.selectedSprite = spriteName;
	}

	/// <summary>
	/// Draw the atlas and sprite selection fields.
	/// </summary>

	protected override bool ShouldDrawProperties ()
	{
		GUILayout.BeginHorizontal();
		if (NGUIEditorTools.DrawPrefixButton("Atlas"))
			ComponentSelector.Show<UIAtlas>(OnSelectAtlas);
		SerializedProperty atlas = NGUIEditorTools.DrawProperty("", serializedObject, "mAtlas", GUILayout.MinWidth(20f));
		
		if (GUILayout.Button("Edit", GUILayout.Width(40f)))
		{
			if (atlas != null)
			{
				UIAtlas atl = atlas.objectReferenceValue as UIAtlas;
				NGUISettings.atlas = atl;
				NGUIEditorTools.Select(atl.gameObject);
			}
		}
		GUILayout.EndHorizontal();

		SerializedProperty sp = serializedObject.FindProperty("mSpriteName");
		NGUIEditorTools.DrawAdvancedSpriteField(atlas.objectReferenceValue as UIAtlas, sp.stringValue, SelectSprite, false);
		return true;
	}

	/// <summary>
	/// Sprites's custom properties based on the type.
	/// </summary>

	protected override void DrawCustomProperties ()
	{
		GUILayout.Space(6f);

		SerializedProperty sp = NGUIEditorTools.DrawProperty("Sprite Type", serializedObject, "mType", GUILayout.MinWidth(20f));

		EditorGUI.BeginDisabledGroup(sp.hasMultipleDifferentValues);
		{
			UISprite.Type type = (UISprite.Type)sp.intValue;

			if (type == UISprite.Type.Simple)
			{
				NGUIEditorTools.DrawProperty("Flip", serializedObject, "mFlip");
			}
			else if (type == UISprite.Type.Sliced)
			{
				NGUIEditorTools.DrawProperty("Flip", serializedObject, "mFlip");
				sp = serializedObject.FindProperty("centerType");
				bool val = (sp.intValue != (int)UISprite.AdvancedType.Invisible);

				if (val != EditorGUILayout.Toggle("Fill Center", val))
				{
					sp.intValue = val ? (int)UISprite.AdvancedType.Invisible : (int)UISprite.AdvancedType.Sliced;
				}
			}
			else if (type == UISprite.Type.Filled)
			{
				NGUIEditorTools.DrawProperty("Flip", serializedObject, "mFlip");
				NGUIEditorTools.DrawProperty("Fill Dir", serializedObject, "mFillDirection", GUILayout.MinWidth(20f));
				GUILayout.BeginHorizontal();
				GUILayout.Space(4f);
				NGUIEditorTools.DrawProperty("Fill Amount", serializedObject, "mFillAmount", GUILayout.MinWidth(20f));
				GUILayout.Space(4f);
				GUILayout.EndHorizontal();
				NGUIEditorTools.DrawProperty("Invert Fill", serializedObject, "mInvert", GUILayout.MinWidth(20f));
			}
			else if (type == UISprite.Type.Advanced)
			{
				NGUIEditorTools.DrawProperty("  - Left", serializedObject, "leftType");
				NGUIEditorTools.DrawProperty("  - Right", serializedObject, "rightType");
				NGUIEditorTools.DrawProperty("  - Top", serializedObject, "topType");
				NGUIEditorTools.DrawProperty("  - Bottom", serializedObject, "bottomType");
				NGUIEditorTools.DrawProperty("  - Center", serializedObject, "centerType");
				NGUIEditorTools.DrawProperty("Flip", serializedObject, "mFlip");
			}
		}
		EditorGUI.EndDisabledGroup();

		//GUI.changed = false;
		//Vector4 draw = EditorGUILayout.Vector4Field("Draw Region", mWidget.drawRegion);

		//if (GUI.changed)
		//{
		//    NGUIEditorTools.RegisterUndo("Draw Region", mWidget);
		//    mWidget.drawRegion = draw;
		//}

		GUILayout.Space(4f);
		base.DrawCustomProperties();
	}

	/// <summary>
	/// All widgets have a preview.
	/// </summary>

	public override bool HasPreviewGUI () { return !serializedObject.isEditingMultipleObjects; }

	/// <summary>
	/// Draw the sprite preview.
	/// </summary>

	public override void OnPreviewGUI (Rect rect, GUIStyle background)
	{
		UISprite sprite = target as UISprite;
		if (sprite == null || !sprite.isValid) return;

		Texture2D tex = sprite.mainTexture as Texture2D;
		if (tex == null) return;

		UISpriteData sd = sprite.atlas.GetSprite(sprite.spriteName);
		NGUIEditorTools.DrawSprite(tex, rect, sd, sprite.color);
	}
}
