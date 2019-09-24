using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehicleMovement : MonoBehaviour
{
	public float speed = 5;
	public float strafeSpeed = 3;

	private Rigidbody rb = null;

    public float Smoothness = 2.0f;
    public float TiltAngle = 20.0f;

    // Start is called before the first frame update
    void Awake()
    {
		rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (rb != null)
		{
			Vector3 newPos = rb.position + new Vector3(speed * Time.deltaTime, 0, 0);
			//rb.MovePosition(newPos);	//Keep in mind this doesn't update the position until the end of the frame
            rb.transform.position = newPos;

            float tiltAroundZ = Input.GetAxis("Horizontal") * -TiltAngle;

            Quaternion Target = Quaternion.Euler(0, 90, tiltAroundZ);
            Quaternion Default = Quaternion.Euler(0, 90, 0);

            rb.transform.rotation = Quaternion.Slerp(rb.transform.rotation, Default, Time.deltaTime * Smoothness);

            if (Input.GetKey(KeyCode.LeftArrow))
			{
				Vector3 strafe = newPos + new Vector3(0, 0, strafeSpeed * Time.deltaTime);

                rb.transform.rotation = Quaternion.Slerp(rb.transform.rotation, Target, Time.deltaTime * Smoothness);
                //rb.MovePosition(strafe);
                rb.transform.position = strafe;



            }
			if (Input.GetKey(KeyCode.RightArrow))
			{
				Vector3 strafe = newPos + new Vector3(0, 0, -strafeSpeed * Time.deltaTime);
				rb.MovePosition(strafe);
                rb.transform.rotation = Quaternion.Slerp(rb.transform.rotation, Target, Time.deltaTime * Smoothness);
                //rb.MovePosition(strafe);
                rb.transform.position = strafe;
            }


		}

    }
}
