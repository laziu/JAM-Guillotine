using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyInteractSender : MonoBehaviour
{
    [SerializeField] private KeyCode interact = KeyCode.F;

    private new Transform transform;

    void Start()
    {
        transform = GetComponent<Transform>();
    }

    void Update()
    {
        if (Input.GetKeyDown(interact))
        {
            var hit = Physics2D.OverlapCircleAll(transform.position, 0.5f);
            foreach (var h in hit)
            {
                h.gameObject.GetComponent<ButtonController>()?.Interact();
            }
        }        
    }
}
