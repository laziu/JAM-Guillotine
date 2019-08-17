using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyController : MonoBehaviour
{
    [SerializeField] private float maxSpeed = 10f;
    [SerializeField] private float jumpForce = 550f;
    [SerializeField] private LayerMask groundLayer;

    [Range(0, .3f)] [SerializeField] private float movementSmoothing = .05f;

    private new Transform transform;
    private new Rigidbody2D rigidbody;
    private new Collider2D collider;

    private bool IsGround => Physics2D.Linecast(
        transform.position + new Vector3(-collider.bounds.extents.x + .02f, -collider.bounds.extents.y - .1f),
        transform.position + new Vector3(+collider.bounds.extents.x - .02f, -collider.bounds.extents.y - .1f),
        groundLayer).collider != null;

    private void OnDrawGizmos()
    {
        if (!transform) transform = GetComponent<Transform>();
        if (!collider) collider = GetComponent<Collider2D>();

        Gizmos.color = IsGround ? Color.blue : Color.red;
        Gizmos.DrawLine(
            transform.position + new Vector3(-collider.bounds.extents.x + .02f, -collider.bounds.extents.y - .1f),
            transform.position + new Vector3(+collider.bounds.extents.x - .02f, -collider.bounds.extents.y - .1f)
        );
    }

    private void Start()
    {
        transform = GetComponent<Transform>();
        collider = GetComponent<Collider2D>();
        rigidbody = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        BodyMovementControl();
    }

    private void BodyMovementControl()
    {
        float horizontal = Input.GetAxis("Horizontal Body");

        rigidbody.velocity = new Vector2(maxSpeed * horizontal, rigidbody.velocity.y);

        if (Input.GetButtonDown("Jump Body") && IsGround)
        {
            rigidbody.AddForce(new Vector2(0, jumpForce));
        }
    }
}
