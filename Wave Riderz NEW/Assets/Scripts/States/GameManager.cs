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
	public SkierController playerOneSkier = null;	//Reference to the red skier script
	public SkierController playerTwoSkier = null;	//Reference to the green skier script
	public SkierController playerThreeSkier = null;	//Reference to the orange skier script
	public SkierController playerFourSkier = null;	//Reference to the purple skier script
	private SkierController[] m_skiers = null;      //Array containing the skiers in numerical order
	private SkierController[] m_sortedSkiers = null;//Array containing the skiers in descending order of score
	private SkierController m_prevTopSkier = null;	//Keep track of the previous top scoring skier
	public Texture axlPlaneTexture = null;
    public Texture carlPlaneTexture = null;
	public Texture mannyPlaneTexture = null;
	public Texture hydraPlaneTexture = null;
    private Texture[] m_planeTextures = null;
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
	public float roundTimeLimit = 45;                              //How long a round lasts
                                                                   //-------------------------------------------------------------------------

    //UI references
    public Canvas canvas = null;
	public Text startCountdownDisplay = null;       //Reference to the countdown timer text at the start of the round
    public Text roundNumberText = null;
    public Text planePlayerText = null;
	public GameObject roundTimerPanel = null;		// Reference to the round timer panel.
	public Image playingCountDownDisplay = null;    //Reference to the round timer image
	public Text scoreOne = null;
	public Text scoreTwo = null;
	public Text scoreThree = null;
	public Text scoreFour = null;
	private Text[] m_skierScoresText = null;
	public Text roundOverScoreOne = null;
	public Text roundOverScoreTwo = null;
	public Text roundOverScoreThree = null;
	public Text roundOverScoreFour = null;
	private Text[] m_roundOverScoresText = null;
	public RectTransform playerOneUI = null;
	public RectTransform playerTwoUI = null;
	public RectTransform playerThreeUI = null;
	public RectTransform playerFourUI = null;
	private RectTransform[] m_playerUI = null;
	public GameObject beachBombAbilityUI = null;
	public GameObject tetheredMineAbilityUI = null;
	public Image livesOne = null;
	public Image livesTwo = null;
	public Image livesThree = null;
	public Image livesFour = null;
	private Image[] m_skierLives = null;
    public Image planeOne = null;
    public Image planeTwo = null;
    public Image planeThree = null;
    public Image planeFour = null;
    private Image[] m_planeImage = null;
	public Text multiplierOne = null;
	public Text multiplierTwo = null;
	public Text multiplierThree = null;
	public Text multiplierFour = null;
	private Text[] m_skierMultipliers = null;
	public Image beachBombAbility = null;
	public Image tetheredMineAbility = null;
	public Image beachBombControllerAim = null;
	public Image beachBombControllerShoot = null;
	public Image tetheredMineController = null;
	public GameObject woodenSign = null;
	public GameObject roundOverPanel = null;        //Reference to the panel with all the round over stuff
	public Text bonus = null;
    public RectTransform wavePanel = null;
    private Vector3 m_panelOffScreenLeft = new Vector3(-1600, 0, 0);
    private Vector3 m_panelOffScreenRight = new Vector3(2600, 0, 0);
    private int m_t = 0;
    private bool m_nextRound = false;
	//-------------------------------------------------------------------------
    
	public delegate void Function(int skierNumber);

    private string[] m_skierHurtSounds;

	void Awake()
	{
		//Set the reference arrays
		m_skiers = new SkierController[6] { playerOneSkier, playerTwoSkier, playerThreeSkier, playerFourSkier, null, null };    //Add two nulls at the end so they don't throw errors if functions check those indexes
		m_skierScoresText = new Text[4] { scoreOne, scoreTwo, scoreThree, scoreFour };
		m_roundOverScoresText = new Text[4] { roundOverScoreOne, roundOverScoreTwo, roundOverScoreThree, roundOverScoreFour };
		m_prevTopSkier = null;
		m_planeTextures = new Texture[4] { axlPlaneTexture, carlPlaneTexture, mannyPlaneTexture, hydraPlaneTexture  };
        m_playerUI = new RectTransform[4] { playerOneUI, playerTwoUI, playerThreeUI, playerFourUI };
		m_skierLives = new Image[4] { livesOne, livesTwo, livesThree, livesFour };
        m_planeImage = new Image[4] { planeOne, planeTwo, planeThree, planeFour };
		m_skierMultipliers = new Text[4] { multiplierOne, multiplierTwo, multiplierThree, multiplierFour };
        m_skierHurtSounds = new string[4] {"Player1Damage", "Player2Damage","Player3Damage","Player4Damage" };

		//Make a new array of references to the skiers to sort
		m_sortedSkiers = new SkierController[m_skiers.Length];
		for (int i = 0; i < m_skiers.Length; i++)
			m_sortedSkiers[i] = m_skiers[i];

		//Set the spawnpoints in the 2D array
		m_skierSpawnPos = new Vector3[3, 3]{
			{ m_twoPlayerSkierPos, Vector3.zero, Vector3.zero },
			{ m_threePlayerSkierPosOne, m_threePlayerSkierPosTwo, Vector3.zero},
			{ m_fourPlayerSkierPosOne, m_fourPlayerSkierPosTwo, m_fourPlayerSkierPosThree}
			};

		planeHatch.setIsUsingAbility(false);

        wavePanel.transform.position = canvas.transform.position;

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
		m_skiers[0].SetScore(GameInfo.playerOneScore);
		playerTwoSkier.SetScore(GameInfo.playerTwoScore);
		if (m_playerCount >= 3)
		{
			playerThreeSkier.SetScore(GameInfo.playerThreeScore);
			if (m_playerCount == 4)
				playerFourSkier.SetScore(GameInfo.playerFourScore);
		}
		ApplyTopScoreParticle();
		
		//Ensure no text is displayed at the very start
		startCountdownDisplay.text = "";
        roundNumberText.text = "";
        planePlayerText.text = "";
		roundTimerPanel.SetActive(false);
		foreach (RectTransform rect in m_playerUI)
			rect.gameObject.SetActive(false);
		beachBombAbilityUI.SetActive(false);
		tetheredMineAbilityUI.SetActive(false);
		woodenSign.SetActive(false);
		foreach (Text score in m_skierScoresText)
			score.text = "";
		foreach (Image lives in m_skierLives)
			lives.fillAmount = 0;
        foreach (Image plane in m_planeImage)
            plane.gameObject.SetActive(false);
		foreach (Text multiplier in m_skierMultipliers)
			multiplier.text = "";
		foreach (Text score in m_roundOverScoresText)
			score.text = "";
		beachBombAbility.fillAmount = 0;
		tetheredMineAbility.fillAmount = 0;
		bonus.text = "";

		CallOnSkiers(SetSkierUIPosition);

		roundOverPanel.SetActive(false);

		SceneMovementActive(false);     //Wait for the countdown before starting movement

		SetupScene();

        AudioManager.Play("Race_Start");
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
                    planePlayerText.text = "";
					roundNumberText.text = "";
					woodenSign.SetActive(false);
					startCountdownDisplay.fontSize = 130;
					startCountdownDisplay.text = "GO!";
					StartCoroutine(ClearText(1));               //Set the text to turn off after 1 second
					SceneMovementActive(true);                  //Activate scene movement
                    AudioManager.Play("LevelMusic1");
				}
				else                                            //Otherwise the timer is still going,
				{
                    m_t += 40;

                    wavePanel.transform.position = Vector3.MoveTowards(canvas.transform.position, m_panelOffScreenRight, m_t); // Slowly moves the position to the target.
					int closestSecond = (int)Math.Ceiling(m_startRoundTimer.T); //Round the timer up to the nearest second
					if (closestSecond == 4)
						startCountdownDisplay.fontSize = 50;
					else if (closestSecond == 3)
						startCountdownDisplay.fontSize = 70;
					else if (closestSecond == 2)
						startCountdownDisplay.fontSize = 90;
					else if (closestSecond == 1)
						startCountdownDisplay.fontSize = 110;

					startCountdownDisplay.text = closestSecond.ToString();

					woodenSign.SetActive(true);
                    roundNumberText.text = "Round " + GameInfo.roundNumber;
                    planePlayerText.text = "Player " + ((int)m_eCurrentPlaneState + 1) + " is in the plane!"; // Plane state needs to be added by 1 as Player One = 0 usually.
				}

				break;
			//-------------------------------------------------------------------------

			case RoundState.ePlayingRound:

				//If the time limit runs out
				if (!m_playingRoundTimer.UnderMax())
				{
					CallOnSkiers(ApplySkierAliveBonus);		//Apply score bonuses to any skiers that are still alive
                    AudioManager.Play("RoundOver");
                    m_eCurrentState = RoundState.eRoundOver;    //Swap to the round over screen
					roundTimerPanel.SetActive(false);
					SceneMovementActive(false);                 //Deactivate scene movement
                    AudioManager.Stop("PlaneAudio");
					bonus.text = "Alive skiers get a bonus of " + skierBonus.ToString();
					roundOverPanel.SetActive(true);             //Show the round over screen
					UpdateSortedSkiers();
					CallOnSkiers(SetScoreboardPosition);
				}
				//If all skiers get wiped out (non-skiers are set to not alive by default)
				else if (!playerOneSkier.GetAlive() && !playerTwoSkier.GetAlive() && !playerThreeSkier.GetAlive() && !playerFourSkier.GetAlive())
				{
					//Add a score bonus to the player controlling the plane
					m_skiers[(int)m_eCurrentPlaneState].AddScore(planeBonus);
                    AudioManager.Play("RoundOver");
					m_eCurrentState = RoundState.eRoundOver;    //Swap to the round over screen
					roundTimerPanel.SetActive(false);
					SceneMovementActive(false);                 //Deactivate scene movement
                    AudioManager.Stop("PlaneAudio");
                    bonus.text = "All skiers wiped out! Plane gets " + planeBonus.ToString() + " bonus score!";
					roundOverPanel.SetActive(true);             //Show the round over screen
					UpdateSortedSkiers();
					CallOnSkiers(SetScoreboardPosition);
				}

				roundTimerPanel.SetActive(true);
				playingCountDownDisplay.fillAmount = m_playingRoundTimer.T / roundTimeLimit;

				//Check if any skiers are hit
				CallOnSkiers(SkierHurtBonusCheck);

				//Sort the skiers by score and change which skier has the top score particle if needed
				UpdateSortedSkiers();
				ApplyTopScoreParticle();

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
                    wavePanel.transform.position = m_panelOffScreenLeft;
					//Update the static GameInfo scores
					GameInfo.playerOneScore = playerOneSkier.GetScore();
					GameInfo.playerTwoScore = playerTwoSkier.GetScore();
					if (m_playerCount >= 3)
						GameInfo.playerThreeScore = playerThreeSkier.GetScore();
					if (m_playerCount == 4)
						GameInfo.playerFourScore = playerFourSkier.GetScore();

                    m_t = 0;
                    m_nextRound = true;
				}

                if (m_nextRound)
                {
                    m_t += 40;

                    wavePanel.transform.position = Vector3.MoveTowards(m_panelOffScreenLeft, canvas.transform.position, m_t); // Slowly moves the position to the target.

                    if (wavePanel.transform.position == canvas.transform.position)
                    {
                        m_t = 0;
                        m_nextRound = false;

                        ++GameInfo.roundNumber;                     //Update the round number
                        if (GameInfo.roundNumber <= m_playerCount)  //If the round number is under the number of players,
                            SceneManager.LoadScene(1);              //Load the next level
                        else                                        //The round number exceeds the number of players,
                            SceneManager.LoadScene(2);				//Go to the game finished scene
                    }
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
		Tether[] tethers = new Tether[4];	//Array of tethers to pass onto the plane

		int place = 0;	//Which place the skier will be spawned to, such as 0, 1, or 2, based on how many players

		for (int i = 0; i < m_playerCount; ++i)				//For all current players,
		{
			if (i == (int)m_eCurrentPlaneState)				//If this player is in the plane,
			{
				m_skiers[i].gameObject.SetActive(false);	//Set their skier to inactive
				m_skiers[i].SetAlive(false);				//Make sure they aren't considered alive
				//Set the material of the plane
				planeBody.GetComponent<Renderer>().material.SetTexture("Texture2D_C6055840", m_planeTextures[i]);
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

	// Sets the position of the Skier UI depending on the amount of players.
	private void SetSkierUIPosition(int skierNumber)
	{
		//Calculates where along the x axis this player's UI should be
		float xPos = (1.0f / (m_playerCount + 1.0f)) * (skierNumber + 1.0f);
		m_playerUI[skierNumber].anchorMin = new Vector2(xPos, m_playerUI[skierNumber].anchorMin.y);
		m_playerUI[skierNumber].anchorMax = new Vector2(xPos, m_playerUI[skierNumber].anchorMax.y);
	}

	//Sets/updates all the UI related to a skier
	private void SetSkierUI(int skierNumber)
	{
		m_skierScoresText[skierNumber].text = m_skiers[skierNumber].GetScore().ToString();	//Display the score
		
		if (m_skiers[skierNumber].GetAlive())											//If the skier is still alive,
		{
			m_skierLives[skierNumber].fillAmount = m_skiers[skierNumber].lives / 3.0f;  //Display the lives
			if (m_skiers[skierNumber].GetPlayerMultiplier() == 1)
				m_skierMultipliers[skierNumber].fontSize = 30;
			else if (m_skiers[skierNumber].GetPlayerMultiplier() == 2)
				m_skierMultipliers[skierNumber].fontSize = 32;
			else if (m_skiers[skierNumber].GetPlayerMultiplier() == 3)
				m_skierMultipliers[skierNumber].fontSize = 34;
			else if (m_skiers[skierNumber].GetPlayerMultiplier() == 4)
				m_skierMultipliers[skierNumber].fontSize = 36;
			else if (m_skiers[skierNumber].GetPlayerMultiplier() == 5)
				m_skierMultipliers[skierNumber].fontSize = 38;
			m_skierMultipliers[skierNumber].text = "x" + m_skiers[skierNumber].GetPlayerMultiplier().ToString();	//Display the multiplier
		}
        else if ((int)m_eCurrentPlaneState == skierNumber) //If the player is in the plane,
        {
            m_planeImage[skierNumber].gameObject.SetActive(true); //Activate the plane image.
            m_skierLives[skierNumber].fillAmount = 0;   //Show no lives
            m_skierMultipliers[skierNumber].text = "";	//Don't show a multiplier
        }
		else											//If the skier isn't alive,
		{
			m_skierLives[skierNumber].fillAmount = 0;	//Show no lives
			m_skierMultipliers[skierNumber].text = "";	//Don't show a multiplier
		}
	}

	//Sorts the secondary skier array to be in descending order of score
	private void UpdateSortedSkiers()
	{
		//Bubble sort in descending order
		SkierController temp;
		for (int j = 0; j < m_playerCount - 1; j++)
		{
			for (int i = 0; i < m_playerCount - 1; i++)
			{
				if (m_sortedSkiers[i].GetScore() < m_sortedSkiers[i + 1].GetScore())
				{
					temp = m_sortedSkiers[i + 1];
					m_sortedSkiers[i + 1] = m_sortedSkiers[i];
					m_sortedSkiers[i] = temp;
				}
			}
		}
	}

	//Change which player is playing the particle if needed
	private void ApplyTopScoreParticle()
	{
		if (((int)m_sortedSkiers[0].controller - 1) != (int)m_eCurrentPlaneState)	//If the top score player isn't in the plane,
		{
			if (m_sortedSkiers[0] != m_prevTopSkier)			//If the top player has changed,
			{
				m_sortedSkiers[0].SetTopScoreParticle(true);	//Start the particle on the new player
				if (m_prevTopSkier != null)							
					m_prevTopSkier.SetTopScoreParticle(false);	//Stop the particle on the previous player
				m_prevTopSkier = m_sortedSkiers[0];				//Update the previous top player
			}
		}
		else													//The top score player is in the plane,
		{
			if (m_sortedSkiers[1] != m_prevTopSkier)			//If the top player has changed,
			{
				m_sortedSkiers[1].SetTopScoreParticle(true);	//Start the particle on the new player
				if (m_prevTopSkier != null)
					m_prevTopSkier.SetTopScoreParticle(false);	//Stop the particle on the previous player
				m_prevTopSkier = m_sortedSkiers[1];				//Update the previous top player
			}
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

	//Sets the position of each skier's score on the scoreboard, in order
	private void SetScoreboardPosition(int skierNumber)
	{
		int currentSkier = (int)m_sortedSkiers[skierNumber].controller;	//Which number skier is in this place?
		//Write which skier this is and their score
		m_roundOverScoresText[currentSkier - 1].text = "Player " + currentSkier + ": " + m_sortedSkiers[skierNumber].GetScore();

		//Calculate where on the scoreboard this skier should be placed
		float newYPos = (1.0f - (skierNumber * 0.1f)) - 0.4f;
		RectTransform rect = m_roundOverScoresText[currentSkier - 1].rectTransform;
		Vector2 newPos = new Vector2(rect.anchorMin.x, newYPos);
		rect.anchorMin = newPos;
		rect.anchorMax = newPos;
	}

	//Starts/stops movement in the scene
	private void SceneMovementActive(bool value)
	{
		plane.enabled = value;
		playerOneSkier.enabled = value;
		playerTwoSkier.enabled = value;
		playerThreeSkier.enabled = value;
		playerFourSkier.enabled = value;
		playerOneSkier.tether.enabled = value;
		playerOneUI.gameObject.SetActive(value);
		playerTwoSkier.tether.enabled = value;
		playerTwoUI.gameObject.SetActive(value);
		if (playerThreeSkier.gameObject.activeSelf == true)  //If the purple skier is in the game,
			playerThreeSkier.tether.enabled = value;         //Set them
		if (playerFourSkier.gameObject.activeSelf == true)  //If the orange skier is in the game,
			playerFourSkier.tether.enabled = value;         //Set them too
		if (m_playerCount >= 3)
			playerThreeUI.gameObject.SetActive(value);
		if (m_playerCount == 4)
			playerFourUI.gameObject.SetActive(value);
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
