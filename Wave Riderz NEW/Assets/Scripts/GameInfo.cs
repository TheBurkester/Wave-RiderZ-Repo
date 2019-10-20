/*-------------------------------------------------------------------*
|  Title:			GameInfo
|
|  Author:			Seth Johnston / Thomas Maltezos
| 
|  Description:		Static class that stores variables across rounds/scenes.
*-------------------------------------------------------------------*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameInfo
{
	public static int roundNumber;		//Round number, set to 1 in main menu
	public static int playerOneScore;	//Red's score
	public static int playerTwoScore;	//Green's score
	public static int playerThreeScore;	//Purple's score
	public static int playerFourScore;	//Orange's score
}
