using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour
{
    public Sprite open, close;

    private SpriteRenderer sprite;
    private new Collider2D collider;

    public bool isOpen = false;

    private void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
        collider = GetComponent<Collider2D>();
    }

    public void Signal()
    {
        isOpen = !isOpen;
        sprite.sprite = isOpen ? open : close;
        collider.enabled = !isOpen;
    }
}
