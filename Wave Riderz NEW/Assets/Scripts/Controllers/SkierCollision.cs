/*-------------------------------------------------------------------*
|  Title:			SkierCollision
|
|  Author:			Max Atkinson / Seth Johnston / Thomas Maltezos
| 
|  Description:		DEPRECIATED. THIS SCRIPT IS A LEGACY SCRIPT FOR REFERENCE ONLY.
*-------------------------------------------------------------------*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkierCollision : MonoBehaviour
{
    public GameObject objectToFlash = null;		//Reference to the gameobject with the mesh to flash
    public Rigidbody PlayerToBePushed;          // Player that will be pushed by other player.

    public float Force = 5000;                  // Force to be applied to other player.
    public float flashDelay = 0.3f;				//How fast the mesh should flash on and off
	public int numberOfFlashes = 3;             //How many times the mesh should flash

    private Rigidbody m_rb;                     // Rigidbody of the player.
    void Awake()
    {
        m_rb = GetComponent<Rigidbody>();
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
        else if (collision.gameObject.CompareTag("Player"))
        {
            PlayerToBePushed.AddForce(0, 0, -Force);
            m_rb.AddForce(0, 0, Force / 2);
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
