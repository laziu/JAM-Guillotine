using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : Actor, ISoundTrigger
{
	[SerializeField]
	private float moveSpeed;
	[SerializeField]
	private FieldOfView fov;

	private Transform target;
	private Vector3 destination;
	public bool IsAggressive { get { return aggressiveTimer > 0; } }

	private Vector2 direction = Vector2.right;

	private float nextMoveTimer = 1f;
	private float aggressiveTimer = 0f;

	protected virtual void Update()
	{
		fov.eyesightDirection = (Vector2.right * transform.localScale.x).normalized;

		if (aggressiveTimer > 0)
			aggressiveTimer -= Time.deltaTime;
		if (nextMoveTimer > 0 && !IsAggressive)
			nextMoveTimer -= Time.deltaTime;
		if (nextMoveTimer <= 0)
		{
			Flip();
			destination += new Vector3(direction.x * 2, 0);
			nextMoveTimer = Random.Range(3f, 5f);
		}

		DetectPlayer();
		if (IsAggressive && target != null)
		{
			destination = target.position;

		}
		if (destination.x > transform.position.x && direction.x < 0 || destination.x < transform.position.x && direction.x > 0)
		{
			Flip();
		}
		if (aggressiveTimer <= 0)
		{
			target = null;
		}
	}

	private void FixedUpdate()
	{
		if (Mathf.Abs(transform.position.x - destination.x) > 0.01f)
			transform.Translate(direction * Time.deltaTime);
	}

	public virtual void SoundTriggered(Vector3 source)
	{
		aggressiveTimer = 8;
		destination = source;
	}

	private void Flip()
	{
		direction = -direction;
		transform.localScale = new Vector3(direction.x, transform.localScale.y, transform.localScale.z);
	}

	private void DetectPlayer()
	{
		Transform newTarget = null;
		float minDistance = float.PositiveInfinity;
		foreach (var player in fov.GetDetectionResultList<Player>().Item1)
		{
			float distance = Vector2.Distance(player.transform.position, transform.position);
			if (minDistance > distance)
			{
				minDistance = distance;
				newTarget = player.transform;
			}
		}
		if (newTarget != null)
		{
			target = newTarget;
			aggressiveTimer = 8;
		}
	}
}