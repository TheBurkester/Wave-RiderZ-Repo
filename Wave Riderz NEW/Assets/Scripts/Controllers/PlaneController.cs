﻿/*-------------------------------------------------------------------*
|  Title:			PlaneController
|
|  Author:			Seth Johnston / Max Atkinson
| 
|  Description:		Handles the plane's movement.
*-------------------------------------------------------------------*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XboxCtrlrInput;

public class PlaneController : MonoBehaviour
{
	//Movement
	[HideInInspector]
    public XboxController controller;	//Stores which controller is meant to control the plane
    public float forwardSpeed = 5;		//How fast the plane moves up the river
	public float strafeSpeed = 3;       //How fast the plane can move left/right

	//Visual tilting
    public float tiltAngle = 20.0f;     //How far the plane tilts when moving left/right
    public float tiltSmoothness = 2.0f; //How quickly the plane tilts
	public GameObject beachBallTarget = null; // Reference to the target so that when using the beach ball ability, the rotation of the plane stops.
	private BeachBallAbility m_bba = null;

	//Clamping
	public Transform river = null;		//Reference to the river transform
	public float riverBorderSize = 0;	//The minimum distance the plane must be from the sides of the river
	private float m_clampWidth = 75;    //How far side-to-side the plane can move, calculated automatically, default 75

	//Swinging
	public float swingMoveTimeRequirement = 2;
	public float swingForce = 100;
	public float swingForceDuration = 0.5f;
	private Tether[] m_skierTethers;
	private Timer m_movementTimer;
	private int m_movementDirection = 0;

	private Rigidbody rb = null;        //Keep reference to the plane rigidbody

	void Awake()
    {
		m_bba = beachBallTarget.GetComponent<BeachBallAbility>();
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

	private void Start()
	{
		m_movementTimer = gameObject.AddComponent<Timer>();		//Create the timer
		m_movementTimer.maxTime = swingMoveTimeRequirement;		//Set the max time
		m_movementTimer.autoDisable = true;						//Make the timer stop when it's reached max
	}

	void Update()
    {		
		Vector3 newPos = rb.position + new Vector3(0, 0, forwardSpeed * Time.deltaTime);	//New position is the current position moved forward slightly

		//Controller movement
        float axisX = XCI.GetAxis(XboxAxis.LeftStickX, controller);			//Get the direction and magnitude of the controller stick
        newPos.x += (axisX * strafeSpeed * Time.deltaTime);                 //Move the plane in that direction and with that % magnitude (0-1)

		//Skier swinging
		if (axisX > 0.9f)					//If the plane is moving strongly right,
		{
			m_movementDirection = 1;		//Set direction to positive
			if (m_movementTimer.T == 0)		//If the timer hasn't been started yet,
				m_movementTimer.SetTimer();	//Start the timer
			//If the plane is moving but the timer is already started, 
			//Do nothing and let the timer keep counting
		}
		else if (axisX < -0.9f)
		{
			m_movementDirection = -1;        //Set direction to positive
			if (m_movementTimer.T == 0)     //If the timer hasn't been started yet,
				m_movementTimer.SetTimer(); //Start the timer
			//If the plane is moving but the timer is already started, 
			//Do nothing and let the timer keep counting
		}
		else if (m_movementTimer.UnderMax())	//Otherwise if the timer hasn't reached max yet,
			m_movementTimer.enabled = false;	//Turn the timer off
		else									//Otherwise the plane must have stopped moving whilst at max time,
		{
			ApplySwingForce();
			m_movementTimer.enabled = false;
		}

		//Rotation
		if (!m_bba.isShotting())
		{
			float tiltAroundZ = axisX * -tiltAngle;                             //What percentage of the tilt should be aimed for based on movement speed
			Quaternion targetRotation = Quaternion.Euler(0, 0, tiltAroundZ);    //Set the target rotation
			rb.transform.rotation = Quaternion.Slerp(rb.transform.rotation, targetRotation, tiltSmoothness * Time.deltaTime);   //Move towards the target rotation
		}
		else
		{
			Quaternion targetRotation = Quaternion.Euler(0, 0, 0);    //Set the target rotation
			rb.transform.rotation = Quaternion.Slerp(rb.transform.rotation, targetRotation, tiltSmoothness * Time.deltaTime);   //Move towards the target rotation
		}
		
		//Keyboard movement
		if (Input.GetKey(KeyCode.LeftArrow) )							//If left is pressed,
			newPos += new Vector3(-strafeSpeed * Time.deltaTime, 0, 0);	//Move the plane to the left
        if (Input.GetKey(KeyCode.RightArrow))							//If right is pressed,
			newPos += new Vector3(strafeSpeed * Time.deltaTime, 0, 0);	//Move the plane to the right

		//Clamp the plane to stay within the river
		if (newPos.x < -m_clampWidth)
			newPos.x = -m_clampWidth;
		if (newPos.x > m_clampWidth)
			newPos.x = m_clampWidth;

		rb.transform.position = newPos;		//Set the new position
	}

	public void ApplySwingForce()
	{

	}

	public void SetTetherReferences(Tether[] skierTethers)
	{
		m_skierTethers = skierTethers;
	}
}
