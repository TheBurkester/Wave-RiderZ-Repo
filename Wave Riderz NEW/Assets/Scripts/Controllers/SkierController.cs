 /*-------------------------------------------------------------------*
|  Title:			SkierController
|
|  Author:			Seth Johnston / Thomas Maltezos
| 
|  Description:		Handles the skier's movement.
*-------------------------------------------------------------------*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XboxCtrlrInput;
using System;
using XInputDotNetPure;

public class SkierController : MonoBehaviour
{
	//Movement
	public XboxController controller;	//Reference to the in-scene assigned controller
	public float movingForce = 50;   //How fast the skier moves sideways

	//Bonking
	public float bonkForce = 100;				//How much force to always apply when bonking other skiers
	public float bonkVelocityForce = 75;		//How much maximum additional force to apply based on how fast this skier is moving
	public float bonkForceDuration = 0.5f;		//How long bonking forces are applied
	private float maxXVelocity = 8;				//The maximum reachable velocity of the skiers, based on testing, used for proportioning the bonk velocity transfer
	public float obstacleForce = 100;			//How much sidewards force is applied when hitting an obstacle
	public float obstacleForceDuration = 0.5f;  //How long obstacle forces are applied
	[HideInInspector]
	public bool bonkResolved = false;						//If another skier has pushed this skier this frame already
	public GameObject bonkParticle = null;					//Reference to the bonk particle prefab
	private ParticleSystem[] m_bonkParticles = null;		//All actual particles of the prefab
	public GameObject obstacleParticle = null;				//Reference to the obstacle bonk particle prefab
	private ParticleSystem[] m_obstacleParticles = null;	//All actual particles of the prefab

    public Animator characterAnim;

	//Keyboard controls
	//public KeyCode MoveLeft;		//Which keyboard key moves the skier left
    //public KeyCode MoveRight;       //Which keyboard key moves the skier right
	//public KeyCode TetherLengthen;	//Which keyboard key lengthens the rope
	//public KeyCode TetherShorten;	//Which keyboard key shortens the rope

	//Score/lives
	private int m_score = 0;			// Player's score.
	public int coinScore = 2;			// Score increased everytime collision with a coin occurs.
	public int skierScoreInc = 1;		// Base Increase every second.
    public int skierMultiplierSpeed = 5;	// The time it takes for the skier's multiplier to increase.
	public int skierMultiplierCap = 5;		// The max value that the multipler can be.
	private int m_skierMultiplier = 1;		// Skier's current multiplier.
    public GameObject coinCollectParticle = null;	//The coin collection particle prefab
    private ParticleSystem[] m_coinParticles = null;    //The actual particle systems of the prefab's children
	public GameObject topScoreParticle = null;  //Gold particle to play when this player is in the lead
	private ParticleSystem m_topScoreParticle = null;   //Actual particle system of the prefab

	private Timer m_scoreTimer;				// Timer used to increment score.
	private Timer m_scoreMultiplierTimer;	// Timer used to add the multiplier to the score over time.
	public int lives = 3;				// The amount of lives the skier has
	private bool m_isAlive = false;			//If the skier has the will to live

	//Invincibility
	public int numberOfFlashes = 3;			//How many times the mesh should flash when damaged
	public float flashDelay = 0.3f;			//How fast the mesh should flash on and off when damaged
	private bool m_invincible = false;		//If the skier can collide with other objects
	public SkinnedMeshRenderer meshRend = null;	//Reference to the mesh to be flashed during invincibility

	[HideInInspector]
	public Tether tether = null;    //Reference to the tether attached to this skier, public so forces can be applied from other scripts
	[HideInInspector]
	public bool hurtThisFrame = false;

	//Easter Egg
	private XboxButton[] m_sequence;
	private int m_progress;
	private bool m_eggUnlocked = false;

	void Awake()
    {
		tether = GetComponent<Tether>();
		Debug.Assert(tether != null, "Skier missing tether component");

		m_coinParticles = coinCollectParticle.GetComponentsInChildren<ParticleSystem>();
		foreach (ParticleSystem p in m_coinParticles)
			p.Stop();   //Make sure the particle doesn't play immediately, has to be in awake

		m_topScoreParticle = topScoreParticle.GetComponentInChildren<ParticleSystem>();
		m_topScoreParticle.Stop();

		bonkParticle = Instantiate(bonkParticle, transform);
		m_bonkParticles = bonkParticle.GetComponentsInChildren<ParticleSystem>();
		foreach (ParticleSystem p in m_bonkParticles)
			p.Stop();   //Make sure the particle doesn't play immediately, has to be in awake

		obstacleParticle = Instantiate(obstacleParticle, transform);
		m_obstacleParticles = obstacleParticle.GetComponentsInChildren<ParticleSystem>();
		foreach (ParticleSystem p in m_obstacleParticles)
			p.Stop();   //Make sure the particle doesn't play immediately, has to be in awake

		m_sequence = new XboxButton[6] { (XboxButton)1, (XboxButton)2, (XboxButton)3, (XboxButton)3, (XboxButton)2, 0 };
	}

	void Start()
	{
		m_scoreMultiplierTimer = gameObject.AddComponent<Timer>();  //Create the timer
		m_scoreMultiplierTimer.maxTime = skierMultiplierSpeed;      //Set how often the multiplier increases
		m_scoreMultiplierTimer.autoRepeat = true;                   //Make it repeat on its own
		m_scoreMultiplierTimer.SetRepeatFunction(AddMultiplier);    //Make the timer call the AddMultiplier function each time it repeats
		m_scoreMultiplierTimer.SetTimer();							//Start the timer
          
		m_scoreTimer = gameObject.AddComponent<Timer>();	//Create the score timer
		m_scoreTimer.maxTime = 1;							//The timer will go for one second,
		m_scoreTimer.autoRepeat = true;                     //Then automatically repeat
		m_scoreTimer.SetRepeatFunction(IncrementScore);		//Make the timer call the IncrementScore function each time it repeats
        m_scoreTimer.SetTimer();                            //Start the timer
	}

	private void Update()
    {
		if (Input.GetKeyDown(KeyCode.N))
		{
			for (int i = 0; i < 4; ++i)
			{
				GamePad.SetVibration((PlayerIndex)i, 0.2f, 0.2f);
			}
		}
		if (Input.GetKeyDown(KeyCode.L))
		{
			for (int i = 0; i < 4; ++i)
			{
				GamePad.SetVibration((PlayerIndex)i, 1, 1);
			}
		}

		//KEYBOARD
		//---------------------------------------------
		//Tether movement
		//if (Input.GetKey(TetherLengthen))									//If pressing the lengthen key,
		//	tether.currentLength += tether.changeSpeed * Time.deltaTime;	//Make the tether longer over time
		//if (Input.GetKey(TetherShorten))									//If pressing the shorten key,
		//	tether.currentLength -= tether.changeSpeed * Time.deltaTime;	//Make the tether shorter over time

		////Sideways movement
		//if (tether.Distance() >= (tether.currentLength * 0.95))	//As long as the skier is close to the arc of the tether,
		//{
		//	if (Input.GetKey(MoveRight))                //If the right key is pressed,
		//		tether.ApplyForce(new Vector3(movingForce, 0, 0));
		//	if (Input.GetKey(MoveLeft))                 //If the left key is pressed,
		//		tether.ApplyForce(new Vector3(-movingForce, 0, 0));
		//}
		//---------------------------------------------

		//XBOX
		//---------------------------------------------
		//Tether movement
		float axisY = XCI.GetAxis(XboxAxis.LeftStickY, controller);				//Store the direction and magnitude of the left joystick Y axis
		tether.currentLength += tether.changeSpeed * -axisY * Time.deltaTime;   //Move tether length up/down based on this

        // CHARACTER ANIMATION 
       //if the control stick is pushed 40% of the way upwards
        if (axisY >= 0.4)
        {
            //sets the animation state to moving forward
            characterAnim.SetBool("MovingForward", true);
            characterAnim.SetBool("MovingBackward", false);
        }
        //if the control stick is pushed 40% of the way downwards
        else if (axisY <= -0.4)
        {
            //sets the animation state to moving backwards
            characterAnim.SetBool("MovingForward", false);
            characterAnim.SetBool("MovingBackward", true);
        }
        //if neither
        else
        {
            //sets the animation state to idle
            characterAnim.SetBool("MovingForward", false);
            characterAnim.SetBool("MovingBackward", false);
        }

       
        //------------------------------------------


        //Sideways movement
        if (tether.Distance() >= (tether.currentLength * 0.95f))		//As long as the skier is close to the arc of the tether,
		{
			float axisX = XCI.GetAxis(XboxAxis.LeftStickX, controller);	//Store the direction and magnitude of the left joystick X axis
			tether.ApplyForce(new Vector3(movingForce * axisX, 0, 0));  //Apply an instantaneous sideways force
                                                                        //if the control stick is pushed 40% of the way to the right
          // CHARACTER ANIMATION 
            if (axisX >= 0.4)
            {
                //sets the animation state to moving  right
                characterAnim.SetBool("MovingLeft", false);
                characterAnim.SetBool("MovingRight", true);
            }
            //if the control stick is pushed 40% of the way to the left
            else if (axisX <= -0.4)
            {
                //sets the animation state to moving left
                characterAnim.SetBool("MovingRight", false);
                characterAnim.SetBool("MovingLeft", true);
            }
            //if neither
            else
            {
                //sets the animation state to idle
                characterAnim.SetBool("MovingRight", false);
                characterAnim.SetBool("MovingLeft", false);
            }
        }

		//Easter egg
		if (!m_eggUnlocked)
		{
			for (XboxButton i = 0; i <= XboxButton.Y; ++i)
			{
				if (XCI.GetButtonDown(i))
				{
					if (i == m_sequence[m_progress])
					{
						++m_progress;
						if (m_sequence[m_progress] == 0)
							m_eggUnlocked = true;
					}
					else
						m_progress = 0;
				}
			}
		}
		else
		{
			if (XCI.GetButtonDown(XboxButton.A))
				tether.ApplyForce(new Vector3(0, 500, 0));
		}
		//---------------------------------------------
		
		//Wipeout
		if (m_isAlive == false)
		{
			//Make them sink
			Vector3 newPos = transform.position;
			newPos.y -= Time.deltaTime;
			transform.position = newPos;

			//Make their tether fade
			RopeLine renderedTether = GetComponentInChildren<RopeLine>();
			if (renderedTether != null)
				renderedTether.Fade(1);
		}

		bonkResolved = false;
	}

	private void OnTriggerEnter(Collider other)
	{
		if (!m_invincible)					//If the skier is not currently invincible,
		{
            if (other.CompareTag("Coin"))   //If the other object is a coin,
            {
                m_score += coinScore;       //Add a coin's worth of points to the score
                foreach (ParticleSystem p in m_coinParticles)
                    p.Play();
            }

            if (other.CompareTag("Rock"))   //If the other object is a rock,
            {
                HurtSkier();//Hurt the skier
                AudioManager.Play("PLayerHitObstacle"); // plays sound when hit obstacle 
            }
		}

		if (other.CompareTag("Skier") && !bonkResolved)				//If the other object is a skier and this collision hasn't been resolved yet,
		{
			Tether otherTether = other.GetComponent<Tether>();		//Get the other skier's tether

			if (tether.VelocityMagnitude() >= otherTether.VelocityMagnitude())	//If this skier is moving faster than the other skier,
			{
				float velocityForce = (tether.VelocityXMagnitude() / maxXVelocity) * bonkVelocityForce;	//Proportion the force based on how close to max velocity this skier is
				Vector3 totalBonkForce = (bonkForce + velocityForce) * tether.Direction();				//Add the flat force and velocity-dependent force, then point them in the direction of movement
				otherTether.ForceOverTime(totalBonkForce, bonkForceDuration);							//Apply the final force to the other skier
				tether.ReduceVelocity(2);																//Halve the velocity of this skier
				other.GetComponent<SkierController>().bonkResolved = true;								//For this frame, set the collision as resolved
                AudioManager.Play("Bonk3"); // plays bonk sound effect
				other.GetComponent<SkierController>().PlayBonkParticle(tether.Direction());				//Play the bonk particle on the other skier in the direction they are pushed
			}
		}

		if (other.CompareTag("Rock"))	//Regardless of if invincible or not, if colliding with an obstacle,
		{
			float pushDirection = transform.position.x - other.transform.position.x;			//Calculate if the skier should be pushed left or right
			if (pushDirection > 0)																//If positive,
				tether.ForceOverTime(new Vector3(obstacleForce, 0, 0), obstacleForceDuration);	//Push right
			else																				//If negative,
				tether.ForceOverTime(new Vector3(-obstacleForce, 0, 0), obstacleForceDuration); //Push left
		}

		if (other.CompareTag("River") && m_isAlive)	//If the skier somehow touches the river,
		{
			//Reset their position and velocity
			tether.ResetVelocity();
			transform.position = new Vector3(transform.position.x, 0.0f, transform.position.z);
		}
	}
	
	//Hurts the skier and checks their lives
	public void HurtSkier()
	{
		if (!m_invincible)  //Double checking that they aren't invincible,
		{
			lives--;   //Hurt the skier
			hurtThisFrame = true;
			StartCoroutine(HurtOff());
            //sets DamageTaken trigger in animator
            characterAnim.SetTrigger("DamageTaken");
			ControllerVibrate.VibrateController((int)controller - 1, 0.025f, 0.15f);
			foreach (ParticleSystem p in m_obstacleParticles)
				p.Play();
		}

		if (lives <= 0)				//If the skier is out of lives,
		{
            characterAnim.SetBool("IsDead", true);
            m_isAlive = false;				//He dead
			tether.enabled = false;			//Turn movement off
			m_scoreTimer.enabled = false;	//Stop the score from increasing
            AudioManager.Play("Wipeout");   // plays the wipeout sound effect 
		}
		else										//If the skier is still alive,
		{
			m_invincible = true;					//Make it invincible
            m_skierMultiplier = 1;					// Multiplier is reset back to 1.
			m_scoreMultiplierTimer.enabled = false; // Resets the timer before the flashes start so they don't have the multiplier increase during thier invincibility.

			//Flash the skier mesh
			for (int i = 0; i < numberOfFlashes; ++i)                       //Repeating for the number of flashes,
			{
				StartCoroutine(MeshOff(flashDelay * i * 2));                //Schedule the mesh to turn off, every even interval
				StartCoroutine(MeshOn(flashDelay * i * 2 + flashDelay));    //Schedule the mesh to turn on, every odd interval
			}
			StartCoroutine(InvincibleOff(flashDelay * numberOfFlashes * 2));    //Schedule invincibility to turn off after the flashes are complete
		}
	}

	//Does what it says
	public void PlayBonkParticle(Vector3 direction)
	{
		bonkParticle.transform.forward = direction;		//Orients the particle parent to face the direction specified
		foreach (ParticleSystem p in m_bonkParticles)	//For all child particles,
			p.Play();									//Play them
	}

	public void SetScore(int score)
	{
		m_score = score;
	}
	public void AddScore(int score)
	{
		m_score += score;
	}
	public int GetScore()
	{
		return m_score;
	}

	public int GetPlayerMultiplier()
	{
		return m_skierMultiplier;
	}

	//Increments the score based on preset amount and score multiplier
	public void IncrementScore()
	{
		m_score += skierScoreInc * m_skierMultiplier;
	}

	//Makes the multiplier go up by one
	public void AddMultiplier()
	{
		if (m_skierMultiplier < skierMultiplierCap)
			m_skierMultiplier++;
	}

	//To set the skier to wiped out if not in the game
	public void SetAlive(bool value)
	{
		m_isAlive = value;
	}
	public bool GetAlive()
	{
		return m_isAlive;
	}

	public bool IsInvincible()
	{
		return m_invincible;
	}

	//Turns the gold particle on/off
	public void SetTopScoreParticle(bool value)
	{
		if (value && m_topScoreParticle.isStopped)
			m_topScoreParticle.Play();
		else if (value == false && m_topScoreParticle.isPlaying)
			m_topScoreParticle.Stop();
	}
	
	IEnumerator MeshOff(float interval)
	{
		yield return new WaitForSeconds(interval);  //Wait for a certain amount of time
        meshRend.enabled = false;					//Turn the mesh off
	}
	IEnumerator MeshOn(float interval)
	{
		yield return new WaitForSeconds(interval);  //Wait for a certain amount of time
        meshRend.enabled = true;					//Turn the mesh on
	}
	IEnumerator InvincibleOff(float interval)
	{
		yield return new WaitForSeconds(interval);  //Wait for a certain amount of time
		m_invincible = false;                       //Make vincible
		m_scoreMultiplierTimer.SetTimer();			//Restart the multiplier timer
	}
	IEnumerator HurtOff()
	{
		yield return new WaitForEndOfFrame();
		hurtThisFrame = false;
	}
}
