/*-------------------------------------------------------------------*
|  Title:			MainMenu
|
|  Author:			Thomas Maltezos / Seth Johnston
| 
|  Description:		Controls UI elements of the main menu.
*-------------------------------------------------------------------*/

using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using XboxCtrlrInput;
using XInputDotNetPure;
public class MainMenu : MonoBehaviour
{
	//Enum for the different UI screens in the menu
	public enum PanelState
	{
		eSplashScreen,		// First screen visible when launching game.
		eCharacterScreen,	// Character selection / ready screen.
		eControlsScreen,	// Controls for both the plane and skiers.
		eCreditsScreen,		// Credits.
		eQuitScreen			// Warning before quitting.
	}

	//-------------------------------------------------------------------------

	// Enums for the character states.
	public enum CharacterState
	{
		eJoining,			// In the process of joining.
		eJoined,			// Has joined the game.
		eLeaving,			// In the process of leaving.
		eIdle				// Idle for when they aren't in the game and aren't joining.
	}

	//-------------------------------------------------------------------------

	//UI references
	public Canvas canvas;						// Reference to the UI canvas as a whole
	public RectTransform splashPanel;			// Reference to the main menu UI transform
	public RectTransform characterPanel;		// Reference to the character selection UI transform
	public RectTransform allPLayersReadyPanel;	// Reference to the all players ready panel.
	public RectTransform controlsPanel;			// Reference to the controls panel UI transform.
	public RectTransform creditsPanel;			// Reference to the credits panel UI transforn.
	public RectTransform quitPanel;             // Reference to the quit panel UI transform.
    public RectTransform wavePanel;
	public Image addPlayerOneImage;
	public Image addPlayerTwoImage;
	public Image addPlayerThreeImage;
	public Image addPlayerFourImage;
	public Image readyPlayerOneImage;
	public Image readyPlayerTwoImage;
	public Image readyPlayerThreeImage;
	public Image readyPlayerFourImage;
	public Image notReadyPlayerOneImage;
	public Image notReadyPlayerTwoImage;
	public Image notReadyPlayerThreeImage;
	public Image notReadyPlayerFourImage;
	public Image playerOneChar;
	public Image playerTwoChar;
	public Image playerThreeChar;
	public Image playerFourChar;
	public Image playerOneColour;
	public Image playerTwoColour;
	public Image playerThreeColour;
	public Image playerFourColour;

	//-------------------------------------------------------------------------

	// All off-screen positions.
	private Vector3 m_panelOffScreenBottomPos;
    private Vector3 m_panelOffScreenLeft = new Vector3(-1600, 0, 0);
    private Vector3 m_panelOffScreenTopPos;

	//-------------------------------------------------------------------------

	//UI-related variables
	public float panelSpeed = 40;					// How quickly the panels will shift.
    private float m_t = 0;							// Timer which increases via the panelSpeed.
	private bool m_playButtonPress = false;         // Has the play button been selected?
	private bool m_controlsButtonPress = false;		// Has the controls button been selected?
	private bool m_creditsButtonPress = false;      // Has the credits button been selected?
	private bool m_quitButtonPress = false;			// Has the quit button been selected?
    private bool m_addPlayerButtonPress = false;	// Has the add player button been pressed?
    private bool m_removePlayer = false;			// Is a player currently being removed from the game?
	private bool m_showPlay = false;                // Are there enough players ready to show the play button?
	private bool m_returningToMenu = false;			// Are the players returning to the menu?
    private bool m_wavePanel = false;

	//-------------------------------------------------------------------------

	//Xbox controls
	public XboxButton addPlayerXbox = XboxButton.A;			// Adds a player to the game with Xbox controls.
    public XboxButton removePlayerXbox = XboxButton.B;		// Removes a player from the game with Xbox controls.
    public XboxButton readyPlayerXbox = XboxButton.Start;   // Player one is ready.

	//-------------------------------------------------------------------------

	//Player variables
	public static int playerNumber = 0;			// The number of players.
    private bool m_playerOneReady = false;
	private CharacterState m_playerOneState = CharacterState.eIdle;
    private bool m_playerTwoReady = false;
	private CharacterState m_playerTwoState = CharacterState.eIdle;
	private bool m_playerThreeReady = false;
	private CharacterState m_playerThreeState = CharacterState.eIdle;
	private bool m_playerFourReady = false;
	private CharacterState m_playerFourState = CharacterState.eIdle;

	//-------------------------------------------------------------------------

	//Misc
	private PanelState m_eCurrentState = 0; // Starting state is splash screen.

	//-------------------------------------------------------------------------

	void Awake()
    {
		m_panelOffScreenBottomPos = new Vector3(canvas.transform.position.x, -900, canvas.transform.position.z); // Bottom position for moving onto the canvas.
		m_panelOffScreenTopPos = new Vector3(canvas.transform.position.x, 1800, canvas.transform.position.z); // Top position for moving off the canvas.
		splashPanel.transform.position = canvas.transform.position; // Ensures that the splash starts within the canvas.
		characterPanel.transform.position = m_panelOffScreenBottomPos; // Ensures that the character panel starts at the bottom.
		allPLayersReadyPanel.transform.position = m_panelOffScreenBottomPos;
		controlsPanel.transform.position = m_panelOffScreenBottomPos; // Ensures that the controls panel starts at the bottom.
		creditsPanel.transform.position = m_panelOffScreenBottomPos; // Ensures that the credits panel starts at the bottom.
		quitPanel.transform.position = m_panelOffScreenBottomPos; // Ensures that the quit panel starts at the bottom.
        wavePanel.transform.localPosition = m_panelOffScreenLeft;

        addPlayerOneImage.enabled = true;
		addPlayerTwoImage.enabled = true;
		addPlayerThreeImage.enabled = true;
		addPlayerFourImage.enabled = true;

		readyPlayerOneImage.enabled = false;
		readyPlayerTwoImage.enabled = false;
		readyPlayerThreeImage.enabled = false;
		readyPlayerFourImage.enabled = false;

		notReadyPlayerOneImage.enabled = false;
		notReadyPlayerTwoImage.enabled = false;
		notReadyPlayerThreeImage.enabled = false;
		notReadyPlayerFourImage.enabled = false;

		playerOneColour.enabled = false;
		playerTwoColour.enabled = false;
		playerThreeColour.enabled = false;
		playerFourColour.enabled = false;

		playerOneChar.enabled = false;
		playerTwoChar.enabled = false;
		playerThreeChar.enabled = false;
		playerFourChar.enabled = false;

		playerNumber = 0;
	}

	void Update()
	{
		switch (m_eCurrentState)
		{
			case PanelState.eSplashScreen:
				SplashScreen();
				break;

			case PanelState.eCharacterScreen:
				CharacterScreen();
				break;

			case PanelState.eControlsScreen:
				ControlsScreen();
				break;

			case PanelState.eCreditsScreen:
				CreditsScreen();
				break;

			case PanelState.eQuitScreen:
				QuitScreen();
				break;
		}	
	}

	//-------------------------------------------------------------------------
								// FUNCTIONS

	public void PlayGame()
    {
		//Reset all scores
		GameInfo.playerOneScore = 0;
		GameInfo.playerTwoScore = 0;
		GameInfo.playerThreeScore = 0;
		GameInfo.playerFourScore = 0;
		GameInfo.playerBeenPlane = new bool[4];

        GameInfo.roundNumber = 1;	//Start the game on round 1

        SceneManager.LoadScene(1); //Load the next scene within build settings
    }

	//-------------------------------------------------------------------------

	private void SplashScreen()
	{
		if (XCI.GetButtonDown(addPlayerXbox, XboxController.First) && splashPanel.transform.position == canvas.transform.position)  // If player one presses the A button,
			playButtonPress();                                      // Activate the play button
		if (XCI.GetButtonDown(XboxButton.X, XboxController.First) && splashPanel.transform.position == canvas.transform.position)   // If player one presses the X button.
			controlsButtonPress();
		if (XCI.GetButtonDown(XboxButton.Y, XboxController.First) && splashPanel.transform.position == canvas.transform.position)   // If player one presses the Y button.
			creditsButtonPress();
		if (XCI.GetButtonDown(XboxButton.Back, XboxController.First) && splashPanel.transform.position == canvas.transform.position)   // Only player one can quit the game.
			quitButtonPress();

		if (m_playButtonPress)  //If the play button is active,
		{
			//Increment the timers
			m_t += panelSpeed;

			splashPanel.transform.position = Vector3.MoveTowards(canvas.transform.position, m_panelOffScreenTopPos, m_t); // Slowly moves the position to the target.
			characterPanel.transform.position = Vector3.MoveTowards(m_panelOffScreenBottomPos, canvas.transform.position, m_t);

			addPlayerOneImage.enabled = true;
			addPlayerTwoImage.enabled = true;
			addPlayerThreeImage.enabled = true;
			addPlayerFourImage.enabled = true;
			// If everything is in its correct place.
			if (splashPanel.transform.position == m_panelOffScreenTopPos && characterPanel.transform.position == canvas.transform.position)
			{
				m_t = 0;    // Reset panel timer.
				m_playButtonPress = false;
				m_eCurrentState = PanelState.eCharacterScreen; // Change state to the character screen.
			}
		}

		if (m_controlsButtonPress)
		{
			m_t += panelSpeed;

			splashPanel.transform.position = Vector3.MoveTowards(canvas.transform.position, m_panelOffScreenTopPos, m_t); // Slowly moves the position to the target.
			controlsPanel.transform.position = Vector3.MoveTowards(m_panelOffScreenBottomPos, canvas.transform.position, m_t);

			if (splashPanel.transform.position == m_panelOffScreenTopPos && controlsPanel.transform.position == canvas.transform.position)
			{
				m_t = 0;    // Reset panel timer.
				m_controlsButtonPress = false;
				m_eCurrentState = PanelState.eControlsScreen; // Change state to the controls screen.
			}

		}

		if (m_creditsButtonPress)
		{
			m_t += panelSpeed;

			splashPanel.transform.position = Vector3.MoveTowards(canvas.transform.position, m_panelOffScreenTopPos, m_t); // Slowly moves the position to the target.
			creditsPanel.transform.position = Vector3.MoveTowards(m_panelOffScreenBottomPos, canvas.transform.position, m_t);

			if (splashPanel.transform.position == m_panelOffScreenTopPos && creditsPanel.transform.position == canvas.transform.position)
			{
				m_t = 0;    // Reset panel timer.
				m_creditsButtonPress = false;
				m_eCurrentState = PanelState.eCreditsScreen; // Change state to the credits screen.
			}
		}

		if (m_quitButtonPress)
		{
			quitPanel.transform.position = canvas.transform.position;

			if (quitPanel.transform.position == canvas.transform.position)
			{
				m_quitButtonPress = false;
				m_eCurrentState = PanelState.eQuitScreen;
			}
		}

		//Debug skip menu button, automatically assigns two skiers
		if (GetButtonDownAny(XboxButton.RightBumper) || Input.GetKeyDown(KeyCode.Alpha2))
		{
			playerNumber = 2;
			PlayGame();
		}

		//Debug skip menu button, automatically assigns three skiers
		if (Input.GetKeyDown(KeyCode.Alpha3))
		{
			playerNumber = 3;
			PlayGame();
		}

        //Debug skip menu button, automatically assigns four skiers
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            playerNumber = 4;
            PlayGame();
        }
    }

	//-------------------------------------------------------------------------

	private void CharacterScreen()
	{
		if (XCI.GetButtonDown(XboxButton.Back, XboxController.First))
		{
			m_returningToMenu = true;
		}

		if (m_returningToMenu)
		{
			m_playerOneReady = false;
			m_playerOneState = CharacterState.eIdle;
			m_playerTwoReady = false;
			m_playerTwoState = CharacterState.eIdle;
			m_playerThreeReady = false;
			m_playerThreeState = CharacterState.eIdle;
			m_playerFourReady = false;
			m_playerFourState = CharacterState.eIdle;

			readyPlayerOneImage.enabled = false;
			readyPlayerTwoImage.enabled = false;
			readyPlayerThreeImage.enabled = false;
			readyPlayerFourImage.enabled = false;

			notReadyPlayerOneImage.enabled = false;
			notReadyPlayerTwoImage.enabled = false;
			notReadyPlayerThreeImage.enabled = false;
			notReadyPlayerFourImage.enabled = false;

			playerOneColour.enabled = false;
			playerTwoColour.enabled = false;
			playerThreeColour.enabled = false;
			playerFourColour.enabled = false;

			playerOneChar.enabled = false;
			playerTwoChar.enabled = false;
			playerThreeChar.enabled = false;
			playerFourChar.enabled = false;

			m_t += panelSpeed;

			if (allPLayersReadyPanel.transform.position != m_panelOffScreenTopPos && allPLayersReadyPanel.transform.position != m_panelOffScreenBottomPos)
				allPLayersReadyPanel.transform.position = Vector3.MoveTowards(canvas.transform.position, m_panelOffScreenTopPos, m_t); // Slowly moves the position to the target.

			if (characterPanel.transform.position != m_panelOffScreenTopPos && characterPanel.transform.position != m_panelOffScreenBottomPos)
				characterPanel.transform.position = Vector3.MoveTowards(canvas.transform.position, m_panelOffScreenTopPos, m_t); // Slowly moves the position to the target.

			splashPanel.transform.position = Vector3.MoveTowards(m_panelOffScreenBottomPos, canvas.transform.position, m_t);

			if (splashPanel.transform.position == canvas.transform.position && (characterPanel.transform.position == m_panelOffScreenBottomPos || characterPanel.transform.position == m_panelOffScreenTopPos) && (allPLayersReadyPanel.transform.position == m_panelOffScreenBottomPos || allPLayersReadyPanel.transform.position == m_panelOffScreenTopPos))
			{
				m_t = 0;    // Reset panel timer.
				playerNumber = 0;

				m_returningToMenu = false;
				m_eCurrentState = PanelState.eSplashScreen; // Change state to the splash screen.

			}
		}

		//'Add player' input checks
		if (XCI.GetButtonDown(addPlayerXbox, XboxController.First) && m_playerOneState != CharacterState.eJoined)
		{
			if (m_removePlayer != true)                         //As long as a player isn't currently getting removed, 
			{
				notReadyPlayerOneImage.enabled = true;
				addPlayerOneImage.enabled = false;
				playerOneColour.enabled = true;
				playerOneChar.enabled = true;
				m_addPlayerButtonPress = true;                  //Add a player
				m_playerOneState = CharacterState.eJoining;     // Player one is now joining.
			}
		}
		if (XCI.GetButtonDown(addPlayerXbox, XboxController.Second) && m_playerTwoState != CharacterState.eJoined)      // If player two presses add.
		{
			if (m_removePlayer != true)                         //As long as a player isn't currently getting removed, 
			{
				notReadyPlayerTwoImage.enabled = true;
				addPlayerTwoImage.enabled = false;
				playerTwoColour.enabled = true;
				playerTwoChar.enabled = true;
				m_addPlayerButtonPress = true;                  //Add a player
				m_playerTwoState = CharacterState.eJoining;     // Player two is now joining.
			}
		}
		if (XCI.GetButtonDown(addPlayerXbox, XboxController.Third) && m_playerThreeState != CharacterState.eJoined)     // If player three presses add.
		{
			if (m_removePlayer != true)                         //As long as a player isn't currently getting removed,
			{
				notReadyPlayerThreeImage.enabled = true;
				addPlayerThreeImage.enabled = false;
				playerThreeColour.enabled = true;
				playerThreeChar.enabled = true;
				m_addPlayerButtonPress = true;                  //Add a player
				m_playerThreeState = CharacterState.eJoining;   // Player three is now joining.
			}
		}
		if (XCI.GetButtonDown(addPlayerXbox, XboxController.Fourth) && m_playerFourState != CharacterState.eJoined)     // If player four presses add.
		{
			if (m_removePlayer != true)                         //As long as a player isn't currently getting removed,
			{
				notReadyPlayerFourImage.enabled = true;
				addPlayerFourImage.enabled = false;
				playerFourColour.enabled = true;
				playerFourChar.enabled = true;
				m_addPlayerButtonPress = true;                  //Add a player
				m_playerFourState = CharacterState.eJoining;    // Player four is now joining.
			}
		}

		//'Remove player' input checks
		if (XCI.GetButtonDown(removePlayerXbox, XboxController.First) && m_playerOneState == CharacterState.eJoined)   // If player one presses remove.
		{
			if (m_addPlayerButtonPress != true)                 //As long as a player isn't currently getting added,
			{
				notReadyPlayerOneImage.enabled = false;
				readyPlayerOneImage.enabled = false;
				addPlayerOneImage.enabled = true;
				playerOneColour.enabled = false;
				playerOneChar.enabled = false;
				m_removePlayer = true;                          //Remove a player
				m_playerOneState = CharacterState.eLeaving;     // Player one is leaving.
			}
		}
		if (XCI.GetButtonDown(removePlayerXbox, XboxController.Second) && m_playerTwoState == CharacterState.eJoined)   // If player two presses remove.
		{
			if (m_addPlayerButtonPress != true)                 //As long as a player isn't currently getting added,
			{
				notReadyPlayerTwoImage.enabled = false;
				readyPlayerTwoImage.enabled = false;
				addPlayerTwoImage.enabled = true;
				playerTwoColour.enabled = false;
				playerTwoChar.enabled = false;
				m_removePlayer = true;                          //Remove a player
				m_playerTwoState = CharacterState.eLeaving;     // Player two is leaving.
			}
		}
		if (XCI.GetButtonDown(removePlayerXbox, XboxController.Third) && m_playerThreeState == CharacterState.eJoined)  // If player three presses remove.
		{
			if (m_addPlayerButtonPress != true)                 //As long as a player isn't currently getting added,
			{
				notReadyPlayerThreeImage.enabled = false;
				readyPlayerThreeImage.enabled = false;
				addPlayerThreeImage.enabled = true;
				playerThreeColour.enabled = false;
				playerThreeChar.enabled = false;
				m_removePlayer = true;                          //Remove a player
				m_playerThreeState = CharacterState.eLeaving;   // Player three is leaving.
			}
		}
		if (XCI.GetButtonDown(removePlayerXbox, XboxController.Fourth) && m_playerFourState == CharacterState.eJoined)  // If player four presses remove.
		{
			if (m_addPlayerButtonPress != true)                 //As long as a player isn't currently getting added,
			{
				notReadyPlayerFourImage.enabled = false;
				readyPlayerFourImage.enabled = false;
				addPlayerFourImage.enabled = true;
				playerFourColour.enabled = false;
				playerFourChar.enabled = false;
				m_removePlayer = true;                          //Remove a player
				m_playerFourState = CharacterState.eLeaving;    // Player four is leaving.
			}
		}

		//Adding and moving players
		if (m_addPlayerButtonPress)
		{
			//m_t2 += panelSpeed * Time.deltaTime; //Increment the player moving timer
			if (m_playerOneState == CharacterState.eJoining) // If player one is in the process of joining.
			{
				playerNumber++; // Increases player number by 1.
				m_addPlayerButtonPress = false;
				m_playerOneState = CharacterState.eJoined;

			}
			else if (m_playerTwoState == CharacterState.eJoining) // If player two is in the process of joining.
			{
				playerNumber++; // Increases player number by 1.
				m_addPlayerButtonPress = false;
				m_playerTwoState = CharacterState.eJoined;

			}
			else if (m_playerThreeState == CharacterState.eJoining) // If player three is joining.
			{
				playerNumber++; // Increases player number by 1.
				m_addPlayerButtonPress = false;
				m_playerThreeState = CharacterState.eJoined;

			}
			else if (m_playerFourState == CharacterState.eJoining) // If player four is joining.
			{
				playerNumber++; // Increases player number by 1.
				m_addPlayerButtonPress = false;
				m_playerFourState = CharacterState.eJoined;

			}
		}
		//Removing and moving players
		else if (m_removePlayer)
		{
			if (m_playerOneState == CharacterState.eLeaving)    // If player two is leaving.
			{
				m_playerOneReady = false;
				playerNumber--;
				m_removePlayer = false;
				m_playerOneState = CharacterState.eIdle;

			}
			else if (m_playerTwoState == CharacterState.eLeaving)   // If player two is leaving.
			{
				m_playerTwoReady = false;
				playerNumber--;
				m_removePlayer = false;
				m_playerTwoState = CharacterState.eIdle;

			}
			else if (m_playerThreeState == CharacterState.eLeaving) // If player three is leaving.
			{
				m_playerThreeReady = false;
				playerNumber--;
				m_removePlayer = false;
				m_playerThreeState = CharacterState.eIdle;

			}
			else if (m_playerFourState == CharacterState.eLeaving) // If player four is leaving.
			{
				m_playerFourReady = false;
				playerNumber--;
				m_removePlayer = false;
				m_playerFourState = CharacterState.eIdle;

			}
		}

		//'Player ready' input checks
		if (XCI.GetButtonDown(readyPlayerXbox, XboxController.First) && m_playerOneState == CharacterState.eJoined) //If the first player presses ready,
		{
			//Turn their ready state on/off
			if (m_playerOneReady)
			{
				m_playerOneReady = false;
				readyPlayerOneImage.enabled = false;
				notReadyPlayerOneImage.enabled = true;
			}
			else
			{
				m_playerOneReady = true;
				readyPlayerOneImage.enabled = true;
				notReadyPlayerOneImage.enabled = false;
			}
		}
		else if (XCI.GetButtonDown(readyPlayerXbox, XboxController.Second) && m_playerTwoState == CharacterState.eJoined)   //If the second player presses ready, and player two has joined.
		{
			//Turn their ready state on/off
			if (m_playerTwoReady)
			{
				m_playerTwoReady = false;
				readyPlayerTwoImage.enabled = false;
				notReadyPlayerTwoImage.enabled = true;
			}
			else
			{
				m_playerTwoReady = true;
				readyPlayerTwoImage.enabled = true;
				notReadyPlayerTwoImage.enabled = false;
			}
		}
		else if (XCI.GetButtonDown(readyPlayerXbox, XboxController.Third) && m_playerThreeState == CharacterState.eJoined)  //If the third player presses ready, and player three joined.
		{
			//Turn their ready state on/off
			if (m_playerThreeReady)
			{
				m_playerThreeReady = false;
				readyPlayerThreeImage.enabled = false;
				notReadyPlayerThreeImage.enabled = true;
			}
			else
			{
				m_playerThreeReady = true;
				readyPlayerThreeImage.enabled = true;
				notReadyPlayerThreeImage.enabled = false;
			}
		}
		else if (XCI.GetButtonDown(readyPlayerXbox, XboxController.Fourth) && m_playerFourState == CharacterState.eJoined)  //If the fourth player presses ready, and player four joined.
		{
			//Turn their ready state on/off
			if (m_playerFourReady)
			{
				m_playerFourReady = false;
				readyPlayerFourImage.enabled = false;
				notReadyPlayerFourImage.enabled = true;
			}
			else
			{
				m_playerFourReady = true;
				readyPlayerFourImage.enabled = true;
				notReadyPlayerFourImage.enabled = false;
			}
		}

		//Enough players ready checking
		if (m_playerOneState != CharacterState.eIdle && m_playerTwoState != CharacterState.eIdle && m_playerThreeState != CharacterState.eJoined && m_playerFourState != CharacterState.eJoined) // If only player one and two are in the game.
		{
			if (m_playerOneReady && m_playerTwoReady                                //If all players are ready,
				&& allPLayersReadyPanel.transform.position != canvas.transform.position)    //and the play button isn't already on-screen,
			{
				m_t += panelSpeed;  //Increment the panel movement timer
				allPLayersReadyPanel.transform.position = Vector3.MoveTowards(m_panelOffScreenBottomPos, canvas.transform.position, m_t); // Slowly moves the panel to the target.

				if (allPLayersReadyPanel.transform.position == canvas.transform.position)
				{
					m_t = 0;
					m_showPlay = true;
				}
			}
			else if (m_showPlay)                            //If the play button is already on-screen,
			{
				if (!m_playerOneReady || !m_playerTwoReady) //And one of the players unreadies,
				{
					m_t += panelSpeed;  //Increment the panel movement timer
					allPLayersReadyPanel.transform.position = Vector3.MoveTowards(canvas.transform.position, m_panelOffScreenBottomPos, m_t); // Slowly moves the panel to the target.
					if (allPLayersReadyPanel.transform.position == m_panelOffScreenBottomPos)
					{
						m_t = 0;
						m_showPlay = false;
					}
				}
			}
		}
		else if (m_playerOneState != CharacterState.eIdle && m_playerTwoState != CharacterState.eIdle && m_playerThreeState != CharacterState.eIdle && m_playerFourState != CharacterState.eJoined) // If player one, two and three have joined without player four.
		{
			if (m_playerOneReady && m_playerTwoReady && m_playerThreeReady && allPLayersReadyPanel.transform.position != canvas.transform.position)
			{
				m_t += panelSpeed;  //Increment the panel movement timer
				allPLayersReadyPanel.transform.position = Vector3.MoveTowards(m_panelOffScreenBottomPos, canvas.transform.position, m_t); // Slowly moves the panel to the target.

				if (allPLayersReadyPanel.transform.position == canvas.transform.position)
				{
					m_t = 0;
					m_showPlay = true;
				}
			}
			else if (m_showPlay)                                                    //If the play button is already on-screen,
			{
				if (!m_playerOneReady || !m_playerTwoReady || !m_playerThreeReady)  //And one of the players unreadies,
				{
					m_t += panelSpeed;  //Increment the panel movement timer
					allPLayersReadyPanel.transform.position = Vector3.MoveTowards(canvas.transform.position, m_panelOffScreenBottomPos, m_t); // Slowly moves the panel to the target.
					if (allPLayersReadyPanel.transform.position == m_panelOffScreenBottomPos)
					{
						m_t = 0;
						m_showPlay = false;
					}
				}
			}
		}
		else if (m_playerOneState != CharacterState.eIdle && m_playerTwoState != CharacterState.eIdle && m_playerThreeState != CharacterState.eIdle || m_playerThreeState != CharacterState.eJoined && m_playerFourState != CharacterState.eIdle) // If all players have joined.
		{
			if (m_playerOneReady && m_playerTwoReady && m_playerThreeReady && m_playerFourReady && allPLayersReadyPanel.transform.position != canvas.transform.position)
			{
				m_t += panelSpeed;  //Increment the panel movement timer
				allPLayersReadyPanel.transform.position = Vector3.MoveTowards(m_panelOffScreenBottomPos, canvas.transform.position, m_t); // Slowly moves the panel to the target.

				if (allPLayersReadyPanel.transform.position == canvas.transform.position)
				{
					m_t = 0;
					m_showPlay = true;
				}
			}
			else if (m_showPlay)                                                                            //If the play button is already on-screen,
			{
				if (!m_playerOneReady || !m_playerTwoReady || !m_playerThreeReady || !m_playerFourReady)    //And one of the players unreadies,
				{
					m_t += panelSpeed;  //Increment the panel movement timer
					allPLayersReadyPanel.transform.position = Vector3.MoveTowards(canvas.transform.position, m_panelOffScreenBottomPos, m_t); // Slowly moves the panel to the target.
					if (allPLayersReadyPanel.transform.position == m_panelOffScreenBottomPos)
					{
						m_t = 0;
						m_showPlay = false;
					}
				}
			}
		}

		//Play button input check
		if (m_showPlay && XCI.GetButton(addPlayerXbox, XboxController.First))   //If the play button is on-screen and any controller presses play,
			m_wavePanel = true;

		if (m_wavePanel)
		{
			m_t += panelSpeed;
			wavePanel.transform.position = Vector3.MoveTowards(m_panelOffScreenLeft, canvas.transform.position, m_t); // Slowly moves the panel to the target.

			if (wavePanel.transform.position == canvas.transform.position)
				PlayGame();
		}
	}

	//-------------------------------------------------------------------------

	private void ControlsScreen()
	{
		if (XCI.GetButtonDown(removePlayerXbox, XboxController.First)) // If player one presses the 'B' button.
		{
			m_returningToMenu = true;
		}

		if (m_returningToMenu)
		{
			m_t += panelSpeed;

			controlsPanel.transform.position = Vector3.MoveTowards(canvas.transform.position, m_panelOffScreenTopPos, m_t); // Slowly moves the position to the target.
			splashPanel.transform.position = Vector3.MoveTowards(m_panelOffScreenBottomPos, canvas.transform.position, m_t);

			if (splashPanel.transform.position == canvas.transform.position && controlsPanel.transform.position == m_panelOffScreenTopPos)
			{
				m_t = 0;    // Reset panel timer.
				m_returningToMenu = false;
				m_eCurrentState = PanelState.eSplashScreen; // Change state to the splash screen.
			}
		}
	}

	//-------------------------------------------------------------------------

	private void CreditsScreen()
	{
		if (XCI.GetButtonDown(removePlayerXbox, XboxController.First)) // If player one presses the 'B' button.
		{
			m_returningToMenu = true;
		}

		if (m_returningToMenu)
		{
			m_t += panelSpeed;

			creditsPanel.transform.position = Vector3.MoveTowards(canvas.transform.position, m_panelOffScreenTopPos, m_t); // Slowly moves the position to the target.
			splashPanel.transform.position = Vector3.MoveTowards(m_panelOffScreenBottomPos, canvas.transform.position, m_t);

			if (splashPanel.transform.position == canvas.transform.position && creditsPanel.transform.position == m_panelOffScreenTopPos)
			{
				m_t = 0;
				m_returningToMenu = false;
				m_eCurrentState = PanelState.eSplashScreen; // Change state to the splash screen.
			}
		}
	}

	//-------------------------------------------------------------------------

	private void QuitScreen()
	{
		if (XCI.GetButtonDown(XboxButton.A, XboxController.First))
		{
			Debug.Log("App has Quit."); // Used to check if it will quit within unity.
			Application.Quit();
		}

		if (XCI.GetButtonDown(XboxButton.B, XboxController.First))
			m_returningToMenu = true;

		if (m_returningToMenu)
		{
			quitPanel.transform.position = m_panelOffScreenBottomPos;

			if (quitPanel.transform.position == m_panelOffScreenBottomPos)
			{
				m_returningToMenu = false;
				m_eCurrentState = PanelState.eSplashScreen;
			}
		}
	}

	//-------------------------------------------------------------------------

	public void playButtonPress()
	{
		m_playButtonPress = true;
	}

	//-------------------------------------------------------------------------

	public void controlsButtonPress()
	{
		m_controlsButtonPress = true;
	}

	//-------------------------------------------------------------------------

	public void creditsButtonPress()
	{
		m_creditsButtonPress = true;
	}

	//-------------------------------------------------------------------------

	public void quitButtonPress()
	{
		m_quitButtonPress = true;
	}

	//-------------------------------------------------------------------------

	//Checks if any controller has the specified button pressed
	public bool GetButtonDownAny(XboxButton button)
	{
		if (XCI.GetButtonDown(button, XboxController.First)
			|| XCI.GetButtonDown(button, XboxController.Second)
			|| XCI.GetButtonDown(button, XboxController.Third)
			|| XCI.GetButtonDown(button, XboxController.Fourth))
			return true;
		return false;
	}
}
