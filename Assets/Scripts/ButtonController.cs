using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ButtonController : MonoBehaviour
{
    public bool isPress = false;

    public UnityEvent triggerOn;
    public UnityEvent triggerOff;

    public Sprite on, off;

    [SerializeField] private int triggerCount = 0;
    [SerializeField] private bool toggleState = false;

    private SpriteRenderer sprite;

    private void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (triggerCount == 0)
            triggerTo(isPress ? true : (toggleState = !toggleState));
        triggerCount++;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        triggerCount--;
        if (triggerCount == 0)
            if (isPress)
                triggerTo(false);
    }

    private void triggerTo(bool ison)
    {
        if (ison)
        {
            sprite.sprite = on;
            triggerOn.Invoke();
        }
        else
        {
            sprite.sprite = off;
            triggerOff.Invoke();
        }
    }
}
