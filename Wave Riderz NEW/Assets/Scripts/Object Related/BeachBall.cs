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

    private GameObject m_Target;
    private BeachBallAbility m_bbAbility;
    private Rigidbody m_rb;
    private Vector3 m_reset;

    void Awake()
    {
        m_rb = gameObject.GetComponent<Rigidbody>();
        m_reset = new Vector3(0, 0, 0); // Basic reset vector.

        m_Target = GameObject.FindWithTag("Target"); // Will search for the target with the tag.
        if (m_Target != null)
        {
            m_bbAbility = m_Target.GetComponent<BeachBallAbility>(); // Will get the script from the target.
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("River")) // Will be called if collision with the river occurs.
        {
            Vector3 explosionPos = transform.position; // explosion will occur at the impact site.
			explosionPos.y = 0;		//Make sure that there is no y component
            Collider[] colliders = Physics.OverlapSphere(explosionPos, radius); // List of colliders within the radius.
            foreach (Collider hit in colliders)
            {
                //Rigidbody rb = hit.GetComponent<Rigidbody>(); // Will get the rigidbodies within the radius.

                //if (rb != null)
                //{
                    //rb.AddExplosionForce(power, explosionPos, radius); // Will force each rigidbody away from the origin.

                //}
				
				if (hit.CompareTag("Skier"))
				{
					Tether otherTether = hit.GetComponent<Tether>();
					Vector3 distanceToHit = hit.transform.position - explosionPos;
					distanceToHit.y = 0;    //Make sure there is no y compenent
											//float distanceToHit.magnitude		//Do a check of distance magnitude and adjust force amount here
					otherTether.forceToApply += power * distanceToHit.normalized;
					//otherTether.forceToApply += bonkForce * tether.Direction();
				}
            }

            gameObject.SetActive(false); // Deactivates the beachball.
            m_rb.velocity = m_reset; // Resets velocity.
            m_rb.angularVelocity = m_reset; // Resets angular velocity.
            m_rb.transform.rotation = Quaternion.Euler(m_reset); // Resets rotation.

            m_bbAbility.toggleIsShooting(false); // Player isn't shooting anymore.
            m_bbAbility.toggleMeshEnable(false); // Disabled target's mesh.
        }
    }
}
