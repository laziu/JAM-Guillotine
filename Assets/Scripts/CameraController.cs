using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
	[SerializeField]
	private Transform target;
	private float offsetZ;

	private void Start()
	{
		offsetZ = transform.position.z - target.position.z;
	}

	private void LateUpdate()
	{
		transform.position = Vector3.Lerp(transform.position, target.position + new Vector3(0, 0, offsetZ), 0.8f);
	}
}
