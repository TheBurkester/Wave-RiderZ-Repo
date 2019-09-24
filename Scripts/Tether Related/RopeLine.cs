using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RopeLine : MonoBehaviour
{
	public Transform plane;

	private LineRenderer line = null;

	void Awake()
    {
		line = gameObject.GetComponent<LineRenderer>();
	}
	
    void Update()
    {
		Vector3[] linePos = new Vector3[2];
		linePos[0] = gameObject.transform.position;
		linePos[1] = plane.position;
		line.SetPositions(linePos);
	}
}
