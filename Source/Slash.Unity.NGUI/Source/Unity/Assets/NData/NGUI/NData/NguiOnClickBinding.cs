using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
[AddComponentMenu("NGUI/NData/OnClick Binding")]
public class NguiOnClickBinding : NguiCommandBinding
{
	void OnClick()
	{
		if (_command == null)
		{
			return;
		}
		
		_command.DynamicInvoke();
	}
}
