/*-------------------------------------------------------------------*
|  Title:			SkierController
|
|  Author:			Seth Johnston / Thomas Maltezos
| 
|  Description:		Handles the skier's movement.
*-------------------------------------------------------------------*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XboxCtrlrInput;

public class SkierController : MonoBehaviour
{
	//Movement
	public XboxController controller;	//Reference to the in-scene assigned controller
	public float movingForce = 5;   //How fast the skier moves sideways
	public float bonkForce = 10;    //How strong bonking other players is

	//Keyboard controls
	public KeyCode MoveLeft;		//Which keyboard key moves the skier left
    public KeyCode MoveRight;       //Which keyboard key moves the skier right
	public KeyCode TetherLengthen;	//Which keyboard key lengthens the rope
	public KeyCode TetherShorten;	//Which keyboard key shortens the rope

	//Score/lives
	private int m_playerScore = 0;		// Player's score.
	public int coinScore = 2;			// Score increased everytime collision with a coin occurs.
	public int skierScoreInc = 1;		// Increase every second.
	public int planeScoreInc = 5;		// Increase every time a skier loses a life.
	public int planeBonus = 10;			// Bonus is added if all skiers are eliminated.
	public int skierBonus = 10;			// Bonus is added if a skier survives the round.

	private int skierMultiplier = 1;	// Skier's multiplier.

	private Timer m_scoreTimer;			// Timer used to increment score.
	private Timer m_skierMultiplier;	// 
	public int skierLives = 3;			// The amount of lives the skiers will have.
	private bool m_isAlive = false;		// If the skier has the will to live

	//Invincibility
	public int numberOfFlashes = 3;			//How many times the mesh should flash when damaged
	public float flashDelay = 0.3f;			//How fast the mesh should flash on and off when damaged
	private bool m_invincible = false;		//If the skier can collide with other objects
	private MeshRenderer m_meshRend = null;	//Reference to the mesh to be flashed during invincibility

	[HideInInspector]
	public Tether tether = null;	//Reference to the tether attached to this skier, public so forces can be applied from other scripts


	void Awake()
    {
		tether = GetComponent<Tether>();
		Debug.Assert(tether != null, "Skier missing tether component");

		m_meshRend = GetComponent<MeshRenderer>();
	}

	void Start()
	{
		m_scoreTimer = gameObject.AddComponent<Timer>();	//Create the score timer
		m_scoreTimer.maxTime = 1;							//The timer will go for one second,
		m_scoreTimer.autoDisable = true;					//then automatically disable and be reset
		m_scoreTimer.SetTimer();							//Start the timer
	}


	private void FixedUpdate()
	{
		tether.forceToApply = new Vector3(0, 0, 0);  //Reset the previous frame's force before any physics/updates
	}

	private void Update()
    {
		//KEYBOARD
		//---------------------------------------------
		//Tether movement
		if (Input.GetKey(TetherLengthen))									//If pressing the lengthen key,
			tether.currentLength += tether.changeSpeed * Time.deltaTime;	//Make the tether longer over time
		if (Input.GetKey(TetherShorten))									//If pressing the shorten key,
			tether.currentLength -= tether.changeSpeed * Time.deltaTime;	//Make the tether shorter over time

		//Sideways movement
		if (tether.Distance() >= (tether.currentLength * 0.95))	//As long as the skier is close to the arc of the tether,
		{
			if (Input.GetKey(MoveRight))				//If the right key is pressed,
				tether.forceToApply.x += movingForce;	//Apply a force to the right
			if (Input.GetKey(MoveLeft))					//If the left key is pressed,
				tether.forceToApply.x -= movingForce;	//Apply a force to the left
		}
		//---------------------------------------------

		//XBOX
		//---------------------------------------------
		//Tether movement
		float axisY = XCI.GetAxis(XboxAxis.RightStickY, controller);			//Store the direction and magnitude of the right joystick
		tether.currentLength += tether.changeSpeed * -axisY * Time.deltaTime;	//Move tether length up/down based on this

		//Sideways movement
		if (tether.Distance() >= (tether.currentLength * 0.95))			//As long as the skier is close to the arc of the tether,
		{
			float axisX = XCI.GetAxis(XboxAxis.LeftStickX, controller);	//Store the direction and magnitude of the left joystick
			tether.forceToApply.x += movingForce * axisX;				//Add a left/right force to the tether movement script
		}
		//---------------------------------------------

		if (!m_scoreTimer.UnderMax())			//If the score timer goes past 1,
		{
			m_playerScore += skierScoreInc;		//Increment the score once
			m_scoreTimer.SetTimer();			//Reset the timer
		}
	}

	private void OnTriggerEnter(Collider other)
	{
		if (!m_invincible)					//If the skier is not currently invincible,
		{
			if (other.CompareTag("Skier"))	//If the other object is a skier,
			{
				Tether otherTether = other.GetComponent<Tether>();			//Get the other skier's tether
				otherTether.forceToApply += bonkForce * tether.Direction();	//Add a force to the other skier, in the direction which this skier is currently moving
			}

			if (other.CompareTag("Coin"))	//If the other object is a coin,
				m_playerScore += coinScore;	//Add a coin's worth of points to the score

			if (other.CompareTag("Rock"))	//If the other object is a rock,
				HurtSkier();				//Hurt the skier
		}
	}
	
	public void SetPlayerScore(int score)
	{
		m_playerScore = score;
	}
	public int GetPlayerScore()
	{
		return m_playerScore;
	}

	public void SetAlive(bool value)
	{
		m_isAlive = value;
	}
	public bool GetAlive()
	{
		return m_isAlive;
	}

	public void HurtSkier()
	{
		if (!m_invincible)
			skierLives--;               //Subtract a life

		if (skierLives <= 0)                //If the skier is out of lives,
		{
			m_isAlive = false;              //Kill it
			gameObject.SetActive(false);    //Disable it
		}
		else						//If the skier is still alive,
		{
			m_invincible = true;    //Make it invincible

			//Flash the skier mesh
			for (int i = 0; i < numberOfFlashes; ++i)                       //Repeating for the number of flashes,
			{
				StartCoroutine(MeshOff(flashDelay * i * 2));                //Schedule the mesh to turn off, every even interval
				StartCoroutine(MeshOn(flashDelay * i * 2 + flashDelay));    //Schedule the mesh to turn on, every odd interval
			}

			StartCoroutine(InvincibleOff(flashDelay * numberOfFlashes * 2));	//Schedule invincibility to turn off after the flashes are complete
		}
	}

	public bool isInvincible()
	{
		return m_invincible;
	}

	IEnumerator MeshOff(float interval)
	{
		yield return new WaitForSeconds(interval);  //Wait for a certain amount of time
		m_meshRend.enabled = false;					//Turn the mesh off
	}
	IEnumerator MeshOn(float interval)
	{
		yield return new WaitForSeconds(interval);  //Wait for a certain amount of time
		m_meshRend.enabled = true;					//Turn the mesh on
	}
	IEnumerator InvincibleOff(float interval)
	{
		yield return new WaitForSeconds(interval);  //Wait for a certain amount of time
		m_invincible = false;                       //Make vincible
	}
}
