using UnityEngine;
using System.Collections;

public class OnDropHandler : MonoBehaviour
{
	public event System.Action<GameObject, Vector2> OnDragCallback;
	public event System.Action<GameObject, GameObject> OnDropCallback;
	public event System.Action<GameObject, bool> OnPressCallback;
	
	void OnDrag(Vector2 delta)
	{
		if (OnDragCallback != null)
		{
			OnDragCallback(gameObject, delta);
		}
	}
	
	void OnDrop(GameObject drag)
	{
		if (OnDropCallback != null)
		{
			OnDropCallback(drag, gameObject);
		}
	}
	
	void OnPress(bool isDown)
	{
		if (OnPressCallback != null)
		{
			OnPressCallback(gameObject, isDown);
		}
	}
}
