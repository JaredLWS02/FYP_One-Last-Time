using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ControllerMovement : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private float speed;
    [SerializeField] private float jumpForce = 10f;

    [Header("Dash")]
    [SerializeField] private float dashVelocity = 14f;
    [SerializeField] private float dashDuration = 0.2f;
    [SerializeField] private float dashCooldown = 0.5f;
    private Vector2 dashDir;
    private bool isDashing;
    private bool canDash = true;

    [Header("Ground Check")]
    [SerializeField] Transform groundCheck;
    [SerializeField] LayerMask groundLayer;

    private Vector2 moveInputValue;

    private bool isJumping;
    private int jumpCount = 0;
    private int maxJumpCount = 2;
    private bool jumpInput;

    private bool faceRight = true;

    private void OnInputMove(InputValue value)
    {
        moveInputValue = value.Get<Vector2>();
        //Debug.Log(moveInputValue);
    }

    private void OnInputJump()
    {
        jumpInput = true;
    }

    private void OnInputDash()
    {
        if (canDash)
        {            
            Debug.Log("dash");
            StartCoroutine(Dash());

        }
    }
    private IEnumerator Dash()
    {
        canDash = false;
        isDashing = true;

        float originalGravity = rb.gravityScale;
        rb.gravityScale = 0f;
        rb.velocity = new Vector2(transform.localScale.x * dashVelocity, 0f);

        yield return new WaitForSeconds(dashDuration);

        rb.gravityScale = originalGravity;
        isDashing = false;
        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
    }

    private void MoveLogicMethod()
    {
        Vector2 horizontalMovement = new Vector2(moveInputValue.x, 0);

        Vector2 result = horizontalMovement * speed * Time.fixedDeltaTime;

        rb.velocity = new Vector2(result.x, rb.velocity.y);
    }

    private void FixedUpdate()
    {
        if (isDashing) 
        {
            return;
        }

        MoveLogicMethod();
        ResetJumpCount();

        if (jumpInput)
        {
            HandleJump();
        }

        jumpInput = false;
        Flip();
    }

    private void HandleJump()
    {
        if (jumpCount < maxJumpCount)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            jumpCount++;
        }
    }

    private void ResetJumpCount()
    {
        if (IsGrounded())
        {
            jumpCount = 0;
        }
    }

    public bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
    }

    void Flip()
    {
        if (faceRight && moveInputValue.x < 0f || !faceRight && moveInputValue.x > 0f)
        {
            faceRight = !faceRight;
            Vector3 localScale = transform.localScale;
            localScale.x *= -1f;
            transform.localScale = localScale;
        }
    }
}
