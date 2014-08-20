using UnityEngine;

[System.Serializable]
[AddComponentMenu("NGUI/NData/Local Position Binding")]
public class NguiLocalPositionBinding : NguiPollingVector3Binding
{
	public Vector3 mul = Vector3.one;
	public Vector3 add = Vector3.zero;
		
	protected override Vector3 GetValue()
	{
		var v = gameObject.transform.localPosition;
		v.x -= add.x;
		v.y -= add.y;
		v.z -= add.z;
		if (Mathf.Abs(mul.x) > float.Epsilon)
			v.x /= mul.x;
		if (Mathf.Abs(mul.y) > float.Epsilon)
			v.y /= mul.y;
		if (Mathf.Abs(mul.z) > float.Epsilon)
			v.z /= mul.z;
		return v;
	}
	
	protected override void SetValue(Vector3 value)
	{
		gameObject.transform.localPosition = new Vector3(
				value.x * mul.x + add.x,
				value.y * mul.y + add.y,
				value.z * mul.z + add.z);
	}
}
