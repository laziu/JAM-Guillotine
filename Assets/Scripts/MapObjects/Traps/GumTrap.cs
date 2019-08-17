using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class GumTrap : MonoBehaviour
{
	private void OnTriggerEnter2D(Collider2D collision)
	{
		collision.GetComponent<HeadController>()?.headState.Transition("binded");
	}
}
