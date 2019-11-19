using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XboxCtrlrInput;

public class NickAnimationTest : MonoBehaviour
{
    public Animator characterAnim;
    public XboxController controller;


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        tiltCharacter();
        damageCharacter();
    }

    void tiltCharacter()
    {
        //gets references to the left stick input of the controller
        float axisX = XCI.GetAxis(XboxAxis.LeftStickX, controller);
        float axisY = XCI.GetAxis(XboxAxis.LeftStickY, controller);

        //if the control stick is pushed 40% of the way to the right
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

    }

    void damageCharacter()
    {
        //if condition is met
       if (Input.GetKeyDown(KeyCode.A))
        {
            //sets DamageTaken trigger in animator
            characterAnim.SetTrigger("DamageTaken");
            //reduces current lives counter in animator by 1
            characterAnim.SetInteger("Lives", characterAnim.GetInteger("Lives") - 1); 

        }
       
    }
}
