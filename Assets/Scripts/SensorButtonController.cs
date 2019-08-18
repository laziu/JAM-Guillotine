using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SensorButtonController : MonoBehaviour
{
    public bool triggerOnce = false;
    public enum SensorType { Sound, Motion, Finger, Eye };
    public SensorType sensorType;

    public UnityEvent triggerOn;
    public UnityEvent triggerOff;

    public Sprite on, off;

    [Header("Sound")]
    [Range(1f, 10f)] public float soundRange;

    [SerializeField] private bool toggleState = false;

    private SpriteRenderer sprite;

    private void OnDrawGizmos()
    {
        if (sensorType == SensorType.Sound)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, soundRange);
        }
    }

    private void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        switch (sensorType)
        {
            case SensorType.Motion:
                if ((collision.tag == "Head" && collision.gameObject.GetComponent<HeadController>().headState.IsState("splited"))
                    || collision.tag == "Body")
                    Trigger(collision);
                break;
            case SensorType.Finger:
                if (collision.tag == "Body")
                    Trigger(collision);
                break;
            case SensorType.Eye:
                if (collision.tag == "Head")
                    Trigger(collision);
                break;
        }
    }

    public void Trigger(Collider2D collision)
    {
        toggleState = triggerOnce || !toggleState;
        if (toggleState)
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
