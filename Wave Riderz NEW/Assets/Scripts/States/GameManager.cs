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
using XboxCtrlrInput;

public class GameManager : MonoBehaviour
{
	//Enum for the different round states
	private enum RoundState
	{
		eBeforeRound,	//The single frame before the round for switching players, etc
		eStartRound,	//The countdown before control is given to the players
		ePlayingRound,	//When players are in control and playing the game
		eRoundOver		//Once time is out or all skiers are wiped out, show scoreboard
	}


	//Player references/variables
	public PlaneController plane = null;        //Reference to the plane
	public BeachBallAbility target = null;      //Reference to the target
	public SkierController redSkier = null;     //Reference to the red skier script
	public SkierController greenSkier = null;   //Reference to the green skier script
	public SkierController purpleSkier = null;  //Reference to the purple skier script
	public SkierController orangeSkier = null;	//Reference to the orange skier script
	public GameObject planeBody = null;			// Reference to the body of the plane.
	private int m_playerCount = MainMenu.playerNumber; // Reference to the number of players from the main menu.
	//-------------------------------------------------------------------------

	//Camera reference
	public CameraController mainCamera = null;

	//Round variables
	private RoundState m_eCurrentState = 0;     //Stores the current state of the game
	private Timer m_startRoundTimer;            //The countdown at the start of the round
	private Timer m_playingRoundTimer;			//The round timer
	public float roundTimeLimit = 45;           //How long a round lasts
	//-------------------------------------------------------------------------

	//UI references
	public Text startCountdownDisplay = null;		//Reference to the countdown timer text at the start of the round
	public Text playingCountDownDisplay = null;     //Reference to the round timer text
	public Text scoreRed = null;
	public Text scoreGreen = null;
	public Text scorePurple = null;
	public Text scoreOrange = null;
	public Text livesRed = null;
	public Text livesGreen = null;
	public Text livesPurple = null;
	public Text livesOrange = null;
	public Text beachBombAbility = null;			//Reference to the cooldown timer text
	public GameObject roundOverPanel = null;        //Reference to the panel with all the round over stuff
	//-------------------------------------------------------------------------


	void Awake()
	{
		redSkier.controller = XboxController.First;     //First player is red
		greenSkier.controller = XboxController.Second;  //Second player is green

		if (m_playerCount == 2)
		{
			plane.controller = XboxController.Second;	//Make green control the plane this round

			purpleSkier.gameObject.SetActive(false); // Won't be used.
			orangeSkier.gameObject.SetActive(false); // Won't be used.
		}
		else if (m_playerCount == 3)
		{
			purpleSkier.controller = XboxController.Third;	//Third player is purple

			plane.controller = XboxController.Third;    //Make purple control the plane this round

			orangeSkier.gameObject.SetActive(false); // Won't be used.
		}
		else if (m_playerCount == 4)
		{
			purpleSkier.controller = XboxController.Third;  //Third player is purple
			orangeSkier.controller = XboxController.Fourth;	//Fourth player is orange

			plane.controller = XboxController.Fourth;   //Make orange control the plane this round
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

		//At the start of each round, set the scores
		redSkier.SetPlayerScore(GameInfo.playerOneScore);
		greenSkier.SetPlayerScore(GameInfo.playerTwoScore);
		if (m_playerCount >= 3)
			purpleSkier.SetPlayerScore(GameInfo.playerThreeScore);
		if (m_playerCount == 4)
			orangeSkier.SetPlayerScore(GameInfo.playerFourScore);

		//Ensure no text is displayed at the very start
		startCountdownDisplay.text = "";
		playingCountDownDisplay.text = "";
		scoreRed.text = "";
		scoreGreen.text = "";
		scorePurple.text = "";
		scoreOrange.text = "";
		livesRed.text = "";
		livesGreen.text = "";
		livesPurple.text = "";
		livesOrange.text = "";
		beachBombAbility.text = "";

		roundOverPanel.SetActive(false);

		SceneMovementActive(false);     //Wait for the countdown before starting movement

		if (m_playerCount == 2)
		{
			//In the first round, green is plane, red is skier
			redSkier.gameObject.SetActive(true);
			redSkier.SetAlive(true);
			greenSkier.gameObject.SetActive(false); // Player two will start in the plane.
			greenSkier.SetAlive(false);

			planeBody.GetComponent<Renderer>().material = greenSkier.gameObject.GetComponent<Renderer>().material; // Plane's colour will be the same as player two.
		}
		else if (m_playerCount == 3)
		{
			//In the first round, purple is plane, red/green are skiers
			redSkier.gameObject.SetActive(true); // Player one skier will be active.
			redSkier.SetAlive(true);
			greenSkier.gameObject.SetActive(true);// Player two skier will be active.
			greenSkier.SetAlive(true);
			purpleSkier.gameObject.SetActive(false); // Player three will start in the plane.
			purpleSkier.SetAlive(false);

			planeBody.GetComponent<Renderer>().material = purpleSkier.gameObject.GetComponent<Renderer>().material; // Plane colour = player three colour.
		}
		else if (m_playerCount == 4)
		{
			//In the first round, orange is plane, red/green/purple are skiers
			redSkier.gameObject.SetActive(true); // Player one skier will be active.
			redSkier.SetAlive(true);
			greenSkier.gameObject.SetActive(true); // Player two skier will be active.
			greenSkier.SetAlive(true);
			purpleSkier.gameObject.SetActive(true); // Player three skier will be active.
			purpleSkier.SetAlive(true);
			orangeSkier.gameObject.SetActive(false); // Player four will start in the plane.
			orangeSkier.SetAlive(false);

			planeBody.GetComponent<Renderer>().material = orangeSkier.gameObject.GetComponent<Renderer>().material; // Plane colour = player four colour.
		}

	}

	void Update()
    {
		//The state machine
		switch (m_eCurrentState)
		{
			case RoundState.eBeforeRound:

				if (m_playerCount == 2)
				{
					if (GameInfo.roundNumber == 2)
					{
						redSkier.gameObject.SetActive(false); // Player one is now in the plane.
						redSkier.SetAlive(false);
						greenSkier.gameObject.SetActive(true); // Player two is now the skier.
						greenSkier.SetAlive(true);
						plane.controller = XboxController.First;    //Player one now controls the plane

						planeBody.GetComponent<Renderer>().material = redSkier.gameObject.GetComponent<Renderer>().material; // Changes colour to player one.
					}
				}
				else if (m_playerCount == 3)
				{
					if (GameInfo.roundNumber == 2)
					{
						redSkier.gameObject.SetActive(false); // Player one is now in the plane.
						redSkier.SetAlive(false);
						greenSkier.gameObject.SetActive(true); // Player two is a skier.
						greenSkier.SetAlive(true);
						purpleSkier.gameObject.SetActive(true); // Player three is a skier.
						purpleSkier.SetAlive(true);
						plane.controller = XboxController.First;    //Player one now controls the plane

						planeBody.GetComponent<Renderer>().material = redSkier.gameObject.GetComponent<Renderer>().material; // Changes colour to player one.
					}
					else if (GameInfo.roundNumber == 3)
					{
						redSkier.gameObject.SetActive(true); // Player one is a skier.
						redSkier.SetAlive(true);
						greenSkier.gameObject.SetActive(false); // Player two is now in the plane.
						greenSkier.SetAlive(false);
						purpleSkier.gameObject.SetActive(true); // Player three is a skier.
						purpleSkier.SetAlive(true);
						plane.controller = XboxController.Second;    //Player one now controls the plane

						planeBody.GetComponent<Renderer>().material = greenSkier.gameObject.GetComponent<Renderer>().material; // Changes colour to the player two.
					}
				}
				else if (m_playerCount == 4)
				{
					if (GameInfo.roundNumber == 2)
					{
						redSkier.gameObject.SetActive(false); // Player one is now in the plane.
						redSkier.SetAlive(false);
						greenSkier.gameObject.SetActive(true); // Player two is a skier.
						greenSkier.SetAlive(true);
						purpleSkier.gameObject.SetActive(true); // Player three is a skier.
						purpleSkier.SetAlive(true);
						orangeSkier.gameObject.SetActive(true); // Player four is a skier.
						orangeSkier.SetAlive(true);
						plane.controller = XboxController.First;    //Player one now controls the plane

						planeBody.GetComponent<Renderer>().material = redSkier.gameObject.GetComponent<Renderer>().material; // Changes colour to player one.
					}
					else if (GameInfo.roundNumber == 3)
					{
						redSkier.gameObject.SetActive(true); // Player one is a skier.
						redSkier.SetAlive(true);
						greenSkier.gameObject.SetActive(false); // Player two is now in the plane.
						greenSkier.SetAlive(false);
						purpleSkier.gameObject.SetActive(true); // Player three is a skier.
						purpleSkier.SetAlive(true);
						orangeSkier.gameObject.SetActive(true); // Player four is a skier.
						orangeSkier.SetAlive(true);
						plane.controller = XboxController.Second;    //Player one now controls the plane

						planeBody.GetComponent<Renderer>().material = greenSkier.gameObject.GetComponent<Renderer>().material; // Changes colour to player one.
					}
					else if (GameInfo.roundNumber == 4)
					{
						redSkier.gameObject.SetActive(true); // Player one is a skier.
						redSkier.SetAlive(true);
						greenSkier.gameObject.SetActive(true); // Player two is a skier.
						greenSkier.SetAlive(true);
						purpleSkier.gameObject.SetActive(false); // Player three is now in the plane.
						purpleSkier.SetAlive(false);
						orangeSkier.gameObject.SetActive(true); // Player four is a skier.
						orangeSkier.SetAlive(true);
						plane.controller = XboxController.Third;    //Player one now controls the plane

						planeBody.GetComponent<Renderer>().material = purpleSkier.gameObject.GetComponent<Renderer>().material; // Changes colour to player one.
					}
				}

				m_eCurrentState = RoundState.eStartRound;   //After a single frame in the BeforeRound state, start the actual round stuff

				break;
			//-------------------------------------------------------------------------

			case RoundState.eStartRound:

				if (!m_startRoundTimer.UnderMax())              //If the timer has run out,
				{
					m_eCurrentState = RoundState.ePlayingRound; //Swap to playing the round
					m_playingRoundTimer.SetTimer();             //Start the round timer
					startCountdownDisplay.text = "GO!";
					StartCoroutine(clearText(1));               //Set the text to turn off after 1 second
					SceneMovementActive(true);                  //Activate scene movement
				}
				else                                                            //Otherwise the timer is still going,
				{
					int closestSecond = (int)Math.Ceiling(m_startRoundTimer.T); //Round the timer up to the nearest second

					//Display the countdown
					//if (closestSecond == 3)
					//	startCountdownDisplay.text = "3";
					//else if (closestSecond == 2)
					//	startCountdownDisplay.text = "2";
					//else if (closestSecond == 1)
					//	startCountdownDisplay.text = "1";
					startCountdownDisplay.text = closestSecond.ToString();
				}

				break;
			//-------------------------------------------------------------------------

			case RoundState.ePlayingRound:

				if (!m_playingRoundTimer.UnderMax()																				//If the timer has run out,
					|| !redSkier.GetAlive() && !greenSkier.GetAlive() && !purpleSkier.GetAlive() && !orangeSkier.GetAlive())	//or all skiers are wiped out,
				{
					m_eCurrentState = RoundState.eRoundOver;    //Swap to the round over screen
					playingCountDownDisplay.text = "";          //Turn the timer text off
					SceneMovementActive(false);                 //Deactivate scene movement
					roundOverPanel.SetActive(true);             //Show the round over screen
				}

				int nearestSecond = (int)Math.Ceiling(m_playingRoundTimer.T);   //Round the timer up to the nearest second
				playingCountDownDisplay.text = nearestSecond.ToString();        //Show the timer

				//Display scores and lives
				scoreRed.text = redSkier.GetPlayerScore().ToString();
				scoreGreen.text = greenSkier.GetPlayerScore().ToString();
				if (redSkier.GetAlive())
					livesRed.text = redSkier.skierLives.ToString();
				else
					livesRed.text = "";
				if (greenSkier.GetAlive())
					livesGreen.text = greenSkier.skierLives.ToString();
				else
					livesGreen.text = "";
				if (m_playerCount >= 3)
				{
					scorePurple.text = purpleSkier.GetPlayerScore().ToString();
					if (purpleSkier.GetAlive())
						livesPurple.text = purpleSkier.skierLives.ToString();
					else
						livesPurple.text = "";
				}
				if (m_playerCount == 4)
				{
					scoreOrange.text = orangeSkier.GetPlayerScore().ToString();
					if (orangeSkier.GetAlive())
						livesOrange.text = orangeSkier.skierLives.ToString();
					else
						livesOrange.text = "";
				}

				beachBombAbility.text = ((int)Math.Ceiling(target.abilityCooldown.T)).ToString();	//Display the beach bomb ability cooldown timer
				
				break;
				//-------------------------------------------------------------------------

			case RoundState.eRoundOver:

				if (Input.GetKeyDown(KeyCode.Space) || XCI.GetButtonDown(XboxButton.A, XboxController.All))	//If next round is selected,
				{
					//Update the static GameInfo scores
					GameInfo.playerOneScore = redSkier.GetPlayerScore();
					GameInfo.playerTwoScore = greenSkier.GetPlayerScore();
					if (m_playerCount >= 3)
						GameInfo.playerThreeScore = purpleSkier.GetPlayerScore();
					if (m_playerCount == 4)
						GameInfo.playerFourScore = orangeSkier.GetPlayerScore();

					++GameInfo.roundNumber;								//Update the round number
					if (GameInfo.roundNumber <= m_playerCount)			//If the round number is under the number of players,
					{
						SceneManager.LoadScene(GameInfo.roundNumber);	//Load the next level

						//m_eCurrentState = RoundState.eBeforeRound;		//Go back to round start state
						//m_startRoundTimer.SetTimer();					//Start the countdown timer
						//roundOverPanel.SetActive(false);
					}
					else                                                //The round number exceeds the number of players,
						SceneManager.LoadScene(5);						//Go to the game finished scene
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
		if (purpleSkier.gameObject.activeSelf == true)	//If the purple skier is in the game,
			purpleSkier.tether.enabled = value;			//Set them
		if (orangeSkier.gameObject.activeSelf == true)	//If the orange skier is in the game,
			orangeSkier.tether.enabled = value;			//Set them too
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
