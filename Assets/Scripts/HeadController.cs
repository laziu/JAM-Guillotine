using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]
public class HeadController : MonoBehaviour, IBodyInteractor
{
	private Vector3 prePos;
	[SerializeField]
	private CircleCollider2D col;
	private Rigidbody2D rb;

	public float maxSpeed = 5.0f;

	[SerializeField]
	private float groundBias = 0.1f;
	private bool IsGround
	{
		get
		{
			return Physics2D.OverlapPoint(transform.position - new Vector3(0, col.bounds.extents.y + groundBias)) != null;
		}
	}

	[SerializeField]
	private float interactorDetectDistance = 3f;

	[SerializeField]
	private IHeadInteractor selectedInteractor;

#if UNITY_EDITOR
	[SerializeField]
	private Transform targetInteractor = null;
	private void OnDrawGizmos()
	{
		Gizmos.DrawWireSphere(transform.position, interactorDetectDistance);
		if (IsGround)
			Gizmos.color = Color.red;
		else
			Gizmos.color = Color.green;
		Gizmos.DrawSphere(transform.position - new Vector3(0, col.bounds.extents.y + groundBias), 0.05f);


		DetectInteractor();
		if (targetInteractor != null)
		{
			Gizmos.color = Color.yellow;
			Gizmos.DrawSphere(targetInteractor.position, 0.1f);
		}
	}
#endif

	private void Start()
	{
		prePos = transform.position;
		rb = GetComponent<Rigidbody2D>();
	}

	private void HeadMovementControl()
	{
		float horizontal = Input.GetAxis("Horizontal Head");
		
		rb.velocity = new Vector2(maxSpeed * horizontal, rb.velocity.y);
		
		if (Input.GetButtonDown("Jump Head") && IsGround)
		{
			rb.velocity += new Vector2(0, 5f);
		}

		if (Input.GetButtonDown("Interact Head"))
		{
			selectedInteractor.Head_Interact();
		}
	}

	private void Update()
	{
		DetectInteractor();
		HeadMovementControl();
	}

	public void Body_Interact()
	{
		//TODO:Throw mode
	}

	private void DetectInteractor()
	{
		float minDistance = float.PositiveInfinity;
		selectedInteractor = null;
#if UNITY_EDITOR
		targetInteractor = null;
#endif
		foreach (var detected in Physics2D.OverlapCircleAll(transform.position, interactorDetectDistance))
		{
			IHeadInteractor interactor = detected.GetComponent<IHeadInteractor>();
			if (interactor != null)
			{
				Debug.Log(detected);
				float distance = Vector3.Distance(transform.position, detected.transform.position);
				if (minDistance > distance)
				{
					minDistance = distance;
					selectedInteractor = interactor;
#if UNITY_EDITOR
					targetInteractor = detected.transform;
#endif	
				}
			}
		}
	}
}
