using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Actor : MonoBehaviour
{
	[SerializeField]
	private int _health;
    public int Health { get { return _health; } private set { _health = value; } }

	[SerializeField]
	private ParticleSystem hitEffect;

	public virtual void GetDamaged(int damage)
	{
		if (hitEffect != null)
			hitEffect.Play();
		Health -= damage;
		if (Health <= 0)
		{
			OnDead();
		}
	}

	protected virtual void OnDead()
	{

	}

}
