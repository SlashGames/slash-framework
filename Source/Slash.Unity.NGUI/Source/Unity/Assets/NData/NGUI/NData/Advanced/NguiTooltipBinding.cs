using UnityEngine;
using System.Collections;

[System.Serializable]
[AddComponentMenu("NGUI/NData/Tooltip Binding")]
public class NguiTooltipBinding : NguiTextBinding
{
	private string _tooltipText;
	
	void OnTooltip (bool show)
	{
		if (!show)
		{
			UITooltip.ShowText(null);
			return;
		}
		
		if (string.IsNullOrEmpty(_tooltipText))
			return;
		
		UITooltip.ShowText(_tooltipText);
	}
	
	protected override void ApplyNewValue (string newValue)
	{
		_tooltipText = newValue;
	}
}
