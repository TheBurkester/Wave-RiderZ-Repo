/*-------------------------------------------------------------------*
|  Title:			Timer
|
|  Author:		    Thomas Maltezos	/ Seth Johnston
| 
|  Description:		Handles timers.
*-------------------------------------------------------------------*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer : MonoBehaviour
{
	public float T { get; private set; }    //Property to set the timer from inside the class, but read only for outside

    public float maxTime;					//Optional maximum time
    public bool autoDisable = false;		//If the timer automatically stops counting when max is hit or not
	public bool reverseTimer = false;		//If the timer counts backwards from the max or not


	void Awake()
    {
		enabled = false;	//Timer doesn't immediately start counting when created
	}

	//Should be called after all the timer's options have been set,
	//to initialise the timer properly and start it
	public void Initialise()
	{
		T = 0.0f;			//Timer starts at 0
		if (reverseTimer)   //If using a reverse timer,
			T = maxTime;    //Start at the max instead

		enabled = true;		//Timer starts counting when initialised
	}

	void Update()
    {
		if (!reverseTimer)			//If it's a normal timer,
			T += Time.deltaTime;	//Add 1 per second, adjusted for framerate
		else						//If it's a reverse timer,
			T -= Time.deltaTime;	//Subtract 1 per second, adjusted for framerate


		if (autoDisable == true)	//If auto-disable is enabled,
        {
            if (!UnderMax())		//And the timer is within the timelimit,
				enabled = false;	//Stop counting
		}
    }

	//Returns true if the timer is under the max, and false otherwise
    public bool UnderMax()
    {
		if (!reverseTimer)
			return (T < maxTime);
		return (T > 0);
	}

	//Resets the timer and enables it
    public void ResetTimer()
    {
		T = 0.0f;
		if (reverseTimer)		//If using a reverse timer,
			T = maxTime;        //Reset to max instead
		enabled = true;
    }
}
