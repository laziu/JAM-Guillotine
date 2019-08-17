using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class SoundField : MonoBehaviour
{
	[SerializeField]
	private LayerMask layerMask;

	private Vector3[] vertices = new Vector3[361];
	[SerializeField]
	private LineRenderer lineRenderer;

	private float maxRadius = 3;
	private float radius;
	private float t = 0;
	private void Update()
	{
		UpdateFieldOfSoundView(radius);
		t += Time.deltaTime;
		radius = Mathf.Lerp(0, maxRadius, 1 - Mathf.Pow(1 - t, 3));
		lineRenderer.startColor = lineRenderer.endColor = new Color(1, 1, 1, 1 - t);
		if (t >= 1)
			Destroy(gameObject);
	}

	public void Initialize(Vector3 origin, float maxRadius)
	{
		transform.position = origin;
		this.maxRadius = maxRadius;
		TriggerSoundTriggers();
	}

	private void UpdateFieldOfSoundView(float radius)
	{
		for (int i = 0; i < 360; i++)
		{
			Vector2 direction = (Quaternion.Euler(0, 0, i) * Vector2.right).normalized;
			RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, radius, layerMask);

			if (hit.collider != null)
				vertices[i] = hit.point;
			else
				vertices[i] = transform.position + new Vector3(direction.x, direction.y) * radius;
		}
		vertices[360] = vertices[0];

		lineRenderer.SetPositions(vertices);
	}

	private void TriggerSoundTriggers()
	{
		foreach(var detected in Physics2D.OverlapCircleAll(transform.position, maxRadius))
		{
			ISoundTrigger soundTrigger = detected.GetComponent<ISoundTrigger>();
			if (soundTrigger != null)
			{
				Vector2 delta = detected.transform.position - transform.position;
				if (Physics2D.Raycast(transform.position, delta, delta.magnitude, layerMask).collider == null)
				{
					soundTrigger.SoundTriggered();
				}
			}
		}
	}
}
