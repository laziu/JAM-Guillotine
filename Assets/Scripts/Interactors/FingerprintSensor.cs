using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class FingerprintSensor : MonoBehaviour, IBodyInteractor
{
	public List<UnityEvent> actions;

	public void BodyInteract()
	{
		foreach (var action in actions)
		{
			action.Invoke();
		}
	}

}
