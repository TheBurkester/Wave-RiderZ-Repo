﻿/*-------------------------------------------------------------------*
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
		eStartRound,	//The countdown before control is given to the players
		ePlayingRound,	//When players are in control and playing the game
		eRoundOver		//Once time is out or all skiers are wiped out, show scoreboard
	}


	//Player references/variables
	public PlaneController plane = null;        //Reference to the plane
	public TetheredMineAbility planeHatch = null;        // Reference to the plane's hatch.
	public Tether mine = null;					// Reference to the mine.
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
	public Text multiplierRed = null;
	public Text multiplierGreen = null;
	public Text multiplierPurple = null;
	public Text multiplierOrange = null;
	public Text beachBombAbility = null;            //Reference to the cooldown timer text
	public Text tetheredMineAbility = null;			// Reference to the cooldown timer text.
	public GameObject roundOverPanel = null;        //Reference to the panel with all the round over stuff
	public Text bonus = null;
	//-------------------------------------------------------------------------


	void Awake()
	{
		planeHatch.setIsUsingAbility(false);
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
		multiplierRed.text = "";
		multiplierGreen.text = "";
		multiplierPurple.text = "";
		multiplierOrange.text = "";
		beachBombAbility.text = "";
		tetheredMineAbility.text = "";
		bonus.text = "";

		roundOverPanel.SetActive(false);

		SceneMovementActive(false);     //Wait for the countdown before starting movement

		if (m_playerCount == 2)
		{
			if (GameInfo.roundNumber == 1)
			{
				//In the first round, green is plane, red is skier
				redSkier.gameObject.SetActive(true);
				redSkier.SetAlive(true);
				greenSkier.gameObject.SetActive(false); // Player two will start in the plane.
				greenSkier.SetAlive(false);

				planeBody.GetComponent<Renderer>().material = greenSkier.gameObject.GetComponent<Renderer>().material; // Plane's colour will be the same as player two.
			}
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
			if (GameInfo.roundNumber == 1)
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
			if (GameInfo.roundNumber == 1)
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
					m_playingRoundTimer.SetTimer();             //Start the round timer
					startCountdownDisplay.text = "GO!";
					StartCoroutine(clearText(1));               //Set the text to turn off after 1 second
					SceneMovementActive(true);                  //Activate scene movement
				}
				else                                                            //Otherwise the timer is still going,
				{
					int closestSecond = (int)Math.Ceiling(m_startRoundTimer.T); //Round the timer up to the nearest second
					startCountdownDisplay.text = closestSecond.ToString();
				}

				break;
			//-------------------------------------------------------------------------

			case RoundState.ePlayingRound:

				if (!m_playingRoundTimer.UnderMax())
				{
					if (redSkier.GetAlive()) // If red skier was alive at the end of the timer.
						redSkier.SetPlayerScore(redSkier.GetPlayerScore() + redSkier.skierBonus); // Increase its score by the bonus.
					if (greenSkier.GetAlive()) //  If green skier was alive at the end of the timer. 
						greenSkier.SetPlayerScore(greenSkier.GetPlayerScore() + greenSkier.skierBonus); // Increase its score by the bonus.
					if (purpleSkier) // If purple skier exists.
					{
						if (purpleSkier.GetAlive()) // If purple skier was alive at the end of the timer.
							purpleSkier.SetPlayerScore(purpleSkier.GetPlayerScore() + purpleSkier.skierBonus); // Increase its score by the bonus.
					}
					if (orangeSkier) // If orange skier exists.
					{
						if (orangeSkier.GetAlive()) // If the orange skier was alive at the end of the timer.
						{
							orangeSkier.SetPlayerScore(orangeSkier.GetPlayerScore() + orangeSkier.skierBonus); // Increase its score by the bonus.
						}
					}

					m_eCurrentState = RoundState.eRoundOver;    //Swap to the round over screen
					playingCountDownDisplay.text = "";          //Turn the timer text off
					SceneMovementActive(false);                 //Deactivate scene movement
					bonus.text = "Alive skiers get a bonus of " + redSkier.skierBonus.ToString();
					roundOverPanel.SetActive(true);             //Show the round over screen
				}
				else if (!redSkier.GetAlive() && !greenSkier.GetAlive() && !purpleSkier.GetAlive() && !orangeSkier.GetAlive())
				{
					if (plane.controller == XboxController.First)
						redSkier.SetPlayerScore(redSkier.GetPlayerScore() + redSkier.planeBonus);
					else if (plane.controller == XboxController.Second)
						greenSkier.SetPlayerScore(greenSkier.GetPlayerScore() + greenSkier.planeBonus);
					else if (plane.controller == XboxController.Third)
						purpleSkier.SetPlayerScore(purpleSkier.GetPlayerScore() + purpleSkier.planeBonus);
					else if (plane.controller == XboxController.Fourth)
						orangeSkier.SetPlayerScore(orangeSkier.GetPlayerScore() + orangeSkier.planeBonus);

					m_eCurrentState = RoundState.eRoundOver;    //Swap to the round over screen
					playingCountDownDisplay.text = "";          //Turn the timer text off
					SceneMovementActive(false);                 //Deactivate scene movement
					bonus.text = "All skiers wiped out! Plane gets " + redSkier.planeBonus.ToString() + " bonus score!";
					roundOverPanel.SetActive(true);             //Show the round over screen
				}

				int nearestSecond = (int)Math.Ceiling(m_playingRoundTimer.T);   //Round the timer up to the nearest second
				playingCountDownDisplay.text = nearestSecond.ToString();        //Show the timer

				//Display scores and lives
				scoreRed.text = redSkier.GetPlayerScore().ToString();
				multiplierRed.text = "x" + redSkier.GetPlayerMultiplier().ToString();
				scoreGreen.text = greenSkier.GetPlayerScore().ToString();
				multiplierGreen.text = "x" + greenSkier.GetPlayerMultiplier().ToString();
				if (redSkier.GetAlive())
					livesRed.text = redSkier.skierLives.ToString();
				else
				{ 
					livesRed.text = "";
					multiplierRed.text = "";
				}
				if (greenSkier.GetAlive())
					livesGreen.text = greenSkier.skierLives.ToString();
				else
				{
					livesGreen.text = "";
					multiplierGreen.text = "";
				}
				if (m_playerCount >= 3)
				{
					scorePurple.text = purpleSkier.GetPlayerScore().ToString();
					multiplierPurple.text = "x" + purpleSkier.GetPlayerMultiplier().ToString();
					if (purpleSkier.GetAlive())
						livesPurple.text = purpleSkier.skierLives.ToString();
					else
					{
						livesPurple.text = "";
						multiplierPurple.text = "";
					}
				}
				if (m_playerCount == 4)
				{
					scoreOrange.text = orangeSkier.GetPlayerScore().ToString();
					multiplierOrange.text = "x" + orangeSkier.GetPlayerMultiplier().ToString();
					if (orangeSkier.GetAlive())
						livesOrange.text = orangeSkier.skierLives.ToString();
					else
					{
						livesOrange.text = "";
						multiplierOrange.text = "";
					}
				}

				beachBombAbility.text = ((int)Math.Ceiling(target.abilityCooldown.T)).ToString();   //Display the beach bomb ability cooldown timer
				tetheredMineAbility.text = ((int)Math.Ceiling(planeHatch.abilityCooldown.T)).ToString(); // Display the mine ability cooldown timer.
				
				if (plane.controller == XboxController.First)
				{
					if (greenSkier.hurt && greenSkier.GetAlive())
						redSkier.SetPlayerScore(redSkier.GetPlayerScore() + redSkier.planeScoreInc);
					if (purpleSkier)
					{
						if (purpleSkier.hurt && purpleSkier.GetAlive())
							redSkier.SetPlayerScore(redSkier.GetPlayerScore() + redSkier.planeScoreInc);
					}
					if (orangeSkier)
					{
						if (orangeSkier.hurt && orangeSkier.GetAlive())
							redSkier.SetPlayerScore(redSkier.GetPlayerScore() + redSkier.planeScoreInc);
					}
				}
				else if (plane.controller == XboxController.Second)
				{
					if (redSkier.hurt && redSkier.GetAlive())
						greenSkier.SetPlayerScore(greenSkier.GetPlayerScore() + greenSkier.planeScoreInc);
					if (purpleSkier)
					{
						if (purpleSkier.hurt && purpleSkier.GetAlive())
							greenSkier.SetPlayerScore(greenSkier.GetPlayerScore() + greenSkier.planeScoreInc);
					}
					if (orangeSkier)
					{
						if (orangeSkier.hurt && orangeSkier.GetAlive())
							greenSkier.SetPlayerScore(greenSkier.GetPlayerScore() + greenSkier.planeScoreInc);
					}
				}
				else if (plane.controller == XboxController.Third)
				{
					if (redSkier.hurt && redSkier.GetAlive())
						purpleSkier.SetPlayerScore(purpleSkier.GetPlayerScore() + purpleSkier.planeScoreInc);
					if (greenSkier.hurt && greenSkier.GetAlive())
						purpleSkier.SetPlayerScore(purpleSkier.GetPlayerScore() + purpleSkier.planeScoreInc);
					if (orangeSkier) // If orange skier exists.
					{
						if (orangeSkier.hurt && orangeSkier.GetAlive())
						{
							purpleSkier.SetPlayerScore(purpleSkier.GetPlayerScore() + purpleSkier.planeScoreInc);
						}
					}
				}
				else if (plane.controller == XboxController.Fourth)
				{
					if (redSkier.hurt)
						orangeSkier.SetPlayerScore(orangeSkier.GetPlayerScore() + orangeSkier.planeScoreInc);
					if (greenSkier.hurt)
						orangeSkier.SetPlayerScore(orangeSkier.GetPlayerScore() + orangeSkier.planeScoreInc);
					if (purpleSkier.hurt)
						orangeSkier.SetPlayerScore(orangeSkier.GetPlayerScore() + orangeSkier.planeScoreInc);
				}

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

					++GameInfo.roundNumber;						//Update the round number
					if (GameInfo.roundNumber <= m_playerCount)	//If the round number is under the number of players,
						SceneManager.LoadScene(1);				//Load the next level
					else                                        //The round number exceeds the number of players,
						SceneManager.LoadScene(2);				//Go to the game finished scene
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
			orangeSkier.tether.enabled = value;         //Set them too
		mine.enabled = value;
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
