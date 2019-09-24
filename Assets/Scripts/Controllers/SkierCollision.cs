using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollision : MonoBehaviour
{
	public GameObject objectToFlash;

	public float flashDelay = 0.3f;
	public int numberOfFlashes = 3;
	
    void Awake()
    {
       
    }
	
    void Update()
    {
        
    }

    void OnCollisionEnter(Collision collision)
    {
		if (collision.gameObject.CompareTag("Rock"))
		{
			for (int i = 0; i < numberOfFlashes; ++i)
			{
				StartCoroutine(waitChangeRed(flashDelay * i * 2));
				StartCoroutine(waitChangeWhite(flashDelay * i * 2 + flashDelay));
			}
		}
    }

    IEnumerator waitChangeRed(float interval)
    {
        yield return new WaitForSeconds(interval);
        //Renderer rend = GetComponent<Renderer>();
		//rend.material.SetColor("_Color", Color.red);
		objectToFlash.GetComponent<MeshRenderer>().enabled = false;
	}

    IEnumerator waitChangeWhite(float interval)
    {
        yield return new WaitForSeconds(interval);
		//Renderer rend = GetComponent<Renderer>();
		//rend.material.SetColor("_Color", Color.white);
		objectToFlash.GetComponent<MeshRenderer>().enabled = true;
	}
}
