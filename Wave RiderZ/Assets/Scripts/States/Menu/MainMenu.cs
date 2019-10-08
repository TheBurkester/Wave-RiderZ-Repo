/*-------------------------------------------------------------------*
|  Title:			MainMenu
|
|  Author:			Thomas Maltezos
| 
|  Description:		Handles the main menu.
*-------------------------------------------------------------------*/

using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
	public enum PanalState
	{
		eSplashScreen // Splash screen state.
	}


    public GameObject splashPanel;
    public Canvas canvas;
    public float panelSpeed = 1;


    // Individual bools for each player.
    private bool m_playerOneReady = false;
    private bool m_playerTwoReady = false;
    private bool m_playerThreeReady = false;
    private bool m_playerFourReady = false;

	private PanalState m_eCurrentState = 0;
    private int m_playerNumber = 1; // The number of players.
    private Vector3 panalStartPos;
    private Vector3 panalOffScreenPos;
    private float t;

    void Awake()
    {
        panalStartPos = canvas.transform.position;
        panalOffScreenPos = new Vector3(splashPanel.transform.position.x, 900, splashPanel.transform.position.z);
    }

	void Update()
	{
		switch (m_eCurrentState)
		{
			case PanalState.eSplashScreen:
				
				break;
		}
	}

	public void SplashScreenChange()
    {
            t += Time.deltaTime / panelSpeed;
            //splashPanel.transform.position = Vector3.Lerp(panalStartPos, panalOffScreenPos, t);
            splashPanel.transform.position = Vector3.MoveTowards(panalStartPos, panalOffScreenPos, t);
    }

    public void PlayGame()
    {
        SceneManager.LoadScene(1); // Loads the next scene within build settings.
    }

    public void QuitGame()
    {
        Debug.Log("App has Quit."); // Used to check if it will quit within unity.
        Application.Quit();
    }
}
