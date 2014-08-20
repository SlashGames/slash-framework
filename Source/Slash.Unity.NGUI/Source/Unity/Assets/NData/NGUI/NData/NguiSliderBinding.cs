using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
[AddComponentMenu("NGUI/NData/Slider Binding")]
public class NguiSliderBinding : NguiPollingCustomBoundsNumericBinding
{
	private UISlider _UiSliderReceiver;
	
	public override void Awake()
	{
		base.Awake();
		_UiSliderReceiver = gameObject.GetComponent<UISlider>();
	}
	
	protected override double GetValue()
	{
		return (_UiSliderReceiver != null) ?
#if NGUI_2
			_UiSliderReceiver.sliderValue : 0;
#else
			_UiSliderReceiver.value : 0;
#endif
	}
	
	protected override void SetValue(double val)
	{
		if (_UiSliderReceiver != null)
		{
#if NGUI_2
			_UiSliderReceiver.sliderValue = (float)val;
#else
			_UiSliderReceiver.value = (float)val;
#endif
		}
	}
}
