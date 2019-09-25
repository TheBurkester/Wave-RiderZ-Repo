/*-------------------------------------------------------------------*
|  Title:			SkierCollision
|
|  Author:			Max Atkinson / Seth Johnston
| 
|  Description:		Handles the skier's collision with obstacles.
*-------------------------------------------------------------------*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkierCollision : MonoBehaviour
{
	public GameObject objectToFlash = null;		//Reference to the gameobject with the mesh to flash

	public float flashDelay = 0.3f;				//How fast the mesh should flash on and off
	public int numberOfFlashes = 3;				//How many times the mesh should flash
	
    void Awake()
    {
       
    }
	
    void Update()
    {
        
    }

    void OnCollisionEnter(Collision collision)
    {
		if (collision.gameObject.CompareTag("Rock"))						//If the skier collides with an obstacle,
		{
			for (int i = 0; i < numberOfFlashes; ++i)						//Repeating for the number of flashes,
			{
				StartCoroutine(MeshOff(flashDelay * i * 2));				//Schedule the mesh to turn off, every even interval
				StartCoroutine(MeshOn(flashDelay * i * 2 + flashDelay));	//Schedule the mesh to turn on, every odd interval
			}
		}
    }

    IEnumerator MeshOff(float interval)
    {
        yield return new WaitForSeconds(interval);						//Wait for a certain amount of time
		objectToFlash.GetComponent<MeshRenderer>().enabled = false;		//Turn the mesh off
	}

    IEnumerator MeshOn(float interval)
    {
        yield return new WaitForSeconds(interval);						//Wait for a certain amount of time
		objectToFlash.GetComponent<MeshRenderer>().enabled = true;      //Turn the mesh on
	}
}
