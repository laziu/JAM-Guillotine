using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonController : MonoBehaviour
{
    public ButtonAdapter[] triggerList;

    public void Interact()
    {
        foreach (var i in triggerList)
            i.Signal();
    }
}
