/*-------------------------------------------------------------------*
|  Title:			BeachBall
|
|  Author:		    Thomas Maltezos / Seth Johnston
| 
|  Description:		Handles the beach ball's collision.
*-------------------------------------------------------------------*/

using UnityEngine;

public class BeachBall : MonoBehaviour
{	
    private BeachBallAbility m_bbAbility;
    private Rigidbody m_rb;

    void Awake()
    {
        m_rb = gameObject.GetComponent<Rigidbody>();

        GameObject target = GameObject.FindWithTag("Target"); // Will search for the target with the tag.
        if (target != null)
            m_bbAbility = target.GetComponent<BeachBallAbility>(); // Will get the script from the target.
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("River")) // Will be called if collision with the river occurs.
        {
            Vector3 explosionPos = transform.position;	// explosion will occur at the impact site.
			explosionPos.y = 0;							//Make sure that there is no y component
            Collider[] colliders = Physics.OverlapSphere(explosionPos, m_bbAbility.getRadius()); // List of colliders within the radius.

            foreach (Collider hit in colliders)										//For all the objects in the radius,
            {
				if (hit.CompareTag("Skier"))										//If this object is a skier,
				{
					Tether tether = hit.GetComponent<Tether>();						//Get their tether
					Vector3 distanceToHit = hit.transform.position - explosionPos;	//Get the difference in position between the skier and the explosion point
					distanceToHit.y = 0;											//Make sure there is no y compenent
					//float distanceToHit.magnitude		*Do a check of distance magnitude and adjust force amount here*
					tether.ForceOverTime(m_bbAbility.getPower() * distanceToHit.normalized, m_bbAbility.forceDuration); //Add a force on the skier, pushing away from the explosion point
				}
            }

            gameObject.SetActive(false); // Deactivates the beachball.
            m_rb.velocity = Vector3.zero; // Resets velocity.
            m_rb.angularVelocity = Vector3.zero; // Resets angular velocity.
            m_rb.transform.rotation = Quaternion.Euler(Vector3.zero); // Resets rotation.

            m_bbAbility.ToggleIsShooting(false); // Player isn't shooting anymore.
            m_bbAbility.ToggleMeshEnable(false); // Disabled target's mesh.
        }
    }
}
