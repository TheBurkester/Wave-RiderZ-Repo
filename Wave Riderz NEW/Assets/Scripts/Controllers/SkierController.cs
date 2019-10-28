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
	public float bonkForce = 300;    //How strong bonking other players is
	public float obstacleForce = 2000;

	//Keyboard controls
	public KeyCode MoveLeft;		//Which keyboard key moves the skier left
    public KeyCode MoveRight;       //Which keyboard key moves the skier right
	public KeyCode TetherLengthen;	//Which keyboard key lengthens the rope
	public KeyCode TetherShorten;	//Which keyboard key shortens the rope

	//Score/lives
	private int m_playerScore = 0;		    // Player's score.
	public int coinScore = 2;			    // Score increased everytime collision with a coin occurs.
	public int skierScoreInc = 1;		    // Base Increase every second.
	public int planeScoreInc = 5;		    // Increase every time a skier loses a life.
	public int planeBonus = 10;			    // Bonus is added if all skiers are eliminated.
	public int skierBonus = 10;			    // Bonus is added if a skier survives the round.

    public int skierMultiplierSpeed = 5;	// The time it takes for the skier's multiplier to increase.
	public int skierMultiplierCap = 5;		// The max value that the multipler can be.
	private int m_skierMultiplier = 1;		// Skier's multiplier.

	private Timer m_scoreTimer;			// Timer used to increment score.
	private Timer m_skierMultiplierTimer;	// Timer used to add the multiplier to the score over time.
	public int skierLives = 3;			// The amount of lives the skiers will have.
	private bool m_isAlive = false;		// If the skier has the will to live

	//Invincibility
	public int numberOfFlashes = 3;			//How many times the mesh should flash when damaged
	public float flashDelay = 0.3f;			//How fast the mesh should flash on and off when damaged
	private bool m_invincible = false;		//If the skier can collide with other objects
	private MeshRenderer m_meshRend = null;	//Reference to the mesh to be flashed during invincibility

	[HideInInspector]
	public Tether tether = null;    //Reference to the tether attached to this skier, public so forces can be applied from other scripts
	[HideInInspector]
	public bool hurt = false;

	void Awake()
    {
		tether = GetComponent<Tether>();
		Debug.Assert(tether != null, "Skier missing tether component");

		m_meshRend = GetComponent<MeshRenderer>();
	}

	void Start()
	{
        m_skierMultiplierTimer = gameObject.AddComponent<Timer>();
        m_skierMultiplierTimer.maxTime = skierMultiplierSpeed;
        m_skierMultiplierTimer.autoDisable = true;
          

		m_scoreTimer = gameObject.AddComponent<Timer>();	//Create the score timer
		m_scoreTimer.maxTime = 1;							//The timer will go for one second,
		m_scoreTimer.autoDisable = true;                    //then automatically disable and be reset

        m_scoreTimer.SetTimer();							//Start the timer
        m_skierMultiplierTimer.SetTimer();                  // Start the multiplier timer.
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
			m_playerScore += skierScoreInc * m_skierMultiplier;		//Increment the score once
			m_scoreTimer.SetTimer();			//Reset the timer
		}

        if (!m_skierMultiplierTimer.UnderMax())
        {
			if (m_skierMultiplier < skierMultiplierCap)
			{
				m_skierMultiplier++;
				m_skierMultiplierTimer.SetTimer();
			}
        }
	}

	private void OnTriggerEnter(Collider other)
	{
		if (!m_invincible)					//If the skier is not currently invincible,
		{
			if (other.CompareTag("Skier"))	//If the other object is a skier,
			{
				Tether otherTether = other.GetComponent<Tether>();          //Get the other skier's tether
				otherTether.forceToApply = Vector3.zero;
				otherTether.forceToApply += bonkForce * tether.Direction();	//Add a force to the other skier, in the direction which this skier is currently moving
			}

			if (other.CompareTag("Coin"))	//If the other object is a coin,
				m_playerScore += coinScore; //Add a coin's worth of points to the score

			if (other.CompareTag("Rock"))   //If the other object is a rock,
				HurtSkier();                //Hurt the skier
		}
		if (other.CompareTag("Rock"))
		{
			float pushDirection = transform.position.x - other.transform.position.x;
			tether.forceToApply = Vector3.zero;
			if (pushDirection > 0)
				tether.forceToApply += new Vector3(obstacleForce, 0, 0);
			else
				tether.forceToApply += new Vector3(-obstacleForce, 0, 0);
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

	public int GetPlayerMultiplier()
	{
		return m_skierMultiplier;
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
		if (skierLives > 0)
			hurt = true;
		
		if (!m_invincible)
			skierLives--;               //Subtract a life

		if (skierLives <= 0)                //If the skier is out of lives,
		{
			if (m_isAlive)
			{
				hurt = true;
				StartCoroutine(HurtOffKilled());
			}
		}
		else						//If the skier is still alive,
		{
			m_invincible = true;    //Make it invincible
            m_skierMultiplier = 1; // Multiplier is reset back to 1.
			m_skierMultiplierTimer.enabled = false; // Resets the timer before the flashes start so they don't have the multiplier increase during thier invincibility.
			//Flash the skier mesh
			for (int i = 0; i < numberOfFlashes; ++i)                       //Repeating for the number of flashes,
			{
				StartCoroutine(MeshOff(flashDelay * i * 2));                //Schedule the mesh to turn off, every even interval
				StartCoroutine(MeshOn(flashDelay * i * 2 + flashDelay));    //Schedule the mesh to turn on, every odd interval
			}

			StartCoroutine(InvincibleOff(flashDelay * numberOfFlashes * 2));    //Schedule invincibility to turn off after the flashes are complete
			StartCoroutine(HurtOff());
			m_skierMultiplierTimer.enabled = true;
            m_skierMultiplierTimer.SetTimer(); // Resets the timer again to ensure that the time is fresh out of invincibility.
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
	IEnumerator HurtOff()
	{
		yield return new WaitForEndOfFrame();
		hurt = false;
	}
	IEnumerator HurtOffKilled()
	{
		yield return new WaitForEndOfFrame();
		hurt = false;
		m_isAlive = false;
		gameObject.SetActive(false);
	}
}
