using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Targets
{
    public GameObject target;
}
public class FollowObject : MonoBehaviour
{
    public Targets[] targetArray;
    private MovementManager movementManager;

    private void Awake()
    {
        movementManager = FindObjectOfType<MovementManager>();
    }
    void Update()
    {
        this.transform.position = targetArray[movementManager.GetTargetNum()].target.transform.position + new Vector3(0,0,-5);
        Debug.Log(targetArray[movementManager.GetTargetNum()]);
    }
}
