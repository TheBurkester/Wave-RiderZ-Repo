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
using XboxCtrlrInput;

public class SkierController : MonoBehaviour
{
	public XboxController controller;	//Reference to the manually assigned controller

	public KeyCode MoveLeft;		//Which keyboard key moves the skier left
    public KeyCode MoveRight;       //Which keyboard key moves the skier right
	public KeyCode TetherLengthen;	//Which keyboard key lengthens the rope
	public KeyCode TetherShorten;	//Which keyboard key shortens the rope

	public float movingForce = 5;   //How fast the skier moves sideways
	public float bonkForce = 10;    //How strong bonking other players is

	public int coinScore = 2;		// Score increased everytime collision with a coin occurs.
	public int skierScoreInc = 1;	// Increase every second.
	public int planeScoreInc = 5;	// Increase every time a skier loses a life.
	public int planeBonus = 10;		// Bonus is added if all skiers are eliminated.
	public int skierBonus = 10;     // Bonus is added if a skier survives the round.
	public int skierLives = 3;      // The amount of lives the skiers will have.

	private Timer m_scoreTimer;    // Timer used to increment score.
	private int m_playerScore = 0; // Player's Score.

	[HideInInspector]
	public Tether tether = null;

	void Awake()
    {
		tether = GetComponent<Tether>();
		Debug.Assert(tether != null, "Skier missing tether component");
	}

	void Start()
	{
		m_scoreTimer = gameObject.AddComponent<Timer>();
		m_scoreTimer.maxTime = 1;
		m_scoreTimer.autoDisable = true;
		m_scoreTimer.SetTimer();
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

		if (!m_scoreTimer.UnderMax())
		{
			m_playerScore += skierScoreInc;
			m_scoreTimer.SetTimer();
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
			m_playerScore += coinScore;
		}

		if (other.CompareTag("Rock"))
		{
			skierLives--;
		}
	}

	public int getPlayerScore()
	{
		return m_playerScore;
	}
}
