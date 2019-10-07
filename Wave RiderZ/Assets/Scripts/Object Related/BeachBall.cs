/*-------------------------------------------------------------------*
|  Title:			BeachBall
|
|  Author:		    Thomas Maltezos
| 
|  Description:		Handles the beach ball's collision.
*-------------------------------------------------------------------*/

using UnityEngine;

public class BeachBall : MonoBehaviour
{
    public float radius = 5.0f;
    public float power = 10.0f;

    private GameObject Target;
    private BeachBallAbility bbAbility;
    private Rigidbody rb;
    private Vector3 reset;

    void Awake()
    {
        rb = gameObject.GetComponent<Rigidbody>();
        reset = new Vector3(0, 0, 0); // Basic reset vector.

        Target = GameObject.FindWithTag("Target"); // Will search for the target with the tag.
        if (Target != null)
        {
            bbAbility = Target.GetComponent<BeachBallAbility>(); // Will get the script from the target.
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("River")) // Will be called if collision with the river occurs.
        {
            Vector3 explosionPos = transform.position; // explosion will occur at the impact site.
            Collider[] colliders = Physics.OverlapSphere(explosionPos, radius); // List of colliders within the radius.
            foreach (Collider hit in colliders)
            {
                Rigidbody rb = hit.GetComponent<Rigidbody>(); // Will get the rigidbodies within the radius.

                if (rb != null)
                {
                    rb.AddExplosionForce(power, explosionPos, radius); // Will force each rigidbody away from the origin.
                }
            }

            gameObject.SetActive(false); // Deactivates the beachball.
            rb.velocity = reset; // Resets velocity.
            rb.angularVelocity = reset; // Resets angular velocity.
            rb.transform.rotation = Quaternion.Euler(reset); // Resets rotation.

            bbAbility.toggleIsShooting(false); // Player isn't shooting anymore.
            bbAbility.toggleMeshEnable(false); // Disabled target's mesh.
        }
    }
}
