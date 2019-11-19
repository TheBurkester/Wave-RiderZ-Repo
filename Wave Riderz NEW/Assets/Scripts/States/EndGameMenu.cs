/*-------------------------------------------------------------------*
|  Title:			EndGameMenu
|
|  Author:			Seth Johnston
| 
|  Description:		Displays player scores when all rounds are over,
|					and lets players return to menu
*-------------------------------------------------------------------*/

using UnityEngine;
using UnityEngine.SceneManagement;
using XboxCtrlrInput;
using UnityEngine.UI;

public class EndGameMenu : MonoBehaviour
{
	public XboxButton menuButton = XboxButton.A;		//Stores which Xbox button takes players back to the menu

	private int m_playerCount = MainMenu.playerNumber;	//Reference to the number of players
	public Text scoreRed = null;		//Reference to the red player's scoreboard
	public Text scoreGreen = null;      //Reference to the green player's scoreboard
	public Text scorePurple = null;     //Reference to the purple player's scoreboard
	public Text scoreOrange = null;     //Reference to the orange player's scoreboard
    public Canvas canvas = null;
    public RectTransform wavePanel = null;
    private Vector3 m_panelOffScreenRight = new Vector3(2300, 315.5f, 0);
    private int m_t = 0;

    void Awake()
	{
		scorePurple.text = "";	//Player 3's score is invisible by default
		scoreOrange.text = "";  //Player 4's score is invisible by default

        wavePanel.transform.position = canvas.transform.position;

		scoreRed.text = GameInfo.playerOneScore.ToString();				//Display red's score
		scoreGreen.text = GameInfo.playerTwoScore.ToString();			//Display green's score
		if (m_playerCount >= 3)											//If player 3 is present,
			scorePurple.text = GameInfo.playerThreeScore.ToString();	//Display purple's score
		if (m_playerCount == 4)											//If player 4 is present,
			scoreOrange.text = GameInfo.playerFourScore.ToString();		//Display orange's score
	}

	void Update()
	{
        m_t += 40;
        wavePanel.transform.position = Vector3.MoveTowards(canvas.transform.position, m_panelOffScreenRight, m_t); // Slowly moves the position to the target.

        if (XCI.GetButtonDown(menuButton, XboxController.First))	//If player 1 presses the menu button,
			GoToMenu();												//It takes them to the main menu
	}

	private void GoToMenu()
	{
		SceneManager.LoadScene(0);	//Loads the main menu scene
	}
}
