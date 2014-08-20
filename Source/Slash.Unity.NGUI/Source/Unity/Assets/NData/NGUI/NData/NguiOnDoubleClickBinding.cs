using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
[AddComponentMenu("NGUI/NData/OnDoubleClick Binding")]
public class NguiOnDoubleClickBinding : NguiCommandBinding
{
	private float _tappedTime = 0;
	
	public float duration = 400.0f;
	
	public void OnClick()
	{
		var diff = (Time.realtimeSinceStartup - _tappedTime) * 1000;
		if (diff < duration && _command != null)
		{
			_command.DynamicInvoke();
		}
		
		_tappedTime = Time.realtimeSinceStartup;
	}	
}
