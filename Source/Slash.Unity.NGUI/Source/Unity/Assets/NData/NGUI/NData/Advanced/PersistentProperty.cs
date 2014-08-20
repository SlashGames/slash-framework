using UnityEngine;

namespace EZData
{
	public class PersistentProperty<T> : Property<T>
	{
		private string _key;
		private bool _persistValue;
		
		public PersistentProperty(string key)
			: this(key, default(T))
		{
		}
		
		public PersistentProperty(string key, T defaultValue)
			: base(defaultValue)
		{
			_key = key;
			InitPrefValue(_key, defaultValue);
		}
		
		public override void SetValue (T value)
		{
			if (_persistValue)
				SetPrefValue(_key, value);
			base.SetValue(value);
		}
		
		private void InitPrefValue(string key, T defaultValue)
		{
			if (!PlayerPrefs.HasKey(key))
				SetPrefValue(key, defaultValue);
			SetValue(GetPrefValue(key, defaultValue));
			_persistValue = true;
		}
		
		private T GetPrefValue(string key, T defaultValue)
		{
			if (typeof(T) == typeof(long) ||
				typeof(T) == typeof(ulong) ||
				typeof(T) == typeof(int) ||
				typeof(T) == typeof(uint) ||
				typeof(T) == typeof(short) ||
				typeof(T) == typeof(ushort) ||
				typeof(T) == typeof(sbyte) ||
				typeof(T) == typeof(byte))
				return (T)(object)PlayerPrefs.GetInt(key, (int)(object)defaultValue);
			if (typeof(T) == typeof(float) ||
				typeof(T) == typeof(double))
				return (T)(object)PlayerPrefs.GetFloat(key, (float)(object)defaultValue);
#if !UNITY_FLASH
			if (typeof(T) == typeof(decimal))
				return (T)(object)PlayerPrefs.GetFloat(key, (float)(object)defaultValue);
#endif
			if (typeof(T) == typeof(string))
				return (T)(object)PlayerPrefs.GetString(key, (string)(object)defaultValue);
			if (typeof(T) == typeof(bool))
				return (T)(object)(PlayerPrefs.GetInt(key, ((bool)(object)defaultValue) ? 1 : 0) != 0);
			
			Debug.LogWarning("Type " + typeof(T).ToString() + " is not supported for persistent properties");
			return default(T);
		}
		
		private void SetPrefValue(string key, T value)
		{
			if (typeof(T) == typeof(long) ||
				typeof(T) == typeof(ulong) ||
				typeof(T) == typeof(int) ||
				typeof(T) == typeof(uint) ||
				typeof(T) == typeof(short) ||
				typeof(T) == typeof(ushort) ||
				typeof(T) == typeof(sbyte) ||
				typeof(T) == typeof(byte))
				PlayerPrefs.SetInt(key, (int)(object)value);
			else if (typeof(T) == typeof(float) ||
				typeof(T) == typeof(double))
				PlayerPrefs.SetFloat(key, (float)(object)value);
#if !UNITY_FLASH
			else if (typeof(T) == typeof(decimal))
				PlayerPrefs.SetFloat(key, (float)(object)value);
#endif
			else if (typeof(T) == typeof(string))
				PlayerPrefs.SetString(key, (string)(object)value);
			else if (typeof(T) == typeof(bool))
				PlayerPrefs.SetInt(key, ((bool)(object)value) ? 1 : 0);
			else
				Debug.LogWarning("Type " + typeof(T).ToString() + " is not supported for persistent properties");
		}
	}
}
