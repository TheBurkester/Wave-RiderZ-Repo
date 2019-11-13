/*-------------------------------------------------------------------*
|  Title:			CoinCollectable
|
|  Author:			Max Atkinson / Thomas Maltezos
| 
|  Description:		Handles the coin rotation and trigger.
*-------------------------------------------------------------------*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinCollectable : MonoBehaviour
{
	public float spinSpeed = 180;     //How fast the coin spins
	
    void Update()
    {
        transform.Rotate(0, spinSpeed * Time.deltaTime, 0);		//Spin the coin around the Y axis
    }

    void OnTriggerEnter(Collider collider)
    {
		if (collider.gameObject.CompareTag("Skier"))                //If colliding with a skier,
		{
			SkierController skier = collider.gameObject.GetComponent<SkierController>();

            if (!skier.isInvincible())
            {
                gameObject.GetComponent<Renderer>().enabled = false;    //Stop rendering the coin
                AudioManager.Play("CoinCollected"); // plays coin sound effect 
            }
		}
    }
}
