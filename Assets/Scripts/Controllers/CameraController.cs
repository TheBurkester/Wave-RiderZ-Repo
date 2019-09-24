using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
	private float m_speed = 5;

    private Rigidbody rb = null;

    // Start is called before the first frame update
    void Awake()
	{
		GameObject vehicle = GameObject.Find("Vehicle");
		if (vehicle != null)
			m_speed = vehicle.GetComponent<VehicleMovement>().speed;

        rb = GetComponent<Rigidbody>();
    }

	// Update is called once per frame
	void Update()
	{
        Vector3 newPos = rb.position + new Vector3(m_speed * Time.deltaTime, 0, 0);
        rb.MovePosition(newPos);	//Keep in mind this doesn't update the position until the end of the frame

        //Vector3 newPos = transform.position + new Vector3(m_speed * Time.deltaTime, 0, 0);
        //transform.Translate(newPos);
    }
}
