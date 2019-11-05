/*-------------------------------------------------------------------*
|  Title:			GameFreezer
|
|  Author:			Seth Johnston
| 
|  Description:		Acts like a static class which can pause the game from anywhere.
*-------------------------------------------------------------------*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameFreezer : MonoBehaviour
{
	public static GameFreezer instance;	//Store a static instance of itself

	private static float m_duration;	//How long the pause will be

	private void Awake()
	{
		instance = this;	//Set the instance to itself
	}

	//Callable without reference to the game freezer, starts the freeze coroutine
	public static void Freeze(float duration)
	{
		m_duration = duration;									//Set the duration
		instance.StartCoroutine(instance.FreezeCoroutine());	//Start the coroutine
	}

	//Freeze the timescale temporarily
	IEnumerator FreezeCoroutine()
	{
		Time.timeScale = 0;										//Stop time
		yield return new WaitForSecondsRealtime(m_duration);	//Wait for the duration using real seconds and not timescale
		Time.timeScale = 1;										//Return to normal speed
	}
}
