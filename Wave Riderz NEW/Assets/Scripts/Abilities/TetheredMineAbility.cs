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
	public Rigidbody planeRB = null;

	private Transform m_planeHatch;
	private bool m_isUsingAbility = false;

	[HideInInspector]
	public Timer abilityCooldown;       // Timer used for the cooldown.

	public float cooldown = 10;			// Cooldown for the ability.

	void Start() // KEEP THIS AS START OR I WILL PERSONALLY SMITE YOU. WE SPENT TOO LONG ON THIS.
    {
		mineRB.gameObject.SetActive(false); // Disables the mine on startup.
		m_planeHatch = GetComponent<Transform>();
		m_controller = planeRB.GetComponent<PlaneController>().controller;

		abilityCooldown = gameObject.AddComponent<Timer>();
		abilityCooldown.maxTime = cooldown;
		abilityCooldown.reverseTimer = true;
		abilityCooldown.autoDisable = true;

		abilityCooldown.SetTimer(); // Starts the timer.
	}

    void Update()
    {
		float LT = XCI.GetAxis(XboxAxis.LeftTrigger, m_controller);
		
		if ((1.0f - LT) < 0.1f && !m_isUsingAbility && !abilityCooldown.UnderMax())
		{
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

		if (mineRB.transform.position.y <= 0)
		{
			mineRB.transform.position = new Vector3(mineRB.transform.position.x, 0, mineRB.transform.position.z);
		}
	}

	public void setIsUsingAbility(bool value)
	{
		m_isUsingAbility = value;
	}
}
