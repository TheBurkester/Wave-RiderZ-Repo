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
	public KeyCode TetherShorten;
	public KeyCode TetherLengthen;

	public Transform planeTetherPoint = null;

	private Vector3 m_velocity;
	private Vector3 m_drag;
	public float backwardsDrag = 5;
	public float resistanceX = 1.1f;
	public float resistanceZ = 1.1f;
	public float velocityCapX = 2.5f;
	public float velocityCapZ = 2.5f;
	public float tetherLength = 8;
	public float tetherChangeSpeed = 0.2f;
	public float movingForce = 5;	//How fast the skier moves left/right

	//private Rigidbody rb = null;    //Keep reference to the plane rigidbody
	
	void Awake()
    {
		//rb = GetComponent<Rigidbody>();
		//Debug.Assert(rb != null, "Skier missing rigidbody component");
		Debug.Assert(planeTetherPoint != null, "Skier missing plane rigidbody reference");

		m_drag = new Vector3(0, 0, -backwardsDrag);
	}
	
    void Update()
    {
		Vector3 forceToApply = new Vector3(0, 0, 0);              //Current force, deleted at the end of update
		Vector3 distance = transform.position - planeTetherPoint.position;

		if (Input.GetKey(TetherLengthen))
			tetherLength += tetherChangeSpeed;
		if (Input.GetKey(TetherShorten))
			tetherLength -= tetherChangeSpeed;

		if (distance.magnitude >= (tetherLength - 5))
		{
			//Vector3 tangent = new Vector2();
			if (Input.GetKey(MoveRight))     //If w is pressed,
			{
				//tangent = Vector2(distance.y, -distance.x);	//Tangent Vector = (y, -x)
				//tangent.normalise();
				forceToApply.x += movingForce;                     //Apply an upwards force
														//force = tangent * m_Force;
			}
			if (Input.GetKey(MoveLeft))     //If s is pressed,
			{
				//tangent = Vector2(-distance.y, distance.x);	//Tangent Vector = (-y, x)
				//tangent.normalise();
				forceToApply.x -= movingForce;                     //Apply a downwards force
														//force = tangent * m_Force;
			}
		}

		Vector3 testVelocity = m_velocity + (m_drag + forceToApply) * Time.deltaTime;               //V += a*t
		//if (!Input.GetKey(MoveLeft) && !Input.GetKey(MoveRight))						//If not currently accelerating,
			//testVelocity.x /= 1.05f;                                                    //Start slowing down by 5% per frame
		if (testVelocity.x > velocityCapX || testVelocity.x < -velocityCapX)              //If the new velocity is too high,
			testVelocity.x /= resistanceX;                                        //Reduce the velocity by 5% so it doesn't stay high for long
		if (testVelocity.z > velocityCapZ || testVelocity.z < -velocityCapZ)              //If the new velocity is too high,
			testVelocity.z /= resistanceZ;                                        //Reduce the velocity by 5% so it doesn't stay high for long

		//if (testVelocity.x > velocityCapX)              //If the new velocity is too high,
		//	testVelocity.x = velocityCapX;                                        //Reduce the velocity by 5% so it doesn't stay high for long
		//if (testVelocity.x < -velocityCapX)              //If the new velocity is too high,
		//	testVelocity.x = -velocityCapX;                                        //Reduce the velocity by 5% so it doesn't stay high for long
		//if (testVelocity.z > velocityCapZ)              //If the new velocity is too high,
		//	testVelocity.z = resistanceZ;                                        //Reduce the velocity by 5% so it doesn't stay high for long
		//if (testVelocity.z < -velocityCapZ)              //If the new velocity is too high,
		//	testVelocity.z = -resistanceZ;                                        //Reduce the velocity by 5% so it doesn't stay high for long

		Vector3 testPosition = transform.position + (testVelocity) * Time.deltaTime;   //Where is it going to go next?
		Vector3 testDistance = testPosition - planeTetherPoint.position;                //Distance between the new point and the plane
		if (testDistance.magnitude > tetherLength)                              //If the new point is outside the length of the rope,
			testPosition = planeTetherPoint.position + testDistance.normalized * tetherLength;  //Pull it back to the rope length in the direction of the rope

		m_velocity = (testPosition - transform.position) / Time.deltaTime;   //Adjust new velocity to force it to move to the new position
		transform.position = testPosition;                              //Move to the new position

		Debug.Log(m_velocity.z);
		//if (Input.GetKey(MoveLeft))		//If left is pressed,
		//	rb.AddForce(0, 0, movingForce);	//Apply a force left
		//if (Input.GetKey(MoveRight))	//If right is pressed,
		//	rb.AddForce(0, 0, -movingForce);	//Apply a force right
	}
}
