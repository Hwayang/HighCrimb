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

    //Accel�� Decel�� �ٸ� ��ȣ�ۿ����� �Ͼ �� �ִ� ����̱� ������ public���� Ÿ ��ü���� ȣ�� �� �� �ְ� ����
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
