using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Elevator : MonoBehaviour
{
	[SerializeField]
	private Vector3 localDown, localUp;
	private Vector3 worldDown, worldUp;

	[SerializeField]
	private bool isUp = false;

#if UNITY_EDITOR
	private void OnDrawGizmos()
	{
		Gizmos.DrawWireCube(transform.position + localDown, new Vector3(1, 1));
		Gizmos.DrawWireCube(transform.position + localUp, new Vector3(1, 1));
		Gizmos.DrawLine(transform.position + localDown, transform.position + localUp);
	}
#endif

	private void Start()
	{
		worldUp = transform.position + localUp;
		worldDown += transform.position + localDown;
	}

	private void Update()
	{
		if (isUp)
		{
			transform.position += new Vector3(0, 1, 0) * Time.deltaTime;
			if (worldUp.y < transform.position.y)
			{
				transform.position = worldUp;
			}
		}
		else
		{
			transform.position -= new Vector3(0, 1, 0) * Time.deltaTime;
			if (worldDown.y > transform.position.y)
			{
				transform.position = worldDown;
			}
		}

	}

	public void SetIsUp(bool isUp)
	{
		this.isUp = isUp;
	}
}
