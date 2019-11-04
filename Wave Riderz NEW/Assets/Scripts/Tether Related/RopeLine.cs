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

[ExecuteInEditMode]
public class RopeLine : MonoBehaviour
{
	public Transform planeTetherPoint = null;       //Reference to the tether point

	private Tether m_tether = null;

	private LineRenderer lineRenderer = null;   //Reference to the line renderer
	private int m_numberOfPoints = 20;

	private bool m_isFading = false;
	private float m_fadeSpeed = 2;

	void Awake()
    {
		Debug.Assert(planeTetherPoint != null, "RopeLine missing plane tether point reference");

		m_tether = GetComponentInParent<Tether>();
		Debug.Assert(m_tether != null, "RopeLine failed accessing tether component of skier parent");

		lineRenderer = gameObject.GetComponent<LineRenderer>();
		Debug.Assert(lineRenderer != null, "RopeLine missing LineRenderer component");
		lineRenderer.useWorldSpace = true;
		lineRenderer.positionCount = m_numberOfPoints;
	}
	
    void Update()
    {
		Vector3 p0 = transform.position;			//First point is the skier handle
		Vector3 p2 = planeTetherPoint.position;		//Last point is the plane tether point
		Vector3 p1 = (p0 + p2) / 2;					//The middle point is the midpoint between p0 and p2

		//The further away the skier is from the tether arc, the lower the midpoint should dip
		//If the skier is along the tether arc, the dip is 0 so that the rope looks stretched
		float ropeDip = (1 - (m_tether.Distance() / m_tether.currentLength)) * 15;
		p1.y -= ropeDip;	//Apply the dip to the midpoint

		float t;			//0-1 number representing the interpolation between the first/last points
		Vector3 position;	//Temporary position set to where each point along the line will be
		for (int i = 0; i < m_numberOfPoints; i++)		//For however many vertexes along the line we want,
		{
			t = i / (m_numberOfPoints - 1.0f);          //Calculate how far along the curve we are
			//Quadratic bezier equation = (1 - t)^2 * p0 + 2(1 - t)tp1 + t^2 * p2
			position = (1.0f - t) * (1.0f - t) * p0 + 2.0f * (1.0f - t) * t * p1 + t * t * p2;
			lineRenderer.SetPosition(i, position);		//Set the position of the current vertex
		}

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
