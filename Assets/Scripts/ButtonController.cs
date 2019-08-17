using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ButtonController : MonoBehaviour
{
    public UnityEvent triggerOn;
    public UnityEvent triggerOff;

    [SerializeField] private int triggerCount = 0;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (triggerCount == 0)
            triggerOn.Invoke();
        triggerCount++;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        triggerCount--;
        if (triggerCount == 0)
            triggerOff.Invoke();
    }
}
