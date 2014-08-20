using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public abstract class NguiPollingNumericBinding : NguiNumericBinding
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
			SetNumericValue(_prevValue);
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
			SetNumericValue(newScale);
		}
	}
	
	protected sealed override void ApplyNewValue(double val)
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
