using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
[AddComponentMenu("NGUI/NData/Audible Binding")]
public class NguiAudibleBinding : NguiBooleanBinding
{
	private bool _audible;
	private float _volume;
	private AudioSource [] _audioSources;
	private EZData.Property<float> _masterVolumeProperty;
	
	public string MasterVolume;
	public float FadeSpeed = 1.0f;
	
	public override IList<string> ReferencedPaths
	{
		get
		{
			var paths = base.ReferencedPaths;
			paths.Add(MasterVolume);
			return paths;
		}
	}
	
	protected override void Unbind()
	{
		base.Unbind();
		
		if (_masterVolumeProperty != null)
			_masterVolumeProperty.OnChange -= ApplyVolume;
		_masterVolumeProperty = null;	
	}
	
	protected override void Bind()
	{
		var context = GetContext(MasterVolume);
		if (context != null)
			_masterVolumeProperty = context.FindProperty<float>(MasterVolume, this);
		
		if (_masterVolumeProperty != null)
			_masterVolumeProperty.OnChange += ApplyVolume;

		base.Bind();
	}
	
	private void ApplyVolume()
	{
		var v = _volume * ((_masterVolumeProperty == null) ? 1 : _masterVolumeProperty.GetValue());
		foreach(var s in _audioSources)
			s.volume = v;
	}
	
	protected override void ApplyNewValue(bool newValue)
	{
		_audible = newValue;
	}
	
	public override void Awake()
	{
		base.Awake();
		_audioSources = GetComponents<AudioSource>();
	}
	
	void Update()
	{
		if ((_audible && _volume < 1.0f) || (!_audible && _volume > 0.0f))
		{
			_volume = _audible ?
				Mathf.Min(1, _volume + Time.deltaTime * FadeSpeed) :
				Mathf.Max(0, _volume - Time.deltaTime * FadeSpeed);
			ApplyVolume();
		}
	}
}
