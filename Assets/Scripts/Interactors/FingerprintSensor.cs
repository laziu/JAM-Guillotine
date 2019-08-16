using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class FingerprintSensor : MonoBehaviour, IBodyInteractor
{
	public UnityAction OnInteract;

	public void BodyInteract()
	{
		OnInteract();
	}

}
