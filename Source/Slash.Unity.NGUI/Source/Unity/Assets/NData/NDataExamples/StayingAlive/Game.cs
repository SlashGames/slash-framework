using UnityEngine;

namespace StayingAliveContexts
{
	public enum GameState
	{
		MainMenu = 0,
		Settings = 1,
		Game = 2,
		Paused = 3,
		GameOver = 4,
	}
	
	public class Game : EZData.Context
	{
		#region Property State
		private readonly EZData.Property<int> _privateStateProperty = new EZData.Property<int>();
		public EZData.Property<int> StateProperty { get { return _privateStateProperty; } }
		public GameState State
		{
		get    { return (GameState)StateProperty.GetValue();    }
		set    { StateProperty.SetValue((int)value); }
		}
		#endregion
		
		#region Property QuitRequested
		private readonly EZData.Property<bool> _privateQuitRequestedProperty = new EZData.Property<bool>();
		public EZData.Property<bool> QuitRequestedProperty { get { return _privateQuitRequestedProperty; } }
		public bool QuitRequested
		{
		get    { return QuitRequestedProperty.GetValue();    }
		set    { QuitRequestedProperty.SetValue(value); }
		}
		#endregion
		
		public void ConfirmQuit()
		{
			Application.Quit();
		}
		
		public void CancelQuit()
		{
			QuitRequested = false;
		}
		
		public void RequestQuit()
		{
			if (QuitRequested)
				ConfirmQuit();
			QuitRequested = true;
		}
		
		#region Session
		public readonly EZData.VariableContext<Session> SessionEzVariableContext = new EZData.VariableContext<Session>(null);
		public Session Session
		{
		    get { return SessionEzVariableContext.Value; }
		    set { SessionEzVariableContext.Value = value; }
		}
		#endregion
		
		public Settings Settings { get; private set; }
		
		public void GoToMainMenu()
		{
			State = GameState.MainMenu;
		}
		
		public void GoToSettings()
		{
			State = GameState.Settings;
		}
		
		public void Pause()
		{
			if (State == GameState.Game)
				State = GameState.Paused;
		}
		
		public void Unpause()
		{
			if (State == GameState.Paused)
				State = GameState.Game;
		}
		
		public void StartGame()
		{
			Session = new Session(Settings.Balance);
			Session.TimeLimitProperty.OnChange += () =>
			{
				if (State == GameState.Game && Session.IsOver())
					State = GameState.GameOver;
			};
			State = GameState.Game;
		}
		
		public Game(Balance balance)
		{
			Settings = new Settings()
			{
				Balance = balance,
			};
			StateProperty.OnChange += () =>
			{
				Time.timeScale = (State == GameState.Game) ? 1 : 0;
			};
		}
	}
}
