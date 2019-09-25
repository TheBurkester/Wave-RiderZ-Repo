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
    public bool autoDisable = false;	//If the timer automatically stops counting when max is hit or not

    private float m_timer;				//The timer

    void Awake()
    {
        m_timer = 0.0f;		//Timer starts at 0
    }

    void Update()
    {
        m_timer += UnityEngine.Time.deltaTime;	//Add 1 per second, adjusted for framerate

        if (autoDisable == true)				//If auto-disable is enabled,
        {
            if (!UnderMax())					//And the timer is over the max time,
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
        return (m_timer < maxTime);
    }

	//Sets the timer to 0 and enables it
    public void ResetTimer()
    {
        m_timer = 0.0f;
        gameObject.SetActive(true);
    }

	//Enables/disables the timer
    public void SetActive(bool value)
    {
        gameObject.SetActive(value);
    }
}
