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

public class TetheredMineAbility : MonoBehaviour
{
	private XboxController m_controller;        // Reference to which controller to use (same as plane's)
	public Rigidbody mineRB;
	private Tether m_mineTether = null;
	public Rigidbody planeRB = null;
	private PlaneController m_planeController;

	private Transform m_planeHatch;
	private bool m_isUsingAbility = false;
	private float m_planeRelation;

	[HideInInspector]
	public Timer mineAbilityCooldown;       // Timer used for the cooldown.

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
		mineRB.gameObject.SetActive(false); // Disables the mine on startup.
		m_planeHatch = GetComponent<Transform>();
		m_controller = planeRB.GetComponent<PlaneController>().controller;
		m_planeController = planeRB.GetComponent<PlaneController>();
		m_planeRelation = m_planeController.forwardSpeed;
		m_mineTether = mineRB.GetComponent<Tether>();
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

            m_mineTether.ResetVelocity();
			mineRB.transform.position = m_planeHatch.transform.position;
            
            mineRB.gameObject.SetActive(true);
			m_isUsingAbility = true;
            
		}
        if (Input.GetKeyDown(KeyCode.M) && !m_isUsingAbility && !mineAbilityCooldown.UnderMax())
        {
            
         
			m_mineTether.ResetVelocity();
			mineRB.transform.position = m_planeHatch.transform.position;
            mineRB.gameObject.SetActive(true);
            m_isUsingAbility = true;
           
        }
        
        if (m_isUsingAbility)
		{
            ActivateAbility();
		}
    }

	void ActivateAbility()
	{
		if (mineRB.transform.position.y > 0)
		{
			mineRB.isKinematic = false;
			mineRB.velocity = new Vector3(0, -5.0f, m_planeRelation - 3);
		}
		else if (mineRB.transform.position.y <= 0)
		{
			mineRB.isKinematic = true;
			mineRB.transform.position = new Vector3(mineRB.transform.position.x, 0, mineRB.transform.position.z);
		}
	}

	public void setIsUsingAbility(bool value)
	{
		m_isUsingAbility = value;
	}
}
