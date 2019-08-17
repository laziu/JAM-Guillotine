using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IngameUIManager : SingletonBehaviour<IngameUIManager>
{
	public GameObject[] bodyHealthUIs = new GameObject[4];
	public GameObject[] headHealthUIs = new GameObject[2];

	public void UpdateBodyHealthUI(int health)
	{
		for (int i = 0; i < 4; ++i)
		{
			bodyHealthUIs[i].SetActive(i < health);
		}
	}

	public void UpdateHeadHealthUI(int health)
	{
		for (int i = 0; i < 2; ++i)
		{
			headHealthUIs[i].SetActive(i < health);
		}
	}
}
