/*-------------------------------------------------------------------*
|  Title:			Timer
|
|  Author:		    Seth Johnston / Thomas Maltezos
| 
|  Description:		Component which handles a timer to use in other scripts.
*-------------------------------------------------------------------*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Timer : MonoBehaviour
{
	public float T { get; private set; }    //Property to set the timer from inside the class, but read only for outside

    public float maxTime;					//Optional maximum time
    public bool autoDisable = false;		//If the timer automatically stops counting when max is hit or not
	public bool autoRepeat = false;			//If the timer automatically restarts when max is hit or not
	public bool reverseTimer = false;       //If the timer counts backwards from the max or not

	private int m_timerDirection;           //If the timer is counting up or down, stored as positive or negative 1

	public delegate void RepeatFunction();	//Defines a delegate which can take in a function with void return type and no parameters
	private RepeatFunction handler = null;	//References an optional function to call when an auto-repeat timer repeats

	void Awake()
    {
		enabled = false;	//Timer doesn't immediately start counting when created
	}

	//Should be called after all the timer's options have been set,
	//to initialise the timer properly and start it.
	//Acts as a reset timer function too
	public void SetTimer()
	{
		T = 0.0f;				//Timer starts at 0
		m_timerDirection = 1;	//Timer default to counting up
		if (reverseTimer)			//If using a reverse timer,
		{
			T = maxTime;			//Start at the max instead
			m_timerDirection = -1;	//Set the timer to count down
		}

		enabled = true;		//Timer starts counting when initialised
	}

	void Update()
    {
		T += Time.deltaTime * m_timerDirection;		//Add or subtract 1 per second

		if (!UnderMax())			//If over max,
		{
			if (autoDisable)        //If auto-disable is on,
				enabled = false;    //Disable the timer
			else if (autoRepeat)    //If auto-repeat is on,
			{
				T += maxTime * -m_timerDirection;   //Add or subtract the max time to 'reset' close to 0 or max
				if (handler != null)				//If a function is set,
					handler();						//Call the repeat function
			}
		}
    }

	//Returns true if the timer is under the max, and false otherwise
    public bool UnderMax()
    {
		if (!reverseTimer)
			return (T < maxTime);
		return (T > 0);
	}

	//Sets the repeat function to a function from another script
	public void SetRepeatFunction(RepeatFunction function)
	{
		handler = function;
	}

	//Return the time rounded up to the nearest whole number
	public int RoundedUp()
	{
		return (int)Math.Ceiling(T);
	}
}