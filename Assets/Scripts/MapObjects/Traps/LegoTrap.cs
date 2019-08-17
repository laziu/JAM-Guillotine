using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class LegoTrap : MonoBehaviour
{
	private void OnTriggerEnter2D(Collider2D collision)
	{
		collision.GetComponent<Player>()?.GetDamaged(1);
	}
}
