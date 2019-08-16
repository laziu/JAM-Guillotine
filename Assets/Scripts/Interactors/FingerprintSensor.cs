using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class FingerprintSensor : IBodyInteractor
{
	public UnityAction OnInteract;

	public void Body_Interact()
	{
		OnInteract();
	}

}
