using UnityEngine;

[System.Serializable]
public class NguiLocalizationKeyBinding : NguiTextBinding
{
	private UILocalize _localize;
	
	public override void Awake()
	{
		base.Awake();
		
		_localize = GetComponent<UILocalize>();
	}
	
	protected override void ApplyNewValue (string newValue)
	{
		_localize.key = newValue;
        _localize.value = string.IsNullOrEmpty(newValue) ? newValue : Localization.Get(newValue);
	}
}
