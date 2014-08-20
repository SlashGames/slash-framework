using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
[AddComponentMenu("NGUI/NData/Advanced/Numeric Comparison Visibility Binding")]
public class NguiNumericComparisonVisibilityBinding : NguiNumericMultiBinding, IVisibilityBinding
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
	
	public enum Operation
	{
		Less,
		LessEqual,
		Equal,
		GreaterEqual,
		Greater,
	}
	
	public Operation operation;
	
	protected override void ApplyNewValue (List<double> v)
	{
		if (v.Count >= 2)
		{
			var val = false;
			switch(operation)
			{
			case Operation.Less:
				val = v[0] < v[1];
				break;
			case Operation.LessEqual:
				val = v[0] <= v[1];
				break;
			case Operation.Equal:
				val = System.Math.Abs(v[0] - v[1]) < 0.0001;
				break;
			case Operation.GreaterEqual:
				val = v[0] >= v[1];
				break;
			case Operation.Greater:
				val = v[0] > v[1];
				break;
			}
			_nvc.ApplyNewValue(val);
		}
	}
}
