using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    [Header("���۰� ����")]
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

    //Accel�� Decel�� �ٸ� ��ȣ�ۿ����� �Ͼ �� �ִ� ����̱� ������ public���� Ÿ ��ü���� ȣ�� �� �� �ְ� ����
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
