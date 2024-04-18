using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private float horizontal;
    private bool isFacingRight = true;
    private bool isWallSliding;
    private bool isWallJumping;
    private float wallJumpingDirection;
    private float wallJumpingCounter;
    

    [Header("�÷��̾� ����")] [SerializeField] private float moveSpeed;
    [SerializeField] private float jumpPower;
    [SerializeField] private float acceleration;
    [SerializeField] private float decceleration;
    [SerializeField] [Range(0, 1)] private float velPower;
    [SerializeField] [Range(0, 1)] private float downScale;
    [SerializeField] private float wallSlidingSpeed;
    [SerializeField] private float wallJumpingTime;
    [SerializeField] private float wallJumpingDuration;
    [SerializeField] private Vector2 wallJumpingPower;

    [Space]
    [SerializeField] [Range(0, 1)] private float groundCheckRange;
    [SerializeField] [Range(0, 1)] private float wallCheckRange;

    [Space] [Header("��ü ����")]
    [SerializeField] private Rigidbody2D _rigid;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private Transform wallCheck;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask wallLayer;

    // Update is called once per frame
    void Update()
    {
        /*
        Ű �ٿ�� ���� �ܹ߼� Ŀ�ǵ带 FixedUpdate�� �ξ��� �� ���� �����Ӱ� �浹�ϱ⿡ ������ �߻��� ���ɼ��� ����.
        �̿� ���� Update���� ���� �������� ��Ȱ�ϰ� ���ư� �� �ֵ��� ����
        */

        horizontal = Input.GetAxis("Horizontal");

        if (Input.GetButtonDown("Jump") && IsGrounded())
        {
            _rigid.velocity = new Vector2(_rigid.velocity.x, jumpPower);
        }

        if (Input.GetButtonUp("Jump") && _rigid.velocity.y > 0f)
        {
            _rigid.velocity = new Vector2(_rigid.velocity.x, _rigid.velocity.y * downScale);
        }

        WallSlide();
        WallJump();

        if(!isWallJumping)
        {
            Flip();
        }
    }
    private bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, groundCheckRange, groundLayer);
    }

    private void FixedUpdate()
    {
        if (!isWallJumping)
            Movement();
    }

    private void Movement()
    {
        float moveInput = horizontal;
        float targetSpeed = moveInput * moveSpeed;
        float speedDif = targetSpeed - _rigid.velocity.x;
        float accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? acceleration : decceleration;
        float movement = Mathf.Pow(Mathf.Abs(speedDif) * accelRate, velPower) * Mathf.Sign(speedDif);
        _rigid.AddForce(movement * Vector2.right);
    }

    private void Flip()
    {
        if (isFacingRight && horizontal < 0f || !isFacingRight && horizontal > 0f)
        {
            isFacingRight = !isFacingRight;

            //x�� ũ�� ���� ������ �����־� �ݴ�� ���ҽ��� ����ǵ���
            //localScale�� Vector3�̹Ƿ� �ٷ� �����ؼ� ������ �� �����Ƿ� ������ ��� ����
            Vector3 localScale = transform.localScale;
            localScale.x *= -1f;
            transform.localScale = localScale;
        }
    }

    private bool IsWalled()
    {
        return Physics2D.OverlapCircle(wallCheck.position, 0.2f, wallLayer);
    }

    private void WallSlide()
    {
        if (IsWalled() && !IsGrounded() && horizontal != 0f)
        {
            isWallSliding = true;
            _rigid.velocity = new Vector2(_rigid.velocity.x, Mathf.Clamp(_rigid.velocity.y, -wallSlidingSpeed, float.MaxValue));
        }
        else
        {
            isWallSliding = false;
        }
    }

    private void WallJump()
    {
        if (isWallSliding)
        {
            isWallJumping = false;
            wallJumpingDirection = -transform.localScale.x;
            wallJumpingCounter = wallJumpingTime;

            CancelInvoke(nameof(StopWallJumping));
        }
        else
        {
            wallJumpingCounter -= Time.deltaTime;
        }

        if (Input.GetButtonDown("Jump") && wallJumpingCounter > 0f)
        {
            isWallJumping = true;
            _rigid.velocity = new Vector2(wallJumpingDirection * wallJumpingPower.x, wallJumpingPower.y);
            wallJumpingCounter = 0f;

            if (transform.localScale.x != wallJumpingDirection)
            {
                isFacingRight = !isFacingRight;
                Vector3 localScale = transform.localScale;
                localScale.x *= -1f;
                transform.localScale = localScale;
            }

            Invoke(nameof(StopWallJumping), wallJumpingDuration);
        }
    }

    private void StopWallJumping()
    {
        isWallJumping = false;
    }


}
