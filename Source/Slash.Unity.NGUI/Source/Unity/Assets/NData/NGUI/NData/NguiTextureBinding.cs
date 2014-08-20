using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
[AddComponentMenu("NGUI/NData/Texture Binding")]
public class NguiTextureBinding : NguiBinding
{
	private EZData.Property<Texture2D> _texture;
	private UITexture _uiTexture;
	
	private float width;
	private float height;
	
	public enum ALIGNMENT
	{
		STRETCH,
		UNIFORM_STRETCH,
		SOURCE_SIZE,
		NONE
	}
	
	public ALIGNMENT alignment = ALIGNMENT.UNIFORM_STRETCH;
	public bool stretchOutside = false;
	
	public override void Awake()
	{
		base.Awake();
		
		_uiTexture = gameObject.GetComponent<UITexture>();
#if NGUI_2
		width = transform.localScale.x;
		height = transform.localScale.y;
#else
		width = _uiTexture.width;
		height = _uiTexture.height;
#endif
	}
	
	protected override void Unbind()
	{
		base.Unbind();
		
		if (_texture != null)
		{
			_texture.OnChange -= OnChange;
			_texture = null;
		}
	}
	
	protected override void Bind()
	{
		base.Bind();
		
		var context = GetContext(Path);
		if (context == null)
		{
			Debug.LogWarning("NguiTexture.UpdateBinding - context is null");
			return;
		}
		
		_texture = context.FindProperty<Texture2D>(Path, this);
		
		if (_texture != null)
		{
			_texture.OnChange += OnChange;
		}
	}
	
	protected override void OnChange()
	{
		base.OnChange();
		
		var aspect = (height == 0) ? 1 : (width / height);
		
		var imageWidth = width;
		var imageHeight = height;
		
		if (_texture != null && _texture.GetValue() != null)
		{
			imageWidth = _texture.GetValue().width;
			imageHeight = _texture.GetValue().height;
		}
		
		var imageAspect = (imageHeight == 0) ? 1 : (imageWidth / imageHeight);
		
		var spriteWidth = 0.0f;
		var spriteHeight = 0.0f;
		
		if (_texture != null && _texture.GetValue() != null)
		{
			switch(alignment)
			{
			case ALIGNMENT.STRETCH:
				spriteWidth = width;
				spriteHeight = height;
				break;
			case ALIGNMENT.UNIFORM_STRETCH:
				if ((aspect > imageAspect) ^ stretchOutside)
				{
					spriteHeight = height;
					spriteWidth = (imageHeight == 0) ? 0 : (imageWidth * spriteHeight / imageHeight);
				}
				else
				{
					spriteWidth = width;
					spriteHeight = (imageWidth == 0) ? 0 : (imageHeight * spriteWidth / imageWidth);
				}
				break;
			case ALIGNMENT.SOURCE_SIZE:
				spriteWidth = imageWidth;
				spriteHeight = imageHeight;
				break;
			}
		}
		
		spriteWidth = Mathf.Max(spriteWidth, 0.001f);
		spriteHeight = Mathf.Max(spriteHeight, 0.001f);
		
		if (_uiTexture != null)
		{
			if (_texture != null)
			{
#if NGUI_2
				_uiTexture.material.mainTexture = _texture.GetValue();
#else
				_uiTexture.mainTexture = _texture.GetValue();
#endif
			}
			if (alignment != ALIGNMENT.NONE)
			{
#if NGUI_2
				_uiTexture.transform.localScale = new Vector3(spriteWidth, spriteHeight, 1);
#else
				_uiTexture.width = (int)spriteWidth;
				_uiTexture.height = (int)spriteHeight;
#endif
			}
		}
	}
}
