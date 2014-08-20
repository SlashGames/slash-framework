using System;
using System.IO;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


public class NdataQuickStart
{
	[MenuItem("Tools/NData/Quick Start")]
	public static void QuickStart()
	{
		Debug.Log("Adding NData components and assets to the scene");
		var uiRoots = GameObject.FindObjectsOfType(typeof(UIRoot));
		if (uiRoots.Length == 0)
		{
			Debug.LogWarning("Failed to find NGUI UIRoot in the current scene.");
			return;
		}
		var uiRoot = ((UIRoot)uiRoots[0]).gameObject;
		if (uiRoots.Length > 1)
		{
			Debug.LogWarning("More than one NGUI UIRoot was found in the current scene. Applying NData only to " + uiRoot.name);
		}
		var rootContext = uiRoot.GetComponent<NguiRootContext>();
		if (rootContext == null)
		{
			Debug.Log("Adding NData context root to " + uiRoot.name);
			rootContext = uiRoot.AddComponent<NguiRootContext>();
		}
		else
		{
			Debug.Log(uiRoot.name + " already contains NData context root");
		}
		
		var existingViewModelMarker = "/viewmodel.cs";
		var viewModelPath = "Assets/ViewModel.cs";
		var viewModelCode = 
@"using UnityEngine;
 
public class Ui : EZData.Context
{
 //TODO: add your dependency properties and collections here
}
 
public class ViewModel : MonoBehaviour
{
 public NguiRootContext View;
 public Ui Context;
 
 void Awake()
 {
  Context = new Ui();
  View.SetContext(Context);
 }
}
";
		var allAssets = AssetDatabase.GetAllAssetPaths();
		var viewModelExists = false;
		foreach(var a in allAssets)
		{
			if (a.ToLower().EndsWith(existingViewModelMarker.ToLower()))
			{
				var fileContent = System.IO.File.ReadAllText(a);
				if (fileContent == viewModelCode)
				{
					Debug.Log("Default ViewModel already exists in file " + a);
					viewModelExists = true;
					break;
				}
				else if (fileContent.Contains("NguiRootContext") &&
						 fileContent.Contains("ViewModel"))
				{
					Debug.Log("Existing ViewModel modification already exists in file " + a);
					viewModelExists = true;
					break;
				}
			}
		}
		if (!viewModelExists)
		{
			Debug.Log("Creating ViewModel asset");
			System.IO.File.WriteAllText(viewModelPath, viewModelCode);
			AssetDatabase.Refresh();
		}
		
		var viewModelObjectExists = false;
		var potentialViewModels = GameObject.FindObjectsOfType(typeof(MonoBehaviour));
		foreach(var i in potentialViewModels)
		{
			var pvm = ((MonoBehaviour)i).gameObject;
			if (pvm.GetComponent("ViewModel") != null)
			{
				Debug.Log("Scene already contains view model");
				viewModelObjectExists = true;
				break;
			}
		}
		
		if (!viewModelObjectExists)
		{
			Debug.Log("Creating ViewModel object in scene");
			var viewModelObject = new GameObject("ViewModel");
			viewModelObject.AddComponent<QuickStartFinalizer>();
			Selection.objects = new GameObject[] { viewModelObject };
		}
	}
}
