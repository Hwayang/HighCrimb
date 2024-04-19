using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private float horizontal; //방향값을 받아오는 변수
    private bool isFacingRight = true;  //오른쪽을 바라보고 있는지를 판정하는 변수
    private Rigidbody2D rigid;
    private bool isJump = false;
    private bool isGround = true;
    private float jumpTimeCounter = 0;

    [Header("플레이어 설정")]
    [Header("좌, 우 움직임 세부 설정")]
    [SerializeField] private float moveSpeed; //이동 속도
    [SerializeField] private float jumpForce; //점프 힘
    [SerializeField] private float airCorrectionValue; //체공 시 보정값
    [SerializeField] private float acceleration; //이동 시작 시 가속 보정값
    [SerializeField] private float decceleration; //이동 종료 시 감속 보정값
    [SerializeField] [Range(0, 1)] private float velPower; //속도 그래프 보정값
    [SerializeField] private float jumpTime; //점프 가능 시간

    [Header("기타 플레이어 세부 설정")]
    [SerializeField] [Range(0, 1)] private float downScale; //떨어질 때의 속도값


    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        /*
        물리 프레임은 FixedUpdate에서 동작함. 이에 따라 FixedUpdate보다 더 많이 호출되는 Update에 물리 프레임을 호출하여
        프레임간의 충돌을 방지하도록 설계함
        */

        horizontal = Input.GetAxis("Horizontal");
        Flip();
        GroundCheck();
        Jump();
    }
    private void FixedUpdate()
    {
        Movement();
    }

    private void Jump()
    {

        if (Input.GetKeyDown(KeyCode.X) && isGround is true)
        {
            isJump = true;
            jumpTimeCounter = jumpTime;
            rigid.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }

        if (Input.GetKey(KeyCode.X) && isJump is true)
        {
            if(jumpTimeCounter > 0)
            {
                rigid.AddForce(Vector2.up * airCorrectionValue, ForceMode2D.Impulse);
                jumpTimeCounter -= Time.deltaTime;
            }
            else
            {
                isJump = false;
            }
        }

        if(Input.GetKeyUp(KeyCode.X))
        {
            rigid.AddForce(Vector2.down * downScale, ForceMode2D.Impulse);
            isJump = false;
        }
    }

    private void GroundCheck()
    {
        RaycastHit2D rayHit = Physics2D.Raycast(rigid.position, Vector3.down, 0.1f, LayerMask.GetMask("Ground"));
        Debug.DrawRay(rigid.position, Vector3.down, new Color(0, 1, 0));

        isGround = rayHit.collider != null;

        Debug.Log(isGround);
        return;

    }

    private void Movement()
    {
        float moveInput = horizontal;
        float targetSpeed = moveInput * moveSpeed;
        float speedDif = targetSpeed - rigid.velocity.x;
        float accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? acceleration : decceleration;
        float movement = Mathf.Pow(Mathf.Abs(speedDif) * accelRate, velPower) * Mathf.Sign(speedDif);
        rigid.AddForce(movement * Vector2.right);
    }

    private void Flip()
    {
        if (isFacingRight && horizontal < 0f || !isFacingRight && horizontal > 0f)
        {
            isFacingRight = !isFacingRight;

            //x축 크기 값에 음수를 곱해주어 반대로 리소스가 적용되도록
            //localScale은 Read Only이므로 바로 접근해서 수정할 수 없으므로 변수에 담아 수정
            Vector3 localScale = transform.localScale;
            localScale.x *= -1f;
            transform.localScale = localScale;
        }
    }
}
