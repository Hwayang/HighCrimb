using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private float horizontal; //���Ⱚ�� �޾ƿ��� ����
    private float isFacingRight = 1;  //�������� �ٶ󺸰� �ִ����� �����ϴ� ����
    private Rigidbody2D rigid;
    private bool isJump = false; //���� ���� ������ �Ǵ��ϴ� ����
    private bool isGround = true; // ���� ���� �پ��ִ��� �Ǵ��ϴ� ����
    private float jumpTimeCounter = 0; // ü�� �ð��� �����ϴ� ����
    private bool isWall = false; //���� ���� �پ��ִ��� �Ǵ��ϴ� ����
    private bool isWallJump = false; //�������� �����ߴ��� �Ǵ��ϴ� ����

    [Header("�÷��̾� ����")]
    [Header("��, �� ������ ���� ����")]
    [SerializeField] private float moveSpeed; //�̵� �ӵ�
    [SerializeField] private float jumpForce; //���� ��
    [SerializeField] private float airCorrectionValue; //ü�� �� ������
    [SerializeField] private float acceleration; //�̵� ���� �� ���� ������
    [SerializeField] private float decceleration; //�̵� ���� �� ���� ������
    [SerializeField][Range(0, 1)] private float velPower; //�ӵ� �׷��� ������
    [SerializeField] private float jumpTime; //���� ���� �ð�

    [Header("��Ÿ �÷��̾� ���� ����")]
    [SerializeField][Range(0, 1)] private float downScale; //������ ���� �ӵ���

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
        WallCheck();
        Jump();
        WallSlide();
        WallJump();
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
            if (jumpTimeCounter > 0)
            {
                rigid.AddForce(Vector2.up * airCorrectionValue, ForceMode2D.Impulse);
                jumpTimeCounter -= Time.deltaTime;
            }
            else
            {
                isJump = false;
            }
        }

        if (Input.GetKeyUp(KeyCode.X))
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
    }

    private void WallCheck()
    {
        RaycastHit2D rayHit = Physics2D.Raycast(rigid.position, Vector2.right * isFacingRight, 0.3f, LayerMask.GetMask("Wall"));
        Debug.DrawRay(rigid.position, Vector2.right * isFacingRight, new Color(1, 0, 0));

        isWall = rayHit.collider != null;
        Debug.Log(isWall);
    }

    private void WallSlide()
    {
        if(isWall)
        {
            //todo : �ð��� �������� �������� �ӵ��� �� ������
            rigid.velocity = new Vector2(rigid.velocity.x, rigid.velocity.y * downScale);
        }
    }

    private void WallJump()
    {
        if(isWall)
        {
            isWallJump = false;

            if (Input.GetKeyDown(KeyCode.X))
            {
                isWallJump = true;
                Invoke("FreezX", 0.3f);
                Vector2 target = new Vector2(-isFacingRight * jumpForce, jumpForce);
                isJump = true;
                rigid.AddForce(target, ForceMode2D.Impulse);

                Flip();
            }
        }
    }

    private void FreezX()
    {
        isWallJump = false;
    }

    private void Movement()
    {
        if (!isWallJump)
        {
            float moveInput = horizontal;
            float targetSpeed = moveInput * moveSpeed;
            float speedDif = targetSpeed - rigid.velocity.x;
            float accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? acceleration : decceleration;
            float movement = Mathf.Pow(Mathf.Abs(speedDif) * accelRate, velPower) * Mathf.Sign(speedDif);
            rigid.AddForce(movement * Vector2.right);
        }
    }

    private void Flip()
    {
        if (isFacingRight == 1 && horizontal < 0f || isFacingRight < 0 && horizontal > 0f)
        {
            isFacingRight = -isFacingRight;

            //x�� ũ�� ���� ������ �����־� �ݴ�� ���ҽ��� ����ǵ���
            //localScale�� Read Only�̹Ƿ� �ٷ� �����ؼ� ������ �� �����Ƿ� ������ ��� ����
            Vector3 localScale = transform.localScale;
            localScale.x *= -1f;
            transform.localScale = localScale;
        }
    }
}
