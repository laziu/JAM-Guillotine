using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Actor
{
	private float graceTimer = 0;
	public bool IsDamagable { get { return graceTimer <= 0; } }

	private SpriteRenderer sr;

	[SerializeField]
	private GameObject soundFieldPrefab;

	[SerializeField]
	private AudioClip damagedSFX;

	private void Start()
	{
		sr = GetComponent<SpriteRenderer>();
	}

	private void Update()
	{
		if (graceTimer > 0)
			graceTimer -= Time.deltaTime;
	}

	public override void GetDamaged(int damage)
	{
		if (IsDamagable)
		{
			graceTimer = 1;
			StartCoroutine(DamagedRoutine());
			Instantiate(soundFieldPrefab).GetComponent<SoundField>().Initialize(transform.position, 3, damagedSFX);
			base.GetDamaged(damage);
		}
	}

	public void GetDamaged(int damage, Vector2 force)
	{
		GetComponent<Rigidbody2D>().AddForce(force.normalized * 10);
		GetDamaged(damage);
	}

	private IEnumerator DamagedRoutine()
	{
		float oriAlpha = sr.color.a;
		while (graceTimer > 0)
		{
			sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, Mathf.Round(5 * graceTimer - (int)(5 * graceTimer)));
			yield return null;
		}
		sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, oriAlpha);
	}
}
