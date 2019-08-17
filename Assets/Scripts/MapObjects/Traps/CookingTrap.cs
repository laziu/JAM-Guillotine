using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CookingTrap : MonoBehaviour
{
	private void OnTriggerStay2D(Collider2D collision)
	{
		collision.GetComponent<Player>()?.GetDamaged(4);
	}
}
