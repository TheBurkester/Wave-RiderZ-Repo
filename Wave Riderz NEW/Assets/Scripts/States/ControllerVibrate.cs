/*-------------------------------------------------------------------*
|  Title:			ControllerVibrate
|
|  Author:			Seth Johnston
| 
|  Description:		Acts like a static class which can vibrate controllers from anywhere.
*-------------------------------------------------------------------*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XInputDotNetPure;

public class ControllerVibrate : MonoBehaviour
{
	public static ControllerVibrate instance; //Store a static instance of itself

	private void Awake()
	{
		instance = this;    //Set the instance to itself
	}

	//Callable without reference to the script, starts the vibration coroutine on all controllers
	public static void VibrateAll(float amount, float duration)
	{
		instance.StartCoroutine(instance.VibrateAllRoutine(amount, duration));    //Start the coroutine
	}

	//Callable without reference to the script, starts the vibration coroutine on specified controller
	public static void VibrateController(int controller, float amount, float duration)
	{
		instance.StartCoroutine(instance.VibrateControllerRoutine(controller, amount, duration));    //Start the coroutine
	}

	//Vibrates all controllers for a given duration
	IEnumerator VibrateAllRoutine(float amount, float duration)
	{
		for (int i = 0; i < 4; ++i)
			GamePad.SetVibration((PlayerIndex)i, amount, amount);
		yield return new WaitForSecondsRealtime(duration);
		for (int i = 0; i < 4; ++i)
			GamePad.SetVibration((PlayerIndex)i, 0, 0);
	}
	//Vibrates specified controller for a given duration
	IEnumerator VibrateControllerRoutine(int controller, float amount, float duration)
	{
		GamePad.SetVibration((PlayerIndex)controller, amount, amount);
		yield return new WaitForSecondsRealtime(duration);
		GamePad.SetVibration((PlayerIndex)controller, 0, 0);
	}
}
