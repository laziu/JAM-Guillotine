using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap : MonoBehaviour
{
    public enum TrapType { Lego, Dead };
    public TrapType trapType;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Traped " + trapType + " - " + collision.tag);
        switch(trapType)
        {
            case TrapType.Lego: collision?.GetComponent<Actor>()?.GetDamaged(1); break;
            case TrapType.Dead: collision?.GetComponent<Actor>()?.GetDamaged(99999); break;
        }
    }
}
