using System;
using System.IO;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class SceneValidator
{
	private const string MasterPathStopper = "#";
	
	private static string GetGameObjectPath(GameObject gameObject)
	{
		if (gameObject == null)
			return null;
		
		string fullName = "";
		while(gameObject != null)
		{
			fullName = gameObject.name + ((fullName.Length > 0) ? "." : "") + fullName;
			gameObject = (gameObject.transform.parent == null) ? null : gameObject.transform.parent.gameObject;
		}
		return fullName;
	}
	
	private static System.Type GetCollectionValueType(System.Type type)
	{
		if (type == null)
			return null;
		var genericArguments = type.GetGenericArguments();
		if (genericArguments.Length == 0)
			return null;
		return genericArguments[0];
	}
	
	private static System.Type GetBindingValueType(string path, System.Type type, GameObject gameObject, bool command, ref string tail)
	{
		tail = string.Empty;
		
		if (string.IsNullOrEmpty(path))
		{
			Debug.Log("Binding is empty for object " + GetGameObjectPath(gameObject), gameObject);
			return null;
		}
		
		var parts = path.Split('.');
		var pathMessage = "";
		for (var i = 0; i < parts.Length; ++i)
		{
			var part = parts[i];
			pathMessage += part;
						
			if (i < parts.Length - 1)
			{
				int index;
				if (int.TryParse(part, out index))
				{
					for (var j = i + 1; j < parts.Length; ++j)
						tail += parts[j] + ".";
					if (tail.Length > 0)
						tail = tail.Substring(0, tail.Length - 1);
					return type;
				}
				else
				{
					var nodeProperty = type.GetProperty(part);
					if (nodeProperty == null)
					{
						Debug.LogError("Failed to resolve node in binding " + path + " in object " + GetGameObjectPath(gameObject) +
									   "\n[context type is " + type + "], error at " + pathMessage, gameObject);
						return null;
					}
					pathMessage += ".";
					type = nodeProperty.PropertyType;
				}
			}
			else
			{
				if (command)
				{
					var leafCommand = type.GetMethod(part);
					if (leafCommand == null)
					{
						Debug.LogError("Failed to resolve leaf command in binding " + path + " in object " + GetGameObjectPath(gameObject) +
						               "\n[context type is " + type + "], error at " + pathMessage, gameObject);
						return null;
					}
				}
				else
				{
					var leafProperty = type.GetProperty(part);
					if (leafProperty == null)
					{
						Debug.LogError("Failed to resolve leaf property in binding " + path + " in object " + GetGameObjectPath(gameObject) +
						               "\n[context type is " + type + "], error at " + pathMessage, gameObject);
						return null;
					}
					type = leafProperty.PropertyType;
				}
			}
		}
		return type;
	}

	private static System.Type GetBindingValueType(string path, System.Type type, GameObject gameObject, bool command)
	{
		var tail = path;
		while(tail.Length > 0)
		{
			type = GetBindingValueType(tail, type, gameObject, command, ref tail);
			if (tail.Length == 0)
				break;
			type = GetCollectionValueType(type);
		}
		
		return type;
	}

	private static System.Type GetCleanType(Stack<System.Type> type, int depth)
	{
		var array = type.ToArray();
		if (array.Length == 0)
			return null;
		if (depth >= array.Length)
			depth = array.Length - 1;
		return array[depth];
	}
	
	private static string GetFullCleanPath(Stack<string> masterPaths, string path, int depthToGo)
	{
		var masterPath = string.Empty;
		foreach(var p in masterPaths)
		{
			if (string.IsNullOrEmpty(p))
				continue;
			
			if (p == MasterPathStopper)
			{
				if (depthToGo == 0)
					break;
				depthToGo--;
				continue;
			}
			
			if (string.IsNullOrEmpty(masterPath))
				masterPath = p;
			else
				masterPath = p + "." + masterPath;
		}
		return string.IsNullOrEmpty(masterPath) ?
			path : (masterPath + "." + NguiUtils.GetCleanPath(path));
	}
	
	private static void ValidateRootObjectBindings(GameObject gameObject, Stack<System.Type> type, Stack<GameObject> templatesPath, Stack<string> masterPaths)
	{
		if (gameObject == null || type.Count == 0 || type.Peek() == null)
			return;
		
		if (gameObject.GetComponents<NguiMasterPath>().Length > 1)
			Debug.LogWarning("More than one NguiMasterPath detected in object " + GetGameObjectPath(gameObject), gameObject);
		
		var masterPath = gameObject.GetComponent<NguiMasterPath>();
		masterPaths.Push(masterPath == null ? string.Empty : masterPath.MasterPath);
		
		var childrenValidated = false;
		foreach(var c in gameObject.GetComponents<NguiBaseBinding>())
		{
			if (!c.enabled)
			{
				Debug.LogWarning("Binding in object " + GetGameObjectPath(gameObject) + " is disabled", gameObject);
			}
			
			var paths = c.ReferencedPaths;
			var types = new List<System.Type>();
			
			foreach(var p in paths)
			{
				var cleanPath = GetFullCleanPath(masterPaths, NguiUtils.GetCleanPath(p), NguiUtils.GetPathDepth(p));
				var cleanType = GetCleanType(type, NguiUtils.GetPathDepth(p));
				types.Add(GetBindingValueType(cleanPath, cleanType, gameObject, c is NguiCommandBinding));
			}
			
			var templates = c.gameObject.GetComponents<NguiListItemTemplate>();
			if (templates.Length == 0 &&
				types.Count > 0 &&
				c is NguiItemsSourceBinding)
			{
				type.Push(GetCollectionValueType(types[0]));
				masterPaths.Push(MasterPathStopper);
				for (var i = 0; i < c.transform.childCount; ++i)
					ValidateRootObjectBindings(c.transform.GetChild(i).gameObject, type, templatesPath, masterPaths);
				masterPaths.Pop();
				type.Pop();
				childrenValidated = true;
			}
			foreach(var l in templates)
			{
				if (l.Template == null)
				{
					Debug.LogError("List in object " + GetGameObjectPath(gameObject) + " doesn't have item template", gameObject);	
				}
				if (types.Count > 0 && c is NguiItemsSourceBinding)
				{
					if (templatesPath.Contains(l.Template))
					{
						Debug.Log("Skipping recursive template reference: " + l.Template);
						continue;
					}
					templatesPath.Push(l.Template);
					type.Push(GetCollectionValueType(types[0]));
					masterPaths.Push(MasterPathStopper);
					ValidateRootObjectBindings(l.Template, type, templatesPath, masterPaths);
					masterPaths.Pop();
					type.Pop();
					templatesPath.Pop();
				}	
			}
			foreach(var p in c.gameObject.GetComponents<NguiPopupListSourceBinding>())
			{
				var cleanPath = GetFullCleanPath(masterPaths, NguiUtils.GetCleanPath(p.DisplayValuePath), NguiUtils.GetPathDepth(p.DisplayValuePath));
				var cleanType = GetCleanType(type, NguiUtils.GetPathDepth(p.DisplayValuePath));
				GetBindingValueType(cleanPath, cleanType, gameObject, false);
			}	
		}
		if (!childrenValidated)
		{
			for (var i = 0; i < gameObject.transform.childCount; ++i)
			{
				ValidateRootObjectBindings(gameObject.transform.GetChild(i).gameObject, type, templatesPath, masterPaths);
			}
			childrenValidated = true;
		}
		
		masterPaths.Pop();
	}
	
	[MenuItem("Tools/NData/Clear Console &c")]
	public static void ClearConsole()
	{
		Assembly assembly = Assembly.GetAssembly(typeof(PrefabType));
        Type type = assembly.GetType("UnityEditorInternal.LogEntries");
        MethodInfo method = type.GetMethod ("Clear");
        method.Invoke (new object (), null);
	}
	
	[MenuItem("Tools/NData/Validate Bindings &v")]
	public static void ValidateBindings()
	{
		foreach (var s in UnityEditor.Selection.gameObjects)
		{
			if (PrefabUtility.GetPrefabType(s) != PrefabType.None)
				continue;
			
			var type = new Stack<System.Type>();
			var templatesPath = new Stack<GameObject>();
			var masterPaths = new Stack<string>();
			//type.Push(typeof(Ui.Game)); // Use your root data context class type here to speed up validation
			
			if (type.Count == 0)
			{
				var rootContext = NguiUtils.GetComponentInParents<NguiDataContext>(s) as NguiRootContext;
				if (rootContext != null && rootContext.defaultContext != null)
					type.Push(rootContext.defaultContext.GetType());
			}
			
			if (type.Count == 0)
			{
				System.Type bestType = null;
				var types = typeof(NguiRootContext).Assembly.GetTypes();
				foreach(var t in types)
				{
					var hasRootContextProperty = false;
					var hasContext = false;
					Type contextType = null;
					foreach(var f in t.GetFields())
					{
						if (f.FieldType == typeof(NguiRootContext))
							hasRootContextProperty = true;
						if (typeof(EZData.Context).IsAssignableFrom(f.FieldType))
						{
							hasContext = true;
							contextType = f.FieldType;
						}
					}
					foreach(var p in t.GetProperties())
					{
						if (p.PropertyType == typeof(NguiRootContext))
							hasRootContextProperty = true;
						if (typeof(EZData.Context).IsAssignableFrom(p.PropertyType))
						{
							hasContext = true;
							contextType = p.PropertyType;
						}
					}
					var presentInScene = false;
					if (typeof(Component).IsAssignableFrom(t))
						presentInScene = GameObject.FindObjectsOfType(t).Length > 0;
					if (hasRootContextProperty && hasContext && presentInScene)
					{
						Debug.Log(string.Format("Root data context class detected: {0}", contextType));
						bestType = contextType;
					}
				}
				type.Push(bestType);
			}
			
			ValidateRootObjectBindings(s, type, templatesPath, masterPaths);
		}
	}
}
