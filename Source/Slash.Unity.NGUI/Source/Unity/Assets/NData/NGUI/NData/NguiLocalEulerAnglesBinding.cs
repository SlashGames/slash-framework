using UnityEngine;

[System.Serializable]
[AddComponentMenu("NGUI/NData/Local Euler Angles Binding")]
public class NguiLocalEulerAnglesBinding : NguiPollingVector3Binding
{
	protected override Vector3 GetValue()
	{
		return gameObject.transform.localEulerAngles;
	}
	
	protected override void SetValue(Vector3 value)
	{
		gameObject.transform.localEulerAngles = value;
	}
}
