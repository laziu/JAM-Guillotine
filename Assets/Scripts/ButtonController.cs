using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ButtonController : MonoBehaviour
{
    public UnityEvent[] triggerList;
    
    public void Interact()
    {
        foreach (var i in triggerList)
            i.Invoke();
    }
}
