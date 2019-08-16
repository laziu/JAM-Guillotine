using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadController2 : MonoBehaviour
{
    private CircleCollider2D col;
    private Rigidbody2D rb;

    public float maxSpeed = 5.0f;
    public float speedGain = 5.0f;
    [SerializeField] private float groundOffset = 0.1f;
    private bool IsGround => Physics2D.OverlapPoint(
            transform.position - new Vector3(0, col.bounds.extents.y + groundOffset)
        ) != null;

    private void Start()
    {
        col = GetComponent<CircleCollider2D>();
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        float horizontal = Input.GetAxis("Horizontal Head");
        if (Mathf.Abs(rb.angularVelocity) < maxSpeed)
        {
            rb.angularVelocity -= horizontal * speedGain * Time.fixedDeltaTime;
        }
    }
}
