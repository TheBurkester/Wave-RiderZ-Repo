/*-------------------------------------------------------------------*
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
    public float targetMovementSpeed = 10.0f;	// Target's movement speed when aiming.
    private Vector3 m_newPosition;				// Used to update the target's position each frame 
    private float m_targetPlaneRelation;        // Moves the target along with the plane and camera
    private PlaneController m_planeController;	//Reference to the plane to get its speed

	//Clamps
    public float riverClampHorizontal = 14;		// Editable horizontal clamp.
    public float riverClampForward = 3.5f;		// Editable forward clamp.
    public float riverClampBehind = 5;			// Editable behind clamp.
    private float m_riverClampForwardAlter;		// Clamp will always be moving forward.
    private float m_riverClampBehindAlter;		// Clamp will always be moving forward.

	//Ability
    public float cooldown = 5.0f;		// Used to define the length of the ability's cooldown.
	public float radius = 5.0f;			// The radius of the ability.
	public float power = 250;			// Force of the push.
    public float minePower = 150;       //Force of the push only on tethered mines
	public float forceDuration = 0.5f;	//How long to push for
	public float downwardSpeed = 10.0f;	// How quickly the ball will fall from the sky.
	private bool m_isShooting = false;	// Has the player pressed the shoot button.
    private MeshRenderer m_targetMesh;	// Target's Mesh.
    private Rigidbody m_targetRB;		// Target's Rigidbody.
	[HideInInspector]
    public Timer abilityCooldown;		// Timer used for the cooldown.
    private bool m_abilityReady = false;//True on the frame that the ability becomes ready

	[HideInInspector]
	public bool isAiming = false;

    public Rigidbody planeRB = null;	//Reference to the plane rigidbody
    public Animator BeachBallAnimation;

	//Remove in gold
    public KeyCode Up = KeyCode.W;				// Keyboard up control
    public KeyCode Down = KeyCode.S;			// Keyboard down control
    public KeyCode Left = KeyCode.A;			// Keyboard left control
    public KeyCode Right = KeyCode.D;			// Keyboard right control
    public KeyCode Aim = KeyCode.Space;			// Keyboard aim control
    public KeyCode Shoot = KeyCode.G;			// Keyboard shoot control


    void Awake()
    {
        m_targetRB = gameObject.GetComponent<Rigidbody>();
        m_targetMesh = gameObject.GetComponent<MeshRenderer>();
        m_targetMesh.enabled = false; // Target's mesh is disabled on startup.

        abilityCooldown = gameObject.AddComponent<Timer>();
        abilityCooldown.maxTime = cooldown;
		abilityCooldown.reverseTimer = false;
		abilityCooldown.autoDisable = true;
    }

    void Start()
    {
        abilityCooldown.SetTimer(); // Starts the timer.
		m_newPosition = transform.position;

        if (planeRB != null)
        {
            m_controller = planeRB.GetComponent<PlaneController>().controller;
            m_planeController = planeRB.GetComponent<PlaneController>();
            m_targetPlaneRelation = m_planeController.forwardSpeed;
        }
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
            //Animates the plane doors opening
            BeachBallAnimation.SetBool("IsAiming", true);
            BeachBallAnimation.SetBool("IsShooting", false);

            //Xbox movement
            m_newPosition.x += (axisX * targetMovementSpeed * 0.3f * Time.deltaTime); //Move the test position left/right
			m_newPosition.z += (axisY * targetMovementSpeed * 0.3f * Time.deltaTime); //Move the test position up/down

			//Keyboard movement
            if (Input.GetKey(Left)) // Movement Left.
				m_newPosition += Vector3.left * targetMovementSpeed * Time.deltaTime; // Moving Left = Vector.left
            if (Input.GetKey(Right)) // Movement Right.
				m_newPosition += Vector3.right * targetMovementSpeed * Time.deltaTime; // Moving Right = Vector.right.
            if (Input.GetKey(Up)) // Movement Up.
				m_newPosition += Vector3.forward * targetMovementSpeed * Time.deltaTime; // Moving Up = Vector.forward.
            if (Input.GetKey(Down)) // Movement Down.
				m_newPosition += Vector3.back * targetMovementSpeed * Time.deltaTime; // Moving Down = Vector.back.

			//Clamping
			if (!(m_newPosition.x < riverClampHorizontal) || !(m_newPosition.x > -riverClampHorizontal))        //Check if the new position z is oustide the boundaries
				m_newPosition.x = currentPosition.x;															//If it is, undo the z movement
			if (!(m_newPosition.z < m_riverClampForwardAlter) || !(m_newPosition.z > m_riverClampBehindAlter))  //Check if the new position x is oustide the boundaries
				m_newPosition.z = currentPosition.z;															//If it is, undo the x movement
		}
        // Simple transform which combines all directions and allows diagonal movement.
        transform.position = m_newPosition;

        if (Input.GetKey(Shoot) && m_targetMesh.enabled)
        {
            ToggleIsShooting(true);
			ShootBall(); // Shoots the beachball.
			ToggleMeshEnable(true);
            abilityCooldown.SetTimer(); // Resets cooldown.
			isAiming = false;
		}

        if((1.0f - RT) < 0.1f && m_targetMesh.enabled)	//If the right trigger is mostly pressed
        {
			ToggleIsShooting(true);
			ShootBall(); // Shoots the beachball.
			ToggleMeshEnable(true);
            abilityCooldown.SetTimer(); // Resets cooldown.
			isAiming = false;
        }
      
      
        /*===========================================================================================*/

        if (Input.GetKeyDown(Aim) && !abilityCooldown.UnderMax())	//If the aim button is pressed,
        {
			if (m_targetMesh.enabled == false)	//If the target is off,
				m_targetMesh.enabled = true;	//Turn it on
			else								//If the target is on,
				m_targetMesh.enabled = false;	//Turn it off
        }
		if (XCI.GetButtonDown(XboxButton.RightBumper, m_controller) && !abilityCooldown.UnderMax())	//If the right bumper is pressed,
        {
			if (m_targetMesh.enabled == false)  //If the target is off,
			{
				m_targetMesh.enabled = true;    //Turn it on
				isAiming = true;
			}
			else                                //If the target is on,
			{
				m_targetMesh.enabled = false;   //Turn it off
				isAiming = false;
			}
		}

		if (m_isShooting)					//If the ability is currently shooting,
			m_targetMesh.enabled = true;	//Make sure the target is on

        if (!abilityCooldown.UnderMax() && !m_abilityReady)		//If the ability is ready but the bool hasn't been set to true yet,
        {
            m_abilityReady = true;								//Set the bool to true
            AudioManager.Play("BothPlaneCooldownAbiltysReady");	//Play the ability ready sound
        }
        else if (abilityCooldown.UnderMax())					//If the ability isn't ready,
            m_abilityReady = false;								//Set the bool to false
    }

    public void ShootBall()
    {
        // Gets the beach ball from the Object Pool.
        GameObject beachBall = ObjectPool.sharedInstance.GetPooledObject("Beach Ball");
        if (beachBall != null)
        {
            BeachBallAnimation.SetBool("IsShooting", true);
            BeachBallAnimation.SetBool("IsAiming", false);
            // Landing position travels with the plane. Also keeps the ball from spawning within the camera's view.
            Vector3 v3LandingPos = m_targetRB.position + new Vector3(0, 30, m_targetPlaneRelation * Time.deltaTime);
			beachBall.transform.position = v3LandingPos;

            Rigidbody rb = beachBall.GetComponent<Rigidbody>();
            rb.velocity = new Vector3(0, -downwardSpeed, m_targetPlaneRelation - 0.8f); // Keeps the velocity inline with the plane's movement forward.
			beachBall.SetActive(true);
            AudioManager.Play("BeachBallFired");
        }
    }

	public bool GetShooting()
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

	private void OnDrawGizmos()
	{
		if (m_targetRB != null && m_targetMesh.enabled)
		{
			Gizmos.color = Color.yellow;
			Gizmos.DrawWireSphere(m_targetRB.position, radius);
		}
	}
}
