using System;

namespace EZData
{
	public class CommandContext : IBindingPathTarget
	{
		private Command _command;
		
		public Command GetValue()
		{
			return _command;
		}
		
		public CommandContext(Command command)
		{
			_command = command;
		}
	}
}

