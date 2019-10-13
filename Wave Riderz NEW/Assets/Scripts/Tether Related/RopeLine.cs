﻿/*-------------------------------------------------------------------*
|  Title:			RopeLine
|
|  Author:          Seth Johnston
| 
|  Description:		Used with a Line Renderer to draw a basic rope.
*-------------------------------------------------------------------*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RopeLine : MonoBehaviour
{
	public Transform planeTetherPoint = null;		//Reference to the tether point

	private LineRenderer line = null;	//Reference to the line renderer

	void Awake()
    {
		Debug.Assert(planeTetherPoint != null, "RopeLine missing plane tether point reference");

		line = gameObject.GetComponent<LineRenderer>();
		Debug.Assert(line != null, "RopeLine missing LineRenderer component");
	}
	
    void Update()
    {
		Vector3[] linePos = new Vector3[2];			//Create a 2D array of vectors
		linePos[0] = gameObject.transform.position;	//First vector is the skier position
		linePos[1] = planeTetherPoint.position;		//Second vector is the tether point
		line.SetPositions(linePos);					//Draw the line between them
	}
}