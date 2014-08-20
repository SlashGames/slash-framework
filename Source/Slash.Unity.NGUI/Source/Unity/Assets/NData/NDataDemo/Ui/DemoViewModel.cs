using UnityEngine;
using System.Collections;
using System;

public class HighScoreTableItem : EZData.Context
{
	
	#region Property Portrait
	private readonly EZData.Property<Texture2D> _privatePortraitProperty =
		new EZData.Property<Texture2D>();
	public EZData.Property<Texture2D> PortraitProperty 
		{ get { return _privatePortraitProperty; } }
	public Texture2D Portrait
	{
		get	{ return PortraitProperty.GetValue();	}
		set	{ PortraitProperty.SetValue(value); }
	}
	#endregion
	
		
	#region Property Name
	private readonly EZData.Property<string> _privateNameProperty = new EZData.Property<string>();
	public EZData.Property<string> NameProperty { get { return _privateNameProperty; } }
	public string Name
	{
		get	{ return NameProperty.GetValue();	}
		set	{ NameProperty.SetValue(value); }
	}
	#endregion
	
	
	#region Property Score
	private readonly EZData.Property<int> _privateScoreProperty = new EZData.Property<int>();
	public EZData.Property<int> ScoreProperty { get { return _privateScoreProperty; } }
	public int Score
	{
		get	{ return ScoreProperty.GetValue();	}
		set	{ ScoreProperty.SetValue(value); }
	}
	#endregion
	
	
	#region Property Time
	private readonly EZData.Property<DateTime> _privateTimeProperty = new EZData.Property<DateTime>();
	public EZData.Property<DateTime> TimeProperty { get { return _privateTimeProperty; } }
	public DateTime Time
	{
		get	{ return TimeProperty.GetValue();	}
		set	{ TimeProperty.SetValue(value); }
	}
	#endregion
}

public class HighScoresScreen : EZData.Context
{
	
	
	#region Property PlayerName
	private readonly EZData.Property<string> _privatePlayerNameProperty = new EZData.Property<string>();
	public EZData.Property<string> PlayerNameProperty { get { return _privatePlayerNameProperty; } }
	public string PlayerName
	{
		get	{ return PlayerNameProperty.GetValue();	}
		set	{ PlayerNameProperty.SetValue(value); }
	}
	#endregion
	
	
	#region Property Score
	private readonly EZData.Property<int> _privateScoreProperty =
		new EZData.Property<int>();
	public EZData.Property<int> ScoreProperty
		{ get { return _privateScoreProperty; } }
	public int Score
	{
		get	{ return ScoreProperty.GetValue();	}
		set	{ ScoreProperty.SetValue(value); }
	}
	#endregion
	
	
	
	
	
	
	#region Property SubmitInProgress
	private readonly EZData.Property<bool> _privateSubmitInProgressProperty = new EZData.Property<bool>();
	public EZData.Property<bool> SubmitInProgressProperty { get { return _privateSubmitInProgressProperty; } }
	public bool SubmitInProgress
	{
		get	{ return SubmitInProgressProperty.GetValue();	}
		set	{ SubmitInProgressProperty.SetValue(value); }
	}
	#endregion
	

	#region Property SubmitCompleted
	private readonly EZData.Property<bool> _privateSubmitCompletedProperty = new EZData.Property<bool>();
	public EZData.Property<bool> SubmitCompletedProperty { get { return _privateSubmitCompletedProperty; } }
	public bool SubmitCompleted
	{
		get	{ return SubmitCompletedProperty.GetValue();	}
		set	{ SubmitCompletedProperty.SetValue(value); }
	}
	#endregion
	
	
	#region Collection Table
	private readonly EZData.Collection<HighScoreTableItem> _privateTable =
		new EZData.Collection<HighScoreTableItem>(false);
	public EZData.Collection<HighScoreTableItem> Table { get { return _privateTable; } }
	#endregion
	
	
	
	
	
	public void Submit()
	{
		_root.Model.SubmitScore(PlayerName, Score);
	}
	
	
	private Root _root;
	public HighScoresScreen(Root root)
	{
		_root = root;
		
		var r = new System.Random();
		Score = (int)(r.NextDouble() * 800 + 1);
	}
}

public class Root : EZData.Context
{
	public HighScoresScreen HighScores { get; private set; }
	public GameLogic Model { get; private set; }
	
	public Root(GameLogic model)
	{
		Model = model;
		HighScores = new HighScoresScreen(this);
	}
}

[System.Serializable]
public class DemoViewModel : MonoBehaviour
{
	public NguiRootContext View;
	public GameLogic Model;
	
	public Root Root { get; private set; }
	
	void Awake()
	{
		Root = new Root(Model);
		View.SetContext(Root);
	}
}
