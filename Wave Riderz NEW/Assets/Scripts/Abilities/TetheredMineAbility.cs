/*-------------------------------------------------------------------*
|  Title:		TetheredMineAbility
|
|  Author:		Thomas Maltezos	
| 
|  Description:	Handles the tethered mine ability.
|
|  Where to Place:	Place on the plane's hatch.
*-------------------------------------------------------------------*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TetheredMineAbility : MonoBehaviour
{

	public KeyCode shoot = KeyCode.G;
	public Rigidbody mineRB;

	private Transform m_planeHatch;
	private bool m_isUsingAbility = false;

	void Awake()
    {
		mineRB.gameObject.SetActive(false); // Disables the mine on startup.
		m_planeHatch = GetComponent<Transform>();
    }

    void Update()
    {
		if (Input.GetKey(shoot) && !m_isUsingAbility)
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
