using UnityEngine;
using MasterPathContexts;

namespace MasterPathContexts
{
	public class Preferences : EZData.Context
	{
		#region Property FillPercent
		private readonly EZData.Property<int> _privateFillPercentProperty = new EZData.Property<int>(75);
		public EZData.Property<int> FillPercentProperty { get { return _privateFillPercentProperty; } }
		public int FillPercent
		{
		get    { return FillPercentProperty.GetValue();    }
		set    { FillPercentProperty.SetValue(value); }
		}
		#endregion

		#region Property SwitchState
		private readonly EZData.Property<bool> _privateSwitchStateProperty = new EZData.Property<bool>(true);
		public EZData.Property<bool> SwitchStateProperty { get { return _privateSwitchStateProperty; } }
		public bool SwitchState
		{
		get    { return SwitchStateProperty.GetValue();    }
		set    { SwitchStateProperty.SetValue(value); }
		}
		#endregion
	}
	
	public class Options : EZData.Context
	{
		public Preferences Preferences { get; private set; }
		public Options() { Preferences = new Preferences(); }
	}
	
	public class Settings : EZData.Context
	{
		public Options Options { get; private set; }
		public Settings() { Options = new Options(); }
	}
	
	public class Game : EZData.Context
	{
		public Settings Settings { get; private set; }
		public Game() { Settings = new Settings(); }
	}
}

public class MasterPath : MonoBehaviour
{
	public NguiRootContext View;
	public Game Context;
	
	void Awake()
	{
		Context = new Game();
		View.SetContext(Context);
	}
}
