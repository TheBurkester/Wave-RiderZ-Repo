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
	public Text scoreOne = null;		//Reference to the red player's scoreboard
	public Text scoreTwo = null;      //Reference to the green player's scoreboard
	public Text scoreThree = null;     //Reference to the purple player's scoreboard
	public Text scoreFour = null;     //Reference to the orange player's scoreboard
    public Canvas canvas = null;
    public RectTransform wavePanel = null;
    private Vector3 m_panelOffScreenRight = new Vector3(2300, 315.5f, 0);
    private int m_t = 0;

    void Awake()
	{
		scoreThree.text = "";	//Player 3's score is invisible by default
		scoreFour.text = "";  //Player 4's score is invisible by default

        wavePanel.transform.position = canvas.transform.position;

		scoreOne.text = GameInfo.playerOneScore.ToString();				//Display red's score
		scoreTwo.text = GameInfo.playerTwoScore.ToString();			//Display green's score
		if (m_playerCount >= 3)											//If player 3 is present,
			scoreThree.text = GameInfo.playerThreeScore.ToString();	//Display purple's score
		if (m_playerCount == 4)											//If player 4 is present,
			scoreFour.text = GameInfo.playerFourScore.ToString();		//Display orange's score
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
