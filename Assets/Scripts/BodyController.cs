using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyController : MonoBehaviour
{
    public float runSpeed = 10f;
    public KeyCode m_left = KeyCode.A;
    public KeyCode m_right = KeyCode.D;
    public KeyCode m_jump = KeyCode.Space;
    public KeyCode m_crouch = KeyCode.C;

    private float horizontalMove = 0f;
    private bool jump = false;

    private Movement2D mv;

    private void Start()
    {
        mv = GetComponent<Movement2D>();
    }

    private void FixedUpdate()
    {
        float horizontalMove = (Input.GetKey(m_right) ? runSpeed : 0) 
                             - (Input.GetKey(m_left) ? runSpeed : 0);
        bool jump = Input.GetKeyDown(m_jump);
        bool crouch = Input.GetKey(m_crouch);
        mv.Move(horizontalMove * runSpeed * Time.fixedDeltaTime, crouch, jump);
    }
}
