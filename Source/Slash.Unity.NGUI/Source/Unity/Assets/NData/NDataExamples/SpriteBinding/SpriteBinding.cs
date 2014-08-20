using EZData;

class SpriteBinding : MonoBehaviourContext
{
	#region Property Name
	private readonly EZData.Property<string> _privateNameProperty = new EZData.Property<string>("MadCat");
	public EZData.Property<string> NameProperty { get { return _privateNameProperty; } }
	public string Name
	{
	get    { return NameProperty.GetValue();    }
	set    { NameProperty.SetValue(value); }
	}
	#endregion
}
