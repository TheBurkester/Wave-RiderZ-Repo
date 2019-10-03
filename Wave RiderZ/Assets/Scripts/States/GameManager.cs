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



public class GameManager : MonoBehaviour
{
	//Enum for the different round states
	public enum RoundState
	{
		eStartRound,	//The countdown before control is given to the players
		ePlayingRound,	//When players are in control and playing the game
		eRoundOver		//Once time is out or all skiers are wiped out, show scoreboard
	}


	public PlaneController plane = null;		//Reference to the plane
	public SkierController redSkier = null;		//Reference to the red skier
	public SkierController greenSkier = null;	//Reference to the green skier
	public SkierController purpleSkier = null;  //Reference to the purple skier


	private RoundState m_eCurrentState = 0;     //Stores the current state of the game
	private int m_roundNumber = 1;				//Stores the current round number

	private Timer m_roundStartTimer;            //The countdown at the start of the round
	private Timer m_roundTimer;                 //The round timer
	public float roundTimeLimit = 45;			//How long a round lasts


	void Start()
    {
		m_roundStartTimer.maxTime = 3;			//Set the timer for 3 seconds
		m_roundStartTimer.autoDisable = true;   //Make the timer disable itself after the timelimit

		m_roundTimer.maxTime = roundTimeLimit;	//Set how long rounds last
		m_roundStartTimer.autoDisable = true;   //Make the timer disable itself after the timelimit
	}

    void Update()
    {
		//The state machine
        switch (m_eCurrentState)
		{
			case RoundState.eStartRound:
				if (!m_roundStartTimer.UnderMax())				//If the timer has run out,
					m_eCurrentState = RoundState.ePlayingRound;	//Swap to playing the round


				break;

			case RoundState.ePlayingRound:

				break;

			case RoundState.eRoundOver:

				break;
		}
    }
}
