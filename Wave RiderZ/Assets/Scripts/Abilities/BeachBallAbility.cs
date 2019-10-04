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
    public float Cooldown = 5.0f; // Replace with timer script functions at a later date.
    public float targetMovementSpeed = 10.0f; // Target's movement speed when aiming.

    private MeshRenderer targetMesh; // Target's Mesh.
    private Rigidbody targetRB; // Target's Rigidbody.
    private float targetPlaneRelation = 5.0f; // Moves the target along with the plane and camera. KEEP VARIABLE THE SAME AS PLANE SPEED IN PLANE CONTROLLER.

    void Awake()
    {
        targetRB = gameObject.GetComponent<Rigidbody>();
        targetMesh = gameObject.GetComponent<MeshRenderer>();
        targetMesh.enabled = false; // Target's mesh is disabled on startup.
    }

    void Update()
    {
        Vector3 v3PlaneRelation = targetRB.position + new Vector3(targetPlaneRelation * Time.deltaTime, 0, 0); // Target's movement forward in relation to the plane.
        targetRB.transform.position = v3PlaneRelation;


        Vector3 v3 = new Vector3();

        /*===========================================================================================*/
        /*      Leave this outside of the aim If statement. Won't be called if inside.               */

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

        targetRB.transform.Translate(targetMovementSpeed * v3.normalized * Time.deltaTime);
        /*===========================================================================================*/

        if (Input.GetKeyDown(Aim))
        {
            targetMesh.enabled = true; // Activate target's mesh when aiming.

        }
        else if (Input.GetKeyUp(Aim))
        {
            targetMesh.enabled = false; // Disable target's mesh when not aiming.
        }
    }
}
