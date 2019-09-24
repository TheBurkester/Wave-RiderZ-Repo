using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
	public float speed = 5;
    public KeyCode MoveLeft;
    public KeyCode MoveRight;

	private Rigidbody rb = null;

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
            if (Input.GetKey(MoveLeft))
                rb.AddForce(0, 0, speed);
			if (Input.GetKey(MoveRight))
				rb.AddForce(0, 0, -speed);
		}
	}
}
