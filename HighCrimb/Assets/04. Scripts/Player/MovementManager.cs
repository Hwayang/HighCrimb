using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementManager : MonoBehaviour
{
    int targetNum = 0;

    void Update()
    {
        if (targetNum == 3)
        {
            targetNum = 0;
        }

        if (Input.GetButtonDown("ChangeCharacter"))
        {
            targetNum++;
            Debug.Log("targetNum : " + targetNum);
        }
    }

    public string OrderMovement()
    {
        string returnTag = null;

        switch(targetNum)
        {
            case 0:
                returnTag = "all";
                Debug.Log("target : all");
                break;
            case 1:
                returnTag = "Player_Sister";
                Debug.Log("target : sister");
                break;
            case 2:
                returnTag = "Player_Brother";
                Debug.Log("target : brother");
                break;
        }

        return returnTag;
    }
}
