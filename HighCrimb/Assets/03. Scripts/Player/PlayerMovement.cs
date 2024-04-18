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
    

    [Header("플레이어 설정")] [SerializeField] private float moveSpeed;
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

    [Space] [Header("객체 설정")]
    [SerializeField] private Rigidbody2D _rigid;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private Transform wallCheck;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask wallLayer;

    // Update is called once per frame
    void Update()
    {
        /*
        키 다운과 같은 단발성 커맨드를 FixedUpdate로 두었을 때 물리 프레임과 충돌하기에 오류가 발생할 가능성이 높음.
        이에 따라 Update에서 물리 프레임이 원활하게 돌아갈 수 있도록 설정
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

            //x축 크기 값에 음수를 곱해주어 반대로 리소스가 적용되도록
            //localScale은 Vector3이므로 바로 접근해서 수정할 수 없으므로 변수에 담아 수정
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
