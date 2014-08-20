using UnityEngine;
using ProjectedPositionContexts;

namespace ProjectedPositionContexts
{
	public class Unit : EZData.Context
	{
		#region Property Name
		private readonly EZData.Property<string> _privateNameProperty = new EZData.Property<string>();
		public EZData.Property<string> NameProperty { get { return _privateNameProperty; } }
		public string Name
		{
		get    { return NameProperty.GetValue();    }
		set    { NameProperty.SetValue(value); }
		}
		#endregion
		
		#region Property Target
		private readonly EZData.Property<GameObject> _privateTargetProperty = new EZData.Property<GameObject>();
		public EZData.Property<GameObject> TargetProperty { get { return _privateTargetProperty; } }
		public GameObject Target
		{
		get    { return TargetProperty.GetValue();    }
		set    { TargetProperty.SetValue(value); }
		}
		#endregion
	}
	
	public class Cloud : EZData.Context
	{
		#region Collection Units
		private readonly EZData.Collection<Unit> _privateUnits = new EZData.Collection<Unit>(false);
		public EZData.Collection<Unit> Units { get { return _privateUnits; } }
		#endregion
	}
}

public class ProjectedPosition : MonoBehaviour
{
	public NguiRootContext View;
	public GameObject UnitPrefab;
	public Cloud Context;
	
	void Awake()
	{
		Context = new Cloud();
		
		for (var i = 0; i < 5; ++i)
		{
			Context.Units.Add(new Unit
			{
				Target = (GameObject)GameObject.Instantiate(UnitPrefab),
				Name = "Unit " + i,
			});
		}
		
		View.SetContext(Context);
	}
}
