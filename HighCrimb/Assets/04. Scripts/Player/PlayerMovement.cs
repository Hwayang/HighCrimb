using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    [Range(300, 400)]
    public float moveSpeed = 300f;

    [SerializeField]
    [Range(0, 100)]
    public float accelSpeed = 1f;

    [SerializeField]
    [Range(0, 100)]
    public float decelSpeed = 1f;

    private Rigidbody2D playerRb;

    private void Awake()
    {
        playerRb = GetComponent<Rigidbody2D>();
    }
    void FixedUpdate()
    {
        float moveControlValue = MoveControl();

        playerRb.velocity = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized * (moveSpeed + moveControlValue) * Time.deltaTime;
        Debug.Log("MoveSpeed" + playerRb.velocity);
    }

    //Accel과 Decel은 다른 상호작용으로 일어날 수 있는 요소이기 때문에 public으로 타 객체에서 호출 할 수 있게 설정
    public float MoveControl()
    {
        if(Input.GetAxisRaw("Accel") == 1)
        {
            return accelSpeed;
        }
        else if(Input.GetAxisRaw("Decel") == 1)
        {
            return -decelSpeed;
        }

        return 0f;
    }
}
