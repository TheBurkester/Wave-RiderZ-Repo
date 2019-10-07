/*-------------------------------------------------------------------*
|  Title:			BeachBallAbility
|
|  Author:		    Thomas Maltezos
| 
|  Description:		Handles the plane's beach ball ability.
*-------------------------------------------------------------------*/

using System.Collections;
using System.Collections.Generic;
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
    public float Cooldown = 5.0f; // Replace with timer script functions at a later date.
    public float targetMovementSpeed = 10.0f; // Target's movement speed when aiming.

    public static MeshRenderer targetMesh; // Target's Mesh.
    private Rigidbody targetRB; // Target's Rigidbody.
    private float targetPlaneRelation = 5.0f; // Moves the target along with the plane and camera. KEEP VARIABLE THE SAME AS PLANE SPEED IN PLANE CONTROLLER.
    private GameObject prefab; // Beachball prefab.
    public static bool isShooting = false; // Has the player pressed the shoot button?

    void Awake()
    {
        targetRB = gameObject.GetComponent<Rigidbody>();
        targetMesh = gameObject.GetComponent<MeshRenderer>();
        targetMesh.enabled = false; // Target's mesh is disabled on startup.

        prefab = Resources.Load("BeachBall") as GameObject;
    }

    void Update()
    {
        aimTarget();
    }

    void aimTarget()
    {
        // Target's movement forward in relation to the plane.
        Vector3 v3PlaneRelation = targetRB.position + new Vector3(targetPlaneRelation * Time.deltaTime, 0, 0);
        targetRB.transform.position = v3PlaneRelation;

        Vector3 v3 = new Vector3();

        /*===========================================================================================*/
        /*      Leave this outside of the aim If statement. Won't be called if inside.               */

        if (!isShooting)
        {
            if (Input.GetKey(Left) && targetMesh.enabled) // Movement Left.
            {
                v3 += Vector3.forward; // Moving Left = Vector.forward due to scene direction.
            }

            if (Input.GetKey(Right) && targetMesh.enabled) // Movement Right.
            {
                v3 += Vector3.back; // Moving Right = Vector.back due to scene direction.
            }

            if (Input.GetKey(Up) && targetMesh.enabled) // Movement Up.
            {
                v3 += Vector3.right; // Moving Up = Vector.right due to scene direction.
            }

            if (Input.GetKey(Down) && targetMesh.enabled) // Movement Down.
            {
                v3 += Vector3.left; // Moving Down = Vector.left due to scene direction.
            }
        }
        // Simple transform which combines all directions and allows diagonal movement.
        targetRB.transform.Translate(targetMovementSpeed * v3.normalized * Time.deltaTime);

        if (Input.GetKey(Shoot) && targetMesh.enabled)
        {
            isShooting = true;
            shootBall(); // Shoots the beachball.
            targetMesh.enabled = true;
        }
        /*===========================================================================================*/

        if (Input.GetKeyDown(Aim) || isShooting)
        {
            targetMesh.enabled = true; // Activate target's mesh when aiming.
        }
        else if (Input.GetKeyUp(Aim))
        {
            targetMesh.enabled = false; // Disable target's mesh when not aiming.
        }
    }

    void shootBall()
    {
        // This will break if more than one object pool is used, will need to change the class.
        GameObject BeachBall = ObjectPool.sharedInstance.GetPooledObject();
        if (BeachBall != null)
        {
            // Landing position travels with the plane. Also keeps the ball from spawning within the camera's view.
            Vector3 v3LandingPos = targetRB.position + new Vector3(targetPlaneRelation * Time.deltaTime, 20, 0);
            BeachBall.transform.position = v3LandingPos;

            Rigidbody rb = BeachBall.GetComponent<Rigidbody>();
            rb.velocity = BeachBall.transform.right * 4; // Keeps the velocity inline with the plane's movement forward.
            BeachBall.SetActive(true);
        }
    }
}
