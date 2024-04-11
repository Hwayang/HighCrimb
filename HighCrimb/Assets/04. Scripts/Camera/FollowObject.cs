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
    [Header("ī�޶� ����")]
    public float cameraSpeed = 1.0f;

    [Header("ī�޶� Ÿ�� ����")]
    public Targets[] targetArray;
    
    private MovementManager movementManager;
    private int targetNum = 0;

    private void Awake()
    {
        movementManager = FindObjectOfType<MovementManager>();
    }

    void FixedUpdate()
    {
        targetNum = movementManager.GetTargetNum();
        Vector2 pos = Vector2.Lerp(transform.position, targetArray[targetNum].target.transform.position, Time.deltaTime * cameraSpeed);
        this.transform.position = new Vector3(pos.x, pos.y, -10);
    }
}
