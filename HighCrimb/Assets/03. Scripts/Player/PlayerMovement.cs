using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private float horizontal; //���Ⱚ�� �޾ƿ��� ����
    private bool isFacingRight = true;  //�������� �ٶ󺸰� �ִ����� �����ϴ� ����
    private Rigidbody2D rigid;
    private bool isJump = false;
    private bool isGround = true;
    private float jumpTimeCounter = 0;

    [Header("�÷��̾� ����")]
    [Header("��, �� ������ ���� ����")]
    [SerializeField] private float moveSpeed; //�̵� �ӵ�
    [SerializeField] private float jumpForce; //���� ��
    [SerializeField] private float airCorrectionValue; //ü�� �� ������
    [SerializeField] private float acceleration; //�̵� ���� �� ���� ������
    [SerializeField] private float decceleration; //�̵� ���� �� ���� ������
    [SerializeField] [Range(0, 1)] private float velPower; //�ӵ� �׷��� ������
    [SerializeField] private float jumpTime; //���� ���� �ð�

    [Header("��Ÿ �÷��̾� ���� ����")]
    [SerializeField] [Range(0, 1)] private float downScale; //������ ���� �ӵ���


    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        /*
        ���� �������� FixedUpdate���� ������. �̿� ���� FixedUpdate���� �� ���� ȣ��Ǵ� Update�� ���� �������� ȣ���Ͽ�
        �����Ӱ��� �浹�� �����ϵ��� ������
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

            //x�� ũ�� ���� ������ �����־� �ݴ�� ���ҽ��� ����ǵ���
            //localScale�� Read Only�̹Ƿ� �ٷ� �����ؼ� ������ �� �����Ƿ� ������ ��� ����
            Vector3 localScale = transform.localScale;
            localScale.x *= -1f;
            transform.localScale = localScale;
        }
    }
}
