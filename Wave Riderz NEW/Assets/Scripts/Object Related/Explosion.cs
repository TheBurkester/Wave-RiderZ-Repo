/*-------------------------------------------------------------------*
|  Title:			Explosion
|
|  Author:		    Seth Johnston
| 
|  Description:		Makes a sphere look like an explosion.
*-------------------------------------------------------------------*/

using UnityEngine;

public class Explosion : MonoBehaviour
{
    void Update()
    {
		//Bigger over time
		float sizeIncrease = Time.deltaTime * 20;										//Calculate how much to increase size by per frame
		transform.localScale += new Vector3(sizeIncrease, sizeIncrease, sizeIncrease);	//Increase the size

		//Fade over time
		Renderer renderer = GetComponent<Renderer>();
		Color color = renderer.material.color;
		if (color.a - Time.deltaTime * 3 > 0)	//If decreasing the alpha is still above 0,
		{
			color.a -= Time.deltaTime * 3;		//Decrease the alpha
			renderer.material.color = color;	//Set the colour
		}
		
		if (transform.localScale.x > 10)	//When the explosion is big enough,
			Destroy(gameObject);			//Delete itself
    }
}
