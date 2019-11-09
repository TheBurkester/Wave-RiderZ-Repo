using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PLaneBeachballDoors : MonoBehaviour
{
    //Referance to the animator controller
    public Animator BeachBombDoorsAnim;


    // Update is called once per frame
    void Update()
    {
        //Fires Beach Bomb
        if (Input.GetKey(KeyCode.D))
        {
            BeachBombDoorsAnim.SetBool("FireBeachBomb", true);
            Debug.Log("BANG!");
        }

        //Makes sure it dosent fire
        if (Input.GetKey(KeyCode.C))
        {
            BeachBombDoorsAnim.SetBool("FireBeachBomb", false);
            Debug.Log("not firing");
        }
    }
}
