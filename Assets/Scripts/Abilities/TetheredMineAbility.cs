/*-------------------------------------------------------------------*
|  TetheredMineAbility
|
|  Author:		    Thomas Maltezos	
| 
|  Description:		Handles the plane's tethered mine ability.
*-------------------------------------------------------------------*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TetheredMineAbility : BaseAbility
{
    void Awake()
    {
        m_timer.SetActive(false);
    }

    void Update()
    {
        
    }
}
