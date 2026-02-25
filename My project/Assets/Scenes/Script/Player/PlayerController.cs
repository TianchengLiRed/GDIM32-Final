using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float walkSpeed = 5f;
    [SerializeField] private float runSpeed = 8f;

    [SerializeField] private float jumpForce = 5f;
    [SerializeField] private float groundCheckDistance = 0.1f;

    [SerializeField] private LayerMask interactLayer;
    [SerializeField] private float detectRange = 3f;//检测范围
    private Interactable currentInteractable;

    [Header("GUI Prompt")]
    [SerializeField] private Vector2 guiPromptSize = new Vector2(180f, 36f);
    [SerializeField] private Vector2 guiPromptOffset = new Vector2(0f, -80f);
    [SerializeField] private GUIStyle guiPromptStyle;

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
        InterRange();        

        if (Input.GetKeyDown(KeyCode.E))
        {
            TryInteractCurrent();
        }

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

   private void InterRange()
    {
        Interactable nextInteractable = null;
        float closestSqr = float.MaxValue;
        HashSet<Interactable> candidates = new HashSet<Interactable>();
        // 扫描玩家周围 detectRange 距离内的所有碰撞体
        Collider[] hits = Physics.OverlapSphere(transform.position, detectRange, interactLayer);
        foreach (var hit in hits)
        {
            //如果是interactable
            Interactable interactable = hit.GetComponentInParent<Interactable>();
            if (interactable != null)
            {
                candidates.Add(interactable);
            }
        }

        foreach (var interactable in candidates)
        {
            float sqrDist = (interactable.transform.position - transform.position).sqrMagnitude;
            if (sqrDist < closestSqr)
            {
                closestSqr = sqrDist;
                nextInteractable = interactable;
            }
        }

        if (currentInteractable != nextInteractable)
        {
            if (currentInteractable != null) currentInteractable.SetHighlight(false);
            currentInteractable = nextInteractable;
            if (currentInteractable != null) currentInteractable.SetHighlight(true);
        }
    }

    private void TryInteractCurrent()
    {
        // 对话进行中时，E 键用于推进对话，不触发场景交互
        if (DialogueManager.Instance != null && DialogueManager.Instance.IsInDialogue)
        {
            return;
        }

        if (currentInteractable == null) return;

        currentInteractable.OnInteract();

        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayClick();
        }
    }

    private void OnGUI()
    {
        if (currentInteractable == null) return;
        if (DialogueManager.Instance != null && DialogueManager.Instance.IsInDialogue) return;

        float x = (Screen.width - guiPromptSize.x) * 0.5f + guiPromptOffset.x;
        float y = Screen.height - guiPromptSize.y + guiPromptOffset.y;
        Rect rect = new Rect(x, y, guiPromptSize.x, guiPromptSize.y);

        if (guiPromptStyle != null)
        {
            GUI.Label(rect, currentInteractable.PromptText, guiPromptStyle);
        }
        else
        {
            GUI.Box(rect, currentInteractable.PromptText);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, detectRange);
    }

}
