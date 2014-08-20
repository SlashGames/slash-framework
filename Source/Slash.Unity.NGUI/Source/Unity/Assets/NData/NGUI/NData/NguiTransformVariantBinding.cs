using UnityEngine;
using System.Collections;

[System.Serializable]
[AddComponentMenu("NGUI/NData/Transform Variant Binding")]
public class NguiTransformVariantBinding : NguiBooleanBinding
{
	public enum Target
	{
		Px,
		Py,
		Pz,
		Rx,
		Ry,
		Rz,
		Sx,
		Sy,
		Sz,
	}
	
	public Target target;
	public float falseValue;
	public float trueValue;
	
	protected override void ApplyNewValue (bool newValue)
	{
		var v = newValue ? trueValue : falseValue;
		var newPosition = transform.localPosition;
		var newEulerAngles = transform.localEulerAngles;
		var newScale = transform.localScale;
		switch(target)
		{
		case Target.Px:		newPosition.x = v;	break;
		case Target.Py:		newPosition.y = v;	break;
		case Target.Pz:		newPosition.z = v;	break;
		case Target.Rx:		newEulerAngles.x = v;	break;
		case Target.Ry:		newEulerAngles.y = v;	break;
		case Target.Rz:		newEulerAngles.z = v;	break;
		case Target.Sx:		newScale.x = v;	break;
		case Target.Sy:		newScale.y = v;	break;
		case Target.Sz:		newScale.z = v;	break;
		}
		transform.localPosition = newPosition;
		transform.localEulerAngles = newEulerAngles;
		transform.localScale = newScale;
	}
}
