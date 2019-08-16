using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State
{
	public string name;
	public Action Enter = null;
	public Action Exit = null;
	public Action StateUpdate = null;

	public State(string name)
	{
		this.name = name;
	}
}
