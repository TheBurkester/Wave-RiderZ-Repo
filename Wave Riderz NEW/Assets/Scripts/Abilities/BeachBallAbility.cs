/*-------------------------------------------------------------------*
|  Title:			BeachBallAbility
|
|  Author:		    Thomas Maltezos
| 
|  Description:		Handles the plane's beach ball ability.
*-------------------------------------------------------------------*/
using UnityEngine;
using XboxCtrlrInput;
using XInputDotNetPure;

public class BeachBallAbility : MonoBehaviour
{
    // These Keycodes will be changed later when Xbox Input is implemented.
    public XboxController controller;
    private Vector3 newPosition;
    public KeyCode Up = KeyCode.W;
    public KeyCode Down = KeyCode.S;
    public KeyCode Left = KeyCode.A;
    public KeyCode Right = KeyCode.D;
    public KeyCode Aim = KeyCode.Space;
    public KeyCode Shoot = KeyCode.G;
    public float Cooldown = 5.0f; // Used to define the length of the ability's cooldown.
    public float targetMovementSpeed = 10.0f; // Target's movement speed when aiming.
    public float riverClampHorizontal = 14; // Editable horizontal clamp.
    public float riverClampForward = 3.5f; // Editable forward clamp.
    public float riverClampBehind = 5; // Editable behind clamp.
    public Rigidbody planeRB = null;

    private const float MAX_TRG_SCL = 1.21f;
 
    private float m_targetPlaneRelation = 5.0f; // Moves the target along with the plane and camera. KEEP VARIABLE THE SAME AS PLANE SPEED IN PLANE CONTROLLER.
    private bool m_isShooting = false; // Has the player pressed the shoot button.
    private float m_riverClampForwardAlter; // Clamp will always be moving forward.
    private float m_riverClampBehindAlter; // Clamp will always be moving forward.
    private MeshRenderer m_targetMesh; // Target's Mesh.
    private Rigidbody m_targetRB; // Target's Rigidbody.
    private GameObject m_prefab; // Beachball prefab.
    private Timer m_abilityCooldown; // Timer used for the cooldown.

    void Awake()
    {
        m_targetRB = gameObject.GetComponent<Rigidbody>();
        m_targetMesh = gameObject.GetComponent<MeshRenderer>();
        m_targetMesh.enabled = false; // Target's mesh is disabled on startup.
        m_prefab = Resources.Load("BeachBall") as GameObject;

        m_abilityCooldown = gameObject.AddComponent<Timer>();
        m_abilityCooldown.maxTime = Cooldown;
    }

    void Start()
    {
        m_abilityCooldown.SetTimer(); // Starts the timer.
        newPosition = transform.position;
    }

    void Update()
    {
        aimTarget();
        
      
    }

    void aimTarget()
    {
        // Target's movement forward in relation to the plane.
        Vector3 v3PlaneRelation = m_targetRB.position + new Vector3(0, 0, m_targetPlaneRelation * Time.deltaTime);
        m_targetRB.transform.position = v3PlaneRelation;

        // Handles the clamp movement forward with the plane.
        m_riverClampForwardAlter = planeRB.position.z - riverClampForward;
        m_riverClampBehindAlter = planeRB.position.z - riverClampBehind * 2.5f;

        // RIGHT STICK MOVEMENT 
        newPosition = transform.position;
        float axisX = XCI.GetAxis(XboxAxis.RightStickX, controller);
        float axisY = XCI.GetAxis(XboxAxis.RightStickY, controller);
        // right trigger 
        float RT = XCI.GetAxis(XboxAxis.RightTrigger, controller);
      

        /*===========================================================================================*/
        /*      Leave this outside of the aim If statement. Won't be called if inside.               */

        if (!m_isShooting && m_targetMesh.enabled)
        {
			Vector3 oldPosition = newPosition;

            newPosition.x += (axisX * targetMovementSpeed * 0.3f * Time.deltaTime); //Move the test position left/right
            newPosition.z += (axisY * targetMovementSpeed * 0.3f * Time.deltaTime); //Move the test position up/down

            if (Input.GetKey(Left)) // Movement Left.
            {
				newPosition += Vector3.left * targetMovementSpeed * Time.deltaTime; // Moving Left = Vector.forward due to scene direction.
            }

            if (Input.GetKey(Right)) // Movement Right.
            {
				newPosition += Vector3.right * targetMovementSpeed * Time.deltaTime; // Moving Right = Vector.back due to scene direction.
            }

            if (Input.GetKey(Up)) // Movement Up.
            {
				newPosition += Vector3.forward * targetMovementSpeed * Time.deltaTime; // Moving Up = Vector.right due to scene direction.
            }

            if (Input.GetKey(Down)) // Movement Down.
            {
				newPosition += Vector3.back * targetMovementSpeed * Time.deltaTime; // Moving Down = Vector.left due to scene direction.
            }

			if (!(newPosition.x < riverClampHorizontal) || !(newPosition.x > -riverClampHorizontal))        //Check if the new position z is oustide the boundaries
				newPosition.x = oldPosition.x;                     //If it is, undo the z movement
			if (!(newPosition.z < m_riverClampForwardAlter) || !(newPosition.z > m_riverClampBehindAlter))  //Check if the new position x is oustide the boundaries
				newPosition.z = oldPosition.z;                     //If it is, undo the x movement
		}
        // Simple transform which combines all directions and allows diagonal movement.
        //m_targetRB.transform.Translate(targetMovementSpeed * v3.normalized * Time.deltaTime);
        transform.position = newPosition;

        if (Input.GetKey(Shoot) && m_targetMesh.enabled)
        {
            toggleIsShooting(true);
            shootBall(); // Shoots the beachball.
            toggleMeshEnable(true);
            m_abilityCooldown.SetTimer(); // Resets cooldown.
        }

        if((1.0f - XCI.GetAxis(XboxAxis.RightTrigger, controller)) < 0.1f && m_targetMesh.enabled)
        {
            toggleIsShooting(true);
            shootBall(); // Shoots the beachball.
            toggleMeshEnable(true);
            m_abilityCooldown.SetTimer(); // Resets cooldown.
        }
      
      
        /*===========================================================================================*/

        if (Input.GetKeyDown(Aim) && !m_abilityCooldown.UnderMax() || m_isShooting)
        {
            m_targetMesh.enabled = true; // Activate target's mesh when aiming.
        }
        else if (Input.GetKeyUp(Aim))
        {
            m_targetMesh.enabled = false; // Disable target's mesh when not aiming.
        }
        // Right Bumper
        if (XCI.GetButtonDown(XboxButton.RightBumper, controller) && !m_abilityCooldown.UnderMax() || m_isShooting)
        {
            m_targetMesh.enabled = true; // Activate target's mesh when aiming.
        }
        else if (XCI.GetButtonUp(XboxButton.RightBumper, controller))
        {
            m_targetMesh.enabled = false; // Disable target's mesh when not aiming.
        }
    }

    void shootBall()
    {
        // Gets the beach ball from the Object Pool.
        GameObject BeachBall = ObjectPool.sharedInstance.GetPooledObject("Beach Ball");
        if (BeachBall != null)
        {
            // Landing position travels with the plane. Also keeps the ball from spawning within the camera's view.
            Vector3 v3LandingPos = m_targetRB.position + new Vector3(0, 20, m_targetPlaneRelation * Time.deltaTime);
            BeachBall.transform.position = v3LandingPos;

            Rigidbody rb = BeachBall.GetComponent<Rigidbody>();
            rb.velocity = BeachBall.transform.forward * 4.2f; // Keeps the velocity inline with the plane's movement forward.
            BeachBall.SetActive(true);
        }
    }

    // Used in Beachball script.
    public void toggleIsShooting(bool shooting)
    {
        m_isShooting = shooting;
    }
    // Used in Beachball script.
    public void toggleMeshEnable(bool enable)
    {
        m_targetMesh.enabled = enable;
    }
}
