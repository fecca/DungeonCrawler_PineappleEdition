using System;
using UnityEngine;

public class Game : MonoBehaviour
{
	[SerializeField]
	private WorldHandler WorldHandler;
	[SerializeField]
	private GameObject DudePrefab;

	private void Start()
	{
		WorldHandler.CreateDungeon();

		Instantiate(DudePrefab, Vector3.up, Quaternion.identity);
	}
}