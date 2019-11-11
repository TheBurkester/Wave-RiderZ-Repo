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

	private static float m_duration;    //How long the pause will be
	private static int m_frames;		//How many frames to make the acceleration to normal speed take

	private void Awake()
	{
		instance = this;	//Set the instance to itself
	}

	//Callable without reference to the game freezer, starts the freeze coroutine
	public static void Freeze(float duration, int frames)
	{
		m_duration = duration;                                  //Set the duration
		m_frames = frames;										//Set the number of frames
		instance.StartCoroutine(instance.FreezeCoroutine());	//Start the coroutine
	}

	//Freeze the timescale temporarily
	IEnumerator FreezeCoroutine()
	{
		Time.timeScale = 0;													//Stop time
		while (Time.timeScale < 1)											//Until the timescale is normal speed,
		{
			Time.timeScale += 1 / (float)m_frames;							//Add a percentage of the number of frames
			yield return new WaitForSecondsRealtime(m_duration / m_frames);	//Wait for one percentage of frame, scaled by the duration
		}
		Time.timeScale = 1;													//Return to normal speed
	}
}
