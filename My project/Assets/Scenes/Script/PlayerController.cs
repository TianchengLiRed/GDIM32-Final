using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float walkSpeed = 5f;
    [SerializeField] private float runSpeed = 8f;

    [SerializeField] private float jumpForce = 5f;
    [SerializeField] private float groundCheckDistance = 0.1f;

    private Rigidbody rb;
    private Animator animator;
    private bool IsGrounded;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        CheckGround();
        Move();
        Jump();
        UpdateAnimator();

    }

    void Move()
    {
       float h = Input.GetAxis("Horizontal");
       float v = Input.GetAxis("Vertical");

        float speed = Input.GetKey(KeyCode.LeftShift) ? runSpeed : walkSpeed;

        Vector3 move = transform.forward * v + transform.right * h;

        rb.velocity = move * speed + new Vector3(0, rb.velocity.y, 0);
    }
    void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && IsGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);

            if (animator != null)
            {
                animator.SetTrigger("Jump");
            }
        }
    }
    void CheckGround()
    {
        Vector3 origin = transform.position + Vector3.up * 0.1f;
        IsGrounded = Physics.Raycast(origin, Vector3.down, groundCheckDistance, ~0);
    }

  void UpdateAnimator()
    {
        if (animator == null) return;
        animator.SetFloat("Speed", rb.velocity.magnitude);
        animator.SetBool("IsGrounded", IsGrounded);
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        Vector3 origin = transform.position + Vector3.up * 1.6f; // 头的高度
        Vector3 direction = transform.forward;

        Gizmos.DrawRay(origin, direction * 5f);
    }
}
