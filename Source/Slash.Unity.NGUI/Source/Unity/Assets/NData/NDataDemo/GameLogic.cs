using UnityEngine;
using System.Collections;
using System;

[System.Serializable]
public class GameLogic : MonoBehaviour
{
	public DemoViewModel ViewModel;
	
	void Update ()
	{
		if (_submitting && Time.realtimeSinceStartup - _time > 3)
		{
			OnSubmissionComplete();
		}
	}
	
	public void SubmitScore(string playerName, int score)
	{
		// Here could be real code for submission to server and getting the scores,
		// for demo it'll just make a delay and then provide mock data
		
		_playerName = playerName;
		_score = score;
		_time = Time.realtimeSinceStartup;
		_submitting = true;
		
		ViewModel.Root.HighScores.SubmitInProgress = true;
	}
	
	private bool _submitting;
	private string _playerName;
	private int _score;
	private float _time;
	
	private void OnSubmissionComplete()
	{
		_submitting = false;
		
		var HighScores = ViewModel.Root.HighScores;
		
		HighScores.Table.Clear();
		
		HighScores.Table.Add(new HighScoreTableItem() { 
			Name = "Mad Cat", 
			Score = 9001, 
			Portrait = (Texture2D)Resources.Load("madcat"), 
			Time = DateTime.Now } );
		
		HighScores.Table.Add(new HighScoreTableItem() {
			Name = "Quick Brown Fox",
			Score = 8999,
			Time = DateTime.Now } );
		
		HighScores.Table.Add(new HighScoreTableItem() { 
			Name = "Lazy Dog", 
			Score = 7000, 
			Time = DateTime.Now } );
		
		HighScores.Table.Add(new HighScoreTableItem() { 
			Name = "Angry Bird", 
			Score = 987, 
			Time = DateTime.Now } );
		
		HighScores.Table.Add(new HighScoreTableItem() { 
			Name = _playerName,
			Score = _score, 
			Time = DateTime.Now } );
		
		HighScores.SubmitInProgress = false;
		HighScores.SubmitCompleted = true;
	}
}
