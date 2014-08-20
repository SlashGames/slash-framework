using UnityEngine;

public class ScaleSliderContext : EZData.Context
{
	#region Property Scale
	private readonly EZData.Property<float> _privateScaleProperty = new EZData.Property<float>(1);
	public EZData.Property<float> ScaleProperty { get { return _privateScaleProperty; } }
	public float Scale
	{
	get    { return ScaleProperty.GetValue();    }
	set    { ScaleProperty.SetValue(value); }
	}
	#endregion
}

public class ScaleSlider : MonoBehaviour
{
	public NguiRootContext View;
	public ScaleSliderContext Context;
	
	void Awake()
	{
		Context = new ScaleSliderContext();
		View.SetContext(Context);
	}
}
