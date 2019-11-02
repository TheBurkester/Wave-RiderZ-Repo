using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

	}

    // Update is called once per frame
    void Update()
    {
		float sizeIncrease = Time.deltaTime * 20;
		transform.localScale += new Vector3(sizeIncrease, sizeIncrease, sizeIncrease);

		Renderer renderer = GetComponent<Renderer>();
		Color color = renderer.material.color;
		if (color.a - Time.deltaTime * 3 > 0)
		{
			color.a -= Time.deltaTime * 3;
			renderer.material.color = color;
		}
		
		if (transform.localScale.x > 10)
			Destroy(gameObject);
    }
}
