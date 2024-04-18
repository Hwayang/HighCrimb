using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private float horizontal;
    private bool isFacingRight = true;

    [Header("�÷��̾� ����")] [SerializeField] private float moveSpeed;
    [SerializeField] private float jumpPower;
    [SerializeField] private float acceleration;
    [SerializeField] private float decceleration;
    [SerializeField] [Range(0, 1)]private float velPower;

    [SerializeField] [Range(0, 1)] private float groundCheckRange;

    [Space] [Header("��ü ����")]
    [SerializeField] private Rigidbody2D _rigid;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;

    // Update is called once per frame
    void Update()
    {
        /*
        Ű �ٿ�� ���� �ܹ߼� Ŀ�ǵ带 FixedUpdate�� �ξ��� �� ���� �����Ӱ� �浹�ϱ⿡ ������ �߻��� ���ɼ��� ����.
        �̿� ���� Update���� ���� �������� ��Ȱ�ϰ� ���ư� �� �ֵ��� ����
        */

        if (Input.GetButtonDown("Jump") && isGrounded())
        {
            _rigid.velocity = new Vector2(_rigid.velocity.x, jumpPower);
        }

        if (Input.GetButtonUp("Jump") && _rigid.velocity.y > 0f)
        {
            _rigid.velocity = new Vector2(_rigid.velocity.x, _rigid.velocity.y * 0.5f);
        }
    }
    private bool isGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, groundCheckRange, groundLayer);
    }

    private void FixedUpdate()
    {
        Movement();
        Flip();
    }

    private void Movement()
    {
        float moveInput = Input.GetAxis("Horizontal");
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


}
