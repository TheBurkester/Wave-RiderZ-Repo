﻿/*-------------------------------------------------------------------*
|  Title:			BeachBallAbility
|
|  Author:		    Thomas Maltezos / Seth Johnston
| 
|  Description:		Handles the plane's beach ball ability.
*-------------------------------------------------------------------*/
using UnityEngine;
using XboxCtrlrInput;

public class BeachBallAbility : MonoBehaviour
{
	//Movement
    private XboxController m_controller;		// Reference to which controller to use (same as plane's)
    public KeyCode Up = KeyCode.W;				// Keyboard up control
    public KeyCode Down = KeyCode.S;			// Keyboard down control
    public KeyCode Left = KeyCode.A;			// Keyboard left control
    public KeyCode Right = KeyCode.D;			// Keyboard right control
    public KeyCode Aim = KeyCode.Space;			// Keyboard aim control
    public KeyCode Shoot = KeyCode.G;			// Keyboard shoot control
    public float targetMovementSpeed = 10.0f;	// Target's movement speed when aiming.
    private Vector3 m_newPosition;				// Used to update the target's position each frame 
    private float m_targetPlaneRelation = 5.0f; // Moves the target along with the plane and camera. KEEP VARIABLE THE SAME AS PLANE SPEED IN PLANE CONTROLLER.
	private Vector3 m_debugLandingPos;			// Holds the position for the radius debug sphere.

	//Clamps
    public float riverClampHorizontal = 14;		// Editable horizontal clamp.
    public float riverClampForward = 3.5f;		// Editable forward clamp.
    public float riverClampBehind = 5;			// Editable behind clamp.
    private float m_riverClampForwardAlter;		// Clamp will always be moving forward.
    private float m_riverClampBehindAlter;		// Clamp will always be moving forward.

	//Ability
    public float cooldown = 5.0f;		// Used to define the length of the ability's cooldown.
	public float radius = 5.0f;			// The radius of the ability.
	public float power = 2000.0f;		// Force of the push.
	public float downwardSpeed = 10.0f;	// How quickly the ball will fall from the sky.
	private bool m_isShooting = false;	// Has the player pressed the shoot button.
    private MeshRenderer m_targetMesh;	// Target's Mesh.
    private Rigidbody m_targetRB;		// Target's Rigidbody.
    private GameObject m_prefab;		// Beachball prefab.
	[HideInInspector]
    public Timer abilityCooldown;		// Timer used for the cooldown.
 
    public Rigidbody planeRB = null;	//Reference to the plane rigidbody


    void Awake()
    {
        m_targetRB = gameObject.GetComponent<Rigidbody>();
        m_targetMesh = gameObject.GetComponent<MeshRenderer>();
        m_targetMesh.enabled = false; // Target's mesh is disabled on startup.
        m_prefab = Resources.Load("BeachBall") as GameObject;

        abilityCooldown = gameObject.AddComponent<Timer>();
        abilityCooldown.maxTime = cooldown;
		abilityCooldown.reverseTimer = true;
		abilityCooldown.autoDisable = true;
    }

    void Start()
    {
        abilityCooldown.SetTimer(); // Starts the timer.
		m_newPosition = transform.position;

		if (planeRB != null)
			m_controller = planeRB.GetComponent<PlaneController>().controller;
	}

    void Update()
    {
        AimTarget();
    }

    public void AimTarget()
    {
        // Target's movement forward in relation to the plane.
        Vector3 v3PlaneRelation = m_targetRB.position + new Vector3(0, 0, m_targetPlaneRelation * Time.deltaTime);
        m_targetRB.transform.position = v3PlaneRelation;

        // Handles the clamp movement forward with the plane.
        m_riverClampForwardAlter = planeRB.position.z - riverClampForward;
        m_riverClampBehindAlter = planeRB.position.z - riverClampBehind * 2.5f;

		// RIGHT STICK MOVEMENT 
		m_newPosition = transform.position;
        float axisX = XCI.GetAxis(XboxAxis.RightStickX, m_controller);
        float axisY = XCI.GetAxis(XboxAxis.RightStickY, m_controller);
        // right trigger 
        float RT = XCI.GetAxis(XboxAxis.RightTrigger, m_controller);
      

        /*===========================================================================================*/
        /*      Leave this outside of the aim If statement. Won't be called if inside.               */

        if (!m_isShooting && m_targetMesh.enabled)
        {
			Vector3 currentPosition = m_newPosition;

			m_newPosition.x += (axisX * targetMovementSpeed * 0.3f * Time.deltaTime); //Move the test position left/right
			m_newPosition.z += (axisY * targetMovementSpeed * 0.3f * Time.deltaTime); //Move the test position up/down

            if (Input.GetKey(Left)) // Movement Left.
            {
				m_newPosition += Vector3.left * targetMovementSpeed * Time.deltaTime; // Moving Left = Vector.left
            }

            if (Input.GetKey(Right)) // Movement Right.
            {
				m_newPosition += Vector3.right * targetMovementSpeed * Time.deltaTime; // Moving Right = Vector.right.
            }

            if (Input.GetKey(Up)) // Movement Up.
            {
				m_newPosition += Vector3.forward * targetMovementSpeed * Time.deltaTime; // Moving Up = Vector.forward.
            }

            if (Input.GetKey(Down)) // Movement Down.
            {
				m_newPosition += Vector3.back * targetMovementSpeed * Time.deltaTime; // Moving Down = Vector.back.
            }

			if (!(m_newPosition.x < riverClampHorizontal) || !(m_newPosition.x > -riverClampHorizontal))        //Check if the new position z is oustide the boundaries
				m_newPosition.x = currentPosition.x;															//If it is, undo the z movement
			if (!(m_newPosition.z < m_riverClampForwardAlter) || !(m_newPosition.z > m_riverClampBehindAlter))  //Check if the new position x is oustide the boundaries
				m_newPosition.z = currentPosition.z;															//If it is, undo the x movement
		}
        // Simple transform which combines all directions and allows diagonal movement.
        //m_targetRB.transform.Translate(targetMovementSpeed * v3.normalized * Time.deltaTime);
        transform.position = m_newPosition;

        if (Input.GetKey(Shoot) && m_targetMesh.enabled)
        {
			ToggleIsShooting(true);
			ShootBall(); // Shoots the beachball.
			ToggleMeshEnable(true);
            abilityCooldown.SetTimer(); // Resets cooldown.
        }

        if((1.0f - RT) < 0.1f && m_targetMesh.enabled)	//If the right trigger is mostly pressed
        {
			ToggleIsShooting(true);
			ShootBall(); // Shoots the beachball.
			ToggleMeshEnable(true);
            abilityCooldown.SetTimer(); // Resets cooldown.
        }
      
      
        /*===========================================================================================*/

        if (Input.GetKeyDown(Aim) && !abilityCooldown.UnderMax() || m_isShooting)
        {
            m_targetMesh.enabled = true; // Activate target's mesh when aiming.
        }
        else if (Input.GetKeyUp(Aim))
        {
            m_targetMesh.enabled = false; // Disable target's mesh when not aiming.
        }
        if (XCI.GetButtonDown(XboxButton.RightBumper, m_controller) && !abilityCooldown.UnderMax() || m_isShooting)	//If the right bumper is held,
        {
            m_targetMesh.enabled = true; // Activate target's mesh when aiming.
        }
        else if (XCI.GetButtonUp(XboxButton.RightBumper, m_controller))
        {
            m_targetMesh.enabled = false; // Disable target's mesh when not aiming.
        }
    }

    public void ShootBall()
    {
        // Gets the beach ball from the Object Pool.
        GameObject BeachBall = ObjectPool.sharedInstance.GetPooledObject("Beach Ball");
        if (BeachBall != null)
        {
            // Landing position travels with the plane. Also keeps the ball from spawning within the camera's view.
            Vector3 v3LandingPos = m_targetRB.position + new Vector3(0, 20, m_targetPlaneRelation * Time.deltaTime);
            BeachBall.transform.position = v3LandingPos;

            Rigidbody rb = BeachBall.GetComponent<Rigidbody>();
            rb.velocity = new Vector3(0, -downwardSpeed, m_targetPlaneRelation - 0.8f); // Keeps the velocity inline with the plane's movement forward.
            BeachBall.SetActive(true);

		}
    }

	public bool isShotting()
	{
		return m_isShooting;
	}

    // Used in Beachball script.
    public void ToggleIsShooting(bool shooting)
    {
        m_isShooting = shooting;
    }
    
	// Used in Beachball script.
    public void ToggleMeshEnable(bool enable)
    {
        m_targetMesh.enabled = enable;
    }

	public float getRadius()
	{
		return radius;
	}

	public float getPower()
	{
		return power;
	}

	private void OnDrawGizmos()
	{
		if (m_targetRB != null && m_targetMesh.enabled)
		{
			Gizmos.color = Color.yellow;
			Gizmos.DrawWireSphere(m_targetRB.position, getRadius());
		}
	}
}
