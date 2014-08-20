using UnityEngine;
using HealthBarContexts;

namespace HealthBarContexts
{
	public class Unit : EZData.Context
	{
		#region Property Health
		private readonly EZData.Property<int> _privateHealthProperty = new EZData.Property<int>();
		public EZData.Property<int> HealthProperty { get { return _privateHealthProperty; } }
		public int Health
		{
		get    { return HealthProperty.GetValue();    }
		set    { HealthProperty.SetValue(value); }
		}
		#endregion
		
		#region Property MaxHealth
		private readonly EZData.Property<int> _privateMaxHealthProperty = new EZData.Property<int>();
		public EZData.Property<int> MaxHealthProperty { get { return _privateMaxHealthProperty; } }
		public int MaxHealth
		{
		get    { return MaxHealthProperty.GetValue();    }
		set    { MaxHealthProperty.SetValue(value); }
		}
		#endregion
		
		public void Damage()
		{
			Health = Health - 1;
		}
	}
}

public class HealthBar : MonoBehaviour
{
	public NguiRootContext View;
	public Unit Context;
	
	void Awake()
	{
		Context = new Unit { MaxHealth = 120, Health = 120 };
		View.SetContext(Context);
	}
}
