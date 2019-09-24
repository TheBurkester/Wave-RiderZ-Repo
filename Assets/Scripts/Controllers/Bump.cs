/*-------------------------------------------------------------------*
|  Bump
|
|  Author:			Thomas Maltezos
| 
|  Description:		Handles the bump mechanic.
*-------------------------------------------------------------------*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bump : MonoBehaviour
{
	public float Force = 5000;


	public Rigidbody ObjectToBePushed;

	private Rigidbody rb = null;

	void Awake()
	{
		rb = GetComponent<Rigidbody>();
	}

    // Update is called once per frame
    void Update()
    {

	}

	void OnCollisionEnter(Collision collision)
	{
		if (collision.gameObject.tag == "Player")
		{
			ObjectToBePushed.AddForce(0, 0, -Force);
			rb.AddForce(0, 0, Force / 2);
		}
	}
}
