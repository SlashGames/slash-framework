using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
[AddComponentMenu("NGUI/NData/OnAutoComplete Binding")]
public class NguiOnAutoCompleteBinding : NguiCommandBinding
{
	public float interval = 1f;
	public bool suggestForEmptyInput = false;
	
	private UIInput _uiInput;
	private float _time;
	private string _prevFrameInputText = "";
	
	public override void Awake()
	{
		base.Awake();
		_uiInput = gameObject.GetComponent<UIInput>();
	}
		
	void Update()
	{
		if (_uiInput != null)
		{
#if NGUI_2
			var text = _uiInput.text;
#else
			var text = _uiInput.value;
#endif
			if (text != _prevFrameInputText)
			{
				_prevFrameInputText = text;
				
				if (_command != null &&
					(!string.IsNullOrEmpty(text) || suggestForEmptyInput))
				{
					_time = interval;
				}
			}
		}
		if (_time > 0f)
		{
			_time -= Time.deltaTime;
			if (_time < 0f)
			{
				_time = 0f;
				_command.DynamicInvoke();
			}
		}
	}
}
