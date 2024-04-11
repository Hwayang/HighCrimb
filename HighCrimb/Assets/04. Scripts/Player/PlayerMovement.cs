using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    [Header("�̵� ����")]
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
    private MovementManager moveManager;

    private bool isMove = true;
    private string receiveMoveMessage = null;

    private void Awake()
    {
        playerRb = GetComponent<Rigidbody2D>();
        moveManager = FindObjectOfType<MovementManager>();
    }

    void FixedUpdate()
    {
        IsCanMovement(receiveMoveMessage);
        float moveControlValue = MoveControl();

        float inputX = Input.GetAxisRaw("Horizontal");
        float inputY = Input.GetAxisRaw("Vertical");

        if (isMove)
        {
            playerRb.velocity = new Vector2(inputX, inputY).normalized * (moveSpeed + moveControlValue) * Time.deltaTime;
        }
    }

    private void Update()
    {
        receiveMoveMessage = moveManager.OrderMovement();
    }

    //Accel�� Decel�� �ٸ� ��ȣ�ۿ����� �Ͼ �� �ִ� ����̱� ������ public���� Ÿ ��ü���� ȣ�� �� �� �ְ� ����
    public float MoveControl()
    {
        if(Input.GetAxisRaw("Accel") is 1)
        {
            return accelSpeed;
        }
        else if(Input.GetAxisRaw("Decel") is 1)
        {
            return -decelSpeed;
        }

        return 0f;
    }

    private void IsCanMovement(string movementMessage)
    {
        if(movementMessage is "all" || movementMessage == this.tag)
        {
            isMove = true;
        }
        else
        {
            isMove = false;
        }
    }
}
