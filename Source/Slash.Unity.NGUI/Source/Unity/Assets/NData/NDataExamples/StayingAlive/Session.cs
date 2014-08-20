using UnityEngine;

namespace StayingAliveContexts
{
	public class Session : EZData.Context
	{
		#region Property TimeLimit
		private readonly EZData.Property<float> _privateTimeLimitProperty = new EZData.Property<float>();
		public EZData.Property<float> TimeLimitProperty { get { return _privateTimeLimitProperty; } }
		public float TimeLimit
		{
		get    { return TimeLimitProperty.GetValue();    }
		set    { TimeLimitProperty.SetValue(value); }
		}
		#endregion
		
		#region Property UpTime
		private readonly EZData.Property<float> _privateUpTimeProperty = new EZData.Property<float>();
		public EZData.Property<float> UpTimeProperty { get { return _privateUpTimeProperty; } }
		public float UpTime
		{
		get    { return UpTimeProperty.GetValue();    }
		set    { UpTimeProperty.SetValue(value); }
		}
		#endregion
		
		#region Property Score
		private readonly EZData.Property<float> _privateScoreProperty = new EZData.Property<float>();
		public EZData.Property<float> ScoreProperty { get { return _privateScoreProperty; } }
		public float Score
		{
		get    { return ScoreProperty.GetValue();    }
		set    { ScoreProperty.SetValue(value); }
		}
		#endregion

		private Balance _balance;
		
		public void Update()
		{
			TimeLimit -= Time.deltaTime;
			UpTime += Time.deltaTime;
			Score += Time.deltaTime * _balance.scorePerSecond;
		}
		
		public void StayAlive()
		{
			Score += _balance.scorePerClick;
			if (TimeLimit > 0.001f)
			{
				var s = 2 * Mathf.Atan(_balance.edgeDriftSpeed * UpTime) / Mathf.PI;
				var bonusTime = Mathf.Lerp(_balance.initialEdge, 0, s) / TimeLimit;
				TimeLimit += Mathf.Min(bonusTime, _balance.maxBonusTime);
			}
		}
		
		public bool IsOver()
		{
			return TimeLimit <= 0.0f;
		}
		
		public Session(Balance balance)
		{
			_balance = balance;
			TimeLimit = _balance.initialTimeLimit;
		}
	}
}
