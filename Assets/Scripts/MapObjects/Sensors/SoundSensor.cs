using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SoundSensor : MonoBehaviour, ISoundTrigger
{
	public List<UnityEvent> actions;

	public void SoundTriggered(Vector3 source)
	{
		foreach (var action in actions)
		{
			action.Invoke();
		}
	}
}
