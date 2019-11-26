/*-------------------------------------------------------------------*
|  Title:			Mine
|
|  Author:		    Thomas Maltezos / Seth Johnston
| 
|  Description:		Handles the mine's collision.
*-------------------------------------------------------------------*/

using UnityEngine;
using System.Collections;
using XInputDotNetPure;

public class Mine : MonoBehaviour
{
	public float radius = 5.0f;
	public float freezeAmount = 0.05f;	//How long to freeze the game when the beachbomb pops
	public int freezeFrames = 20;       //The minimum amount of frames to freeze for

	private TetheredMineAbility m_tmAbility;
	private Rigidbody m_rb;
	private Vector3 m_velocity;
	private bool m_hitWater = false;

    private Tether m_tether;	//Reference to the tether the mine is connected to

	public GameObject explosionPrefab = null;   //Reference to the explosion effect to play when popping

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

	void Update()
	{
		if (transform.position.y > 0)						//If the mine hasn't hit the river yet,
		{
			m_velocity.y -= Time.deltaTime * 0.1f;			//Accelerate the mine downwards
			m_velocity.z = (m_tmAbility.planeSpeed * Time.deltaTime) - (3 * Time.deltaTime);	//Mine moves along with the plane, but slightly slower
			transform.Translate(m_velocity);				//Move the mine based on velocity
			m_tether.currentLength = m_tether.Distance();	//Set the tether length directly equal to the current distance from the tether point
		}
		else if (transform.position.y <= 0 && !m_hitWater)	//On the first frame of the mine hitting the water,
		{
			m_tether.tetherActive = true;					//Activate the tether
			transform.position = new Vector3(transform.position.x, 0, transform.position.z);	//Ensure the mine is on y=0
			m_tether.currentLength = m_tether.Distance();	//Ensure the mine is at the right starting length
			m_hitWater = true;
		}
		if (m_hitWater && m_tether.currentLength < 10)							//If the mine is on the water and the tether isn't fully extended yet,
			m_tether.currentLength += m_tether.changeSpeed * Time.deltaTime;	//Extend the tether a little bit
	}

	void OnTriggerEnter(Collider collision)
	{
        if (collision.CompareTag("Skier")) // Will be called if collision with a skier occurs.
        {

            Vector3 explosionPos = transform.position;  // explosion will occur at the impact site.
            explosionPos.y = 0;                         //Make sure that there is no y component
            Collider[] colliders = Physics.OverlapSphere(explosionPos, radius); // List of colliders within the radius.
                // animation for the door 
            TetheredMineAnimation.SetBool("IsDoorClosed", true);
            TetheredMineAnimation.SetBool("IsDoorOpen", false);
            // Hatch Close Door sound
            AudioManager.Play("TetherObs&BBHatchDoorClosed");
            AudioManager.Play("TetheredMineExplosion");
            foreach (Collider hit in colliders)                                     //For all the objects in the radius,
            {
                if (hit.CompareTag("Skier"))                                        //If this object is a skier,
                {
                    SkierController controller = hit.GetComponent<SkierController>(); // Gets all controllers within the radius.
                    if (!controller.IsInvincible())
                        controller.HurtSkier(); // Hurts the skier within the radius.
                }
            }

			ControllerVibrate.VibrateAll(1.0f, 0.5f);	//Vibrate all controllers very moderately
            gameObject.SetActive(false); // Deactivates the mine.
            m_rb.velocity = Vector3.zero; // Resets velocity.
            m_rb.angularVelocity = Vector3.zero; // Resets angular velocity.
            m_rb.transform.rotation = Quaternion.Euler(Vector3.zero); // Resets rotation.
            m_tmAbility.setIsUsingAbility(false);
            m_tmAbility.mineAbilityCooldown.SetTimer();
           
            explosionPrefab.transform.position = explosionPos;	//Make the explosion happen at the right spot
			Instantiate(explosionPrefab);                       //Create the explosion

			GameFreezer.Freeze(freezeAmount, freezeFrames);     //Slow time very briefly for impact
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

	public void Reset()
	{
		m_tether.ResetVelocity();
		m_tether.currentLength = m_tether.Distance();
		m_tether.minLength = 1;
		m_tether.maxLength = 10;
		m_tether.changeSpeed = 1;
		m_tether.tetherActive = false;
		m_tether.GetComponentInChildren<LineRenderer>().enabled = false;
		StartCoroutine(TurnRopeOn());

		m_velocity = Vector3.zero;
		m_hitWater = false;
	}

	IEnumerator TurnRopeOn()
	{
		yield return new WaitForEndOfFrame();
		m_tether.GetComponentInChildren<LineRenderer>().enabled = true;
	}
}
