using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public abstract class NguiNumericMultiBinding : NguiMultiBinding
{
	private readonly List<Dictionary<Type, EZData.Property>> _propertyGroups = new List<Dictionary<Type, EZData.Property>>();
	
	public override void Awake()
	{
		base.Awake();
		
		for (var i = 0; i < Paths.Count; ++i)
		{
			_propertyGroups.Add(new Dictionary<Type, EZData.Property>());
		}
	}
	
	protected override void Unbind()
	{
		base.Unbind();
		
		foreach(var g in _propertyGroups)
		{
			foreach(var p in g)
			{
				p.Value.OnChange -= OnChange;
			}
			g.Clear();
		}
	}
	
	protected override void Bind()
	{
		base.Bind();
	
		for (var i = 0; i < _propertyGroups.Count && i < Paths.Count; ++i)
		{
			FillNumericProperties(_propertyGroups[i], Paths[i]);
		}
		
		foreach(var g in _propertyGroups)
		{
			foreach(var p in g)
			{
				p.Value.OnChange += OnChange;					
			}
		}
	}
	
	protected List<double> GetNumericValue()
	{
		var newValues = new List<double>();
		foreach(var g in _propertyGroups)
		{
			newValues.Add(GetNumericValue(g));
		}
		return newValues;
	}
	
	protected override void OnChange()
	{
		base.OnChange();
		
		var newValues = GetNumericValue();
		ApplyNewValue(newValues);
	}
	
	protected abstract void ApplyNewValue(List<double> values);
}
