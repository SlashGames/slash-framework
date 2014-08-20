using UnityEngine;

public class TransformVariantContext : EZData.Context
{
	#region Property UpsideDown
	private readonly EZData.Property<bool> _privateUpsideDownProperty = new EZData.Property<bool>();
	public EZData.Property<bool> UpsideDownProperty { get { return _privateUpsideDownProperty; } }
	public bool UpsideDown
	{
	get    { return UpsideDownProperty.GetValue();    }
	set    { UpsideDownProperty.SetValue(value); }
	}
	#endregion
}

public class TransformVariant : MonoBehaviour
{
	public NguiRootContext View;
	public TransformVariantContext Context;
	
	void Awake()
	{
		Context = new TransformVariantContext();
		View.SetContext(Context);
	}
}
