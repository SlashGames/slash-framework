using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
[AddComponentMenu("NGUI/NData/OnDrop Binding")]
public class NguiOnDropBinding : NguiCommandBinding
{
	void OnDrop()
	{
		if (_command == null)
		{
			return;
		}
		
		_command.DynamicInvoke();
	}
}
