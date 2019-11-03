/*-------------------------------------------------------------------*
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
	
	private bool m_isFading = false;
	private float m_fadeSpeed = 2;

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

		if (m_isFading)
		{
			Renderer renderer = GetComponent<Renderer>();
			Color color = renderer.material.color;
			float fade = Time.deltaTime * m_fadeSpeed;
			if (color.a - fade > 0)
			{
				color.a -= fade;
				renderer.material.color = color;
			}
			else
			{
				color.a = 0;
				renderer.material.color = color;
				enabled = false;
			}
		}
	}

	public void Fade(float fadeSpeed)
	{
		m_isFading = true;
		m_fadeSpeed = fadeSpeed;
	}
}
