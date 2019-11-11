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
	public float freezeAmount = 0.05f;
	public int freezeFrames = 20;

	private TetheredMineAbility m_tmAbility;
	private Rigidbody m_rb;

    private Tether m_tether;

	public GameObject explosionPrefab = null;

    public Animator TetheredMineAnimation;

    void Awake()
	{
		m_rb = gameObject.GetComponent<Rigidbody>();

		GameObject hatch = GameObject.FindWithTag("PlaneHatch"); // Will search for the hatch with the tag.
		if (hatch != null)
			m_tmAbility = hatch.GetComponent<TetheredMineAbility>(); // Will get the script from the hatch.

        m_tether = GetComponent<Tether>();

		Debug.Assert(explosionPrefab != null, "The explosion prefab hasn't been added to the mine script");
	}

	void OnTriggerEnter(Collider collision)
	{
        if (collision.CompareTag("Skier")) // Will be called if collision with a skier occurs.
        {

            Vector3 explosionPos = transform.position;  // explosion will occur at the impact site.
            explosionPos.y = 0;                         //Make sure that there is no y component
            Collider[] colliders = Physics.OverlapSphere(explosionPos, radius); // List of colliders within the radius.
             TetheredMineAnimation.SetBool("IsDoorClosed", true);
            TetheredMineAnimation.SetBool("IsDoorOpen", false);

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
            m_tmAbility.mineAbilityCooldown.SetTimer();

			explosionPrefab.transform.position = explosionPos;
			Instantiate(explosionPrefab);

			GameFreezer.Freeze(freezeAmount, freezeFrames);
		}
        else if (collision.CompareTag("Rock"))	//If colliding with an obstacle,
        {
            float pushDirection = transform.position.x - collision.transform.position.x;	//Calculate if the mine should be pushed left or right
            if (pushDirection > 0)																							//If positive,
                m_tether.ForceOverTime(new Vector3(m_tmAbility.obstacleForce, 0, 0), m_tmAbility.obstacleForceDuration);	//Push right
            else																											//If negative,
                m_tether.ForceOverTime(new Vector3(-m_tmAbility.obstacleForce, 0, 0), m_tmAbility.obstacleForceDuration);	//Push left
        }
	}
}
