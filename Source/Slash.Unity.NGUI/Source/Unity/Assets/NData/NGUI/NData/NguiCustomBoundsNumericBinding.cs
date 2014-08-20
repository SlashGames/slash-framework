using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public abstract class NguiCustomBoundsNumericBinding : NguiNumericBinding
{
	public double Min = 0;
	public double Max = 1;
	
	public string MinPath = "";
	private Dictionary<System.Type, EZData.Property> _minProperties =
		new Dictionary<System.Type, EZData.Property>();
	
	public string MaxPath = "";
	private Dictionary<System.Type, EZData.Property> _maxProperties =
		new Dictionary<System.Type, EZData.Property>();
	
	public override IList<string> ReferencedPaths
	{
		get
		{
			var paths = new List<string>(base.ReferencedPaths);
			if (!string.IsNullOrEmpty(MinPath))
				paths.Add(MinPath);
			if (!string.IsNullOrEmpty(MaxPath))
				paths.Add(MaxPath);
			return paths;
		}
	}
	
	protected override void Unbind ()
	{
		base.Unbind();
		
		foreach(var p in _minProperties)
			p.Value.OnChange -= OnChange;
		_minProperties.Clear();
		
		foreach(var p in _maxProperties)
			p.Value.OnChange -= OnChange;
		_maxProperties.Clear();
	}
	
	protected override void Bind()
	{
		base.Bind();
		
		if (!string.IsNullOrEmpty(MinPath))
		{
			FillNumericProperties(_minProperties, MinPath);
			foreach(var p in _minProperties)
				p.Value.OnChange += OnChange;
		}
		
		if (!string.IsNullOrEmpty(MaxPath))
		{
			FillNumericProperties(_maxProperties, MaxPath);
			foreach(var p in _maxProperties)
				p.Value.OnChange += OnChange;
		}
	}
	
	private double DataToSlider(double data)
	{
		if (System.Math.Abs(Max - Min) < double.Epsilon)
			return 0.0;
		
		return (data - Min) / (Max - Min);
	}
	
	private double SliderToData(double slider)
	{
		if (System.Math.Abs(Max - Min) < double.Epsilon)
			return 0.0;
		
		return Min + slider * (Max - Min);
	}
		
	protected virtual void SetCustomBoundsNumericValue(double val)
	{
		base.SetNumericValue(SliderToData(val));
	}
	
	protected sealed override void ApplyNewValue(double val)
	{
		if (!string.IsNullOrEmpty(MaxPath))
			Max = GetNumericValue(_maxProperties);
		
		if (!string.IsNullOrEmpty(MinPath))
			Min = GetNumericValue(_minProperties);
		
		ApplyNewCustomBoundsValue(DataToSlider(val));
	}
	
	protected abstract void ApplyNewCustomBoundsValue(double val);
}
