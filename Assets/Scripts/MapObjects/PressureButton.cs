using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PressureButton : MonoBehaviour
{
	[SerializeField]
	private LayerMask layerMask;

	public List<UnityEvent> doActions;
	public List<UnityEvent> undoActions;

	private bool on = false;

	[SerializeField]
	private float requiredMass = 1.0f;

	private void OnTriggerEnter2D(Collider2D collision)
	{
		Debug.Log("Enter");
		if (!on)
		{
			foreach (var col in Physics2D.OverlapBoxAll(transform.position, GetComponent<Collider2D>().bounds.size, 0, layerMask))
			{
				if (col.attachedRigidbody?.mass >= requiredMass)
					on = true;
			}
			if (on)
			{
				foreach (var action in doActions)
				{
					action.Invoke();
				}
			}
		}
	}

	private void OnTriggerExit2D(Collider2D collision)
	{
		Debug.Log("Exit");
		if (on)
		{
			on = false;
			foreach (var col in Physics2D.OverlapBoxAll(transform.position, GetComponent<Collider2D>().bounds.size, 0, layerMask))
			{
				Debug.Log(col.attachedRigidbody?.mass);
				if (col.attachedRigidbody?.mass >= requiredMass)
					on = true;
			}
			if (!on)
			{
				foreach (var action in undoActions)
				{
					action.Invoke();
				}
			}
		}
	}
}
