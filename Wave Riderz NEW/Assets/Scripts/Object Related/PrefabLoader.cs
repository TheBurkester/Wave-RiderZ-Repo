/*-------------------------------------------------------------------*
|  Title:			PrefabLoader
|
|  Author:			Seth Johnston
| 
|  Description:		Script which places groups of objects at runtime.
*-------------------------------------------------------------------*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

public class PrefabLoader : MonoBehaviour
{
	private GameObject[] m_obstacles;		//Array that contains all obstacle prefabs
	private bool[] m_obstacleActive;        //Array that stores if the obstacle prefab at the same index has been used or not

	private GameObject[] m_environment;     //Array that contains all environment prefabs
	private bool[] m_environmentActive;		//Array that stores if the environment prefab at the same index has been used or not

	public PlaneController plane = null;	//Reference to the plane for the plane speed
	public GameManager gameManager = null;	//Reference to the game manager for round timer

	private int m_numberOfPrefabs = 5;		//How many prefabs will be loaded to fill the level
	public int forwardsOffset = 20;			//How far in front of the plane the prefabs will start spawning

    void Start()
    {
		m_numberOfPrefabs = ((int)plane.forwardSpeed * (int)gameManager.roundTimeLimit) / 40;	//(Speed * Time) / prefab size


		//Obstacle Loading
		//---------------------------------------------------------------------
		m_obstacles = Resources.LoadAll<GameObject>("Obstacles");	//Load all the prefabs in the /Resources/Obstacles folder

		//Make sure there are enough prefabs present to fill the level
		Debug.Assert(m_obstacles.Length >= m_numberOfPrefabs, "Number of required obstacle prefabs is larger than the number of available prefabs.");

		m_obstacleActive = new bool[m_obstacles.Length];    //For the amount of prefabs loaded, add false bools to the array

		if (m_obstacles.Length >= m_numberOfPrefabs)		//Prevent getting Unity stuck in an infinite loop
		{
			for (int i = 0; i < m_numberOfPrefabs; ++i)                                             //For the amount of prefabs needed,
			{
				int prefabToSpawn = UnityEngine.Random.Range(0, m_obstacles.Length);                //Select a random index to use
				if (m_obstacleActive[prefabToSpawn] == false)                                       //If the prefab at the index isn't active,
				{
					if (m_obstacles[prefabToSpawn] != null)                                         //If the prefab exists,
					{
						Vector3 positionAlongRiver = new Vector3(0, 0, 40 * i + forwardsOffset);    //Calculate how far down the river it should spawn
						m_obstacles[prefabToSpawn].transform.position = positionAlongRiver;         //Set its position

						if (UnityEngine.Random.value > 0.5f)										//50% chance
							m_obstacles[prefabToSpawn].transform.localScale = new Vector3(-1, 1, 1);//Mirror the entire prefab horizontally

						Instantiate(m_obstacles[prefabToSpawn]);                                    //Spawn the prefab
						m_obstacleActive[prefabToSpawn] = true;                                     //Store that it has been spawned
					}
				}
				else        //If the prefab at the index is active,
					--i;    //Repeat the loop to find a different prefab
			}
		}
		//---------------------------------------------------------------------


		//Environment Loading
		//---------------------------------------------------------------------
		m_environment = Resources.LoadAll<GameObject>("Environment");	//Load all the prefabs in the /Resources/Environment folder

		//Make sure there are enough prefabs present to fill the level
		Debug.Assert(m_environment.Length >= m_numberOfPrefabs, "Number of required environment prefabs is larger than the number of available prefabs.");

		m_environmentActive = new bool[m_environment.Length];   //For the amount of prefabs loaded, add false bools to the array

		if (m_environment.Length >= m_numberOfPrefabs)			//Prevent getting Unity stuck in an infinite loop
		{
			for (int i = 0; i < m_numberOfPrefabs; ++i)                                                 //For the amount of prefabs needed,
			{
				int prefabToSpawn = UnityEngine.Random.Range(0, m_environment.Length);                  //Select a random index to use
				if (m_environmentActive[prefabToSpawn] == false)                                        //If the prefab at the index isn't active,
				{
					if (m_environment[prefabToSpawn] != null)                                           //If the prefab exists,
					{
						Vector3 positionAlongRiver = new Vector3(0, 0, 80 * i + forwardsOffset - 60);   //Calculate how far down the river it should spawn
						m_environment[prefabToSpawn].transform.position = positionAlongRiver;           //Set its position

						if (UnityEngine.Random.value > 0.5f)											//50% chance
							m_environment[prefabToSpawn].transform.localScale = new Vector3(-1, 1, 1);	//Mirror the entire prefab horizontally

						Instantiate(m_environment[prefabToSpawn]);                                      //Spawn the prefab
						m_environmentActive[prefabToSpawn] = true;                                      //Store that it has been spawned
					}
				}
				else        //If the prefab at the index is active,
					--i;    //Repeat the loop to find a different prefab
			}
		}
		//---------------------------------------------------------------------
	}
}
