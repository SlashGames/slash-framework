using UnityEngine;
using StayingAliveContexts;

public class StayingAlive : MonoBehaviour
{
	public NguiRootContext View;
	public Game Context;
	public Balance balance;
	
	void Awake()
	{
		Context = new Game(balance);
		View.SetContext(Context);
	}
	
	void Update()
	{
		if (Context.State == GameState.Game)
		{
			Context.Session.Update();
		}
	}
}
