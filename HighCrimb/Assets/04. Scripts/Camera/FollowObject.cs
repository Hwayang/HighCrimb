using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowObject : MonoBehaviour
{
    [SerializeField]
    public GameObject targetObject; 

    // Update is called once per frame
    void Update()
    {
        
        transform.position = targetObject.transform.position + new Vector3(0,0,-5);
    }
}
