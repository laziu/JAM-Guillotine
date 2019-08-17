using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : Actor, ISoundTrigger
{
	[SerializeField]
	private float moveSpeed;
	[SerializeField]
	private FieldOfView fov;

	public abstract void SoundTriggered();

	private Vector3 destination;
	private bool aggressive = false;

	private Vector2 direction = Vector2.right;

	private float nextMoveTimer = 1f;

	protected virtual void Update()
	{
		fov.eyesightDirection = (Vector2.right * transform.localScale.x).normalized;
		if (nextMoveTimer > 0)
		{
			nextMoveTimer -= Time.deltaTime;
			if (nextMoveTimer <= 0)
			{
				Flip();
				destination += new Vector3(direction.x * 2, 0);
				nextMoveTimer = Random.Range(3f, 5f);
			}
		}
	}

	private void FixedUpdate()
	{
		if (Vector2.Distance(transform.position, destination) > 0.01f)
			transform.Translate(direction * Time.deltaTime);
	}

	private void Flip()
	{
		direction = -direction;
		transform.localScale = new Vector3(direction.x, transform.localScale.y, transform.localScale.z);
	}
}