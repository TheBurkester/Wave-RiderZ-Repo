using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

public class PrefabLoader : MonoBehaviour
{
	private GameObject[] m_prefabs;
	private bool[] m_prefabActive;
	public PlaneController plane = null;
	public GameManager gameManager = null;
	public int numberOfPrefabs = 5;
	public int forwardsOffset = 20;

    void Start()
    {
		m_prefabs = Resources.LoadAll<GameObject>("Obstacles"); //Load all the prefabs in the /Resources/Obstacles folder and store them in an array

		//numberOfPrefabs = plane.forwardSpeed * gameManager.roundTimeLimit;

		Debug.Assert(m_prefabs.Length >= numberOfPrefabs, "Number of required prefabs is larger than the number of available prefabs.");

		m_prefabActive = new bool[m_prefabs.Length];

		for (int i = 0; i < numberOfPrefabs; ++i)
		{
			int prefabToSpawn = UnityEngine.Random.Range(0, m_prefabs.Length);
			if (m_prefabActive[prefabToSpawn] == false)
			{
				if (m_prefabs[prefabToSpawn] != null)
				{
					Vector3 positionAlongRiver = new Vector3(0, 0, 40 * i + forwardsOffset);
					m_prefabs[prefabToSpawn].transform.position = positionAlongRiver;
					Instantiate(m_prefabs[prefabToSpawn]);
					m_prefabActive[prefabToSpawn] = true;
				}
			}
			else
			{
				--i;
			}
		}
	}
}
