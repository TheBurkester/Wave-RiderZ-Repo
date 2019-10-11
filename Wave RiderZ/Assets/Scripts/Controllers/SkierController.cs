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
    public KeyCode MoveLeft;		//Which keyboard key moves the skier left
    public KeyCode MoveRight;       //Which keyboard key moves the skier right
	public KeyCode TetherLengthen;	//Which keyboard key lengthens the rope
	public KeyCode TetherShorten;	//Which keyboard key shortens the rope

	public Transform planeTetherPoint = null;	//Reference to the point the skier should be tethered to

	private Vector3 m_velocity;		//How fast the skier is moving in which directions
	private Vector3 m_drag;			//How much drag force is applied to the skier

	public float movingForce = 5;			//How fast the skier moves sideways
	public float tetherLength = 8;			//How long the rope is
	public float tetherChangeSpeed = 0.2f;	//How quickly the tether lengthens/shortens
	public float backwardsDrag = 5;			//How much force will be applied backwards on the skier

	public float velocityCapX = 2.5f;		//How fast the skier can go sideways before slowing
	public float velocityCapZ = 2.5f;		//How fast the skier can go front/back before slowing
	public float resistanceX = 1.1f;		//How quickly the skier slows at the sideways velocity cap
	public float resistanceZ = 1.1f;        //How quickly the skier slows at the front/back velocity cap

	void Awake()
    {
		Debug.Assert(planeTetherPoint != null, "Skier missing plane tether point reference");

		m_drag = new Vector3(0, 0, -backwardsDrag);		//Set the backwards drag
	}
	
    void Update()
    {
		Vector3 forceToApply = new Vector3(0, 0, 0);	//Current force, deleted at the end of update
		Vector3 distance = transform.position - planeTetherPoint.position;	//Distance between the skier and the tether point

		//Tether movement
		if (Input.GetKey(TetherLengthen))						//If pressing the lengthen key,
			tetherLength += tetherChangeSpeed * Time.deltaTime;	//Make the tether longer over time
		if (Input.GetKey(TetherShorten))						//If pressing the shorten key,
			tetherLength -= tetherChangeSpeed * Time.deltaTime;	//Make the tether shorter over time

		//Sideways movement
		if (distance.magnitude >= (tetherLength * 0.95))	//As long as the skier is close to the arc of the tether,
		{
			//Vector3 tangent = new Vector2();
			if (Input.GetKey(MoveRight))		//If the right key is pressed,
			{
				forceToApply.x += movingForce;	//Apply a force to the right

				//tangent = Vector2(distance.y, -distance.x);	//Tangent Vector = (y, -x)
				//tangent.normalise();
				//force = tangent * m_Force;
			}
			if (Input.GetKey(MoveLeft))			//If the left key is pressed,
			{
				forceToApply.x -= movingForce;	//Apply a force to the left

				//tangent = Vector2(-distance.y, distance.x);	//Tangent Vector = (-y, x)
				//tangent.normalise();
				//force = tangent * m_Force;
			}
		}


		//Tether physics
		Vector3 testVelocity = m_velocity + (m_drag + forceToApply) * Time.deltaTime;	//V += a*t

		//Drag
		//if (!Input.GetKey(MoveLeft) && !Input.GetKey(MoveRight))						//If not currently accelerating,
			//testVelocity.x /= 1.05f;                                                    //Start slowing down by 5% per frame
		if (testVelocity.x > velocityCapX || testVelocity.x < -velocityCapX)	//If the new sideways velocity is too high,
			testVelocity.x /= resistanceX;										//Reduce the velocity so it doesn't stay high for long
		if (testVelocity.z > velocityCapZ || testVelocity.z < -velocityCapZ)	//If the new front/back velocity is too high,
			testVelocity.z /= resistanceZ;										//Reduce the velocity so it doesn't stay high for long

		//if (testVelocity.x > velocityCapX)
		//	testVelocity.x = velocityCapX;	
		//if (testVelocity.x < -velocityCapX)
		//	testVelocity.x = -velocityCapX;	
		//if (testVelocity.z > velocityCapZ)
		//	testVelocity.z = resistanceZ;	
		//if (testVelocity.z < -velocityCapZ)
		//	testVelocity.z = -resistanceZ;	

		Vector3 testPosition = transform.position + (testVelocity) * Time.deltaTime;	//Where is the skier going to go next?
		Vector3 testDistance = testPosition - planeTetherPoint.position;                //Distance between the new point and the plane
		if (testDistance.magnitude > tetherLength)												//If the new point is outside the length of the rope,
			testPosition = planeTetherPoint.position + testDistance.normalized * tetherLength;  //Pull it back to the rope length in the direction of the rope

		m_velocity = (testPosition - transform.position) / Time.deltaTime;	//Adjust the velocity to force it to move to the new position
		transform.position = testPosition;									//Move to the new position
	}
}
