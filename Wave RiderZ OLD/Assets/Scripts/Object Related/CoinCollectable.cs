﻿/*-------------------------------------------------------------------*
|  Title:			CoinCollectable
|
|  Author:			Max Atkinson / Seth Johnston
| 
|  Description:		Handles the coin rotation and trigger.
*-------------------------------------------------------------------*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinCollectable : MonoBehaviour
{
	public float spinSpeed = 5;		//How fast the coin spins

    void Start()
    {
        
    }
	
    void Update()
    {
        transform.Rotate(0, 0, spinSpeed);		//Spin the coin around its z-axis (globally y-axis)
    }

    void OnTriggerEnter(Collider collider)
    {
		if (collider.gameObject.CompareTag("Skier"))				//If colliding with a skier,
			gameObject.GetComponent<Renderer>().enabled = false;	//Stop rendering the coin
    }
}