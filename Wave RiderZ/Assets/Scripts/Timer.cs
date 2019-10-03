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
    public float maxTime;				//Optional maximum time
    public bool autoDisable = false;    //If the timer automatically stops counting when max is hit or not
	public bool reverseTimer = false;	//If the timer counts backwards from the max or not

    private float m_timer;				//The timer

    void Awake()
    {
        m_timer = 0.0f;		//Timer starts at 0
		if (reverseTimer)		//If using a reverse timer,
			m_timer = maxTime;	//Start at the max instead

		SetActive(false);	//Timer doesn't immediately start counting when created
	}

    void Update()
    {
		if (!reverseTimer)							//If it's a normal timer,
			m_timer += UnityEngine.Time.deltaTime;  //Add 1 per second, adjusted for framerate
		else										//If it's a reverse timer,
			m_timer -= UnityEngine.Time.deltaTime;  //Subtract 1 per second, adjusted for framerate

		if (autoDisable == true)				//If auto-disable is enabled,
        {
            if (!UnderMax())					//And the timer is within the timelimit,
                gameObject.SetActive(false);	//Stop counting
		}
    }

	//Returns the current timer value
    public float Time()
    {
        return m_timer;
    }

	//Returns true if the timer is under the max, and false otherwise
    public bool UnderMax()
    {
		if (!reverseTimer)
			return (m_timer < maxTime);
		return (m_timer > 0);
	}

	//Resets the timer and enables it
    public void ResetTimer()
    {
        m_timer = 0.0f;
		if (reverseTimer)       //If using a reverse timer,
			m_timer = maxTime;  //Reset to max instead
		gameObject.SetActive(true);
    }

	//Enables/disables the timer
    public void SetActive(bool value)
    {
        gameObject.SetActive(value);
    }
}
