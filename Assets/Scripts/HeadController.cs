using System;
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
	private LayerMask groundLayerMask;
	[SerializeField]
	private float groundBias = 0.1f;
	private bool IsGround
	{
		get
		{
			return Physics2D.OverlapPoint(transform.position - new Vector3(0, col.bounds.extents.y + groundBias), groundLayerMask) != null;
		}
	}
	private bool wasGround;

	[SerializeField]
	private LayerMask waterLayerMask;
	private bool IsWater
	{
		get
		{
			return Physics2D.OverlapPoint(transform.position,waterLayerMask) != null;
		}
	}

	[SerializeField]
	private float interactorDetectDistance = 3f;

	private IHeadInteractor selectedInteractor;

	public StateMachine headState = new StateMachine();

	[SerializeField]
	private FieldOfView fov;
	[SerializeField]
	private FieldOfView nearFov;

	[SerializeField]
	private GameObject soundFieldPrefab;

	[SerializeField]
	private AudioClip jumpSFX, landSFX, shoutSFX;

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
		InitStateMachine();
	}

	private void HeadMovementControl()
	{
		float horizontal = Input.GetAxis("Horizontal Head");
		
		rb.velocity = new Vector2(maxSpeed * horizontal, rb.velocity.y);
		
		if (Input.GetButtonDown("Jump Head") && IsGround)
		{
			rb.velocity += new Vector2(0, 5f);
			Instantiate(soundFieldPrefab).GetComponent<SoundField>().Initialize(transform.position, 3, jumpSFX);
		}
	}

	private void HeadActionControl()
	{
		if (Input.GetMouseButtonDown(0))
		{
			selectedInteractor?.HeadInteract();
		}
		if (Input.GetMouseButtonDown(1))
		{
			Shout();
		}
	}

	private void Update()
	{
		headState.UpdateStateMachine();
		fov.eyesightDirection = (CameraController.inst.HeadCamera.ScreenToWorldPoint(Input.mousePosition) - transform.position).normalized;
		UpdateRenderers();
		CheckLanding();
	}

	private void FixedUpdate()
	{
		if (IsWater)
			rb.AddForce(Vector2.up * 15);
	}

	private void CheckLanding()
	{
		if (!wasGround && IsGround)
		{
			Instantiate(soundFieldPrefab).GetComponent<SoundField>().Initialize(transform.position, 3, landSFX);
		}
		wasGround = IsGround;
	}

	private void InitStateMachine()
	{
		State splited = new State("splited");
		State binded = new State("binded");
		State throwing = new State("throwing");

		splited.StateUpdate += delegate
		{
			DetectInteractor();
			HeadMovementControl();
			HeadActionControl();
		};

		binded.Enter += delegate
		{
			//rb.simulated = false;
		};

		binded.StateUpdate += delegate
		{
			DetectInteractor();
			HeadActionControl();
		};

		headState.AddNewState(splited.name, splited);
		headState.AddNewState(binded.name, binded);
		headState.AddNewState(throwing.name, throwing);

		headState.Transition(splited.name);
	}

	public void BodyInteract()
	{
		DetectInteractor();
		HeadMovementControl();
	}

	private void Shout()
	{
		Instantiate(soundFieldPrefab).GetComponent<SoundField>().Initialize(transform.position, 10, shoutSFX);
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

	private void UpdateRenderers()
	{
		Tuple<List<Renderer>, List<Renderer>> tuple = fov.GetDetectionResultList<Renderer>();

		foreach(var detected in tuple.Item1)
		{
			detected.enabled = true;
		}
		foreach (var undetected in tuple.Item2)
		{
			undetected.enabled = false;
		}

		tuple = nearFov.GetDetectionResultList<Renderer>();

		foreach (var detected in tuple.Item1)
		{
			detected.enabled = true;
		}

	}
}
