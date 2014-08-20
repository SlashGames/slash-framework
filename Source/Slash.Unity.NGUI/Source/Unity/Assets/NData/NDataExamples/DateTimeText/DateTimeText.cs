using UnityEngine;

public class DateTimeTextContext : EZData.Context
{
	#region Property DateTimeText
	private readonly EZData.Property<string> _privateDateTimeTextProperty = new EZData.Property<string>();
	public EZData.Property<string> DateTimeTextProperty { get { return _privateDateTimeTextProperty; } }
	public string DateTimeText
	{
	get    { return DateTimeTextProperty.GetValue();    }
	set    { DateTimeTextProperty.SetValue(value); }
	}
	#endregion

	public void Change()
	{
		DateTimeText = System.DateTime.Now.ToString();
	}
}

public class DateTimeText : MonoBehaviour
{
	public NguiRootContext View;
	public DateTimeTextContext Context;
	
	void Awake()
	{
		Context = new DateTimeTextContext();
		View.SetContext(Context);
	}
}
