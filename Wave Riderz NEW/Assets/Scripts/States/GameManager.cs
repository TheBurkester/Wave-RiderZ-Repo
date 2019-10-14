/*-------------------------------------------------------------------*
|  Title:			GameManager
|
|  Author:			Seth Johnston / Thomas Maltezos
| 
|  Description:		Handles round states, and keeps track of important variables.
*-------------------------------------------------------------------*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour
{
	//Enum for the different round states
	public enum RoundState
	{
		eStartRound,	//The countdown before control is given to the players
		ePlayingRound,	//When players are in control and playing the game
		eRoundOver		//Once time is out or all skiers are wiped out, show scoreboard
	}


	//Player references/variables
	public PlaneController plane = null;        //Reference to the plane
	public BeachBallAbility target = null;      //Reference to the target
	public SkierController redSkier = null;     //Reference to the red skier script.
	public SkierController greenSkier = null;   //Reference to the green skier script.
	public SkierController purpleSkier = null;  //Reference to the purple skier script.
	public SkierController orangeSkier = null;	// Reference to the orange skier script.
	public GameObject playerOne = null;			// Reference to player one's game object.
	public GameObject playerTwo = null;			// Reference to player two's game object.
	public GameObject playerThree = null;		// Reference to player three's game object.
	public GameObject playerFour = null;        // Reference to player four's game object.
	public GameObject planeBody = null;		// Reference to the body of the plane.

	private int m_playerCount = MainMenu.playerNumber; // Reference to the number of players from the main menu.
	//-------------------------------------------------------------------------

	//Camera reference
	public CameraController mainCamera = null;

	//Round variables
	private RoundState m_eCurrentState = 0;     //Stores the current state of the game
	private int m_roundNumber = 1;              //Stores the current round number
	private Timer m_startRoundTimer;            //The countdown at the start of the round
	private Timer m_playingRoundTimer;			//The round timer
	public float roundTimeLimit = 45;           //How long a round lasts
	//-------------------------------------------------------------------------

	//UI references
	public Text startCountdownDisplay = null;		//Reference to the countdown timer text at the start of the round
	public Text playingCountDownDisplay = null;		//Reference to the round timer text
	public GameObject roundOverPanel = null;        //Reference to the panel with all the round over stuff
													//-------------------------------------------------------------------------


	void Awake()
	{
		if (m_playerCount == 2)
		{
			playerOne.SetActive(true); // Only one skier.
			playerTwo.SetActive(false); // Player two will start in the plane.

			planeBody.GetComponent<Renderer>().material = playerTwo.GetComponent<Renderer>().material; // Plane's colour will be the same as player two.

			playerThree.SetActive(false); // Won't be used.
			playerFour.SetActive(false); // Won't be used.
		}
		else if (m_playerCount == 3)
		{
			playerOne.SetActive(true); // Player one skier will be active.
			playerTwo.SetActive(true);// Player two skier will be active.
			playerThree.SetActive(false); // Player three will start in the plane.

			planeBody.GetComponent<Renderer>().material = playerThree.GetComponent<Renderer>().material; // Plane colour = player three colour.

			playerFour.SetActive(false); // Won't be used.
		}
		else if (m_playerCount == 4)
		{
			playerOne.SetActive(true); // Player one skier will be active.
			playerTwo.SetActive(true); // Player two skier will be active.
			playerThree.SetActive(true); // Player three skier will be active.
			playerFour.SetActive(false); // Player four will start in the plane.

			planeBody.GetComponent<Renderer>().material = playerFour.GetComponent<Renderer>().material; // Plane colour = player four colour.
		}
	}

	void Start()
    {
		m_startRoundTimer = gameObject.AddComponent<Timer>();   //Create the timer
		m_startRoundTimer.maxTime = 4;                          //Set the timer for 4 seconds
		m_startRoundTimer.reverseTimer = true;                  //Make the timer count down
		m_startRoundTimer.autoDisable = true;                   //Make the timer disable itself after the timelimit
		m_startRoundTimer.SetTimer();							//Initialise the timer with these settings and start it

		m_playingRoundTimer = gameObject.AddComponent<Timer>();    //Create the timer
		m_playingRoundTimer.maxTime = roundTimeLimit;              //Set how long rounds last
		m_playingRoundTimer.reverseTimer = true;                   //Make the timer count down
		m_playingRoundTimer.autoDisable = true;                    //Make the timer disable itself after the timelimit

		//Ensure no text is displayed at the very start
		startCountdownDisplay.text = "";
		playingCountDownDisplay.text = "";
		roundOverPanel.SetActive(false);

		SceneMovementActive(false);		//Wait for the countdown before starting movement
	}

	void Update()
    {
		//The state machine
		switch (m_eCurrentState)
		{
			case RoundState.eStartRound:

				if (!m_startRoundTimer.UnderMax())              //If the timer has run out,
				{
					m_eCurrentState = RoundState.ePlayingRound; //Swap to playing the round
					m_playingRoundTimer.SetTimer();				//Start the round timer
					startCountdownDisplay.text = "GO!";
					StartCoroutine(clearText(1));               //Set the text to turn off after 1 second
					SceneMovementActive(true);					//Activate scene movement
				}
				else															//Otherwise the timer is still going,
				{
					int closestSecond = (int)Math.Ceiling(m_startRoundTimer.T); //Round the timer up to the nearest second

					//Display the countdown
					if (closestSecond == 3)
						startCountdownDisplay.text = "3";
					else if (closestSecond == 2)
						startCountdownDisplay.text = "2";
					else if (closestSecond == 1)
						startCountdownDisplay.text = "1";

					if (m_playerCount == 2)
					{
						if (m_roundNumber == 2)
						{
							playerOne.SetActive(false); // Player one is now in the plane.
							playerTwo.SetActive(true); // Player two is now the skier.
							planeBody.GetComponent<Renderer>().material = playerOne.GetComponent<Renderer>().material; // Changes colour to player one.
						}
					}
					else if (m_playerCount == 3)
					{
						if (m_roundNumber == 2)
						{
							playerOne.SetActive(false); // Player one is now in the plane.
							playerTwo.SetActive(true); // Player two is a skier.
							playerThree.SetActive(true); // Player three is a skier.
							planeBody.GetComponent<Renderer>().material = playerOne.GetComponent<Renderer>().material; // Changes colour to player one.
						}
						else if (m_roundNumber == 3)
						{
							playerOne.SetActive(true); // Player one is a skier.
							playerTwo.SetActive(false); // Player two is now in the plane.
							playerThree.SetActive(true); // Player three is a skier.
							planeBody.GetComponent<Renderer>().material = playerTwo.GetComponent<Renderer>().material; // Changes colour to the player two.
						}
					}
					else if (m_playerCount == 4)
					{
						if (m_roundNumber == 2)
						{

						}
						else if (m_roundNumber == 3)
						{

						}
						else if (m_roundNumber == 4)
						{

						}
					}
				}

				break;
				//-------------------------------------------------------------------------

			case RoundState.ePlayingRound:

				if (!m_playingRoundTimer.UnderMax())			//If the timer has run out,
				{
					m_eCurrentState = RoundState.eRoundOver;    //Swap to the round over screen
					playingCountDownDisplay.text = "";			//Turn the timer text off
					SceneMovementActive(false);                 //Deactivate scene movement
					roundOverPanel.SetActive(true);				//Show the round over screen
				}

				int nearestSecond = (int)Math.Ceiling(m_playingRoundTimer.T);	//Round the timer up to the nearest second
				playingCountDownDisplay.text = nearestSecond.ToString();		//Show the timer


				break;
				//-------------------------------------------------------------------------

			case RoundState.eRoundOver:

				if (Input.GetKeyDown(KeyCode.Space))					//If next round is selected,
				{
					++m_roundNumber;
					//if (m_roundNumber <= playerNumber)				//If the round number is under the number of players,
					{
						//SceneManager.LoadScene(m_roundNumber);	//Load the next level
						//I'm not sure if this just stops running code and moves to a whole new scene?
						//In that case, we wouldn't need to bother resetting anything. -Seth

						m_eCurrentState = RoundState.eStartRound;		//Go back to round start state
						m_startRoundTimer.SetTimer();					//Start the countdown timer
						roundOverPanel.SetActive(false);
					}
					//else												//The round number exceeds the number of players,
					//{
					//	SceneManager.LoadScene("GameOver");				//Go to the game finished scene
					//}
				}

				break;
				//-------------------------------------------------------------------------
		}
	}

	//Starts/stops movement in the scene
	private void SceneMovementActive(bool value)
	{
		plane.enabled = value;
		redSkier.enabled = value;
		greenSkier.enabled = value;
		purpleSkier.enabled = value;
		orangeSkier.enabled = value;
		redSkier.tether.enabled = value;
		greenSkier.tether.enabled = value;
		purpleSkier.tether.enabled = value;
		orangeSkier.tether.enabled = value;
		target.enabled = value;
		mainCamera.enabled = value;
	}

	//Coroutine to turn off countdown text
	IEnumerator clearText(float interval)
	{
		yield return new WaitForSeconds(interval);	//Wait for a certain amount of time
		startCountdownDisplay.text = "";			//Turn the countdown text off
	}
}
