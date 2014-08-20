using UnityEngine;

public class VisibilityCheckboxesContext : EZData.Context
{
	#region Property All
	private readonly EZData.Property<bool> _privateAllProperty = new EZData.Property<bool>();
	public EZData.Property<bool> AllProperty { get { return _privateAllProperty; } }
	public bool All
	{
	get    { return AllProperty.GetValue();    }
	set    { AllProperty.SetValue(value); }
	}
	#endregion
	
	#region Property Red
	private readonly EZData.Property<bool> _privateRedProperty = new EZData.Property<bool>();
	public EZData.Property<bool> RedProperty { get { return _privateRedProperty; } }
	public bool Red
	{
	get    { return RedProperty.GetValue();    }
	set    { RedProperty.SetValue(value); }
	}
	#endregion
	
	#region Property Green
	private readonly EZData.Property<bool> _privateGreenProperty = new EZData.Property<bool>();
	public EZData.Property<bool> GreenProperty { get { return _privateGreenProperty; } }
	public bool Green
	{
	get    { return GreenProperty.GetValue();    }
	set    { GreenProperty.SetValue(value); }
	}
	#endregion
	
	#region Property Blue
	private readonly EZData.Property<bool> _privateBlueProperty = new EZData.Property<bool>();
	public EZData.Property<bool> BlueProperty { get { return _privateBlueProperty; } }
	public bool Blue
	{
	get    { return BlueProperty.GetValue();    }
	set    { BlueProperty.SetValue(value); }
	}
	#endregion
}

public class VisibilityCheckboxes : MonoBehaviour
{
	public NguiRootContext View;
	public VisibilityCheckboxesContext Context;
	
	void Awake()
	{
		Context = new VisibilityCheckboxesContext();
		View.SetContext(Context);
	}
}
