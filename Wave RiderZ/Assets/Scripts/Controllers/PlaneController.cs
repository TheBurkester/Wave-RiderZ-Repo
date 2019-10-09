/*-------------------------------------------------------------------*
|  Title:			PlaneController
|
|  Author:			Seth Johnston & Max Atkinson
| 
|  Description:		Handles the plane's movement.
*-------------------------------------------------------------------*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XboxCtrlrInput;
using XInputDotNetPure;

public class PlaneController : MonoBehaviour
{
    public XboxController controller; // apart of the xbox controls
    private Vector3 newPosition;
    public float forwardSpeed = 5;		//How fast the plane moves up the river

	public float strafeSpeed = 3;       //How fast the plane can move left/right

    public float tiltAngle = 20.0f;     //How far the plane tilts when moving left/right
    public float tiltSmoothness = 2.0f; //How quickly the plane tilts

	public Transform river = null;		//Reference to the river transform
	public float riverBorderSize = 0;	//The minimum distance the plane must be from the sides of the river
	private float m_clampWidth = 75;	//How far side-to-side the plane can move, calculated automatically, default 75

	private Rigidbody rb = null;        //Keep reference to the plane rigidbody

	void Awake()
    {
		rb = GetComponent<Rigidbody>();
		Debug.Assert(rb != null, "Plane missing rigidbody component");

		Debug.Assert(river != null, "Plane missing river reference");
		if (river != null)
		{
			//Multiply the river scale by 5 because the river is a 'plane' object,
			//and 1 unit of plane scale is equal to 5 units in space
			m_clampWidth = (river.localScale.x * 5) - riverBorderSize;
			if (m_clampWidth <= 0)  //If the clamp width is too small,
				m_clampWidth = 75;	//Reset to default
		}
	}
    
    void Update()
    {		
		Vector3 newPos = rb.position + new Vector3(0, 0, forwardSpeed * Time.deltaTime);	//New position is the current position moved forward slightly

		float tiltAroundZ = Input.GetAxis("Horizontal") * -tiltAngle;

		Quaternion Target = Quaternion.Euler(0, 0, tiltAroundZ);
		Quaternion Default = Quaternion.Euler(0, 0, 0);

		rb.transform.rotation = Quaternion.Slerp(rb.transform.rotation, Default, Time.deltaTime * tiltSmoothness);
        // Left Stick Movements 
        newPosition = transform.position;
        float axisX = XCI.GetAxis(XboxAxis.LeftStickX, controller);
        newPos.x += (axisX * strafeSpeed * Time.deltaTime);
     
        if (Input.GetKey(KeyCode.LeftArrow) )							//If left is pressed,
		{
			newPos += new Vector3(-strafeSpeed * Time.deltaTime, 0, 0);	//Move the plane to the left

			rb.transform.rotation = Quaternion.Slerp(rb.transform.rotation, Target, Time.deltaTime * tiltSmoothness);
		}
      
        if (Input.GetKey(KeyCode.RightArrow))								//If right is pressed,
		{
			newPos += new Vector3(strafeSpeed * Time.deltaTime, 0, 0);		//Move the plane to the right

			rb.transform.rotation = Quaternion.Slerp(rb.transform.rotation, Target, Time.deltaTime * tiltSmoothness);
		}

		//Clamp the plane to stay within the river
		if (newPos.x < -m_clampWidth)
			newPos.x = -m_clampWidth;
		if (newPos.x > m_clampWidth)
			newPos.x = m_clampWidth;

		rb.transform.position = newPos;		//Set the new position
	}
}
