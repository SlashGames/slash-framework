//----------------------------------------------
//            NGUI: Next-Gen UI kit
// Copyright © 2011-2014 Tasharen Entertainment
//----------------------------------------------

#if UNITY_EDITOR || !UNITY_FLASH
#define REFLECTION_SUPPORT
#endif

#if REFLECTION_SUPPORT
using System.Reflection;
using System.Diagnostics;
using Slash.Unity.NGUIExt.Util;
#endif

using UnityEngine;
using System;

/// <summary>
/// Reference to a specific field or property that can be set via inspector.
/// </summary>

[System.Serializable]
public class PropertyReference
{
	[SerializeField] Component mTarget;
	[SerializeField] string mName;

#if REFLECTION_SUPPORT
	FieldInfo mField = null;
	PropertyInfo mProperty = null;
#endif

	/// <summary>
	/// Event delegate's target object.
	/// </summary>

	public Component target
	{
		get
		{
			return mTarget;
		}
		set
		{
			mTarget = value;
#if REFLECTION_SUPPORT
			mProperty = null;
			mField = null;
#endif
		}
	}

	/// <summary>
	/// Event delegate's method name.
	/// </summary>

	public string name
	{
		get
		{
			return mName;
		}
		set
		{
			mName = value;
#if REFLECTION_SUPPORT
			mProperty = null;
			mField = null;
#endif
		}
	}

	/// <summary>
	/// Whether this delegate's values have been set.
	/// </summary>

	public bool isValid { get { return (mTarget != null && !string.IsNullOrEmpty(mName)); } }

	/// <summary>
	/// Whether the target script is actually enabled.
	/// </summary>

	public bool isEnabled
	{
		get
		{
			if (mTarget == null) return false;
			MonoBehaviour mb = (mTarget as MonoBehaviour);
			return (mb == null || mb.enabled);
		}
	}

	public PropertyReference () { }
	public PropertyReference (Component target, string fieldName)
	{
		mTarget = target;
		mName = fieldName;
	}

	/// <summary>
	/// Helper function that returns the property type.
	/// </summary>

	public Type GetPropertyType ()
	{
#if REFLECTION_SUPPORT
		if (mProperty == null && mField == null && isValid) Cache();
		if (mProperty != null) return mProperty.PropertyType;
		if (mField != null) return mField.FieldType;
#endif
		return typeof(void);
	}

	/// <summary>
	/// Equality operator.
	/// </summary>

	public override bool Equals (object obj)
	{
		if (obj == null)
		{
			return !isValid;
		}
		
		if (obj is PropertyReference)
		{
			PropertyReference pb = obj as PropertyReference;
			return (mTarget == pb.mTarget && string.Equals(mName, pb.mName));
		}
		return false;
	}

	static int s_Hash = "PropertyBinding".GetHashCode();

	/// <summary>
	/// Used in equality operators.
	/// </summary>

	public override int GetHashCode () { return s_Hash; }

	/// <summary>
	/// Set the delegate callback using the target and method names.
	/// </summary>

	public void Set (Component target, string methodName)
	{
		mTarget = target;
		mName = methodName;
	}

	/// <summary>
	/// Clear the event delegate.
	/// </summary>

	public void Clear ()
	{
		mTarget = null;
		mName = null;
	}

	/// <summary>
	/// Reset the cached references.
	/// </summary>

	public void Reset ()
	{
		mField = null;
		mProperty = null;
	}

	/// <summary>
	/// Convert the delegate to its string representation.
	/// </summary>

	public override string ToString () { return ToString(mTarget, name); }

	/// <summary>
	/// Convenience function that converts the specified component + property pair into its string representation.
	/// </summary>

	static public string ToString (Component comp, string property)
	{
		if (comp != null)
		{
			string typeName = comp.GetType().ToString();
			int period = typeName.LastIndexOf('.');
			if (period > 0) typeName = typeName.Substring(period + 1);

			if (!string.IsNullOrEmpty(property)) return typeName + "." + property;
			else return typeName + ".[property]";
		}
		return null;
	}

	/// <summary>
	/// Helper function that returns the value of the specified property.
	/// </summary>

//    static public object GetValue (object obj, string property)
//    {
//#if REFLECTION_SUPPORT
//        if (obj == null) return null;

//        // No property specified? Return the object itself.
//        if (string.IsNullOrEmpty(property)) return obj;

//        // If it's a game object, always return it as-is
//        System.Type type = obj.GetType();
//        if (type == typeof(GameObject)) return obj;

//        // Try to get the property with that name, and if found -- execute it
//        PropertyInfo pi = type.GetProperty(property);
//        if (pi != null) return pi.GetValue(obj, null);

//        // Try to get a field with that name, and if found -- execute it
//        FieldInfo field = type.GetField(property);
//        if (field != null) return field.GetValue(obj);
//#endif
//        return null;
//    }

	/// <summary>
	/// Retrieve the property's value.
	/// </summary>

	[DebuggerHidden]
	[DebuggerStepThrough]
	public object Get ()
	{
#if REFLECTION_SUPPORT
		if (mProperty == null && mField == null && isValid) Cache();

		if (mProperty != null)
		{
			if (mProperty.CanRead)
				return mProperty.GetValue(mTarget, null);
		}
		else if (mField != null)
		{
			return mField.GetValue(mTarget);
		}
#endif
		return null;
	}

	/// <summary>
	/// Assign the bound property's value.
	/// </summary>

	[DebuggerHidden]
	[DebuggerStepThrough]
	public bool Set (object value)
	{
#if REFLECTION_SUPPORT
		if (mProperty == null && mField == null && isValid) Cache();
		if (mProperty == null && mField == null) return false;

		if (value == null)
		{
			try
			{
				if (mProperty != null) mProperty.SetValue(mTarget, null, null);
				else mField.SetValue(mTarget, null);
			}
			catch (Exception) { return false; }
		}

		// Can we set the value?
		if (!Convert(ref value))
		{
			if (Application.isPlaying)
				UnityEngine.Debug.LogError("Unable to convert " + value.GetType() + " to " + GetPropertyType());
		}
		else if (mField != null)
		{
			mField.SetValue(mTarget, value);
			return true;
		}
		else if (mProperty.CanWrite)
		{
			mProperty.SetValue(mTarget, value, null);
			return true;
		}
#endif
		return false;
	}

	/// <summary>
	/// Whether we can assign the property using the specified value.
	/// </summary>

	bool Convert (ref object value)
	{
#if REFLECTION_SUPPORT
		if (mTarget == null) return false;

		Type to = GetPropertyType();
		Type from;

		if (value == null)
		{
		    if (ReflectionUtils.IsValueType(to)) return false;
			from = to;
		}
		else from = value.GetType();
		return Convert(ref value, from, to);
#else
		return false;
#endif
	}

	/// <summary>
	/// Whether we can convert one type to another for assignment purposes.
	/// </summary>

	static public bool Convert (Type from, Type to)
	{
		object temp = null;
		return Convert(ref temp, from, to);
	}

	/// <summary>
	/// Whether we can convert one type to another for assignment purposes.
	/// </summary>

	static public bool Convert (object value, Type to)
	{
		if (value == null)
		{
			value = null;
			return Convert(ref value, to, to);
		}
		return Convert(ref value, value.GetType(), to);
	}

	/// <summary>
	/// Whether we can convert one type to another for assignment purposes.
	/// </summary>

	static public bool Convert (ref object value, Type from, Type to)
	{
#if REFLECTION_SUPPORT
		// If the value can be assigned as-is, we're done
	    if (ReflectionUtils.IsAssignableFrom(to, from)) return true;

		// If the target type is a string, just convert the value
		if (to == typeof(string))
		{
			value = (value != null) ? value.ToString() : "null";
			return true;
		}

		// If the value is null we should not proceed further
		if (value == null) return false;

		if (to == typeof(int))
		{
			if (from == typeof(string))
			{
				int val;

				if (int.TryParse((string)value, out val))
				{
					value = val;
					return true;
				}
			}
			else if (from == typeof(float))
			{
				value = Mathf.RoundToInt((float)value);
				return true;
			}
		}
		else if (to == typeof(float))
		{
			if (from == typeof(string))
			{
				float val;

				if (float.TryParse((string)value, out val))
				{
					value = val;
					return true;
				}
			}
		}
#endif
		return false;
	}

	/// <summary>
	/// Cache the field or property.
	/// </summary>

	[DebuggerHidden]
	[DebuggerStepThrough]
	bool Cache ()
	{
#if REFLECTION_SUPPORT
		if (mTarget != null && !string.IsNullOrEmpty(mName))
		{
			Type type = mTarget.GetType();
		    mField = ReflectionUtils.GetField(type, mName);
            mProperty = ReflectionUtils.GetProperty(type, mName);
		}
		else
		{
			mField = null;
			mProperty = null;
		}
		return (mField != null || mProperty != null);
#else
		return false;
#endif
	}
}
