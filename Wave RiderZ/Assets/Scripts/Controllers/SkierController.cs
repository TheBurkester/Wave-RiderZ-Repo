/*-------------------------------------------------------------------*
|  Title:			SkierController
|
|  Author:			Seth Johnston
| 
|  Description:		Handles the skier's movement.
*-------------------------------------------------------------------*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkierController : MonoBehaviour
{
	public float sidewaySpeed = 5;	//How fast the skier moves left/right
    public KeyCode MoveLeft;		//Which keyboard key moves the skier left
    public KeyCode MoveRight;		//Which keyboard key moves the skier right

	private Rigidbody rb = null;    //Keep reference to the plane rigidbody
	
	void Awake()
    {
		rb = GetComponent<Rigidbody>();
		Debug.Assert(rb != null, "Skier missing rigidbody component");
	}
	
    void Update()
    {
        if (Input.GetKey(MoveLeft))		//If left is pressed,
			rb.AddForce(0, 0, sidewaySpeed);	//Apply a force left
		if (Input.GetKey(MoveRight))	//If right is pressed,
			rb.AddForce(0, 0, -sidewaySpeed);	//Apply a force right
	}
}
