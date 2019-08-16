using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine
{
	private Dictionary<string, State> states = new Dictionary<string, State>();
	public State CurState { get; private set; }

	public StateMachine()
	{
		CurState = null;
	}

	public void AddNewState(string newStateName, State newState)
	{
		states.Add(newStateName, newState);
	}

	public void Transition(string stateName)
	{
		CurState?.Exit?.Invoke();
		CurState = states[stateName];
		CurState?.Enter?.Invoke();
	}

	/// <summary>
	/// Check current state's name is same with input
	/// </summary>
	/// <param name="stateName"></param>
	public bool IsState(string stateName)
	{
		return states[stateName] == CurState;
	}

	public void UpdateStateMachine()
	{
		CurState?.StateUpdate?.Invoke();
	}
}
