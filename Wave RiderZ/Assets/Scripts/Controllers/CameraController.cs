/*-------------------------------------------------------------------*
|  Title:			CameraController
|
|  Author:			Seth Johnston
| 
|  Description:		Handles the movement of the camera.
*-------------------------------------------------------------------*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
	public PlaneController plane = null;	//Reference to the plane 

	private float m_speed = 5;				//How fast the camera moves up the river

    private Rigidbody rb = null;
	
    void Awake()
	{
		Debug.Assert(plane != null, "CameraController script missing PlaneController reference");
		m_speed = plane.forwardSpeed;

        rb = GetComponent<Rigidbody>();
		Debug.Assert(rb != null, "Camera missing rigidbody component");
    }
	
	void Update()
	{
        Vector3 newPos = rb.position + new Vector3(m_speed * Time.deltaTime, 0, 0);     //New position is the current position moved forward slightly
		rb.MovePosition(newPos);														//Move the camera forward
		//Keep in mind MovePosition doesn't update the position until the end of the frame
    }
}
