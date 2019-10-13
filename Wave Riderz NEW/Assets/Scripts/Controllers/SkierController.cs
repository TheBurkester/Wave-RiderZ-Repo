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

	public float movingForce = 5;           //How fast the skier moves sideways

	private Tether tether = null;

	void Awake()
    {
		tether = GetComponent<Tether>();
		Debug.Assert(tether != null, "Skier missing tether component");
	}
	
    void Update()
    {
		//Tether movement
		if (Input.GetKey(TetherLengthen))						//If pressing the lengthen key,
			tether.currentLength += tether.changeSpeed * Time.deltaTime;	//Make the tether longer over time
		if (Input.GetKey(TetherShorten))						//If pressing the shorten key,
			tether.currentLength -= tether.changeSpeed * Time.deltaTime; //Make the tether shorter over time

		//Sideways movement
		tether.forceToApply = new Vector3(0, 0, 0);		//Reset the previous frame's force
		if (tether.Distance() >= (tether.currentLength * 0.95))	//As long as the skier is close to the arc of the tether,
		{
			//Vector3 tangent = new Vector3();
			if (Input.GetKey(MoveRight))		//If the right key is pressed,
			{
				tether.forceToApply.x += movingForce;	//Apply a force to the right

				//tangent = Vector3(distance.y, -distance.x);	//Tangent Vector = (y, -x)
				//tangent.normalise();
				//force = tangent * m_Force;
			}
			if (Input.GetKey(MoveLeft))			//If the left key is pressed,
			{
				tether.forceToApply.x -= movingForce;	//Apply a force to the left

				//tangent = Vector3(-distance.y, distance.x);	//Tangent Vector = (-y, x)
				//tangent.normalise();
				//force = tangent * m_Force;
			}
		}
	}
}
