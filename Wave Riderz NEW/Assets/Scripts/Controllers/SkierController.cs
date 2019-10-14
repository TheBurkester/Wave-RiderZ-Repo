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

	public float movingForce = 5;   //How fast the skier moves sideways
	public float bonkForce = 10;    //How strong bonking other players is

	public int coinScore = 2;		// Score increased everytime collision with a coin occurs.
	public int playerScore = 1;		// Increase every second.
	public int planeScore = 5;		// Increase every time a skier loses a life.
	public int planeBonus = 10;		// Bonus is added if all skiers are eliminated.
	public int playerBonus = 10;    // Bonus is added if a skier survives the round.

	public GameObject player = null; // A reference to the player assigned to the cntroller.

	private int m_playerOneScore = 0;
	private int m_playerTwoScore = 0;
	private int m_playerThreeScore = 0;
	private int m_playerFourScore = 0;

	[HideInInspector]
	public Tether tether = null;

	void Awake()
    {
		tether = GetComponent<Tether>();
		Debug.Assert(tether != null, "Skier missing tether component");
	}

	private void FixedUpdate()
	{
		tether.forceToApply = new Vector3(0, 0, 0);  //Reset the previous frame's force before any physics/updates
	}

	private void Update()
    {
		//Tether movement
		if (Input.GetKey(TetherLengthen))									//If pressing the lengthen key,
			tether.currentLength += tether.changeSpeed * Time.deltaTime;	//Make the tether longer over time
		if (Input.GetKey(TetherShorten))									//If pressing the shorten key,
			tether.currentLength -= tether.changeSpeed * Time.deltaTime;	//Make the tether shorter over time

		//Sideways movement
		//tether.forceToApply = new Vector3(0, 0, 0);				//Reset the previous frame's force
		if (tether.Distance() >= (tether.currentLength * 0.95))	//As long as the skier is close to the arc of the tether,
		{
			if (Input.GetKey(MoveRight))				//If the right key is pressed,
				tether.forceToApply.x += movingForce;	//Apply a force to the right
			if (Input.GetKey(MoveLeft))					//If the left key is pressed,
				tether.forceToApply.x -= movingForce;	//Apply a force to the left
		}
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Skier"))
		{
			Tether otherTether = other.GetComponent<Tether>();
			otherTether.forceToApply += bonkForce * tether.Direction();
		}

		if (other.CompareTag("Coin"))
		{

		}
	}
}
