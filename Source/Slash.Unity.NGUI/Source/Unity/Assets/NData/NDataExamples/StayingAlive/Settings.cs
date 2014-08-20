using UnityEngine;

namespace StayingAliveContexts
{
	[System.Serializable]
	public class Balance
	{
		public int scorePerSecond = 10;
		public int scorePerClick = 1;
		public float initialTimeLimit = 10.0f;
		public float initialEdge = 2.0f;
		public float edgeDriftSpeed = 0.02f;
		public float maxBonusTime = 60.0f;
	}
	
	public class Settings : EZData.Context
	{
		#region PersistentProperty MusicVolume
		private readonly EZData.PersistentProperty<float> _privateMusicVolumeProperty =
			new EZData.PersistentProperty<float>("MusicVolume", 0.5f);
		public EZData.PersistentProperty<float> MusicVolumeProperty { get { return _privateMusicVolumeProperty; } }
		public float MusicVolume
		{
		get    { return MusicVolumeProperty.GetValue();    }
		set    { MusicVolumeProperty.SetValue(value); }
		}
		#endregion
		
		public Balance Balance { get; set; }
	}
}
