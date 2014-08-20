using UnityEngine;

[System.Serializable]
public abstract class NguiPollingVector3Binding : NguiVector3Binding
{
	public NguiBindingDirection Direction = NguiBindingDirection.TwoWay;
	public NguiBindingInitialValue InitialValue = NguiBindingInitialValue.TakeFromModel;
	
	private Vector3 _prevValue;
	private bool _inited;
	
	public override void Start()
	{
		base.Start();
		
		if (InitialValue == NguiBindingInitialValue.TakeFromView)
		{
			_inited = true;
			_prevValue = GetValue();
			SetVector3Value(_prevValue);
		}
	}
	
	void Update()
	{
		if (!Direction.NeedsViewTracking())
			return;
		
		var newScale = GetValue();
		if (_prevValue != newScale)
		{
			_prevValue = newScale;
			SetVector3Value(newScale);
		}
	}
	
	protected sealed override void ApplyNewValue(Vector3 val)
	{
		if (!_inited && InitialValue == NguiBindingInitialValue.TakeFromView)
			return;
		
		_inited = true;
		if (Direction.NeedsViewUpdate())
		{
			_prevValue = val;
			SetValue(val);
		}
	}
	
	protected abstract Vector3 GetValue();
	protected abstract void SetValue(Vector3 val);
}