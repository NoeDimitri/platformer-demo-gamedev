using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "aiController", menuName = "inputController/aiController")]

public class aiController : inputController
{
    public override float RetrieveMoveInput()
    {
        return 1f;
    }

    public override bool RetrieveJumpInput()
    {
        return true;

    }
}
