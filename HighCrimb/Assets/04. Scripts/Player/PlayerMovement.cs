using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    [Header("조작감 영역")]
    [SerializeField]
    [Range(0, 100)]
    public float accelSpeed = 1f;

    [SerializeField]
    [Range(0, 100)]
    public float decelSpeed = 1f;

    private Rigidbody2D playerRb;
    private MovementManager moveManager;

    private bool isMove = true;
    private string receiveTargetMessage = null;
    private int receiveStateMessage = 0;

    private void Awake()
    {
        playerRb = GetComponent<Rigidbody2D>();
        moveManager = FindObjectOfType<MovementManager>();
    }

    void FixedUpdate()
    {
        
        IsCanMovement(receiveTargetMessage);
        float moveControlValue = MoveControl();

        float inputX = Input.GetAxisRaw("Horizontal");
        float inputY = Input.GetAxisRaw("Vertical");

        Debug.Log("Message" + receiveStateMessage);

        if (isMove)
        {
            playerRb.velocity = new Vector2(inputX, inputY).normalized * (GetComponentInParent<PlayerInfo>().GetMoveSpeed() + moveControlValue) * Time.deltaTime;
        }
    }

    private void Update()
    {
        receiveTargetMessage = moveManager.OrderMovement();
    }

    //Accel과 Decel은 다른 상호작용으로 일어날 수 있는 요소이기 때문에 public으로 타 객체에서 호출 할 수 있게 설정
    public float MoveControl()
    {
        ReceiveState();
        
        if(receiveStateMessage == 1)
        {
            return accelSpeed;
        }
        else if (receiveStateMessage == 2)
        {
            return -decelSpeed;
        }

        return 0f;
    }

    public void ReceiveState()
    {
        receiveStateMessage = GetComponentInParent<PlayerState>().GetCurrentState();
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
