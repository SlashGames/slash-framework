using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class NguiCommandBinding : NguiBinding
{
	protected System.Delegate _command;
	
	protected override void Unbind()
	{
		base.Unbind();
		
		_command = null;
	}
	
	protected override void Bind()
	{
		base.Bind();
		
		var context = GetContext(Path);
		if (context == null)
			return;
		
		_command = context.FindCommand(Path, this);
	}
}
