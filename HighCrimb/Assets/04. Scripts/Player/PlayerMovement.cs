using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    [Header("이동 영역")]
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

    //Accel과 Decel은 다른 상호작용으로 일어날 수 있는 요소이기 때문에 public으로 타 객체에서 호출 할 수 있게 설정
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
