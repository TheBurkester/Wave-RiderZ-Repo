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
	private int m_playerScore = 0; // Player's score.

	public int numberOfFlashes = 3;		//How many times the mesh should flash when damaged
	public float flashDelay = 0.3f;     //How fast the mesh should flash on and off when damaged
	private bool m_invincible = false;

	[HideInInspector]
	public Tether tether = null;

	private MeshRenderer m_meshRend = null;

	void Awake()
    {
		tether = GetComponent<Tether>();
		Debug.Assert(tether != null, "Skier missing tether component");

		m_meshRend = GetComponent<MeshRenderer>();
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
		float axisY = XCI.GetAxis(XboxAxis.RightStickY, controller);
		tether.currentLength += tether.changeSpeed * -axisY * Time.deltaTime;

		//Sideways movement
		if (tether.Distance() >= (tether.currentLength * 0.95)) //As long as the skier is close to the arc of the tether,
		{
			float axisX = XCI.GetAxis(XboxAxis.LeftStickX, controller);
			tether.forceToApply.x += movingForce * axisX;
		}
		//---------------------------------------------

		if (!m_scoreTimer.UnderMax())
		{
			m_playerScore += skierScoreInc;
			m_scoreTimer.SetTimer();
		}

	}

	private void OnTriggerEnter(Collider other)
	{
		if (!m_invincible)
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
				if (skierLives > 0)
				{
					m_invincible = true;

					for (int i = 0; i < numberOfFlashes; ++i)                       //Repeating for the number of flashes,
					{
						StartCoroutine(MeshOff(flashDelay * i * 2));                //Schedule the mesh to turn off, every even interval
						StartCoroutine(MeshOn(flashDelay * i * 2 + flashDelay));    //Schedule the mesh to turn on, every odd interval
					}

					StartCoroutine(InvincibleOff(flashDelay * numberOfFlashes * 2));
				}
				else
				{
					gameObject.SetActive(false);
				}
			}
		}
	}

	public int GetPlayerScore()
	{
		return m_playerScore;
	}

	public void SetPlayerScore(int score)
	{
		m_playerScore = score;
	}

	public void removeGreenScore(int one)
	{
		m_playerScore -= one;
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
