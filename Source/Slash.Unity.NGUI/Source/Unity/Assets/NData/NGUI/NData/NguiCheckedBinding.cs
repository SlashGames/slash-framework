using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
[AddComponentMenu("NGUI/NData/Checked Binding")]
public class NguiCheckedBinding : NguiBooleanBinding
{
#if NGUI_2
	private UICheckbox _checkBox;
	private bool _prevState;
	private bool _ignoreChanges;
	
	public override void Awake()
	{
		base.Awake();
		_checkBox = gameObject.GetComponent<UICheckbox>();
	}
	
	void Update()
	{
		if (_checkBox != null)
		{
			if (_prevState != _checkBox.isChecked)
			{
				_prevState = _checkBox.isChecked;
				_ignoreChanges = true;
				ApplyInputValue(_checkBox.isChecked);
				_ignoreChanges = false;
			}
		}
	}
		
	protected override void ApplyNewValue(bool newValue)
	{
		if (_ignoreChanges)
			return;
		
		if (_checkBox != null)
		{
			_checkBox.isChecked = newValue;
		}
	}
#else
	private UIToggle _toggle;
	private bool _prevState;
	private bool _ignoreChanges;
	
	public override void Awake()
	{
		base.Awake();
		_toggle = gameObject.GetComponent<UIToggle>();
	}
	
	void Update()
	{
		if (_toggle != null)
		{
			if (_prevState != _toggle.value)
			{
				_prevState = _toggle.value;
				_ignoreChanges = true;
				ApplyInputValue(_toggle.value);
				_ignoreChanges = false;
			}
		}
	}
		
	protected override void ApplyNewValue(bool newValue)
	{
		if (_ignoreChanges)
			return;
		
		if (_toggle != null)
		{
			_toggle.value = newValue;
		}
	}
#endif
}
