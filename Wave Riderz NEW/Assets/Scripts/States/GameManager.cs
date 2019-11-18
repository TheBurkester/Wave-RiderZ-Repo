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
	private Vector3[,] m_skierSpawnPos;
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
	public GameObject roundOverPanel = null;        //Reference to the panel with all the round over stuff
	public Text bonus = null;
	//-------------------------------------------------------------------------

	public Texture axolotlPlaneTexture = null;
	public delegate void Function(int skierNumber);

	void Awake()
	{
		//Set the reference arrays
		m_skiers = new SkierController[6] { redSkier, greenSkier, purpleSkier, orangeSkier, null, null };	//Add two nulls at the end so they don't throw errors if functions check those indexes
		m_skierScores = new Text[4] { scoreRed, scoreGreen, scorePurple, scoreOrange };
		m_playerUI = new GameObject[4] { playerOneUI, playerTwoUI, playerThreeUI, playerFourUI };
		m_skierLives = new Image[4] { livesRed, livesGreen, livesPurple, livesOrange };
		m_skierMultipliers = new Text[4] { multiplierRed, multiplierGreen, multiplierPurple, multiplierOrange };

		//Set the spawnpoints in the 2D array
		m_skierSpawnPos = new Vector3[3, 3]{
			{ m_twoPlayerSkierPos, Vector3.zero, Vector3.zero },
			{ m_threePlayerSkierPosOne, m_threePlayerSkierPosTwo, Vector3.zero},
			{ m_fourPlayerSkierPosOne, m_fourPlayerSkierPosTwo, m_fourPlayerSkierPosThree}
			};

		planeHatch.setIsUsingAbility(false);

		SetSkiers();	//Set the skiers to their controllers
		SetPlane();		//Set which player controls the plane
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

		SetupScene();
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

	//Sets skier controllers and disables unused skiers
	private void SetSkiers()
	{
		//Setting controllers
		for (int i = 0; i < m_playerCount; ++i)					//For the amount of players,
			m_skiers[i].controller = (XboxController)(i + 1);	//Set each skier to use the correct controller

		//Disable any other skiers which are larger than the player count, if they exist
		if (m_skiers[m_playerCount] != null)						//If the skier at the player count index exists,
		{
			m_skiers[m_playerCount].gameObject.SetActive(false);			//Set it to inactive, since it won't be used
			if (m_skiers[m_playerCount + 1] != null)						//If the skier at the index above that also exists,
				m_skiers[m_playerCount + 1].gameObject.SetActive(false);	//Set it to inactive as well
		}
	}

	//Randomly sets which player controls the plane
	private void SetPlane()
	{
		bool planeSelected = false;	//Keep looping until a player that hasn't been the plane has been selected
		int planePlayer = 0;		//Which player we will set to be the plane
		while (!planeSelected)											//Whilst selection is still happening,
		{
			planePlayer = UnityEngine.Random.Range(0, m_playerCount);	//Randomly pick a player to be plane
			if (GameInfo.playerBeenPlane[planePlayer] == false)			//If this player hasn't been plane yet,
				planeSelected = true;									//Stop the selection process
		}

		m_eCurrentPlaneState = (PlaneState)planePlayer;			//Keep track of who is in the plane
		plane.controller = (XboxController)(planePlayer + 1);	//Set the plane controller
		GameInfo.playerBeenPlane[planePlayer] = true;			//Update the game info
	}

	//Activates the right skiers, sets materials, places skiers, and passes tether references to the plane
	private void SetupScene()
	{
		Tether[] tethers = new Tether[3];	//Array of tethers to pass onto the plane

		int place = 0;	//Which place the skier will be spawned to, such as 0, 1, or 2, based on how many players

		for (int i = 0; i < m_playerCount; ++i)				//For all current players,
		{
			if (i == (int)m_eCurrentPlaneState)				//If this player is in the plane,
			{
				m_skiers[i].gameObject.SetActive(false);	//Set their skier to inactive
				m_skiers[i].SetAlive(false);				//Make sure they aren't considered alive
				//Set the material of the plane
				planeBody.GetComponent<Renderer>().material = m_skiers[i].gameObject.GetComponent<Renderer>().material;
				//planeBody.GetComponent<Renderer>().material.SetTexture("Texture2D_C6055840", axolotlPlaneTexture);
			}
			else										//If this player isn't in the plane,
			{
				m_skiers[i].gameObject.SetActive(true);	//Activate their skier
				m_skiers[i].SetAlive(true);				//Set them to alive
				tethers[i] = m_skiers[i].tether;		//Adds the tether to the references array
				//Put the skier in the right position
				m_skiers[i].transform.position = m_skierSpawnPos[m_playerCount - 2, place];
				++place;	//Increment the place
			}
		}

		plane.SetTetherReferences(tethers);	//Pass the tethers to the plane
	}

	//Function that takes another function as a parameter, 
	//checks which skiers are in the game, then calls the function on the correct skiers
	private void CallOnSkiers(Function FunctionToCall)
	{
		for (int i = 0; i < m_playerCount; ++i)	//For the amount of people currently playing,
			FunctionToCall(i);					//Call the function on the current player
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

	//Coroutine to turn off countdown text
	IEnumerator ClearText(float interval)
	{
		yield return new WaitForSeconds(interval);	//Wait for a certain amount of time
		startCountdownDisplay.text = "";			//Turn the countdown text off
	}
}
