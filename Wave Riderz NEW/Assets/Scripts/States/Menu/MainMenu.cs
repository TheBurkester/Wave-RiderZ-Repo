/*-------------------------------------------------------------------*
|  Title:			MainMenu
|
|  Author:			Thomas Maltezos / Seth Johnston
| 
|  Description:		Controls UI elements of the main menu and 
*-------------------------------------------------------------------*/

using UnityEngine;
using UnityEngine.SceneManagement;
using XboxCtrlrInput;
public class MainMenu : MonoBehaviour
{
	//Enum for the different UI screens in the menu
	public enum PanelState
	{
		eSplashScreen,		// First screen visible when launching game.
		eCharacterScreen	// Character selection / ready screen.
	}


	//UI references
	public Canvas canvas;					//Reference to the UI canvas as a whole
	public RectTransform splashPanel;		//Reference to the main menu UI transform
	public RectTransform characterPanel;	//Reference to the character selection UI transform
	//Positions of the blocks behind skiers in character select
	public Transform playerOneBlock;
	public Transform playerTwoBlock;
	public Transform playerThreeBlock;
	public Transform playerFourBlock;
	//Lights to turn on when the corresponding skier is ready
    public Light readyLightPlayerOne;
	public Light readyLightPlayerTwo;
    public Light readyLightPlayerThree;
    public Light readyLightPlayerFour;
	// All off-screen positions.
	private Vector3 m_panelOffScreenBottomPos;
	private Vector3 m_panelOffScreenTopPos;
	private Vector3 m_playerOffScreenLeft;
	private Vector3 m_playerOffScreenRight;
	// All player positions whilst in view of the camera.
	private Vector3 m_playerOnePos;
	private Vector3 m_playerTwoPos;
	private Vector3 m_playerThreePos;
	private Vector3 m_playerFourPos;
	//-------------------------------------------------------------------------

	//UI-related variables
	public float panelSpeed = 40;					// How quickly the panels will shift.
    private float m_t = 0;							// Timer which increases via the panelSpeed.
	private float m_t2 = 0;							// Second timer for ONLY players.
	private bool m_playButtonPress = false;			// Has the play button been clicked?
    private bool m_addPlayerButtonPress = false;	// Has the add player button been pressed?
    private bool m_removePlayer = false;			// Is a player currently being removed from the game?
	private bool m_showPlay = false;                // Are there enough players ready to show the play button?
	//-------------------------------------------------------------------------

	//Keyboard controls (OUTDATED)
	//public KeyCode readyPlayerOne = KeyCode.Alpha1;		// Player one is ready.
	//public KeyCode readyPlayerTwo = KeyCode.Alpha2;		// Player two is ready.
	//public KeyCode readyPlayerThree = KeyCode.Alpha3;	// Player three is ready.
	//public KeyCode readyPlayerFour = KeyCode.Alpha4;    // Player four is ready.
	//-------------------------------------------------------------------------

	//Xbox controls
	public XboxButton addPlayerXbox = XboxButton.A;			// Adds a player to the game with Xbox controls.
    public XboxButton removePlayerXbox = XboxButton.B;		// Removes a player from the game with Xbox controls.
    public XboxButton readyPlayerXbox = XboxButton.Start;   // Player one is ready.
	//-------------------------------------------------------------------------

	//Player variables
	public static int playerNumber = 0; // The number of players.
    private bool m_playerOneReady = false;
    private bool m_playerTwoReady = false;
    private bool m_playerThreeReady = false;
    private bool m_playerFourReady = false;
	//-------------------------------------------------------------------------

	//Misc
	private PanelState m_eCurrentState = 0; // Starting state is splash screen.
	

	void Awake()
    {
		m_playerOnePos = new Vector3(-3, 12, 3);	// Target position to place player one.
		m_playerTwoPos = new Vector3(3, 12, 3);		// Target position to place player two.
		m_playerThreePos = new Vector3(-3, 9, 3);	// Target position to place player three.
		m_playerFourPos = new Vector3(3, 9, 3);		// Target position to place player four.

		m_panelOffScreenBottomPos = new Vector3(splashPanel.transform.position.x, -900, splashPanel.transform.position.z); // Bottom position for moving onto the canvas.
		m_panelOffScreenTopPos = new Vector3(splashPanel.transform.position.x, 1800, splashPanel.transform.position.z); // Top position for moving off the canvas.
		m_playerOffScreenLeft = new Vector3(-30, 10, 3); // Offset position outside of the camera view towards the left.
		m_playerOffScreenRight = new Vector3(30, 10, 3); // Offset position outside of the camera view towards the right.
		splashPanel.transform.position = canvas.transform.position; // Ensures that the splash starts within the canvas.
		characterPanel.transform.position = m_panelOffScreenBottomPos; // Ensures that the character panel starts at the bottom.

		readyLightPlayerOne.transform.position = new Vector3(-3, 12, 0);
		readyLightPlayerTwo.transform.position = new Vector3(3, 12, 0);
		readyLightPlayerThree.transform.position = new Vector3(-3, 9, 0);
		readyLightPlayerFour.transform.position = new Vector3(3, 9, 0);

		readyLightPlayerOne.enabled = false;
		readyLightPlayerTwo.enabled = false;
		readyLightPlayerThree.enabled = false;
		readyLightPlayerFour.enabled = false;

		if (playerOneBlock != null)
			playerOneBlock.transform.position = m_playerOffScreenLeft; // Sets the starting position to the left of the camera.

		if (playerTwoBlock != null)
			playerTwoBlock.transform.position = m_playerOffScreenRight; // Sets the starting position to the right of the camera.

		if (playerThreeBlock != null)
			playerThreeBlock.transform.position = m_playerOffScreenLeft; // Sets the starting position to the right of the camera.

		if (playerFourBlock != null)
			playerFourBlock.transform.position = m_playerOffScreenRight; // Sets the starting position to the right of the camera.

		playerNumber = 0;
	}

	void Update()
	{
		switch (m_eCurrentState)
		{
			case PanelState.eSplashScreen:

                if (XCI.GetButton(addPlayerXbox, XboxController.First))	//If the first controller presses the play button,
                    m_playButtonPress = true;							//Activate the play button

				if (m_playButtonPress)	//If the play button is active,
				{
					//Increment the timers
					m_t += panelSpeed;	
					m_t2 += panelSpeed * Time.deltaTime;

					splashPanel.transform.position = Vector3.MoveTowards(canvas.transform.position, m_panelOffScreenTopPos, m_t); // Slowly moves the position to the target.
					playerOneBlock.transform.position = Vector3.MoveTowards(m_playerOffScreenLeft, m_playerOnePos, m_t2); // Slowly moves the position to the target.

					// If everything is in its correct place.
					if (splashPanel.transform.position == m_panelOffScreenTopPos && playerOneBlock.transform.position == m_playerOnePos)
					{
						m_t = 0;	// Reset panel timer.
						m_t2 = 0;	// Reset player timer.
						playerNumber++;
                        m_playButtonPress = false;
						m_eCurrentState = PanelState.eCharacterScreen; // Change state to the character screen.
					}
				}
				break;
			/*-------------------------------------------------------------------*/

			case PanelState.eCharacterScreen:
                
				//'Add player' input checks
                if (XCI.GetButton(addPlayerXbox, XboxController.Second) && playerNumber == 1) //If player two presses add and the previous player is already added,
                {
                    if (m_removePlayer != true)			//As long as a player isn't currently getting removed, 
                        m_addPlayerButtonPress = true;	//Add a player
                }
                if (XCI.GetButton(addPlayerXbox, XboxController.Third) && playerNumber == 2) //If player three presses add and the previous player is already added,
				{
                    if (m_removePlayer != true)			//As long as a player isn't currently getting removed,
                        m_addPlayerButtonPress = true;  //Add a player
				}
                if (XCI.GetButton(addPlayerXbox, XboxController.Fourth) && playerNumber == 3) //If player four presses add and the previous player is already added,
				{
                    if (m_removePlayer != true)			//As long as a player isn't currently getting removed,
                        m_addPlayerButtonPress = true;  //Add a player
				}

				//'Remove player' input checks
                if (XCI.GetButton(removePlayerXbox, XboxController.Second) && playerNumber == 2)    //If player two presses remove and they are the last player,
				{
                    if (m_addPlayerButtonPress != true) //As long as a player isn't currently getting added,
                        m_removePlayer = true;			//Remove a player
                }
                if (XCI.GetButton(removePlayerXbox, XboxController.Third) && playerNumber == 3)    //If player three presses remove and they are the last player,
				{
					if (m_addPlayerButtonPress != true) //As long as a player isn't currently getting added,
						m_removePlayer = true;          //Remove a player
				}
				if (XCI.GetButton(removePlayerXbox, XboxController.Fourth) && playerNumber == 4)    //If player four presses remove and they are the last player,
				{
					if (m_addPlayerButtonPress != true) //As long as a player isn't currently getting added,
						m_removePlayer = true;          //Remove a player
				}

				//Adding and moving players
				if (m_addPlayerButtonPress)
				{
					m_t2 += panelSpeed * Time.deltaTime; //Increment the player moving timer
            
					if (playerNumber == 1) // If there is one player. Adds the 2nd player.
					{
						playerTwoBlock.transform.position = Vector3.MoveTowards(m_playerOffScreenRight, m_playerTwoPos, m_t2); // Slowly moves the position to the target.
            
						if (playerTwoBlock.transform.position == m_playerTwoPos)
						{
							m_t2 = 0;		// Resets timer.
							playerNumber++; // Increases player number by 1.
                            m_addPlayerButtonPress = false;
						}
					}
					else if (playerNumber == 2) // If there are two players. Adds the 3rd player.
					{
						playerThreeBlock.transform.position = Vector3.MoveTowards(m_playerOffScreenLeft, m_playerThreePos, m_t2); // Slowly moves the position to the target.
            
						if (playerThreeBlock.transform.position == m_playerThreePos)
						{
							m_t2 = 0;		// Resets timer.
							playerNumber++; // Increases player number by 1.
                            m_addPlayerButtonPress = false;
						}
					}
					else if (playerNumber == 3) // If there are 3 players. Adds the 4th player.
					{
						playerFourBlock.transform.position = Vector3.MoveTowards(m_playerOffScreenRight, m_playerFourPos, m_t2); // Slowly moves the position to the target.
            
						if (playerFourBlock.transform.position == m_playerFourPos)
						{
							m_t2 = 0;		// Resets timer.
							playerNumber++; // Increases player number by 1.
                            m_addPlayerButtonPress = false;
						}
					}
				}
				//Removing and moving players
				else if (m_removePlayer)
				{
					m_t2 += panelSpeed * Time.deltaTime;	//Increment the player moving timer

					if (playerNumber == 2)	//If there are 2 players, remove the second player
					{
						m_playerTwoReady = false;
						readyLightPlayerTwo.enabled = false;
						playerTwoBlock.transform.position = Vector3.MoveTowards(m_playerTwoPos, m_playerOffScreenRight, m_t2); // Moves player two back to its starting pos.
						if (playerTwoBlock.transform.position == m_playerOffScreenRight)
						{
							m_t2 = 0;
							playerNumber--;
							m_removePlayer = false;
						}
					}
					else if (playerNumber == 3) //If there are 3 players, remove the third player
					{
						m_playerThreeReady = false;
						readyLightPlayerThree.enabled = false;
						playerThreeBlock.transform.position = Vector3.MoveTowards(m_playerThreePos, m_playerOffScreenLeft, m_t2); // Moves player three back to its starting pos.
						if (playerThreeBlock.transform.position == m_playerOffScreenLeft)
						{
							m_t2 = 0;
							playerNumber--;
							m_removePlayer = false;
						}
					}
					else if (playerNumber == 4) //If there are 4 players, remove the fourth player
					{
						m_playerFourReady = false;
						readyLightPlayerFour.enabled = false;
						playerFourBlock.transform.position = Vector3.MoveTowards(m_playerFourPos, m_playerOffScreenRight, m_t2); // Moves player four back to its starting pos.
						if (playerFourBlock.transform.position == m_playerOffScreenRight)
						{
							m_t2 = 0;
							playerNumber--;
							m_removePlayer = false;
						}
					}
				}

				//'Player ready' input checks
				if (XCI.GetButtonDown(readyPlayerXbox, XboxController.First))	//If the first player presses ready,
				{
					//Turn their ready state on/off
					if (m_playerOneReady)
					{
						m_playerOneReady = false;
						readyLightPlayerOne.enabled = false;
					}
					else
					{
						m_playerOneReady = true;
						readyLightPlayerOne.enabled = true;
					}
				}
				else if (XCI.GetButtonDown(readyPlayerXbox, XboxController.Second)	//If the second player presses ready,
						&& playerNumber >= 2)										//and there is a second player on screen,
				{
					//Turn their ready state on/off
					if (m_playerTwoReady)
					{
						m_playerTwoReady = false;
						readyLightPlayerTwo.enabled = false;
					}
					else
					{
						m_playerTwoReady = true;
						readyLightPlayerTwo.enabled = true;
					}
				}
				else if (XCI.GetButtonDown(readyPlayerXbox, XboxController.Third)	//If the third player presses ready,
						&& playerNumber >= 3)                                       //and there is a third player on screen,
				{
					//Turn their ready state on/off
					if (m_playerThreeReady)
					{
						m_playerThreeReady = false;
						readyLightPlayerThree.enabled = false;
					}
					else
					{
						m_playerThreeReady = true;
						readyLightPlayerThree.enabled = true;
					}
				}
				else if (XCI.GetButtonDown(readyPlayerXbox, XboxController.Fourth)	//If the fourth player presses ready,
						&& playerNumber == 4)                                       //and there is a fourth player on screen,
				{
					//Turn their ready state on/off
					if (m_playerFourReady)
					{
						m_playerFourReady = false;
						readyLightPlayerFour.enabled = false;
					}
					else
					{
						m_playerFourReady = true;
						readyLightPlayerFour.enabled = true;
					}
				}

				//Enough players ready checking
				if (playerNumber == 2)
				{
					if (m_playerOneReady && m_playerTwoReady								//If all players are ready,
						&& characterPanel.transform.position != canvas.transform.position)	//and the play button isn't already on-screen,
					{
						m_t += panelSpeed;	//Increment the panel movement timer
						characterPanel.transform.position = Vector3.MoveTowards(m_panelOffScreenBottomPos, canvas.transform.position, m_t); // Slowly moves the panel to the target.

						if (characterPanel.transform.position == canvas.transform.position)
						{
							m_t = 0;
							m_showPlay = true;
						}
					}
					else if (m_showPlay)							//If the play button is already on-screen,
					{
						if (!m_playerOneReady || !m_playerTwoReady)	//And one of the players unreadies,
						{
							m_t += panelSpeed;	//Increment the panel movement timer
							characterPanel.transform.position = Vector3.MoveTowards(canvas.transform.position, m_panelOffScreenBottomPos, m_t); // Slowly moves the panel to the target.
							if (characterPanel.transform.position == m_panelOffScreenBottomPos)
							{
								m_t = 0;
								m_showPlay = false;
							}
						}
					}
				}
				else if (playerNumber == 3)
				{
					if (m_playerOneReady && m_playerTwoReady && m_playerThreeReady && characterPanel.transform.position != canvas.transform.position)
					{
						m_t += panelSpeed;  //Increment the panel movement timer
						characterPanel.transform.position = Vector3.MoveTowards(m_panelOffScreenBottomPos, canvas.transform.position, m_t); // Slowly moves the panel to the target.

						if (characterPanel.transform.position == canvas.transform.position)
						{
							m_t = 0;
							m_showPlay = true;
						}
					}
					else if (m_showPlay)													//If the play button is already on-screen,
					{
						if (!m_playerOneReady || !m_playerTwoReady || !m_playerThreeReady)  //And one of the players unreadies,
						{
							m_t += panelSpeed;  //Increment the panel movement timer
							characterPanel.transform.position = Vector3.MoveTowards(canvas.transform.position, m_panelOffScreenBottomPos, m_t); // Slowly moves the panel to the target.
							if (characterPanel.transform.position == m_panelOffScreenBottomPos)
							{
								m_t = 0;
								m_showPlay = false;
							}
						}
					}
				}
				else if (playerNumber == 4)
				{
					if (m_playerOneReady && m_playerTwoReady && m_playerThreeReady && m_playerFourReady && characterPanel.transform.position != canvas.transform.position)
					{
						m_t += panelSpeed;  //Increment the panel movement timer
						characterPanel.transform.position = Vector3.MoveTowards(m_panelOffScreenBottomPos, canvas.transform.position, m_t); // Slowly moves the panel to the target.

						if (characterPanel.transform.position == canvas.transform.position)
						{
							m_t = 0;
							m_showPlay = true;
						}
					}
					else if (m_showPlay)																			//If the play button is already on-screen,
					{
						if (!m_playerOneReady || !m_playerTwoReady || !m_playerThreeReady || !m_playerFourReady)    //And one of the players unreadies,
						{
							m_t += panelSpeed;  //Increment the panel movement timer
							characterPanel.transform.position = Vector3.MoveTowards(canvas.transform.position, m_panelOffScreenBottomPos, m_t); // Slowly moves the panel to the target.
							if (characterPanel.transform.position == m_panelOffScreenBottomPos)
							{
								m_t = 0;
								m_showPlay = false;
							}
						}
					}
				}

				//Play button input check
                if (m_showPlay && XCI.GetButtonDown(addPlayerXbox, XboxController.First))	//If the play button is on-screen and the first controller presses play,
                    PlayGame();

				break;
		}	
	}

	public void PlayGame()
    {
		//Reset all scores
		GameInfo.playerOneScore = 0;
		GameInfo.playerTwoScore = 0;
		GameInfo.playerThreeScore = 0;
		GameInfo.playerFourScore = 0;
		
		GameInfo.roundNumber = 1;	//Start the game on round 1

        SceneManager.LoadScene(1); //Load the next scene within build settings
    }

    public void QuitGame()
    {
        Debug.Log("App has Quit."); // Used to check if it will quit within unity.
        Application.Quit();
    }

	//Leftover function from mouse-controlled menu button (OUTDATED)
	public void buttonPress()
	{
		m_playButtonPress = true;
	}
}
