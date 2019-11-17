/*-------------------------------------------------------------------*
|  Title:			GameManager
|
|  Author:			Thomas Maltezos / Seth Johnston
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

    private enum PlaneState
    {
        eFirst,     // First player in the plane.
        eSecond,    // Second player in the plane.
        eThird,     // Third player in the plane.
        eFourth,    // Fourth player in the plane.
        eNone       // Used when starting a new game.
    }

	//Player references/variables
	public PlaneController plane = null;        //Reference to the plane
	public TetheredMineAbility planeHatch = null;        // Reference to the plane's hatch.
	public Tether mine = null;					// Reference to the mine.
	public BeachBallAbility target = null;      //Reference to the target
	public SkierController redSkier = null;     //Reference to the red skier script
	public SkierController greenSkier = null;   //Reference to the green skier script
	public SkierController purpleSkier = null;  //Reference to the purple skier script
	public SkierController orangeSkier = null;  //Reference to the orange skier script
	private SkierController[] m_skiers = null;
	public GameObject planeBody = null;			// Reference to the body of the plane.
	private int m_playerCount = MainMenu.playerNumber; // Reference to the number of players from the main menu.
	private Vector3 m_twoPlayerSkierPos = new Vector3(0, 0, -11);
	private Vector3 m_threePlayerSkierPosOne = new Vector3(-1, 0, -11);
	private Vector3 m_threePlayerSkierPosTwo = new Vector3(1, 0, -11);
	private Vector3 m_fourPlayerSkierPosOne = new Vector3(0, 0, -11);
	private Vector3 m_fourPlayerSkierPosTwo = new Vector3(-2, 0, -11);
	private Vector3 m_fourPlayerSkierPosThree = new Vector3(2, 0, -11);
	public int skierBonus = 10;					//Score bonus given to skier if they survive a full round
	public int planeBonus = 10;                 //Score bonus given to plane if all skiers wipeout
	public int skierHurtBonus = 5;				//Score bonus given to plane when a skier is hurt
	//-------------------------------------------------------------------------

	//Camera reference
	public CameraController mainCamera = null;

	//Round variables
	private RoundState m_eCurrentState = 0;                        //Stores the current state of the game
    private PlaneState m_eCurrentPlaneState = PlaneState.eNone;    // Stores the current plane state.
	private Timer m_startRoundTimer;                               //The countdown at the start of the round
	private Timer m_playingRoundTimer;                             //The round timer
	private int m_randomPlane;					                   // Random number to select the plane player.
	public float roundTimeLimit = 45;                              //How long a round lasts
	//-------------------------------------------------------------------------

	//UI references
	public Text startCountdownDisplay = null;       //Reference to the countdown timer text at the start of the round
	public GameObject roundTimerPanel = null;		// Reference to the round timer panel.
	public Image playingCountDownDisplay = null;    //Reference to the round timer image
	public Text scoreRed = null;
	public Text scoreGreen = null;
	public Text scorePurple = null;
	public Text scoreOrange = null;
	private Text[] m_skierScores = null;
	public GameObject playerOneUI = null;
	public GameObject playerTwoUI = null;
	public GameObject playerThreeUI = null;
	public GameObject playerFourUI = null;
	private GameObject[] m_playerUI = null;
	public GameObject beachBombAbilityUI = null;
	public GameObject tetheredMineAbilityUI = null;
	public Image livesRed = null;
	public Image livesGreen = null;
	public Image livesPurple = null;
	public Image livesOrange = null;
	private Image[] m_skierLives = null;
	public Text multiplierRed = null;
	public Text multiplierGreen = null;
	public Text multiplierPurple = null;
	public Text multiplierOrange = null;
	private Text[] m_skierMultipliers = null;
	public Image beachBombAbility = null;
	public Image tetheredMineAbility = null;
	public Image beachBombControllerAim = null;
	public Image beachBombControllerShoot = null;
	public Image tetheredMineController = null;
	//public Text beachBombAbility = null;            //Reference to the cooldown timer text
	//public Text tetheredMineAbility = null;			// Reference to the cooldown timer text.
	public GameObject roundOverPanel = null;        //Reference to the panel with all the round over stuff
	public Text bonus = null;
	//-------------------------------------------------------------------------

	public Texture axolotlPlaneTexture = null;
	public delegate void Function(int skierNumber);

	void Awake()
	{
		m_skiers = new SkierController[4];
		m_skiers[0] = redSkier;
		m_skiers[1] = greenSkier;
		m_skiers[2] = purpleSkier;
		m_skiers[3] = orangeSkier;

		m_skierScores = new Text[4];
		m_skierScores[0] = scoreRed;
		m_skierScores[1] = scoreGreen;
		m_skierScores[2] = scorePurple;
		m_skierScores[3] = scoreOrange;

		m_playerUI = new GameObject[4];
		m_playerUI[0] = playerOneUI;
		m_playerUI[1] = playerTwoUI;
		m_playerUI[2] = playerThreeUI;
		m_playerUI[3] = playerFourUI;

		m_skierLives = new Image[4];
		m_skierLives[0] = livesRed;
		m_skierLives[1] = livesGreen;
		m_skierLives[2] = livesPurple;
		m_skierLives[3] = livesOrange;

		m_skierMultipliers = new Text[4];
		m_skierMultipliers[0] = multiplierRed;
		m_skierMultipliers[1] = multiplierGreen;
		m_skierMultipliers[2] = multiplierPurple;
		m_skierMultipliers[3] = multiplierOrange;

		planeHatch.setIsUsingAbility(false);
		redSkier.controller = XboxController.First;     //First player is red
		greenSkier.controller = XboxController.Second;  //Second player is green

		if (m_playerCount == 2)
		{
			if (GameInfo.roundNumber == 1)
			{
				m_randomPlane = UnityEngine.Random.Range(1, 3);

				if (m_randomPlane == 1)
				{
                    m_eCurrentPlaneState = PlaneState.eFirst;
					plane.controller = XboxController.First;
					GameInfo.playerOneHasPlane = true;
				}
				else if (m_randomPlane == 2)
				{
                    m_eCurrentPlaneState = PlaneState.eSecond;
					plane.controller = XboxController.Second;   //Make green control the plane this round
					GameInfo.playerTwoHasPlane = true;
				}
			}
			else if (GameInfo.roundNumber == 2)
			{
				if (GameInfo.playerOneHasPlane)
				{
                    m_eCurrentPlaneState = PlaneState.eSecond;
                    plane.controller = XboxController.Second;
					GameInfo.playerTwoHasPlane = true;
				}
				else if (GameInfo.playerTwoHasPlane)
				{
                    m_eCurrentPlaneState = PlaneState.eFirst;
                    plane.controller = XboxController.First;
					GameInfo.playerOneHasPlane = true;
				}
			}

			purpleSkier.gameObject.SetActive(false); // Won't be used.
			orangeSkier.gameObject.SetActive(false); // Won't be used.
		}
		else if (m_playerCount == 3)
		{
			purpleSkier.controller = XboxController.Third;  //Third player is purple

			if (GameInfo.roundNumber == 1)
			{
				m_randomPlane = UnityEngine.Random.Range(1, 4);
				if (m_randomPlane == 1)
				{
                    m_eCurrentPlaneState = PlaneState.eFirst;
                    plane.controller = XboxController.First;
					GameInfo.playerOneHasPlane = true;
				}
				else if (m_randomPlane == 2)
				{
                    m_eCurrentPlaneState = PlaneState.eSecond;
                    plane.controller = XboxController.Second;
					GameInfo.playerTwoHasPlane = true;
				}
				else if (m_randomPlane == 3)
				{
                    m_eCurrentPlaneState = PlaneState.eThird;
                    plane.controller = XboxController.Third;    //Make purple control the plane this round
					GameInfo.playerThreeHasPlane = true;
				}
			}
			else if (GameInfo.roundNumber == 2)
			{
				m_randomPlane = UnityEngine.Random.Range(1, 3); // Between 1 and 2,

				if (m_randomPlane == 1) // First available player.
				{
					if (GameInfo.playerOneHasPlane) // Player one has already been in the plane.
					{
                        m_eCurrentPlaneState = PlaneState.eSecond;
                        plane.controller = XboxController.Second; // Player two is now in the plane.
						GameInfo.playerTwoHasPlane = true;
					}
					else if (GameInfo.playerTwoHasPlane || GameInfo.playerThreeHasPlane) // Player two has already been in the plane.
					{
                        m_eCurrentPlaneState = PlaneState.eFirst;
                        plane.controller = XboxController.First; // Player one is now in the plane.
						GameInfo.playerOneHasPlane = true;
					}
				}
				else if (m_randomPlane == 2) // Second available player.
				{
					if (GameInfo.playerOneHasPlane || GameInfo.playerTwoHasPlane) // Player one has already been in the plane.
					{
                        m_eCurrentPlaneState = PlaneState.eThird;
                        plane.controller = XboxController.Third; // Player three is now in the plane.
						GameInfo.playerThreeHasPlane = true;
					}
					else if (GameInfo.playerThreeHasPlane) // Player three has already been in the plane.s
					{
                        m_eCurrentPlaneState = PlaneState.eSecond;
                        plane.controller = XboxController.Second; // Player two is now in the plane.
						GameInfo.playerTwoHasPlane = true;
					}
				}
			}
			else if (GameInfo.roundNumber == 3)
			{
				if (!GameInfo.playerOneHasPlane) // If player one hasn't been in the plane.
				{
                    m_eCurrentPlaneState = PlaneState.eFirst;
                    plane.controller = XboxController.First; // Player one is now in the plane.
					GameInfo.playerOneHasPlane = true;
				}
				else if (!GameInfo.playerTwoHasPlane) // If player two hasn't been in the plane.
				{
                    m_eCurrentPlaneState = PlaneState.eSecond;
                    plane.controller = XboxController.Second; // Player two is now in the plane.
					GameInfo.playerTwoHasPlane = true;
				}
				else if (!GameInfo.playerThreeHasPlane) // If player three hasn't been in the plane.
				{
                    m_eCurrentPlaneState = PlaneState.eThird;
                    plane.controller = XboxController.Third; // Player three is now in the plane.
					GameInfo.playerThreeHasPlane = true;
				}
			}

			orangeSkier.gameObject.SetActive(false); // Won't be used.
		}
		else if (m_playerCount == 4)
		{
			purpleSkier.controller = XboxController.Third;  //Third player is purple
			orangeSkier.controller = XboxController.Fourth; //Fourth player is orange

			if (GameInfo.roundNumber == 1)
			{
				m_randomPlane = UnityEngine.Random.Range(1, 5); // Between 1 and 4,

				if (m_randomPlane == 1) // First available player.
				{
                    m_eCurrentPlaneState = PlaneState.eFirst;
                    plane.controller = XboxController.First; // Player one is now in the plane.
					GameInfo.playerOneHasPlane = true;
				}
				else if (m_randomPlane == 2) // Second available player.
				{
                    m_eCurrentPlaneState = PlaneState.eSecond;
                    plane.controller = XboxController.Second; // Player two is now in the plane.
					GameInfo.playerTwoHasPlane = true;
				}
				else if (m_randomPlane == 3) // Third available player.
				{
                    m_eCurrentPlaneState = PlaneState.eThird;
                    plane.controller = XboxController.Third; // Player three is now in the plane.
					GameInfo.playerThreeHasPlane = true;
				}
				else if (m_randomPlane == 4) // Fourth available player.
				{
                    m_eCurrentPlaneState = PlaneState.eFourth;
                    plane.controller = XboxController.Fourth; // Player four is now in the plane.
					GameInfo.playerFourHasPlane = true;
				}
			}
			else if (GameInfo.roundNumber == 2)
			{
				m_randomPlane = UnityEngine.Random.Range(1, 4); // Between 1 and 3,

				if (m_randomPlane == 1) // First available player.
				{
					if (GameInfo.playerOneHasPlane) // Player one has already been in the plane.
					{
                        m_eCurrentPlaneState = PlaneState.eSecond;
                        plane.controller = XboxController.Second; // Second player is in the plane.
						GameInfo.playerTwoHasPlane = true;
					}
					else if (GameInfo.playerTwoHasPlane || GameInfo.playerThreeHasPlane || GameInfo.playerFourHasPlane) // Player two, three and four has already been in the plane.
					{
                        m_eCurrentPlaneState = PlaneState.eFirst;
                        plane.controller = XboxController.First; // First player is in the plane.
						GameInfo.playerOneHasPlane = true;
					}
				}
				else if (m_randomPlane == 2) // Second available player.
				{
					if (GameInfo.playerThreeHasPlane || GameInfo.playerFourHasPlane) // Player three and four has been in the plane.
					{
                        m_eCurrentPlaneState = PlaneState.eSecond;
                        plane.controller = XboxController.Second; // Second player in plane.
						GameInfo.playerTwoHasPlane = true;
					}
					else if (GameInfo.playerOneHasPlane || GameInfo.playerTwoHasPlane) // Player two has been in the plane.
					{
                        m_eCurrentPlaneState = PlaneState.eThird;
                        plane.controller = XboxController.Third; // Third player in plane.
						GameInfo.playerThreeHasPlane = true;
					}
				}
				else if (m_randomPlane == 3) // Third available player.
				{
					if (GameInfo.playerFourHasPlane) // Player four has been in the plane.
					{
                        m_eCurrentPlaneState = PlaneState.eThird;
                        plane.controller = XboxController.Third; // Third player in plane.
						GameInfo.playerThreeHasPlane = true;
					}
                    else if (GameInfo.playerOneHasPlane || GameInfo.playerTwoHasPlane || GameInfo.playerThreeHasPlane)
                    {
                        m_eCurrentPlaneState = PlaneState.eFourth;
                        plane.controller = XboxController.Fourth; // Fourth player in plane.
                        GameInfo.playerFourHasPlane = true;
                    }
				}
			}
			else if (GameInfo.roundNumber == 3)
			{
				m_randomPlane = UnityEngine.Random.Range(1, 3); // Between 1 and 2,

                if (m_randomPlane == 1) // First available player.
                {
                    if (!GameInfo.playerOneHasPlane)
                    {
                        m_eCurrentPlaneState = PlaneState.eFirst;
                        plane.controller = XboxController.First;
                        GameInfo.playerOneHasPlane = true;
                    }
                    else if (!GameInfo.playerTwoHasPlane)
                    {
                        m_eCurrentPlaneState = PlaneState.eSecond;
                        plane.controller = XboxController.Second;
                        GameInfo.playerTwoHasPlane = true;
                    }
                    else if (!GameInfo.playerThreeHasPlane)
                    {
                        m_eCurrentPlaneState = PlaneState.eThird;
                        plane.controller = XboxController.Third;
                        GameInfo.playerThreeHasPlane = true;
                    }
                    else if (!GameInfo.playerFourHasPlane)
                    {
                        m_eCurrentPlaneState = PlaneState.eFourth;
                        plane.controller = XboxController.Fourth;
                        GameInfo.playerFourHasPlane = true;
                    }
                }
                else if (m_randomPlane == 2) // Second available player.
                {
                    if ((GameInfo.playerOneHasPlane && GameInfo.playerTwoHasPlane) || (GameInfo.playerOneHasPlane && GameInfo.playerThreeHasPlane) || (GameInfo.playerTwoHasPlane && GameInfo.playerThreeHasPlane)) // 1 and 2 || 1 and 3 || 2 and 3.
                    {
                        m_eCurrentPlaneState = PlaneState.eFourth;
                        plane.controller = XboxController.Fourth; // Fourth is in plane.
                        GameInfo.playerFourHasPlane = true;
                    }
                    else if ((GameInfo.playerOneHasPlane && GameInfo.playerFourHasPlane) || (GameInfo.playerTwoHasPlane && GameInfo.playerFourHasPlane)) // 1 and 4 || 2 and 4.
                    {
                        m_eCurrentPlaneState = PlaneState.eThird;
                        plane.controller = XboxController.Third; // Third is in plane.
                        GameInfo.playerThreeHasPlane = true;
                    }
                    else if (GameInfo.playerThreeHasPlane && GameInfo.playerFourHasPlane) // 3 and 4.
                    {
                        m_eCurrentPlaneState = PlaneState.eSecond;
                        plane.controller = XboxController.Second; // Second is in plane.
                        GameInfo.playerTwoHasPlane = true;
                    }
                }
			}
			else if (GameInfo.roundNumber == 4)
            {
                if (!GameInfo.playerOneHasPlane)
                {
                    m_eCurrentPlaneState = PlaneState.eFirst;
                    plane.controller = XboxController.First;
                    GameInfo.playerOneHasPlane = true;
                }
                else if (!GameInfo.playerTwoHasPlane)
                {
                    m_eCurrentPlaneState = PlaneState.eSecond;
                    plane.controller = XboxController.Second;
                    GameInfo.playerTwoHasPlane = true;
                }
                else if (!GameInfo.playerThreeHasPlane)
                {
                    m_eCurrentPlaneState = PlaneState.eThird;
                    plane.controller = XboxController.Third;
                    GameInfo.playerThreeHasPlane = true;
                }
                else if (!GameInfo.playerFourHasPlane)
                {
                    m_eCurrentPlaneState = PlaneState.eFourth;
                    plane.controller = XboxController.Fourth;
                    GameInfo.playerFourHasPlane = true;
                }
            }
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
		redSkier.SetScore(GameInfo.playerOneScore);
		greenSkier.SetScore(GameInfo.playerTwoScore);
		if (m_playerCount >= 3)
			purpleSkier.SetScore(GameInfo.playerThreeScore);
		if (m_playerCount == 4)
			orangeSkier.SetScore(GameInfo.playerFourScore);

		//Ensure no text is displayed at the very start
		startCountdownDisplay.text = "";
		roundTimerPanel.SetActive(false);
		playerOneUI.SetActive(false);
		playerTwoUI.SetActive(false);
		playerThreeUI.SetActive(false);
		playerFourUI.SetActive(false);
		beachBombAbilityUI.SetActive(false);
		tetheredMineAbilityUI.SetActive(false);
		scoreRed.text = "";
		scoreGreen.text = "";
		scorePurple.text = "";
		scoreOrange.text = "";
		livesRed.fillAmount = 0;
		livesGreen.fillAmount = 0;
		livesPurple.fillAmount = 0;
		livesOrange.fillAmount = 0;
		multiplierRed.text = "";
		multiplierGreen.text = "";
		multiplierPurple.text = "";
		multiplierOrange.text = "";
		beachBombAbility.fillAmount = 0;
		tetheredMineAbility.fillAmount = 0;
		bonus.text = "";

		roundOverPanel.SetActive(false);

		SceneMovementActive(false);     //Wait for the countdown before starting movement

		if (m_playerCount == 2)
		{
            if (m_eCurrentPlaneState == PlaneState.eFirst)
            {
                redSkier.gameObject.SetActive(false); // Player one will start in the plane.
                redSkier.SetAlive(false);
                greenSkier.gameObject.SetActive(true);  // Green is skier.
                greenSkier.SetAlive(true);

                greenSkier.transform.position = m_twoPlayerSkierPos;
                SetPlaneTetherReferences(greenSkier);
				planeBody.GetComponent<Renderer>().material = redSkier.gameObject.GetComponent<Renderer>().material; // Changes colour to player one.
				//planeBody.GetComponent<Renderer>().material.SetTexture("Texture2D_C6055840", axolotlPlaneTexture);
			}
            else if (m_eCurrentPlaneState == PlaneState.eSecond)
            {
                //In the first round, green is plane, red is skier
                redSkier.gameObject.SetActive(true);
                redSkier.SetAlive(true);
                greenSkier.gameObject.SetActive(false); // Player two will start in the plane.
                greenSkier.SetAlive(false);

                redSkier.transform.position = m_twoPlayerSkierPos;

                SetPlaneTetherReferences(redSkier);

                planeBody.GetComponent<Renderer>().material = greenSkier.gameObject.GetComponent<Renderer>().material; // Plane's colour will be the same as player two.
            }
		}
		else if (m_playerCount == 3)
		{
            if (m_eCurrentPlaneState == PlaneState.eFirst)
            {
                redSkier.gameObject.SetActive(false); // Player one will start in the plane.
                redSkier.SetAlive(false);
                greenSkier.gameObject.SetActive(true); // Player two skier will be active.
                greenSkier.SetAlive(true);
                purpleSkier.gameObject.SetActive(true); // Player three skier will be active.
                purpleSkier.SetAlive(true);

                greenSkier.transform.position = m_threePlayerSkierPosOne;
                purpleSkier.transform.position = m_threePlayerSkierPosTwo;

                SetPlaneTetherReferences(greenSkier, purpleSkier);

                planeBody.GetComponent<Renderer>().material = redSkier.gameObject.GetComponent<Renderer>().material; // Plane colour = player one colour.
            }
            else if (m_eCurrentPlaneState == PlaneState.eSecond)
            {
                redSkier.gameObject.SetActive(true); // Player one skier active.
                redSkier.SetAlive(true);
                greenSkier.gameObject.SetActive(false); // Player two in plane.
                greenSkier.SetAlive(false);
                purpleSkier.gameObject.SetActive(true); // Player three skier will be active.
                purpleSkier.SetAlive(true);

                redSkier.transform.position = m_threePlayerSkierPosOne;
                purpleSkier.transform.position = m_threePlayerSkierPosTwo;

                SetPlaneTetherReferences(redSkier, purpleSkier);

                planeBody.GetComponent<Renderer>().material = greenSkier.gameObject.GetComponent<Renderer>().material; // Plane colour = player two colour.
            }
            else if (m_eCurrentPlaneState == PlaneState.eThird)
            {
                redSkier.gameObject.SetActive(true); // Player one skier.
                redSkier.SetAlive(true);
                greenSkier.gameObject.SetActive(true); // Player two skier will be active.
                greenSkier.SetAlive(true);
                purpleSkier.gameObject.SetActive(false); // Player three in plane.
                purpleSkier.SetAlive(false);

                redSkier.transform.position = m_threePlayerSkierPosOne;
                greenSkier.transform.position = m_threePlayerSkierPosTwo;

                SetPlaneTetherReferences(redSkier, greenSkier);

                planeBody.GetComponent<Renderer>().material = purpleSkier.gameObject.GetComponent<Renderer>().material; // Plane colour = player one colour.
            }
		}
		else if (m_playerCount == 4)
		{
            if (m_eCurrentPlaneState == PlaneState.eFirst)
            {
                redSkier.gameObject.SetActive(false); // Player one will start in the plane.
                redSkier.SetAlive(false);
                greenSkier.gameObject.SetActive(true); // Player two skier will be active.
                greenSkier.SetAlive(true);
                purpleSkier.gameObject.SetActive(true); // Player three skier will be active.
                purpleSkier.SetAlive(true);
                orangeSkier.gameObject.SetActive(true); // Player four skier.
                orangeSkier.SetAlive(true);

                greenSkier.transform.position = m_fourPlayerSkierPosOne;
                purpleSkier.transform.position = m_fourPlayerSkierPosTwo;
                orangeSkier.transform.position = m_fourPlayerSkierPosThree;

                SetPlaneTetherReferences(greenSkier, purpleSkier, orangeSkier);

                planeBody.GetComponent<Renderer>().material = redSkier.gameObject.GetComponent<Renderer>().material; // Plane colour = player one colour.
            }
            else if (m_eCurrentPlaneState == PlaneState.eSecond)
            {
                redSkier.gameObject.SetActive(true); // Player one skier.
                redSkier.SetAlive(true);
                greenSkier.gameObject.SetActive(false); // Player two in plane.
                greenSkier.SetAlive(false);
                purpleSkier.gameObject.SetActive(true); // Player three skier will be active.
                purpleSkier.SetAlive(true);
                orangeSkier.gameObject.SetActive(true); // Player four skier.
                orangeSkier.SetAlive(true);

                redSkier.transform.position = m_fourPlayerSkierPosOne;
                purpleSkier.transform.position = m_fourPlayerSkierPosTwo;
                orangeSkier.transform.position = m_fourPlayerSkierPosThree;

                SetPlaneTetherReferences(redSkier, purpleSkier, orangeSkier);

                planeBody.GetComponent<Renderer>().material = greenSkier.gameObject.GetComponent<Renderer>().material; // Plane colour = player one colour.
            }
            else if (m_eCurrentPlaneState == PlaneState.eThird)
            {
                redSkier.gameObject.SetActive(true); // Player one skier.
                redSkier.SetAlive(true);
                greenSkier.gameObject.SetActive(true); // Player two skier.
                greenSkier.SetAlive(true);
                purpleSkier.gameObject.SetActive(false); // Player three in plane.
                purpleSkier.SetAlive(false);
                orangeSkier.gameObject.SetActive(true); // Player four skier.
                orangeSkier.SetAlive(true);

                redSkier.transform.position = m_fourPlayerSkierPosOne;
                greenSkier.transform.position = m_fourPlayerSkierPosTwo;
                orangeSkier.transform.position = m_fourPlayerSkierPosThree;

                SetPlaneTetherReferences(redSkier, greenSkier, orangeSkier);

                planeBody.GetComponent<Renderer>().material = purpleSkier.gameObject.GetComponent<Renderer>().material; // Plane colour = player one colour.
            }
            else if (m_eCurrentPlaneState == PlaneState.eFourth)
            {
                redSkier.gameObject.SetActive(true); // Player one skier.
                redSkier.SetAlive(true);
                greenSkier.gameObject.SetActive(true); // Player two skier.
                greenSkier.SetAlive(true);
                purpleSkier.gameObject.SetActive(true); // Player three skier.
                purpleSkier.SetAlive(true);
                orangeSkier.gameObject.SetActive(false); // Player four in plane.
                orangeSkier.SetAlive(false);

                redSkier.transform.position = m_fourPlayerSkierPosOne;
                greenSkier.transform.position = m_fourPlayerSkierPosTwo;
                purpleSkier.transform.position = m_fourPlayerSkierPosThree;

                SetPlaneTetherReferences(redSkier, greenSkier, purpleSkier);

                planeBody.GetComponent<Renderer>().material = orangeSkier.gameObject.GetComponent<Renderer>().material; // Plane colour = player one colour.
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
					StartCoroutine(ClearText(1));               //Set the text to turn off after 1 second
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

				//If the time limit runs out
				if (!m_playingRoundTimer.UnderMax())
				{
					CallOnSkiers(ApplySkierAliveBonus);		//Apply score bonuses to any skiers that are still alive

					m_eCurrentState = RoundState.eRoundOver;    //Swap to the round over screen
					roundTimerPanel.SetActive(false);
					SceneMovementActive(false);                 //Deactivate scene movement
					bonus.text = "Alive skiers get a bonus of " + skierBonus.ToString();
					roundOverPanel.SetActive(true);             //Show the round over screen
				}
				//If all skiers get wiped out (non-skiers are set to not alive by default)
				else if (!redSkier.GetAlive() && !greenSkier.GetAlive() && !purpleSkier.GetAlive() && !orangeSkier.GetAlive())
				{
					//Add a score bonus to the player controlling the plane
					m_skiers[(int)m_eCurrentPlaneState].AddScore(planeBonus);

					m_eCurrentState = RoundState.eRoundOver;    //Swap to the round over screen
					roundTimerPanel.SetActive(false);
					SceneMovementActive(false);                 //Deactivate scene movement
					bonus.text = "All skiers wiped out! Plane gets " + planeBonus.ToString() + " bonus score!";
					roundOverPanel.SetActive(true);             //Show the round over screen
				}

				roundTimerPanel.SetActive(true);
				playingCountDownDisplay.fillAmount = m_playingRoundTimer.T / roundTimeLimit;

				//Check if any skiers are hit
				CallOnSkiers(SkierHurtBonusCheck);

				//Display skier and plane related UI
				CallOnSkiers(SetSkierUI);
				beachBombAbilityUI.SetActive(true);
				tetheredMineAbilityUI.SetActive(true);
				beachBombAbility.fillAmount = target.abilityCooldown.T / target.abilityCooldown.maxTime;
				tetheredMineAbility.fillAmount = planeHatch.mineAbilityCooldown.T / planeHatch.mineAbilityCooldown.maxTime;
				
				//Display the controller button sprites for plane abilities
				if (beachBombAbility.fillAmount == 1 && !target.isAiming)
					beachBombControllerAim.enabled = true;
				else
					beachBombControllerAim.enabled = false;

				if (tetheredMineAbility.fillAmount == 1)
					tetheredMineController.enabled = true;
				else
					tetheredMineController.enabled = false;

				if (target.isAiming)
					beachBombControllerShoot.enabled = true;
				else
					beachBombControllerShoot.enabled = false;
				
				break;
				//-------------------------------------------------------------------------

			case RoundState.eRoundOver:

				if (Input.GetKeyDown(KeyCode.Space) || XCI.GetButtonDown(XboxButton.A, XboxController.All))	//If next round is selected,
				{
					//Update the static GameInfo scores
					GameInfo.playerOneScore = redSkier.GetScore();
					GameInfo.playerTwoScore = greenSkier.GetScore();
					if (m_playerCount >= 3)
						GameInfo.playerThreeScore = purpleSkier.GetScore();
					if (m_playerCount == 4)
						GameInfo.playerFourScore = orangeSkier.GetScore();

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

	//Function that takes another function as a parameter, 
	//checks which skiers are in the game, then calls the function on the correct skiers
	private void CallOnSkiers(Function FunctionToCall)
	{
		FunctionToCall(0);			//Red skier always in the game, so call function on red
		FunctionToCall(1);			//Green skier always in the game, so call function on green
		if (m_playerCount >= 3)		//If the purple skier is in the game,
		{
			FunctionToCall(2);		//Call the function on purple
			if (m_playerCount == 4)	//If the orange skier is in the game,
				FunctionToCall(3);	//Call the function on orange
		}
	}

	//Sets/updates all the UI related to a skier
	private void SetSkierUI(int skierNumber)
	{
		m_skierScores[skierNumber].text = m_skiers[skierNumber].GetScore().ToString();	//Display the score
		
		if (m_skiers[skierNumber].GetAlive())											//If the skier is still alive,
		{
			m_skierLives[skierNumber].fillAmount = m_skiers[skierNumber].lives / 3.0f;	//Display the lives
			m_skierMultipliers[skierNumber].text = "x" + m_skiers[skierNumber].GetPlayerMultiplier().ToString();	//Display the multiplier
		}
		else											//If the skier isn't alive,
		{
			m_skierLives[skierNumber].fillAmount = 0;	//Show no lives
			m_skierMultipliers[skierNumber].text = "";	//Don't show a multiplier
		}
	}

	//Adds a bonus to any alive skiers
	private void ApplySkierAliveBonus(int skierNumber)
	{
		if (m_skiers[skierNumber].GetAlive())			//If this skier is alive,
			m_skiers[skierNumber].AddScore(skierBonus); //Add a bonus to its score
	}

	//Adds score to the plane when skiers are hit
	private void SkierHurtBonusCheck(int skierNumber)
	{
		if (m_skiers[skierNumber].hurtThisFrame)							//If this skier got hurt this frame,
			m_skiers[(int)m_eCurrentPlaneState].AddScore(skierHurtBonus);	//Add score to the current plane player
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
		playerOneUI.SetActive(value);
		greenSkier.tether.enabled = value;
		playerTwoUI.SetActive(value);
		if (purpleSkier.gameObject.activeSelf == true)  //If the purple skier is in the game,
			purpleSkier.tether.enabled = value;         //Set them
		if (orangeSkier.gameObject.activeSelf == true)  //If the orange skier is in the game,
			orangeSkier.tether.enabled = value;         //Set them too
		if (m_playerCount >= 3)
			playerThreeUI.SetActive(value);
		if (m_playerCount == 4)
			playerFourUI.SetActive(value);
		mine.enabled = value;
		planeHatch.mineAbilityCooldown.enabled = value;
		target.enabled = value;
		mainCamera.enabled = value;
	}

	//Takes in 1-3 skier references, gets their tethers, and passes them on to the plane
	private void SetPlaneTetherReferences(SkierController skierOne, SkierController skierTwo = null, SkierController skierThree = null)
	{
		Tether[] skierTethers = new Tether[3];
		skierTethers[0] = skierOne.GetComponent<Tether>();
		if (skierTwo != null)
		{
			skierTethers[1] = skierTwo.GetComponent<Tether>();
			if (skierThree != null)
				skierTethers[2] = skierThree.GetComponent<Tether>();
		}
		plane.SetTetherReferences(skierTethers);
	}

	//Coroutine to turn off countdown text
	IEnumerator ClearText(float interval)
	{
		yield return new WaitForSeconds(interval);	//Wait for a certain amount of time
		startCountdownDisplay.text = "";			//Turn the countdown text off
	}
}
