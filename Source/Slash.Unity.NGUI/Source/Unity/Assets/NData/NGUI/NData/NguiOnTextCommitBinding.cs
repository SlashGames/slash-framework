using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
[AddComponentMenu("NGUI/NData/OnTextCommit Binding")]
public class NguiOnTextCommitBinding : NguiCommandBinding
{
	void OnSubmit()
	{
		if (_command == null)
			return;
		
		_command.DynamicInvoke();
	}
}
