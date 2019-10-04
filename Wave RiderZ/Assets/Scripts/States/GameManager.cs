/*-------------------------------------------------------------------*
|  Title:			GameManager
|
|  Author:			Seth Johnston
| 
|  Description:		Handles round states, and keeps track of important variables.
*-------------------------------------------------------------------*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;


public class GameManager : MonoBehaviour
{
	//Enum for the different round states
	public enum RoundState
	{
		eStartRound,	//The countdown before control is given to the players
		ePlayingRound,	//When players are in control and playing the game
		eRoundOver		//Once time is out or all skiers are wiped out, show scoreboard
	}


	//Player references
	public PlaneController plane = null;		//Reference to the plane
	public SkierController redSkier = null;     //Reference to the red skier
	//public SkierController greenSkier = null;	//Reference to the green skier
	//public SkierController purpleSkier = null;  //Reference to the purple skier
	//-------------------------------------------------------------------------

	//Camera reference
	public CameraController mainCamera = null;

	//Round variables
	private RoundState m_eCurrentState = 0;     //Stores the current state of the game
	private int m_roundNumber = 1;              //Stores the current round number
	private Timer m_roundStartTimer;            //The countdown at the start of the round
	private Timer m_roundTimer;                 //The round timer
	public float roundTimeLimit = 45;           //How long a round lasts
	//-------------------------------------------------------------------------

	//UI references
	public Text startCountdownDisplay = null;	//Reference to the countdown timer text at the start of the round
	//-------------------------------------------------------------------------


	void Start()
    {
		m_roundStartTimer = gameObject.AddComponent<Timer>();   //Create the timer
		m_roundStartTimer.maxTime = 4;                          //Set the timer for 4 seconds
		m_roundStartTimer.reverseTimer = true;                  //Make the timer count down
		m_roundStartTimer.autoDisable = true;                   //Make the timer disable itself after the timelimit
		m_roundStartTimer.Initialise();                         //Initialise the timer with these settings and start it

		m_roundTimer = gameObject.AddComponent<Timer>();    //Create the timer
		m_roundTimer.maxTime = roundTimeLimit;              //Set how long rounds last
		m_roundTimer.reverseTimer = true;                   //Make the timer count down
		m_roundTimer.autoDisable = true;                    //Make the timer disable itself after the timelimit

		startCountdownDisplay.text = "";	//Ensure no text is displayed at the very start

		SceneMovementActive(false);		//Wait for the countdown before starting movement
	}

	void Update()
    {
		//The state machine
		switch (m_eCurrentState)
		{
			case RoundState.eStartRound:
				if (!m_roundStartTimer.UnderMax())              //If the timer has run out,
				{
					m_eCurrentState = RoundState.ePlayingRound; //Swap to playing the round
					startCountdownDisplay.text = "GO!";
					StartCoroutine(clearText(1));               //Set the text to turn off after 1 second
					SceneMovementActive(true);
				}
				else															//Otherwise the timer is still going,
				{
					int closestSecond = (int)Math.Ceiling(m_roundStartTimer.T);	//Round the timer up to the nearest second
					//Display the countdown
					if (closestSecond == 3)
						startCountdownDisplay.text = "3";
					else if (closestSecond == 2)
						startCountdownDisplay.text = "2";
					else if (closestSecond == 1)
						startCountdownDisplay.text = "1";
				}

				break;

			case RoundState.ePlayingRound:

				break;

			case RoundState.eRoundOver:

				break;
		}
    }

	//Starts/stops movement in the scene
	private void SceneMovementActive(bool value)
	{
		plane.enabled = value;
		redSkier.enabled = value;
		//greenSkier.enabled = value;
		//purpleSkier.enabled = value;
		mainCamera.enabled = value;
	}

	//Coroutine to turn off countdown text
	IEnumerator clearText(float interval)
	{
		yield return new WaitForSeconds(interval);	//Wait for a certain amount of time
		startCountdownDisplay.text = "";			//Turn the countdown text off
	}
}
