using UnityEngine;
using System;
using System.Collections;

public interface IWwwLoader
{
	void LoadTexture(string url, Action<Texture2D> callback);
}

public class TextureLoaderContext : EZData.Context
{
	#region Property ImageUrl
	private readonly EZData.Property<string> _privateImageUrlProperty = new EZData.Property<string>("http://forum.unity3d.com/customavatars/avatar48859_1.gif");
	public EZData.Property<string> ImageUrlProperty { get { return _privateImageUrlProperty; } }
	public string ImageUrl
	{
	get    { return ImageUrlProperty.GetValue();    }
	set    { ImageUrlProperty.SetValue(value); }
	}
	#endregion

	#region Property LoadedImage
	private readonly EZData.Property<Texture2D> _privateLoadedImageProperty = new EZData.Property<Texture2D>();
	public EZData.Property<Texture2D> LoadedImageProperty { get { return _privateLoadedImageProperty; } }
	public Texture2D LoadedImage
	{
	get    { return LoadedImageProperty.GetValue();    }
	set    { LoadedImageProperty.SetValue(value); }
	}
	#endregion
	
	private IWwwLoader _wwwLoader;
	public TextureLoaderContext(IWwwLoader wwwLoader)
	{
		_wwwLoader = wwwLoader;
	}
	
	public void LoadImage()
	{
		_wwwLoader.LoadTexture(ImageUrl, (texture) => LoadedImage = texture);
	}
}

public class TextureLoader : MonoBehaviour, IWwwLoader
{
	public NguiRootContext View;
	public TextureLoaderContext Context;
	
	private IEnumerator LoadTexture(WWW www, Action<Texture2D> callback)
	{
		yield return www;
		callback(string.IsNullOrEmpty(www.error) ? www.texture : null);
	}
	
	public void LoadTexture(string url, Action<Texture2D> callback)
	{
		StartCoroutine(LoadTexture(new WWW(url), callback));
	}
	
	void Awake()
	{
		Context = new TextureLoaderContext(this);
		View.SetContext(Context);
	}
}
