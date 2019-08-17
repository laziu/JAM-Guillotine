using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyController : MonoBehaviour
{
    [SerializeField] private float maxSpeed = 10f;
    [SerializeField] private float verticalSpeed = 5f;
    [SerializeField] private float jumpForce = 550f;
    [SerializeField] private float powerJumpForce = 660f;
    [SerializeField] private float headForceOffset = 110f;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask jumpAreaLayer;
    [SerializeField] private LayerMask ladderLayer;

    [Range(0, .3f)] [SerializeField] private float movementSmoothing = .05f;
    [SerializeField] private float headOffset = 1f;

    [SerializeField] private GameObject soundFieldPrefab;
    [SerializeField] private GameObject throwIndicatorPrefab;
    private ForceIndicator throwIndicator;

    private Transform head;

    private new Transform transform;
    private new Rigidbody2D rigidbody;
    private new Collider2D collider;

    private StateMachine bodyState = new StateMachine();

    private float preserveGravity;
    private bool wasGround = false;

    private bool LinecastFloor(float offset, LayerMask layer) => Physics2D.Linecast(
        transform.position + new Vector3(-collider.bounds.extents.x + .02f, -collider.bounds.extents.y + offset),
        transform.position + new Vector3(+collider.bounds.extents.x - .02f, -collider.bounds.extents.y + offset),
        layer).collider != null;

    private bool IsStuck => LinecastFloor(-.01f, groundLayer);
    private bool IsGround => LinecastFloor(-.1f, groundLayer);
    private bool IsJumpArea => LinecastFloor(+.1f, jumpAreaLayer);
    private bool IsLadder => LinecastFloor(+.1f, ladderLayer);

    private void OnDrawGizmos()
    {
        if (!transform) transform = GetComponent<Transform>();
        if (!collider) collider = GetComponent<Collider2D>();

        Gizmos.color = IsGround ? Color.blue : Color.red;
        Gizmos.DrawLine(
            transform.position + new Vector3(-collider.bounds.extents.x + .02f, -collider.bounds.extents.y - .1f),
            transform.position + new Vector3(+collider.bounds.extents.x - .02f, -collider.bounds.extents.y - .1f)
        );
        Gizmos.color = IsJumpArea ? Color.blue : Color.red;
        Gizmos.DrawLine(
            transform.position + new Vector3(-collider.bounds.extents.x - .1f, -collider.bounds.extents.y + .1f),
            transform.position + new Vector3(+collider.bounds.extents.x + .1f, -collider.bounds.extents.y + .1f)
        );
    }

    private void Start()
    {
        transform = GetComponent<Transform>();
        collider = GetComponent<Collider2D>();
        rigidbody = GetComponent<Rigidbody2D>();

        InitStateMachine();

        preserveGravity = rigidbody.gravityScale;
    }

    private void Update()
    {
        CheckLanding();
        CheckJoinAction();
        CheckAttackAction();
        CheckThrowingCancel();
    }

    private void FixedUpdate()
    {
        bodyState.UpdateStateMachine();
    }

    private void BodyMovementControl()
    {
        float horizontal = Input.GetAxis("Horizontal Body");

        rigidbody.velocity = new Vector2(maxSpeed * horizontal, rigidbody.velocity.y);

        if (IsLadder)
        {
            if (Mathf.Abs(Input.GetAxis("Vertical Body")) > .001f)
            {
                rigidbody.gravityScale = 0;
            }
            if (rigidbody.gravityScale == 0)
            {
                rigidbody.velocity = new Vector2(rigidbody.velocity.x, verticalSpeed * Input.GetAxis("Vertical Body"));
            }
            if (Input.GetButtonDown("Jump Body") && !IsGround)
            {
                rigidbody.AddForce(new Vector2(0, jumpForce + (bodyState.IsState("splited") ? 0 : headForceOffset)));
                rigidbody.gravityScale = preserveGravity;
                MakeSound();
            }
        }
        else if (rigidbody.gravityScale == 0)
        {
            rigidbody.gravityScale = preserveGravity;
        }

        if (Input.GetButtonDown("Jump Body") && IsGround)
        {
            rigidbody.AddForce(new Vector2(0, (IsJumpArea ? powerJumpForce : jumpForce) + 
                                              (bodyState.IsState("splited") ? 0 : headForceOffset)));
            MakeSound();
        }

        if (IsStuck)
        {
            transform.position += new Vector3(0, 0.01f);
        }
    }

    private void HeadMovementControl()
    {
        head.position = transform.position + Vector3.up;
    }

    private void HeadThrowControl()
    {
        throwIndicator.StartPosition = head.position;
        throwIndicator.TargetPosition = CameraController.inst.HeadCamera.ScreenToWorldPoint(Input.mousePosition);
    }

    private void CheckJoinAction()
    {
        if (Input.GetButtonDown("Join"))
        {
            if (bodyState.IsState("splited"))
            {
                foreach (var c in Physics2D.OverlapCircleAll(transform.position, 1f, LayerMask.GetMask("Player")))
                {
                    if (c.tag == "Head")
                    {
                        c.gameObject.AddComponent<FixedJoint2D>().connectedBody = rigidbody;
                        head = c.gameObject.GetComponent<Transform>();
                        head.GetComponent<HeadController>().headState.Transition("binded");
                        bodyState.Transition("binded");
                        break;
                    }
                }
            }
            else
            {
                head.GetComponent<HeadController>().headState.Transition("splited");
                bodyState.Transition("splited");
            }
        }
    }

    private void CheckThrowingCancel()
    {
        if (bodyState.IsState("throwing") &&
            (Mathf.Abs(Input.GetAxis("Horizontal Body")) > 0.1f ||
             Mathf.Abs(Input.GetAxis("Vertical Body")) > 0.1f) )
        {
            head.GetComponent<HeadController>().headState.Transition("binded");
            bodyState.Transition("binded");
        }
    }

    private void CheckAttackAction()
    {
        if (Input.GetButtonDown("Attack"))
        {
            if (bodyState.IsState("binded"))
            {
                head.GetComponent<HeadController>().headState.Transition("throwing");
                bodyState.Transition("throwing");
            }
            else if (bodyState.IsState("throwing"))
            {
                head.GetComponent<HeadController>().headState.Transition("splited");
                bodyState.Transition("splited");
                Debug.Log(throwIndicator.force);
                head.position += Vector3.up * 0.1f;
                head.GetComponent<Rigidbody2D>().velocity = throwIndicator.force * 5f;
            }
        }
    }

    private void InitStateMachine()
    {
        State splited = new State("splited");
        State binded = new State("binded");
        State throwing = new State("throwing");

        splited.Enter += () => Destroy(head.GetComponent<FixedJoint2D>());

        splited.StateUpdate += BodyMovementControl;

        binded.StateUpdate += () =>
        {
            HeadMovementControl();
            BodyMovementControl();
        };

        throwing.Enter += () =>
            throwIndicator = Instantiate(throwIndicatorPrefab).GetComponent<ForceIndicator>();

        throwing.Exit += () => Destroy(throwIndicator.gameObject);

        throwing.StateUpdate += () => 
        {
            HeadThrowControl();
            BodyMovementControl();
        };

        bodyState.AddNewState(splited.name, splited);
        bodyState.AddNewState(binded.name, binded);
        bodyState.AddNewState(throwing.name, throwing);

        bodyState.Transition(splited.name);
    }

    private void MakeSound() => 
        Instantiate(soundFieldPrefab).GetComponent<SoundField>().Initialize(transform.position, 3);

    private void CheckLanding()
    {
        if (!wasGround && IsGround)
        {
            MakeSound();
        }
        wasGround = IsGround;
    }
}
