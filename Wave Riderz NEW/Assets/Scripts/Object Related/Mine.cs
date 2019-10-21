/*-------------------------------------------------------------------*
|  Title:			Mine
|
|  Author:		    Thomas Maltezos
| 
|  Description:		Handles the mine's collision.
*-------------------------------------------------------------------*/

using UnityEngine;

public class Mine : MonoBehaviour
{
	public float radius = 5.0f;

	private TetheredMineAbility m_tmAbility;
	private Rigidbody m_rb;

	void Awake()
	{
		m_rb = gameObject.GetComponent<Rigidbody>();

		GameObject hatch = GameObject.FindWithTag("PlaneHatch"); // Will search for the hatch with the tag.
		if (hatch != null)
			m_tmAbility = hatch.GetComponent<TetheredMineAbility>(); // Will get the script from the hatch.
	}

	void OnTriggerEnter(Collider collision)
	{
		if (collision.gameObject.CompareTag("Skier")) // Will be called if collision with the river occurs.
		{
			Vector3 explosionPos = transform.position;  // explosion will occur at the impact site.
			explosionPos.y = 0;                         //Make sure that there is no y component
			Collider[] colliders = Physics.OverlapSphere(explosionPos, radius); // List of colliders within the radius.

			foreach (Collider hit in colliders)                                     //For all the objects in the radius,
			{
				if (hit.CompareTag("Skier"))                                        //If this object is a skier,
				{
					SkierController controller = hit.GetComponent<SkierController>(); // Gets all controllers within the radius.

					if (!controller.isInvincible())
						controller.HurtSkier(); // Hurts the skier within the radius.
				}
			}

			gameObject.SetActive(false); // Deactivates the beachball.
			m_rb.velocity = Vector3.zero; // Resets velocity.
			m_rb.angularVelocity = Vector3.zero; // Resets angular velocity.
			m_rb.transform.rotation = Quaternion.Euler(Vector3.zero); // Resets rotation.
			m_tmAbility.setIsUsingAbility(false);
		}
	}
}
