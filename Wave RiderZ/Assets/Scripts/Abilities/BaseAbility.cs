/*-------------------------------------------------------------------*
|  Title:			BaseAbility
|
|  Author:			Thomas Maltezos
| 
|  Description:		Base class for all abilities.
*-------------------------------------------------------------------*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseAbility : MonoBehaviour
{
    protected Timer m_timer = new Timer();      //Counts until the cooldown is hit

    void Awake()
    {

    }

}
