using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class jumpIndicator : MonoBehaviour
{
    [SerializeField] private Image spacebarImage;
    [SerializeField] private inputController input = null;
    // Start is called before the first frame update

    // Update is called once per frame
    void Update()
    {
        if(input.RetrieveJumpHoldInput())
        {
            spacebarImage.color = Color.grey;
        }
        else
        {
            spacebarImage.color = Color.white;
        }


    }
}
