using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class inputController : ScriptableObject
{
    public abstract float RetrieveMoveInput();

    public abstract float RetrieveVerticalInput();

    public abstract bool RetrieveJumpInput();

    public abstract bool RetrieveJumpHoldInput();

    public abstract bool retrieveDashInput();


}
