using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ToggleButton : MonoBehaviour, IHeadInteractor, IBodyInteractor
{
	public List<UnityEvent> doActions;
	public List<UnityEvent> undoActions;

	private bool on = false;

	public void BodyInteract()
	{
		on = !on;
		if (on)
		{
			foreach (var action in doActions)
			{
				action.Invoke();
			}
		}
		else
		{
			foreach (var action in undoActions)
			{
				action.Invoke();
			}
		}
	}

	public void HeadInteract()
	{
		on = !on;
		if (on)
		{
			foreach (var action in doActions)
			{
				action.Invoke();
			}
		}
		else
		{
			foreach (var action in undoActions)
			{
				action.Invoke();
			}
		}
	}
}
