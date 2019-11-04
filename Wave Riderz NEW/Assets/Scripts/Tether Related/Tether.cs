/*-------------------------------------------------------------------*
|  Title:			Tether
|
|  Author:			Seth Johnston
| 
|  Description:		Connects the gameobject to a point and pulls it around.
*-------------------------------------------------------------------*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Tether : MonoBehaviour
{
	public Transform tetherPoint = null;   //Reference to the point the skier should be tethered to
	private Vector3 tetherPosition;

	private Vector3 m_velocity;     //How fast the object is moving in which directions
	private Vector3 m_drag;         //How much drag force is applied to the object
	[HideInInspector]
	public Vector3 forceToApply;	//Extra force to put onto the object from other scripts
	private Timer m_forceTimer;

	public float backwardsDrag = 5;		//How much force will be applied backwards on the object

	public float currentLength = 10;	//How long the rope is currently
	public float changeSpeed = 2;		//How quickly the tether lengthens/shortens
	public float minLength = 6;			//How short the tether can become
	public float maxLength = 14;		//How long the tether can become

	public float velocityCapX = 2.5f;	//How fast the object can go sideways before slowing
	public float velocityCapZ = 2.5f;	//How fast the object can go front/back before slowing
	public float resistanceX = 1.1f;	//How quickly the object slows at the sideways velocity cap
	public float resistanceZ = 1.1f;	//How quickly the object slows at the front/back velocity cap

	void Start()
    {
		Debug.Assert(tetherPoint != null, "Object missing tether point reference");

		m_drag = new Vector3(0, 0, -backwardsDrag);     //Set the backwards drag
		tetherPosition = new Vector3(tetherPoint.position.x, 0, tetherPoint.position.z);    //Position the tether so it's beneath the plane at y = 0

		m_forceTimer = gameObject.AddComponent<Timer>();   //Create the timer
		m_forceTimer.autoDisable = true;
	}

	private void FixedUpdate()
	{
		if (!m_forceTimer.UnderMax())
			forceToApply = new Vector3(0, 0, 0);  //Reset the previous frame's force before any physics/updates
	}

	void Update()
    {
		//Update the tether position
		tetherPosition.x = tetherPoint.position.x;
		tetherPosition.z = tetherPoint.position.z;

		//Tether clamping
		if (currentLength > maxLength)                     //If the tether is too long,
			currentLength -= changeSpeed * Time.deltaTime; //Make it shorter
		if (currentLength < minLength)                     //If the tether is too short,
			currentLength += changeSpeed * Time.deltaTime; //Make it longer
		
		//Tether physics
		Vector3 testVelocity = m_velocity + (m_drag + forceToApply) * Time.deltaTime;   //V += a*t

		//Drag
		if (testVelocity.x > velocityCapX || testVelocity.x < -velocityCapX)    //If the new sideways velocity is too high,
			testVelocity.x /= resistanceX;                                      //Reduce the velocity so it doesn't stay high for long
		if (testVelocity.z > velocityCapZ || testVelocity.z < -velocityCapZ)    //If the new front/back velocity is too high,
			testVelocity.z /= resistanceZ;                                      //Reduce the velocity so it doesn't stay high for long

		//Tether physics
		Vector3 testPosition = transform.position + (testVelocity) * Time.deltaTime;    //Where is the object going to go next?
		Vector3 testDistance = testPosition - tetherPosition;							//Distance between the new point and the tether point
		if (testDistance.magnitude > (currentLength - 0.1f))							//If the new point is outside the length of the rope, or a tiny bit shorter,
			testPosition = tetherPosition + testDistance.normalized * currentLength;	//Pull it back to the rope length in the direction of the rope

		//Set the final values
		m_velocity = (testPosition - transform.position) / Time.deltaTime;  //Adjust the velocity to force it to move to the new position
		transform.position = testPosition;                                  //Move to the new position
	}

	public void ApplyForce(Vector3 force)
	{
		if (!m_forceTimer.UnderMax())
			forceToApply += force;
	}

	public void ForceOverTime(Vector3 force, float duration)
	{
		forceToApply = force;
		m_forceTimer.maxTime = duration;
		m_forceTimer.SetTimer();
		StartCoroutine(ReduceForce(force, duration));
	}

	IEnumerator ReduceForce(Vector3 force, float duration)
	{
		float time = 0;
		while (time < duration)
		{
			time += Time.deltaTime / duration;
			forceToApply = Vector3.Lerp(force, Vector3.zero, time);
			yield return null;
		}
	}

	//Returns the distance between the object and the tether point
	public float Distance()
	{
		return (transform.position - tetherPosition).magnitude;
	}

	//Returns the direction that the object is moving in
	public Vector3 Direction()
	{
		return m_velocity.normalized;
	}

	public float VelocityMagnitude()
	{
		return m_velocity.magnitude;
	}

	public float VelocityXMagnitude()
	{
		return Math.Abs(m_velocity.x);
	}

	public void ReduceVelocity(float reduction)
	{
		m_velocity /= reduction;
	}
}
