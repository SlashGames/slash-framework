//----------------------------------------------
//            NGUI: Next-Gen UI kit
// Copyright © 2011-2014 Tasharen Entertainment
//----------------------------------------------

using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

/// <summary>
/// Inspector class used to edit UITextures.
/// </summary>

[CanEditMultipleObjects]
#if UNITY_3_5
[CustomEditor(typeof(UITexture))]
#else
[CustomEditor(typeof(UITexture), true)]
#endif
public class UITextureInspector : UIWidgetInspector
{
	UITexture mTex;

	protected override void OnEnable ()
	{
		base.OnEnable();
		mTex = target as UITexture;
	}

	protected override bool ShouldDrawProperties ()
	{
		SerializedProperty sp = NGUIEditorTools.DrawProperty("Texture", serializedObject, "mTexture");
		NGUIEditorTools.DrawProperty("Material", serializedObject, "mMat");

		NGUISettings.texture = sp.objectReferenceValue as Texture;

		if (mTex.material == null || serializedObject.isEditingMultipleObjects)
		{
			NGUIEditorTools.DrawProperty("Shader", serializedObject, "mShader");
		}

		EditorGUI.BeginDisabledGroup(serializedObject.isEditingMultipleObjects);
		if (mTex.mainTexture != null)
		{
			Rect rect = EditorGUILayout.RectField("UV Rectangle", mTex.uvRect);

			if (rect != mTex.uvRect)
			{
				NGUIEditorTools.RegisterUndo("UV Rectangle Change", mTex);
				mTex.uvRect = rect;
			}
		}
		EditorGUI.EndDisabledGroup();
		return true;
	}

	/// <summary>
	/// Allow the texture to be previewed.
	/// </summary>

	public override bool HasPreviewGUI ()
	{
		return (mTex != null) && (mTex.mainTexture as Texture2D != null);
	}

	/// <summary>
	/// Draw the sprite preview.
	/// </summary>

	public override void OnPreviewGUI (Rect rect, GUIStyle background)
	{
		Texture2D tex = mTex.mainTexture as Texture2D;
		if (tex != null) NGUIEditorTools.DrawTexture(tex, rect, mTex.uvRect, mTex.color);
	}
}
