using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour
{
    private SpriteRenderer sprite;
    private new Collider2D collider;

    public bool isOpen = false;

    private void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
        collider = GetComponent<Collider2D>();
    }

    public void Signal(bool on)
    {
        isOpen = on;
        sprite.enabled = !on;
        collider.enabled = !on;
    }
}
