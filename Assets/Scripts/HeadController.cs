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
	private bool wasGround;

	[SerializeField]
	private float interactorDetectDistance = 3f;

	private IHeadInteractor selectedInteractor;

	public StateMachine headState = new StateMachine();

	[SerializeField]
	private List<SpriteRenderer> mapObjectRenderers = new List<SpriteRenderer>();

	private Mesh mesh;
	private Vector3[] vertices;
	private int[] triangles;
	[SerializeField]
	private int eyesightAngle = 120;
	[SerializeField]
	private float eyesightRadius = 10f;
	[SerializeField]
	private LayerMask layerMask;

	[SerializeField]
	private GameObject soundFieldPrefab;

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
		foreach(var renderer in FindObjectsOfType<SpriteRenderer>())
		{
			if (renderer.gameObject.layer == LayerMask.NameToLayer("MapObject"))
				mapObjectRenderers.Add(renderer);
		}

		InitStateMachine();
		mesh = transform.Find("EyesightMesh").GetComponent<MeshFilter>().mesh;
	}

	private void HeadMovementControl()
	{
		float horizontal = Input.GetAxis("Horizontal Head");
		
		rb.velocity = new Vector2(maxSpeed * horizontal, rb.velocity.y);
		
		if (Input.GetButtonDown("Jump Head") && IsGround)
		{
			rb.velocity += new Vector2(0, 5f);
			Instantiate(soundFieldPrefab).GetComponent<SoundField>().Initialize(transform.position, 3);
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
		UpdateFieldOfView();
		UpdateFieldOfViewMesh();
		CheckLanding();
	}

	private void CheckLanding()
	{
		if (!wasGround && IsGround)
		{
			Instantiate(soundFieldPrefab).GetComponent<SoundField>().Initialize(transform.position, 3);
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
		Instantiate(soundFieldPrefab).GetComponent<SoundField>().Initialize(transform.position, 10);
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

	private void UpdateFieldOfView()
	{
		foreach(var renderer in mapObjectRenderers)
		{
			Vector3 vec = renderer.transform.position - transform.position;
			RaycastHit2D hit = Physics2D.Raycast(transform.position, vec, vec.magnitude, layerMask);
			renderer.enabled = hit.collider == null;
		}
	}

	private void UpdateFieldOfViewMesh()
	{
		List<Vector3> viewVertices = new List<Vector3>();

		Vector2 direction = Quaternion.Euler(0, 0, -eyesightAngle / 2 - 1) * (CameraController.inst.HeadCamera.ScreenToWorldPoint(Input.mousePosition) - transform.position).normalized;

		for (int i = 0; i < eyesightAngle; i += 1)
		{
			direction = (Quaternion.Euler(0, 0, 1) * direction).normalized;
			RaycastHit2D hit = Physics2D.Raycast(transform.position,direction, eyesightRadius, layerMask);

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
}
