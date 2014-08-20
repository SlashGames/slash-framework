using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class NguiBooleanBinding : NguiBinding
{
	private readonly Dictionary<Type, EZData.Property> _properties = new Dictionary<Type, EZData.Property>();
	
	public enum CHECK_TYPE
	{
		BOOLEAN,
		EQUAL_TO_REFERENCE,
		GREATER_THAN_REFERENCE,
		LESS_THAN_REFERENCE,
		EMPTY,
		IS_LIST_SELECTION,
	}
	
	public CHECK_TYPE CheckType = CHECK_TYPE.BOOLEAN;
	public double Reference = 0;
	
	public bool DefaultValue = false;

	public bool Invert = false;
	
	private bool _ignoreValueChange = false;
	
	private NguiItemDataContext _listItem;
		
	public override void Awake()
	{
		base.Awake();
		
		_properties.Add(typeof(bool), null);
		_properties.Add(typeof(int), null);
		_properties.Add(typeof(Enum), null);
		_properties.Add(typeof(float), null);
		_properties.Add(typeof(double), null);
#if !UNITY_FLASH
		_properties.Add(typeof(decimal), null);
#endif
		_properties.Add(typeof(string), null);
		
	}
	
	void Update()
	{
		if (CheckType == CHECK_TYPE.IS_LIST_SELECTION && _listItem == null)
		{
			_listItem = NguiUtils.GetComponentInParents<NguiItemDataContext>(gameObject);
			if (_listItem != null)
			{
				_listItem.OnSelectedChange += OnChange;
				OnChange();
			}
		} 
	}
		
	protected override void Unbind()
	{
		base.Unbind();
		
		foreach(var p in _properties)
		{
			if (p.Value != null)
			{
				p.Value.OnChange -= OnChange;
				_properties[p.Key] = null;
				break;
			}
		}
	}
	
	protected override void Bind()
	{
		base.Bind();
		
		var context = GetContext(Path);
		if (context != null)
		{
			_properties[typeof(bool)] = context.FindProperty<bool>(Path, this);
			_properties[typeof(int)] = context.FindProperty<int>(Path, this);
			_properties[typeof(Enum)] = context.FindEnumProperty(Path, this);
			_properties[typeof(float)] = context.FindProperty<float>(Path, this);
			_properties[typeof(double)] = context.FindProperty<double>(Path, this);
#if !UNITY_FLASH
			_properties[typeof(decimal)] = context.FindProperty<decimal>(Path, this);
#endif
			_properties[typeof(string)] = context.FindProperty<string>(Path, this);
		}
		
		foreach(var p in _properties)
		{
			if (p.Value != null)
				p.Value.OnChange += OnChange;
		}
	}
	
	protected override void OnChange()
	{
		base.OnChange();
		
		var newValue = DefaultValue;
		
		if (CheckType == CHECK_TYPE.BOOLEAN)
		{
			if (_properties[typeof(bool)] != null)
				newValue = ((EZData.Property<bool>)_properties[typeof(bool)]).GetValue();
		}
		else if (CheckType == CHECK_TYPE.EMPTY)
		{
			if (_properties[typeof(string)] != null)
				newValue = string.IsNullOrEmpty(((EZData.Property<string>)_properties[typeof(string)]).GetValue());
		}
		else if (CheckType == CHECK_TYPE.IS_LIST_SELECTION)
		{
			if (_listItem != null)
				newValue = _listItem.Selected;
		} 
		else
		{
			var val = 0.0;
			if (_properties[typeof(int)] != null)
				val = ((EZData.Property<int>)_properties[typeof(int)]).GetValue();
			if (_properties[typeof(Enum)] != null)
				val = ((EZData.Property<int>)_properties[typeof(Enum)]).GetValue();
			if (_properties[typeof(float)] != null)
				val = ((EZData.Property<float>)_properties[typeof(float)]).GetValue();
			if (_properties[typeof(double)] != null)
				val = ((EZData.Property<double>)_properties[typeof(double)]).GetValue();
#if !UNITY_FLASH
			if (_properties[typeof(decimal)] != null)
				val = (double)((EZData.Property<decimal>)_properties[typeof(decimal)]).GetValue();
#endif
			switch(CheckType)
			{
				case CHECK_TYPE.EQUAL_TO_REFERENCE:
					newValue = (val == Reference);
					break;
				case CHECK_TYPE.GREATER_THAN_REFERENCE:
					newValue = (val > Reference);
					break;
				case CHECK_TYPE.LESS_THAN_REFERENCE:
					newValue = (val < Reference);
					break;
			}
		}
		
		if (!_ignoreValueChange)
			ApplyNewValue(Invert ? (!newValue) : newValue);
	}
	
	protected virtual void ApplyInputValue(bool inputValue)
	{
		if (CheckType != CHECK_TYPE.BOOLEAN)
			return;
		
		inputValue = Invert ? (!inputValue) : inputValue;
		
		_ignoreValueChange = true;
		
		if (_properties[typeof(bool)] != null)
			((EZData.Property<bool>)_properties[typeof(bool)]).SetValue(inputValue);
		
		_ignoreValueChange = false;
	}
	
	protected virtual void ApplyNewValue(bool newValue)
	{
		Debug.LogError("Not supposed to be here for " + Path);
	}
}
