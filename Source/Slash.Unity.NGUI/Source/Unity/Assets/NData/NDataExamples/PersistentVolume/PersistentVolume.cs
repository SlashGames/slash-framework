using UnityEngine;

public class PersistentVolumeContext : EZData.Context
{
	#region PersistentProperty MusicVolume
	private readonly EZData.PersistentProperty<float> _privateMusicVolumeProperty =
		new EZData.PersistentProperty<float>("MusicVolumePref", 0.2f);
	public EZData.PersistentProperty<float> MusicVolumeProperty
	{
		get { return _privateMusicVolumeProperty; }
	}
	public float MusicVolume
	{
		get { return MusicVolumeProperty.GetValue(); }
		set { MusicVolumeProperty.SetValue(value); }
	}
	#endregion
}

public class PersistentVolume : MonoBehaviour
{
	public NguiRootContext View;
	public PersistentVolumeContext Context;
	
	void Awake()
	{
		Context = new PersistentVolumeContext();
		View.SetContext(Context);
	}
}
