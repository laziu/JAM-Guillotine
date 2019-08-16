using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MotionSensor : MonoBehaviour
{
    public List<UnityEvent> actions;

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.tag == "Player")
		{
			foreach (var action in actions)
			{
				action.Invoke();
			}
		}
	}
}
