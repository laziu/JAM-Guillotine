using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class IrisSensor : MonoBehaviour, IHeadInteractor
{
	public UnityAction OnInteract;

	public void HeadInteract()
	{
		OnInteract();
	}
}
