using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : Actor, ISoundTrigger
{
	public abstract void SoundTriggered();
}