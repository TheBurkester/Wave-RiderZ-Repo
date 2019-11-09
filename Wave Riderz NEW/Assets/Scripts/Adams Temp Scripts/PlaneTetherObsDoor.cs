using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaneTetherObsDoor : MonoBehaviour
{
    //Referance to the animator controller
    public Animator TetherObsDoorAnim;
    
    
    // Update is called once per frame
    void Update()
    {
        //Opens Mine back door
        if (Input.GetKey(KeyCode.A))
        {
            TetherObsDoorAnim.SetBool("DeployMine", true);
            Debug.Log("open!");
        }
        if (Input.GetKey(KeyCode.Z))
        {
            TetherObsDoorAnim.SetBool("DeployMine", false);
            Debug.Log("closed!");
        }


    }
}
