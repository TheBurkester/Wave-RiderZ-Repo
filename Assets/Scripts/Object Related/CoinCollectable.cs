/*-------------------------------------------------------------------*
|  CoinCollectable
|
|  Author:			Max Atkinson
| 
|  Description:		Handles the coin rotation and trigger.
*-------------------------------------------------------------------*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0, 0, 5);
    }

    void OnTriggerEnter(Collider collider)
    {
        gameObject.GetComponent<Renderer>().enabled = false;
    }
}
