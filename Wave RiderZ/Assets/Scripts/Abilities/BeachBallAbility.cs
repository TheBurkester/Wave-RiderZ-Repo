/*-------------------------------------------------------------------*
|  Title:			BeachBallAbility
|
|  Author:		    Thomas Maltezos
| 
|  Description:		Handles the plane's beach ball ability.
*-------------------------------------------------------------------*/
using UnityEngine;

public class BeachBallAbility : MonoBehaviour
{
    // These Keycodes will be changed later when Xbox Input is implemented.
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
    }

    void Update()
    {
        aimTarget();
    }

    void aimTarget()
    {
        // Target's movement forward in relation to the plane.
        Vector3 v3PlaneRelation = m_targetRB.position + new Vector3(m_targetPlaneRelation * Time.deltaTime, 0, 0);
        m_targetRB.transform.position = v3PlaneRelation;

        // Handles the clamp movement forward with the plane.
        m_riverClampForwardAlter = planeRB.position.x - riverClampForward;
        m_riverClampBehindAlter = planeRB.position.x - riverClampBehind * 2.5f;

        Vector3 v3 = new Vector3();

        /*===========================================================================================*/
        /*      Leave this outside of the aim If statement. Won't be called if inside.               */

        if (!m_isShooting)
        {
            if (Input.GetKey(Left) && m_targetMesh.enabled && m_targetRB.position.z < riverClampHorizontal) // Movement Left.
            {
                v3 += Vector3.forward; // Moving Left = Vector.forward due to scene direction.
            }

            if (Input.GetKey(Right) && m_targetMesh.enabled && m_targetRB.position.z > -riverClampHorizontal) // Movement Right.
            {
                v3 += Vector3.back; // Moving Right = Vector.back due to scene direction.
            }

            if (Input.GetKey(Up) && m_targetMesh.enabled && m_targetRB.position.x < m_riverClampForwardAlter) // Movement Up.
            {
                v3 += Vector3.right; // Moving Up = Vector.right due to scene direction.
            }

            if (Input.GetKey(Down) && m_targetMesh.enabled && m_targetRB.position.x > m_riverClampBehindAlter) // Movement Down.
            {
                v3 += Vector3.left; // Moving Down = Vector.left due to scene direction.
            }
        }
            // Simple transform which combines all directions and allows diagonal movement.
            m_targetRB.transform.Translate(targetMovementSpeed * v3.normalized * Time.deltaTime);

        if (Input.GetKey(Shoot) && m_targetMesh.enabled)
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
    }

    void shootBall()
    {
        // Gets the beach ball from the Object Pool.
        GameObject BeachBall = ObjectPool.sharedInstance.GetPooledObject("Beach Ball");
        if (BeachBall != null)
        {
            // Landing position travels with the plane. Also keeps the ball from spawning within the camera's view.
            Vector3 v3LandingPos = m_targetRB.position + new Vector3(m_targetPlaneRelation * Time.deltaTime, 20, 0);
            BeachBall.transform.position = v3LandingPos;

            Rigidbody rb = BeachBall.GetComponent<Rigidbody>();
            rb.velocity = BeachBall.transform.right * 4.2f; // Keeps the velocity inline with the plane's movement forward.
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
