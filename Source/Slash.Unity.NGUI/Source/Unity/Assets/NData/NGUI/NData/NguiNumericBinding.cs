using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public abstract class NguiNumericBinding : NguiBinding
{
	private Dictionary<System.Type, EZData.Property> _properties =
		new Dictionary<System.Type, EZData.Property>();
	
	private bool _ignoreChanges;
	
	protected override void Unbind()
	{
		base.Unbind();
		
		foreach(var p in _properties)
			p.Value.OnChange -= OnChange;
		
		_properties.Clear();
	}
	
	protected override void Bind()
	{
		base.Bind();
		
		FillNumericProperties(_properties, Path);
		
		foreach(var p in _properties)
			p.Value.OnChange += OnChange;
	}
	
	private double GetNumericValue()
	{
		return GetNumericValue(_properties);
	}
	
	protected virtual void SetNumericValue(double val)
	{
		SetNumericValue(_properties, val);
	}
	
	protected override void OnChange()
	{
		base.OnChange();
		
		if (_ignoreChanges)
			return;
		
		if (_properties.Count == 0)
			return;
		
		_ignoreChanges = true;
		ApplyNewValue(GetNumericValue());
		_ignoreChanges = false;
	}
	
	protected abstract void ApplyNewValue(double val);
}
