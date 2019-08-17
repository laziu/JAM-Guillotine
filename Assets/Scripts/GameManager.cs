using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : SingletonBehaviour<GameManager>
{
	public int level;

	private void Awake()
	{
		if (inst != this)
			Destroy(gameObject);
		SetStatic();
		SceneManager.sceneLoaded += OnSceneLoaded;
	}

	private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
	{
		if (scene.name == "Ingame")
		{
			LoadLevel();
		}
	}

	private void LoadLevel()
	{
		Resources.Load<GameObject>("Stage/" + level.ToString());
	}
}
