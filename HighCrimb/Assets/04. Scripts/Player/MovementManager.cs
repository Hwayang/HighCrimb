using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementManager : MonoBehaviour
{
    int targetNum = 0;
    string returnTag = null;

    void Update()
    {
        if (Input.GetButtonDown("ChangeCharacter"))
        {
            targetNum = targetNum + 1 > 2 ? 0 : targetNum + 1; 
            Debug.Log("targetNum : " + targetNum);
        }
    }

    public string OrderMovement()
    {
        switch(targetNum)
        {
            case 0:
                returnTag = "all";
                break;
            case 1:
                returnTag = "Player_Sister";
                break;
            case 2:
                returnTag = "Player_Brother";
                break;
        }

        return returnTag;
    }

    public int GetTargetNum() { return targetNum; }   
}
