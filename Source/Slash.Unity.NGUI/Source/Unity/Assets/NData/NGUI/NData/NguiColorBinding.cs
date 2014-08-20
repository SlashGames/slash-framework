using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
[AddComponentMenu("NGUI/NData/Color Binding")]
public class NguiColorBinding : NguiBinding
{
	private bool _ignoreChanges = false;
	
	private EZData.Property<string> _hexProperty = null;
	private EZData.Property<Color> _colorProperty = null;
	
	private UIWidget _widget;
	
    private static int ToColorComponent(float c)
    {
        return (int) (Math.Max(0, Math.Min(c, 255)));
    }
	
	private static string ColorToHex(Color c)
	{
		if (c.a > 0.9999f)
    		return string.Format("[{0:x2}{1:x2}{2:x2}]", ToColorComponent(c.r), ToColorComponent(c.g), ToColorComponent(c.b));
		else
			return string.Format("[{0:x2}{1:x2}{2:x2}{3:x2}]", ToColorComponent(c.r), ToColorComponent(c.g), ToColorComponent(c.b), ToColorComponent(c.a));
	}
	
	private static int GetHexDigitValue(char c)
	{
		switch(c)
		{
		case '0' : return 0;
		case '1' : return 1;
		case '2' : return 2;
		case '3' : return 3;
		case '4' : return 4;
		case '5' : return 5;
		case '6' : return 6;
		case '7' : return 7;
		case '8' : return 8;
		case '9' : return 9;
		case 'a' : return 10;
		case 'A' : return 10;
		case 'b' : return 11;
		case 'B' : return 11;
		case 'c' : return 12;
		case 'C' : return 12;
		case 'd' : return 13;
		case 'D' : return 13;
		case 'e' : return 14;
		case 'E' : return 14;
		case 'f' : return 15;
		case 'F' : return 15;
		}
		return 0;
	}
	
	private static int GetHexByteStringValue(string h)
	{
		var c0 = (h.Length > 0) ? h[0] : '0';
		var c1 = (h.Length > 1) ? h[1] : '0';
		return GetHexDigitValue(c0) * 16 + GetHexDigitValue(c1);
	}
	
	private static bool HexToColor(string h, out Color c)
	{
		c = Color.white;
		
		if (h == null)
			h = "0";
		if (h.Length > 0 && h[0] == '#')
			h = h.Substring(1);
		if (h.Length > 1 && h[0] == '[' && h[h.Length - 1] == ']')
			h = h.Substring(1, h.Length - 2);
		if (h.Length > 8)
			h = h.Substring(0, 8);
		while(h.Length < 6)
			h += "0";
		if (h.Length > 6)
			while(h.Length < 8)
				h += "0";
        
#if UNITY_FLASH
		var b0 = 0;
        var b1 = 0;
        var b2 = 0;
        var b3 = 0;
		
		if (h.Length == 6)
		{
			b1 = GetHexByteStringValue(h.Substring(0));
			b2 = GetHexByteStringValue(h.Substring(2));
			b3 = GetHexByteStringValue(h.Substring(4));
		}
		else if (h.Length == 8)
		{
			b0 = GetHexByteStringValue(h.Substring(0));
			b1 = GetHexByteStringValue(h.Substring(2));
			b2 = GetHexByteStringValue(h.Substring(4));
			b3 = GetHexByteStringValue(h.Substring(6));
		}
#else
		int color = 0;
		if (!int.TryParse(h, System.Globalization.NumberStyles.HexNumber, null, out color))
            return false;
		var b0 = (color >> 24) & 0xff;
        var b1 = (color >> 16) & 0xff;
        var b2 = (color >> 8) & 0xff;
        var b3 = (color) & 0xff;
#endif   
		
		
		if (h.Length == 6)
			c = new Color(b1 / 255.0f, b2 / 255.0f, b3 / 255.0f, 1.0f);
		else
			c = new Color(b0 / 255.0f, b1 / 255.0f, b2 / 255.0f, b3 / 255.0f);
		return true;
    }
    
	public override void Awake()
	{
		base.Awake();
		_widget = GetComponent<UIWidget>();
	}
		
	protected override void Unbind()
	{
		if (_hexProperty != null)
		{
			_hexProperty.OnChange -= OnChange;
			_hexProperty = null;
		}
		if (_colorProperty != null)
		{
			_colorProperty.OnChange -= OnChange;
			_colorProperty = null;
		}
	}
	
	protected override void Bind()
	{
		var context = GetContext(Path);
		if (context == null)
		{
			Debug.LogWarning("NguiColorBinding.UpdateBinding - context is null");
			return;
		}
		
		_colorProperty = context.FindProperty<Color>(Path, this);
		if (_colorProperty == null)
			_hexProperty = context.FindProperty<string>(Path, this);
		
		if (_colorProperty != null)
			_colorProperty.OnChange += OnChange;
		if (_hexProperty != null)
			_hexProperty.OnChange += OnChange;

		OnChange();
	}
	
	public void OnUIColorChange(Color color)
	{
		_ignoreChanges = true;
		if (_colorProperty != null)
			_colorProperty.SetValue(color);
		if (_hexProperty != null)
			_hexProperty.SetValue(ColorToHex(color));
		_ignoreChanges = false;
	}
	
	protected override void OnChange()
	{
		if (_ignoreChanges)
			return;
		
		Color newColor = Color.white;
		if (_colorProperty != null)
			newColor = _colorProperty.GetValue();
		else if (_hexProperty != null && !HexToColor(_hexProperty.GetValue(), out newColor))
			return;
		
		if (_widget != null)
			_widget.color = newColor;
	}
}
