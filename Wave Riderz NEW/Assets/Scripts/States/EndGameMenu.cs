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
	private int m_playerCount = MainMenu.playerNumber;	//Reference to the number of players
	public Text scoreOne = null;			//Reference to the red player's scoreboard
	public Text scoreTwo = null;			//Reference to the green player's scoreboard
	public Text scoreThree = null;			//Reference to the purple player's scoreboard
	public Text scoreFour = null;			//Reference to the orange player's scoreboard
	private Text[] m_scoreDisplays = null;	//Array of the scoreboards
	private int[] m_scores = null;			//Array of the scores
	private int[] m_playerNumber = null;	//Array of player numbers to keep track of who is in what place alongside the scores
    public Canvas canvas = null;
    public RectTransform wavePanel = null;	//Reference to the wave image that washes over the UI
    private Vector3 m_panelOffScreenRight = new Vector3(2600, 0, 0);
    private int m_t = 0;	//Timer for the wave movement

    void Awake()
	{
		//Set the reference arrays
		m_scoreDisplays = new Text[4] { scoreOne, scoreTwo, scoreThree, scoreFour };
		m_scores = new int[] { GameInfo.playerOneScore, GameInfo.playerTwoScore, GameInfo.playerThreeScore, GameInfo.playerFourScore };
		m_playerNumber = new int[] { 1, 2, 3, 4 };

        wavePanel.transform.position = canvas.transform.position;

		SortSkiers();	//Sort the scores
		for (int i = 0; i < 4; ++i)				//For the max number of players,
		{
			if (i < m_playerCount)				//If this player is in the game,
				SetScoreboardPosition(i);		//Display their score
			else								//Otherwise,
				m_scoreDisplays[i].text = "";	//Display nothing
		}
	}

	void Update()
	{
        m_t += 40;
        wavePanel.transform.position = Vector3.MoveTowards(canvas.transform.position, m_panelOffScreenRight, m_t); // Slowly moves the position to the target.

        if (XCI.GetButtonDown(XboxButton.A, XboxController.First))	//If player 1 presses the menu button,
			GoToMenu();												//It takes them to the main menu
	}

	private void GoToMenu()
	{
		SceneManager.LoadScene(0);	//Loads the main menu scene
	}

	//Sorts the scores to be in descending order
	private void SortSkiers()
	{
		//Bubble sort in descending order
		int temp;
		for (int j = 0; j < m_playerCount - 1; j++)
		{
			for (int i = 0; i < m_playerCount - 1; i++)
			{
				if (m_scores[i] < m_scores[i + 1])
				{
					//Swap the scores
					temp = m_scores[i + 1];
					m_scores[i + 1] = m_scores[i];
					m_scores[i] = temp;

					//Swap the skier numbers too to keep track of who is where
					temp = m_playerNumber[i + 1];
					m_playerNumber[i + 1] = m_playerNumber[i];
					m_playerNumber[i] = temp;
				}
			}
		}
	}

	//Sets the position of each skier's score on the scoreboard, in order
	private void SetScoreboardPosition(int place)
	{
		//Write which skier this is and their score
		m_scoreDisplays[m_playerNumber[place] - 1].text = "Player " + m_playerNumber[place] + " - " + m_scores[place];

		//Calculate where on the scoreboard this skier should be placed
		float newYPos = (1.0f - (place * 0.16f)) - 0.35f;
		RectTransform rect = m_scoreDisplays[m_playerNumber[place] - 1].rectTransform;
		Vector2 newPos = new Vector2(rect.anchorMin.x, newYPos);
		rect.anchorMin = newPos;
		rect.anchorMax = newPos;
	}
}
