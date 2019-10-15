/*-------------------------------------------------------------------*
|  Title:			MainMenu
|
|  Author:			Thomas Maltezos
| 
|  Description:		Handles the main menu.
*-------------------------------------------------------------------*/

using UnityEngine;
using UnityEngine.SceneManagement;
using XboxCtrlrInput;		// Be sure to include this if you want an object to have Xbox input
public class MainMenu : MonoBehaviour
{
	public enum PanelState
	{
		eSplashScreen, // First screen visible when launching game.
		eCharacterScreen // Character selection / ready screen.
	}

	public RectTransform splashPanel;
	public RectTransform characterPanel;
	public Canvas canvas;
	public Transform playerOneBlock; // References the position of the block BEHIND the model.
	public Transform playerTwoBlock; // References the position of the block BEHIND the model.
	public Transform playerThreeBlock; // References the position of the block BEHIND the model.
	public Transform playerFourBlock; // References the position of the block BEHIND the model.
    
    
    public KeyCode addPlayer = KeyCode.A; // Adds a player to the game.
    //public KeyCode removePlayer = KeyCode.D; // Removes a player from the game.
	public KeyCode readyPlayerOne = KeyCode.Alpha1; // Player one is ready.
	public KeyCode readyPlayerTwo = KeyCode.Alpha2; // Player two is ready.
	public KeyCode readyPlayerThree = KeyCode.Alpha3; // Player three is ready.
	public KeyCode readyPlayerFour = KeyCode.Alpha4; // Player four is ready.
    public XboxButton addPlayerXbox = XboxButton.A; // Adds a player to the game with Xbox controls.
    public XboxButton removePlayerXbox = XboxButton.B; // Removes a player from the game with Xbox controls.
    public XboxButton readyPlayerXbox = XboxButton.Start; // Player one is ready.




    public Light readyLightPlayerOne; // Ready light which will be turned on when Player One is ready.
	public Light readyLightPlayerTwo; // Ready light which will be turned on when Player Two is ready.
    public Light readyLightPlayerThree; // Ready light which will be turned on when Player Three is ready.
    public Light readyLightPlayerFour; // Ready light which will be turned on when Player Four is ready.

    public float panelSpeed = 40; // How quickly the panels will shift.

    public static int playerNumber = 0; // The number of players.

    // Individual bools for each player.
    private bool m_playerOneReady = false;
    private bool m_playerTwoReady = false;
    private bool m_playerThreeReady = false;
    private bool m_playerFourReady = false;

	private PanelState m_eCurrentState = 0; // Starting state is splash screen.

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

    private float m_t = 0; // Timer which increases via the panelSpeed.
	private float m_t2 = 0; // Second TImer for ONLY players.
	private bool m_playButtonPress = false; // Has the play button been clicked?
    private bool m_addPlayerButtonPress = false;
   // private bool m_XboxbuttonTwoPress = false; // has player two's button been pressed?
    private bool m_removePlayer = false; // Is a player currently being removed from the game?
	private bool m_showPlay = false;

	void Awake()
    {
		m_playerOnePos = new Vector3(-3, 12, 3); // Target position to place player one.
		m_playerTwoPos = new Vector3(3, 12, 3); // Target position to place player two.
		m_playerThreePos = new Vector3(-3, 9, 3); // Target position to place player three.
		m_playerFourPos = new Vector3(3, 9, 3); // Target position to place player four.

		m_panelOffScreenBottomPos = new Vector3(splashPanel.transform.position.x, -900, splashPanel.transform.position.z); // Bottom position for moving onto the canvas.
		m_panelOffScreenTopPos = new Vector3(splashPanel.transform.position.x, 900, splashPanel.transform.position.z); // Top position for moving off the canvas.
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
		{
			playerOneBlock.transform.position = m_playerOffScreenLeft; // Sets the starting position to the left of the camera.
		}

		if (playerTwoBlock != null)
		{
			playerTwoBlock.transform.position = m_playerOffScreenRight; // Sets the starting position to the right of the camera.
		}

		if (playerThreeBlock != null)
		{
			playerThreeBlock.transform.position = m_playerOffScreenLeft; // Sets the starting position to the right of the camera.
		}

		if (playerFourBlock != null)
		{
			playerFourBlock.transform.position = m_playerOffScreenRight; // Sets the starting position to the right of the camera.
		}
	}

	void Update()
	{
		switch (m_eCurrentState)
		{
			case PanelState.eSplashScreen: // Splash screen enum.
                if (XCI.GetButton(addPlayerXbox, XboxController.First))
                    m_playButtonPress = true;

				if (m_playButtonPress)
				{
					m_t += panelSpeed; // Only used for panels.
					m_t2 += panelSpeed * Time.deltaTime; // Only used for player blocks.

					splashPanel.transform.position = Vector3.MoveTowards(canvas.transform.position, m_panelOffScreenTopPos, m_t); // Slowly moves the position to the target.
					playerOneBlock.transform.position = Vector3.MoveTowards(m_playerOffScreenLeft, m_playerOnePos, m_t2); // Slowly moves the position to the target.

					// If everything is in its correct place.
					if (splashPanel.transform.position == m_panelOffScreenTopPos && playerOneBlock.transform.position == m_playerOnePos)
					{
						m_t = 0; // Reset panel timer.
						m_t2 = 0; // Reset player timer.
						playerNumber++;
                        m_playButtonPress = false;
						m_eCurrentState = PanelState.eCharacterScreen; // Change state to the character screen.
					}
				}
				break;
			/*-------------------------------------------------------------------*/

			case PanelState.eCharacterScreen: // Character screen enum.
                //if (Input.GetKey(addPlayer) && playerNumber <= 3)
                //{
                //    if (m_removePlayer != true) // Ensures that the buttons don't clash with one another.
                //    {
                //        m_buttonPress = true;
                //    }
                //}

                //if (Input.GetKey(removePlayer) && playerNumber >= 2)
                //{
                //    if (m_buttonPress != true) // Ensures that the buttons don't clash with one another.
                //    {
                //        m_removePlayer = true;
                //    }
                //}

                if (XCI.GetButton(addPlayerXbox, XboxController.Second) && playerNumber == 1) // Player One
                {
                    if (m_removePlayer != true) // Ensures that the buttons don't clash with one another.
                    {
                        m_addPlayerButtonPress = true;
                    }
                }
                if (XCI.GetButton(addPlayerXbox, XboxController.Third) && playerNumber == 2) // Player One
                {
                    if (m_removePlayer != true) // Ensures that the buttons don't clash with one another.
                    {
                        m_addPlayerButtonPress = true;
                    }
                }
                if (XCI.GetButton(addPlayerXbox, XboxController.Fourth) && playerNumber == 3) // Player One
                {
                    if (m_removePlayer != true) // Ensures that the buttons don't clash with one another.
                    {
                        m_addPlayerButtonPress = true;
                    }
                }

                if (XCI.GetButton(removePlayerXbox, XboxController.Second) && playerNumber == 2)
                {
                    if (m_addPlayerButtonPress != true) // Ensures that the buttons don't clash with one another.
                    {
                        m_removePlayer = true;
                    }
                }
                if (XCI.GetButton(removePlayerXbox, XboxController.Third) && playerNumber == 3)
                {
                    if (m_addPlayerButtonPress != true) // Ensures that the buttons don't clash with one another.
                    {
                        m_removePlayer = true;
                    }
                }
                if (XCI.GetButton(removePlayerXbox, XboxController.Fourth) && playerNumber == 4)
                {
                    if (m_addPlayerButtonPress != true) // Ensures that the buttons don't clash with one another.
                    {
                        m_removePlayer = true;
                    }
                }


                if (m_addPlayerButtonPress) // Adding players
				{
					m_t2 += panelSpeed * Time.deltaTime; // Only used for player blocks.
            
					if (playerNumber == 1) // If there is one player. Adds the 2nd player.
					{
						playerTwoBlock.transform.position = Vector3.MoveTowards(m_playerOffScreenRight, m_playerTwoPos, m_t2); // Slowly moves the position to the target.
            
						if (playerTwoBlock.transform.position == m_playerTwoPos)
						{
							m_t2 = 0; // Resets timer.
							playerNumber++; // Increases player number by 1.
                            m_addPlayerButtonPress = false;
						}
					}
					else if (playerNumber == 2) // If there are two players. Adds the 3rd player.
					{
						playerThreeBlock.transform.position = Vector3.MoveTowards(m_playerOffScreenLeft, m_playerThreePos, m_t2); // Slowly moves the position to the target.
            
						if (playerThreeBlock.transform.position == m_playerThreePos)
						{
							m_t2 = 0; // Resets timer.
							playerNumber++; // Increases player number by 1.
                            m_addPlayerButtonPress = false;
						}
					}
					else if (playerNumber == 3) // If there are 3 players. Adds the 4th player.
					{
						playerFourBlock.transform.position = Vector3.MoveTowards(m_playerOffScreenRight, m_playerFourPos, m_t2); // Slowly moves the position to the target.
            
						if (playerFourBlock.transform.position == m_playerFourPos)
						{
							m_t2 = 0; // Resets timer.
							playerNumber++; // Increases player number by 1.
                            m_addPlayerButtonPress = false;
						}
					}
				}
				else if (m_removePlayer) // Removing player.
				{
					m_t2 += panelSpeed * Time.deltaTime; // Only used for player blocks.

					if (playerNumber == 2)
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
					else if (playerNumber == 3)
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
					else if (playerNumber == 4)
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

				if (Input.GetKeyUp(readyPlayerOne) || XCI.GetButtonDown(readyPlayerXbox, XboxController.First))
				{
					if (m_playerOneReady)
					{
						readyLightPlayerOne.enabled = false;
						m_playerOneReady = false;
					}
					else
					{
						m_playerOneReady = true;
						readyLightPlayerOne.enabled = true;
					}
				}
				else if ((Input.GetKeyUp(readyPlayerTwo) || XCI.GetButtonDown(readyPlayerXbox, XboxController.Second)) && playerNumber >= 2)
				{
					if (m_playerTwoReady)
					{
						readyLightPlayerTwo.enabled = false;
						m_playerTwoReady = false;
					}
					else
					{
						m_playerTwoReady = true;
						readyLightPlayerTwo.enabled = true;
					}
				}
				else if ((Input.GetKeyUp(readyPlayerThree) || XCI.GetButtonDown(readyPlayerXbox, XboxController.Third)) && playerNumber >= 3)
				{
					if (m_playerThreeReady)
					{
						readyLightPlayerThree.enabled = false;
						m_playerThreeReady = false;
					}
					else
					{
						m_playerThreeReady = true;
						readyLightPlayerThree.enabled = true;
					}
				}
				else if ((Input.GetKeyUp(readyPlayerFour) || XCI.GetButtonDown(readyPlayerXbox, XboxController.Fourth)) && playerNumber == 4)
				{
					if (m_playerFourReady)
					{
						readyLightPlayerFour.enabled = false;
						m_playerFourReady = false;
					}
					else
					{
						m_playerFourReady = true;
						readyLightPlayerFour.enabled = true;
					}
				}

				if (playerNumber == 2)
				{
					if (m_playerOneReady && m_playerTwoReady && characterPanel.transform.position != canvas.transform.position)
					{
						m_t += panelSpeed; // Only used for panels.
						characterPanel.transform.position = Vector3.MoveTowards(m_panelOffScreenBottomPos, canvas.transform.position, m_t); // Slowly moves the position to the target.

						if (characterPanel.transform.position == canvas.transform.position)
						{
							m_t = 0;
							m_showPlay = true;
						}
					}
					else if (m_showPlay)
					{
						if (!m_playerOneReady || !m_playerTwoReady)
						{
							m_t += panelSpeed; // Only used for panels.
							characterPanel.transform.position = Vector3.MoveTowards(canvas.transform.position, m_panelOffScreenBottomPos, m_t); // Slowly moves the position to the target.
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
						m_t += panelSpeed; // Only used for panels.
						characterPanel.transform.position = Vector3.MoveTowards(m_panelOffScreenBottomPos, canvas.transform.position, m_t); // Slowly moves the position to the target.

						if (characterPanel.transform.position == canvas.transform.position)
						{
							m_t = 0;
							m_showPlay = true;
						}
					}
					else if (m_showPlay)
					{
						if (!m_playerOneReady || !m_playerTwoReady || !m_playerThreeReady)
						{
							m_t += panelSpeed; // Only used for panels.
							characterPanel.transform.position = Vector3.MoveTowards(canvas.transform.position, m_panelOffScreenBottomPos, m_t); // Slowly moves the position to the target.
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
						m_t += panelSpeed; // Only used for panels.
						characterPanel.transform.position = Vector3.MoveTowards(m_panelOffScreenBottomPos, canvas.transform.position, m_t); // Slowly moves the position to the target.

						if (characterPanel.transform.position == canvas.transform.position)
						{
							m_t = 0;
							m_showPlay = true;
						}
					}
					else if (m_showPlay)
					{
						if (!m_playerOneReady || !m_playerTwoReady || !m_playerThreeReady || !m_playerFourReady)
						{
							m_t += panelSpeed; // Only used for panels.
							characterPanel.transform.position = Vector3.MoveTowards(canvas.transform.position, m_panelOffScreenBottomPos, m_t); // Slowly moves the position to the target.
							if (characterPanel.transform.position == m_panelOffScreenBottomPos)
							{
								m_t = 0;
								m_showPlay = false;
							}
						}
					}
				}

                if (m_showPlay && XCI.GetButtonDown(addPlayerXbox, XboxController.First))
                    PlayGame();

				break;
		}	
	}

	public void PlayGame()
    {
		GameInfo.playerOneScore = 0;
		GameInfo.playerTwoScore = 0;
		if (playerNumber >= 3)
			GameInfo.playerThreeScore = 0;
		if (playerNumber == 4)
			GameInfo.playerFourScore = 0;

		GameInfo.roundNumber = 1;
        SceneManager.LoadScene(1); // Loads the next scene within build settings.
    }

    public void QuitGame()
    {
        Debug.Log("App has Quit."); // Used to check if it will quit within unity.
        Application.Quit();
    }

	public void buttonPress()
	{
		m_playButtonPress = true;
	}
}
