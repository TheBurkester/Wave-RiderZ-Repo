/*-------------------------------------------------------------------*
|  Title:		TetheredMineAbility
|
|  Author:		Thomas Maltezos	
| 
|  Description:	Handles the tethered mine ability.
|
|  Where to Place:	Place on the plane's hatch.
*-------------------------------------------------------------------*/

using UnityEngine;
using XboxCtrlrInput;
using System.Collections;

public class TetheredMineAbility : MonoBehaviour
{
	private XboxController m_controller;        // Reference to which controller to use (same as plane's)
	public GameObject mine;
	private Tether m_mineTether = null;
	public Rigidbody planeRB = null;
	private PlaneController m_planeController;

	private Transform m_planeHatch;
	private bool m_isUsingAbility = false;
	[HideInInspector]
	public float planeSpeed;

	[HideInInspector]
	public Timer mineAbilityCooldown;       // Timer used for the cooldown.

    private bool m_mineabilityReady = false; //True on the frame that the ability becomes ready

    public float cooldown = 10;			// Cooldown for the ability.

    public float obstacleForce = 75;           //How much sidewards force is applied when hitting an obstacle
    public float obstacleForceDuration = 0.5f;  //How long obstacle forces are applied
    public Animator TetheredMineAnimation;

    private void Awake()
    {
        //Create timer first so that it's created before gamemanager tries to use it
        mineAbilityCooldown = gameObject.AddComponent<Timer>();
        mineAbilityCooldown.maxTime = cooldown;
        mineAbilityCooldown.reverseTimer = false;
        mineAbilityCooldown.autoDisable = true;

        mineAbilityCooldown.SetTimer(); // Starts the timer.
    }

    void Start() // KEEP THIS AS START OR I WILL PERSONALLY SMITE YOU. WE SPENT TOO LONG ON THIS.
    {
		mine.SetActive(false); // Disables the mine on startup.
		m_planeHatch = GetComponent<Transform>();
		m_controller = planeRB.GetComponent<PlaneController>().controller;
		m_planeController = planeRB.GetComponent<PlaneController>();
		planeSpeed = m_planeController.forwardSpeed;
		m_mineTether = mine.GetComponent<Tether>();
	}

    void Update()
    {
		float LT = XCI.GetAxis(XboxAxis.LeftTrigger, m_controller);

		if ((1.0f - LT) < 0.1f && !m_isUsingAbility && !mineAbilityCooldown.UnderMax())
		{
            // animation for the door
            TetheredMineAnimation.SetBool("IsDoorOpen", true);
            TetheredMineAnimation.SetBool("IsDoorClosed", false);
            // Hatch Open Door Sound
            AudioManager.Play("TetherObs&BBHatchDoorOpen");
			
			StartCoroutine(SpawnMine());
			m_isUsingAbility = true;
		}
        
        if(!mineAbilityCooldown.UnderMax() && !m_mineabilityReady)
        {
            m_mineabilityReady = true;
            AudioManager.Play("BothPlaneCooldownAbiltysReady");	//Play the ability ready sound
        }
        else if (mineAbilityCooldown.UnderMax())
        {
            m_mineabilityReady = false;
        }
    }

	IEnumerator SpawnMine()
	{
		yield return new WaitForSeconds(1);

		mine.SetActive(true);
		mine.GetComponent<Mine>().Reset();
		mine.transform.position = m_planeHatch.position;
	}

	public void setIsUsingAbility(bool value)
	{
		m_isUsingAbility = value;
	}
}
