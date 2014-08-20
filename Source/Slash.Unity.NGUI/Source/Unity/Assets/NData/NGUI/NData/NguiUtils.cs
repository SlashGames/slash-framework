using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class NguiUtils
{
	public static NguiDataContext FindRootContext(GameObject gameObject, int depthToGo)
	{
		NguiDataContext lastGoodContext = null; 
		var p = gameObject;//.transform.parent == null ? null : gameObject.transform.parent.gameObject;
		depthToGo++;
		while (p != null && depthToGo > 0)
		{
			var context = p.GetComponent<NguiDataContext>();
			if (context != null)
			{
				lastGoodContext = context;
				depthToGo--;
			}
			p = (p.transform.parent == null) ? null : p.transform.parent.gameObject;
		}
		return lastGoodContext;
	}
	
	public const int MaxPathDepth = 100500;
	
	public static int GetPathDepth(string path)
	{
		if (!path.StartsWith("#"))
			return 0;
		var depthString = path.Substring(1);
		var dotIndex = depthString.IndexOf('.');
		if (dotIndex >= 0)
			depthString = depthString.Substring(0, dotIndex);
		if (depthString == "#")
		{
			return MaxPathDepth;
		}
		var depth = 0;
		if (int.TryParse(depthString, out depth))
		{
			return depth;
		}
		Debug.LogWarning("Failed to get binding context depth for: " + path);
		return 0;
	}
	
	public static string GetCleanPath(string path)
	{
		if (!path.StartsWith("#"))
			return path;
		var dotIndex = path.IndexOf('.');
		var result = (dotIndex < 0) ? path : path.Substring(dotIndex + 1);
		return result;
	}
	
	public static GameObject FindParent(GameObject target, GameObject [] possibleParents)
	{
		foreach(var p in possibleParents)
		{
			if (p == target)
				return p;
		}
		return target.transform.parent == null ? null : FindParent(target.transform.parent.gameObject, possibleParents);
	}
	
	public static T GetComponentInParents<T>(GameObject gameObject)
		where T : Component
	{
		var p = gameObject;
		
		T component = null;
		while (p != null && component == null)
		{
			component = p.GetComponent<T>();
			p = (p.transform.parent == null) ? null : p.transform.parent.gameObject;
		}
		return component;
	}
		
	public static T GetComponentInParentsExcluding<T>(GameObject gameObject)
		where T : Component
	{
		return GetComponentInParents<T>((gameObject.transform.parent == null)
			? null
			: gameObject.transform.parent.gameObject);
	}
		
	public static T GetComponentAs<T>(this GameObject gameObject)
		where T : class
	{
		var mbs = gameObject.GetComponents<Component>();
		foreach(var mb in mbs)
		{
			if (mb is T)
				return mb as T;
		}
		return default(T);
	}
	
	public static T[] GetComponentsAs<T>(this GameObject gameObject)
		where T : class
	{
		var result = new List<T>();
		var mbs = gameObject.GetComponents<Component>();
		foreach(var mb in mbs)
		{
			if (mb is T)
				result.Add(mb as T);
		}
		return result.ToArray();
	}
	
	public static T[] GetComponentsInChildrenAs<T>(this GameObject gameObject)
		where T : class
	{
		var result = new List<T>();
		var mbs = gameObject.GetComponentsInChildren<Component>();
		foreach(var mb in mbs)
		{
			if (mb is T)
				result.Add(mb as T);
		}
		return result.ToArray();
	}
	
	public static T GetComponentInParentsAs<T>(GameObject gameObject)
		where T : class
	{
		T component = default(T);
		var p = gameObject;
		while (p != null && component == null)
		{
			var mbs = p.GetComponents<Component>();
			foreach(var mb in mbs)
			{
				if (mb is T)
				{
					component = mb as T;
					break;
				}
			}
			p = (p.transform.parent == null) ? null : p.transform.parent.gameObject;
		}
		return component;
	}
	
	
	public static T GetComponentInParentsExcludingAs<T>(GameObject gameObject)
		where T : class
	{
		return GetComponentInParentsAs<T>((gameObject.transform.parent == null)
			? null
			: gameObject.transform.parent.gameObject);
	}
	
	public delegate void ObjectAction(GameObject target);
	
	public static void ForObjectsSubTree(GameObject root, ObjectAction action)
	{
		if (root == null)
			return;
		action(root);
		
		for (var i = 0; i < root.transform.childCount; ++i)
		{
			ForObjectsSubTree(root.transform.GetChild(i).gameObject, action);
		}
	}
	
	public static string LocalizeFormat(string input)
	{
		var output = "";
		var index = 0;
		
		const char marker = '$';
		
		while(true)
		{
			var localStart = input.IndexOf(marker, index);
			if (localStart < 0)
			{
				output += input.Substring(index);
				break;
			}
			if (localStart > index)
			{
				output += input.Substring(index, localStart - index);
				index = localStart;
			}
			
			var localFinish = input.IndexOf(marker, index + 1);
			if (localFinish < 0)
			{
				break;
			}
			if (localFinish == localStart + 1)
			{
				output += marker;
			}
			else
			{
				var key = input.Substring(localStart + 1, localFinish - localStart - 1);
				output += Localization.Get(key);
			}
			index = localFinish + 1;
		}
		
		return output;
	}
	
	public static void SetVisible(GameObject gameObject, bool visible)
	{
		SetNguiVisible(gameObject, visible);
		for (var i = 0; i < gameObject.transform.childCount; ++i)
		{
			var child = gameObject.transform.GetChild(i).gameObject;
			var childVisibilityBinding = child.GetComponentAs<IVisibilityBinding>();
			if (childVisibilityBinding != null)
			{
				SetVisible(child, visible && childVisibilityBinding.ObjectVisible);
			}
			else
			{
				SetVisible(child, visible);
			}
		}
	}
	
	private static void SetNguiVisible(GameObject gameObject, bool visible)
	{
		foreach(var collider in gameObject.GetComponents<Collider>())
		{
			collider.enabled = visible;	
		}
		
		foreach(var widget in gameObject.GetComponents<UIWidget>())
		{
			widget.enabled = visible;
		}
	}
}
