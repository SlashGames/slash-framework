using UnityEngine;

public class PopupColor : EZData.Context
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

	#region Property Color
	private readonly EZData.Property<Color> _privateColorProperty = new EZData.Property<Color>();
	public EZData.Property<Color> ColorProperty { get { return _privateColorProperty; } }
	public Color Color
	{
	get    { return ColorProperty.GetValue();    }
	set    { ColorProperty.SetValue(value); }
	}
	#endregion
}

public class ColorsPopupList : EZData.MonoBehaviourContext
{
	#region Collection Colors
	private readonly EZData.Collection<PopupColor> _privateColors = new EZData.Collection<PopupColor>(true);
	public EZData.Collection<PopupColor> Colors { get { return _privateColors; } }
	#endregion
	
	void Awake()
	{
		Colors.Add(new PopupColor { Name = "White", Color = Color.white });
		Colors.Add(new PopupColor { Name = "Red", Color = Color.red });
		Colors.Add(new PopupColor { Name = "Green", Color = Color.green });
		Colors.Add(new PopupColor { Name = "Blue", Color = Color.blue });
		Colors.Add(new PopupColor { Name = "Yellow", Color = Color.yellow });
	}
}
