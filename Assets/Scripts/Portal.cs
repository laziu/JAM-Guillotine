using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Portal : MonoBehaviour
{
    public Portal target;
    [SerializeField] bool isSpawned = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Head" || collision.tag == "Body")
        {
            if (isSpawned)
            {
                isSpawned = false;
            }
            else
            {
                target.isSpawned = true;
                collision.transform.position = target.gameObject.transform.position;
            }
        }
    }
}
