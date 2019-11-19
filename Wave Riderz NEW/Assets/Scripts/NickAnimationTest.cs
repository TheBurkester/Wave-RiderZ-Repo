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
        float axisX = XCI.GetAxis(XboxAxis.LeftStickX, controller);
        float axisY = XCI.GetAxis(XboxAxis.LeftStickY, controller);
        if (axisX >= 0.4)
        {
            characterAnim.SetBool("MovingLeft", false);
            characterAnim.SetBool("MovingRight", true);
        }
        else if (axisX <= -0.4)
        {
            characterAnim.SetBool("MovingRight", false);
            characterAnim.SetBool("MovingLeft", true);
        }
        else
        {
            characterAnim.SetBool("MovingRight", false);
            characterAnim.SetBool("MovingLeft", false);
        }
        if (axisY >= 0.4)
        {
            characterAnim.SetBool("MovingForward", true);
            characterAnim.SetBool("MovingBackward", false);
        }
        else if (axisY <= -0.4)
        {
            characterAnim.SetBool("MovingForward", false);
            characterAnim.SetBool("MovingBackward", true);
        }
        else
        {
            characterAnim.SetBool("MovingForward", false);
            characterAnim.SetBool("MovingBackward", false);
        }

    }
    void damageCharacter()
    {
       if (Input.GetKeyDown(KeyCode.A))
        {
            characterAnim.SetTrigger("DamageTaken");
            // characterAnim.SetBool("IsDamaged", false);
            characterAnim.SetInteger("Lives", characterAnim.GetInteger("Lives") - 1); 

        }
       
    }
}
