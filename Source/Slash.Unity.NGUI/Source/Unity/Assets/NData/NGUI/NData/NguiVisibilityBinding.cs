using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
[AddComponentMenu("NGUI/NData/Visibility Binding")]
public class NguiVisibilityBinding : NguiBooleanBinding, IVisibilityBinding
{
	private NguiVisibilityControl _nvc = new NguiVisibilityControl();
	public bool Visible { get { return _nvc.Visible; } }
	public bool ObjectVisible { get { return _nvc.ObjectVisible; } }
	public bool ComponentVisible { get { return _nvc.ComponentVisible; } }
	public void InvalidateParent() { _nvc.InvalidateParent(); }
	public override void Awake()
	{
		base.Awake();
		_nvc.Awake(gameObject);
	}
	
	protected override void ApplyNewValue(bool newValue)
	{
		_nvc.ApplyNewValue(newValue);
	}
}
