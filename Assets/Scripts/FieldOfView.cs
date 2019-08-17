using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class FieldOfView : MonoBehaviour
{
	private Mesh mesh;
	private Vector3[] vertices;
	private int[] triangles;
	[SerializeField]
	private int eyesightAngle = 120;
	[SerializeField]
	private float eyesightRadius = 10f;
	[SerializeField]
	private LayerMask eyesightLayerMask;
	public Vector3 eyesightDirection = Vector3.right;

	[SerializeField]
	private LayerMask detectLayerMask;

	[SerializeField]
	private Color meshColor = new Color(1, 1, 1, 0.2f);

	public bool isMeshUpdate = true;

	private void Start()
	{
		transform.localPosition = Vector3.zero;
		mesh = GetComponent<MeshFilter>().mesh;
		GetComponent<MeshRenderer>().material.color = meshColor;
	}

	private void Update()
	{
		if (isMeshUpdate)
			UpdateFieldOfViewMesh();
	}

	private void UpdateFieldOfViewMesh()
	{
		List<Vector3> viewVertices = new List<Vector3>();

		Vector2 direction = eyesightDirection;
		direction = Quaternion.Euler(0, 0, -eyesightAngle / 2 - 1) * direction;

		for (int i = 0; i < eyesightAngle; i += 1)
		{
			direction = (Quaternion.Euler(0, 0, 1) * direction).normalized;
			RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, eyesightRadius, eyesightLayerMask);

			if (hit.collider != null)
				viewVertices.Add(hit.point);
			else
				viewVertices.Add(transform.position + new Vector3(direction.x, direction.y) * eyesightRadius);
		}

		int vertexCount = viewVertices.Count + 1;

		vertices = new Vector3[vertexCount];
		triangles = new int[(vertexCount - 2) * 3];

		vertices[0] = Vector3.zero;
		for (int i = 0; i < vertexCount - 1; ++i)
		{
			vertices[i + 1] = transform.InverseTransformPoint(viewVertices[i]);
		}

		for (int i = 0; i < vertexCount - 2; ++i)
		{
			triangles[3 * i + 2] = 0;
			triangles[3 * i + 1] = (i + 1) % 361;
			triangles[3 * i] = (i + 2) % 361;
		}

		mesh.Clear();
		mesh.vertices = vertices;
		mesh.triangles = triangles;
	}

	public Tuple<List<T>, List<T>> GetDetectionResultList<T>() where T : Component
	{
		List<T> detectedList = new List<T>();
		List<T> undetectedList = new List<T>();
		foreach (var inRaidus in Physics2D.OverlapCircleAll(transform.position, eyesightRadius, detectLayerMask))
		{
			Vector3 vec = inRaidus.transform.position - transform.position;
			if (Physics2D.Raycast(transform.position, vec, vec.magnitude, eyesightLayerMask).collider == null &&
				Vector2.Angle(vec, eyesightDirection) <= eyesightAngle / 2)
			{
				detectedList.Add(inRaidus.GetComponent<T>());
			}
			else
				undetectedList.Add(inRaidus.GetComponent<T>());
		}
		return new Tuple<List<T>, List<T>>(detectedList, undetectedList);
	}
}
