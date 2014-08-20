using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public abstract class NguiPollingCustomBoundsNumericBinding : NguiCustomBoundsNumericBinding
{
	public NguiBindingDirection Direction = NguiBindingDirection.TwoWay;
	public NguiBindingInitialValue InitialValue = NguiBindingInitialValue.TakeFromModel;
	
	private double _prevValue;
	private bool _inited;
	
	public override void Start()
	{
		base.Start();
		
		if (InitialValue == NguiBindingInitialValue.TakeFromView)
		{
			_inited = true;
			_prevValue = GetValue();
			SetCustomBoundsNumericValue(_prevValue);
		}
	}
	
	void Update()
	{
		if (!Direction.NeedsViewTracking())
			return;
		
		var newScale = GetValue();
		if (_prevValue != newScale)
		{
			_prevValue = newScale;
			SetCustomBoundsNumericValue(newScale);
		}
	}
	
	protected sealed override void ApplyNewCustomBoundsValue(double val)
	{
		if (!_inited && InitialValue == NguiBindingInitialValue.TakeFromView)
			return;
		
		_inited = true;
		if (Direction.NeedsViewUpdate())
		{
			_prevValue = val;
			SetValue(val);
		}
	}
	
	protected abstract double GetValue();
	protected abstract void SetValue(double val);
}
