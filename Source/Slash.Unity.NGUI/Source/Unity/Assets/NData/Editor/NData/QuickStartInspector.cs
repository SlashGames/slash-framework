using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(QuickStartFinalizer))]
public class QuickStartInspector : Editor
{
	public override void OnInspectorGUI()
    {
		GUILayout.Label("Please, wait until the scripts\ncompilation is finished.\nThank you for using NData.");
		
		QuickStartFinalizer qsf;
        qsf = target as QuickStartFinalizer;
		var gameObject = qsf.gameObject;
		
		if (EditorApplication.isCompiling)
		{
			return;
		}
		
		Debug.Log("Finalizing ViewModel object");
		
		var rootContexts = GameObject.FindObjectsOfType(typeof(NguiRootContext));
		var rootContext = rootContexts.Length > 0 ? (NguiRootContext)rootContexts[0] : null;
		var viewModelComponent = gameObject.AddComponent("ViewModel");
		var vmType = viewModelComponent.GetType();
		var viewField = vmType.GetField("View");
		viewField.SetValue(viewModelComponent, rootContext);
		
		GameObject.DestroyImmediate(qsf);
	}
}
