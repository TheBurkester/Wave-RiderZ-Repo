/*-------------------------------------------------------------------*
|  Timer
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
    public float maxTime;
    public bool autoDisable = false;
    private float m_timer;

    void Awake()
    {
        m_timer = 0.0f;
    }

    void Update()
    {
        m_timer += Time.deltaTime;

        if (autoDisable == true)
        {
            if (!UnderMax()) // OVER MAX.
            {
                gameObject.SetActive(false);
            }
        }
    }

    public float ReturnTimer()
    {
        return m_timer;
    }

    public bool UnderMax()
    {
        return (m_timer < maxTime);
    }

    public void ResetTimer()
    {
        m_timer = 0.0f;
        gameObject.SetActive(true);
    }

    public void SetActive(bool enable)
    {
        gameObject.SetActive(enable);
    }
}
