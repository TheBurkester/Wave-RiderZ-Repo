/*-------------------------------------------------------------------*
|  Title:			PlaneController
|
|  Author:			Seth Johnston
| 
|  Description:		Handles the plane's movement.
*-------------------------------------------------------------------*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaneController : MonoBehaviour
{
	public float speed = 5;             //How fast the plane moves up the river
	public float strafeSpeed = 3;       //How fast the plane can move left/right

    public float tiltAngle = 20.0f;     //How far the plane tilts when moving left/right
    public float smoothness = 2.0f;     //How quickly the plane tilts

	private Rigidbody rb = null;        //Keep reference to the plane rigidbody

    
    void Awake()
    {
		rb = GetComponent<Rigidbody>();
		Debug.Assert(rb != null, "Plane missing rigidbody component");
	}
    
    void Update()
    {		
		Vector3 newPos = rb.position + new Vector3(speed * Time.deltaTime, 0, 0);   //New position is the current position moved forward slightly
        rb.transform.position = newPos;                                             //Set the new position

        float tiltAroundZ = Input.GetAxis("Horizontal") * -tiltAngle;

        Quaternion Target = Quaternion.Euler(0, 90, tiltAroundZ);
        Quaternion Default = Quaternion.Euler(0, 90, 0);

        rb.transform.rotation = Quaternion.Slerp(rb.transform.rotation, Default, Time.deltaTime * smoothness);

        if (Input.GetKey(KeyCode.LeftArrow))											//If left is pressed,
		{
			Vector3 strafe = newPos + new Vector3(0, 0, strafeSpeed * Time.deltaTime);	
            rb.transform.position = strafe;												//Move the plane to the left

            rb.transform.rotation = Quaternion.Slerp(rb.transform.rotation, Target, Time.deltaTime * smoothness);
        }
		if (Input.GetKey(KeyCode.RightArrow))											//If right is pressed,
		{
			Vector3 strafe = newPos + new Vector3(0, 0, -strafeSpeed * Time.deltaTime);
            rb.transform.position = strafe;												//Move the plane to the right

            rb.transform.rotation = Quaternion.Slerp(rb.transform.rotation, Target, Time.deltaTime * smoothness);
        }
    }
}
