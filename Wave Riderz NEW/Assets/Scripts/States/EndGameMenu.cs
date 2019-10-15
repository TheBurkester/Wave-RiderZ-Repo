using UnityEngine;
using UnityEngine.SceneManagement;
using XboxCtrlrInput;
using UnityEngine.UI;

public class EndGameMenu : MonoBehaviour
{
	public XboxButton backToMenu = XboxButton.A; // Adds a player to the game with Xbox controls.

	private int m_playerCount = MainMenu.playerNumber; // Reference to the number of players from the main menu.
	public Text scoreRed = null;
	public Text scoreGreen = null;
	public Text scorePurple = null;
	public Text scoreOrange = null;

	void Awake()
	{
		scorePurple.text = "";
		scoreOrange.text = "";

		scoreRed.text = GameInfo.playerOneScore.ToString();
		scoreGreen.text = GameInfo.playerTwoScore.ToString();
		if (m_playerCount >= 3)
			scorePurple.text = GameInfo.playerThreeScore.ToString();
		if (m_playerCount == 4)
			scoreOrange.text = GameInfo.playerFourScore.ToString();
	}

	void Update()
	{
		if (XCI.GetButtonDown(backToMenu, XboxController.First))
			GoToMenu();
	}

	public void GoToMenu()
	{
		SceneManager.LoadScene(0); // Loads the menu scene
	}

	public void QuitGame()
	{
		Debug.Log("App has Quit."); // Used to check if it will quit within unity.
		Application.Quit();
	}
}
