// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DebugInputEditor.cs" company="Rainy Games">
//   Copyright (c) Rainy Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using RainyGames.Unity.Common.Input;
using RainyGames.Unity.Editor.Common.Inspectors;

using UnityEditor;

using UnityEngine;

/// <summary>
///   Custom editor/inspector of the mono behaviour DebugInput.
/// </summary>
[CustomEditor(typeof(DebugInput))]
public class DebugInputEditor : Editor
{
    #region Fields

    /// <summary>
    ///   Current inspected behaviour.
    /// </summary>
    private DebugInput instance;

    /// <summary>
    ///   Property fields of the current inspected behaviour.
    /// </summary>
    private PropertyField[] propertyFields;

    #endregion

    #region Public Methods and Operators

    /// <summary>
    ///   Called when the inspector gets active.
    /// </summary>
    public void OnEnable()
    {
        this.instance = this.target as DebugInput;
        if (this.instance != null)
        {
            this.propertyFields = ExposeProperties.GetProperties(this.instance);
        }
        else
        {
            this.propertyFields = null;
        }
    }

    public void OnDisable()
    {
        this.instance = null;
    }

    /// <summary>
    ///   Called when the inspector GUI should be drawn.
    /// </summary>
    public override void OnInspectorGUI()
    {
        if (this.instance == null)
        {
            return;
        }

        // Draw default inspector.
        this.DrawDefaultInspector();

        // Draw properties.
        ExposeProperties.Expose(this.propertyFields);

        // Draw input information.
        EditorGUILayout.Vector2Field("Screen Dimensions", new Vector2(Screen.width, Screen.height));
        EditorGUILayout.Vector3Field("Mouse Position", Input.mousePosition);
        EditorGUILayout.Vector3Field("GUI Mouse Position", InputUtils.GUIMousePosition);

        // Always update.
        EditorUtility.SetDirty(this.instance);
    }

    #endregion
}