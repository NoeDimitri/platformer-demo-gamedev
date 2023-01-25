using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "playerController" , menuName = "inputController/playerController")]

public class playerController : inputController
{
    public override float RetrieveMoveInput()
    {
        return Input.GetAxisRaw("Horizontal");
    }

    public override float RetrieveVerticalInput()
    {
        return Input.GetAxisRaw("Vertical");
    }

    public override bool RetrieveJumpInput()
    {
        return Input.GetButtonDown("Jump");
        
    }
    public override bool RetrieveJumpHoldInput()
    {
        return Input.GetButton("Jump");

    }
    public override bool retrieveDashInput()
    {
        return Input.GetKey(KeyCode.LeftShift);

    }
}
