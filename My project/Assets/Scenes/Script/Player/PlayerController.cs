using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private enum PlayerState{

    Idle,
    Walking,    // 行走
    Interacting, // 交互
    Jumping //跳跃

   }
   private PlayerState currentState = PlayerState.Idle;
    [SerializeField] private float walkSpeed = 5f;
    [SerializeField] private float runSpeed = 8f;

    [SerializeField] private float jumpForce = 5f;
    [SerializeField] private float groundCheckDistance = 0.1f;
    [SerializeField] private float moveAcceleration = 12f;

    [SerializeField] private LayerMask interactLayer;
    [SerializeField] private float detectRange = 3f;//检测范围
    private Interactable currentInteractable;
/*
    [Header("GUI Prompt")]
    [SerializeField] private Vector2 guiPromptSize = new Vector2(380f, 78f);
    [SerializeField] private Vector2 guiPromptOffset = new Vector2(0f, -80f);
    [SerializeField] private GUIStyle guiPromptStyle;
    [SerializeField] private int guiPromptFontSize = 36;
    [SerializeField] private Color guiPromptTextColor = new Color(1f, 0.95f, 0.2f, 1f);
    [SerializeField] private Color guiPromptOutlineColor = Color.black;
    [SerializeField] private Color guiPromptBgColor = new Color(0f, 0f, 0f, 0.7f);
    [SerializeField] private Color guiPromptBorderColor = new Color(1f, 0.9f, 0.2f, 0.95f);
    [SerializeField] private float guiPromptPulseSpeed = 3f;
    [SerializeField] private float guiPromptPulseStrength = 0.25f;
*/

    private Rigidbody rb;
    private Animator animator;
    private bool IsGrounded;
    private Vector2 moveInput;
    private bool jumpQueued;
    private GUIStyle runtimePromptStyle;
    private GUIStyle mergedPromptStyle;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();

        rb.interpolation = RigidbodyInterpolation.Interpolate;
        rb.collisionDetectionMode = CollisionDetectionMode.Continuous;
    }

    void Update()
    {
        UpdateState();//状态机器
        ReadInput();
        CheckGround();
        UpdateAnimator();
        InterRange();        

        if (Input.GetKeyDown(KeyCode.E))
        {
            TryInteractCurrent();
        }

    }

    private void FixedUpdate()
    {
        Move();
        Jump();
    }
    private void UpdateState()
{
    if (currentInteractable != null && Input.GetKey(KeyCode.E))//如果监测到交互物体
    {
        currentState = PlayerState.Interacting;//切换状态
        return;
    }
       if (!IsGrounded)//如果监测跳跃
    {
        currentState = PlayerState.Jumping;//切换跳跃状态
        return;
    }

    if (moveInput.magnitude > 0f)//如果监测走路
    {
        currentState =PlayerState.Walking;//切换走路状态
    }


}

    private void ReadInput()
    {
           if (currentState == PlayerState.Interacting){

            return;
        } // 交互中不能跳
        moveInput.x = Input.GetAxisRaw("Horizontal");
        moveInput.y = Input.GetAxisRaw("Vertical");
        moveInput = Vector2.ClampMagnitude(moveInput, 1f);

        if (Input.GetKeyDown(KeyCode.Space) && IsGrounded)
        {
            jumpQueued = true;
        }
    }

    void Move()
    {
        if (currentState == PlayerState.Interacting){

            return;
        } // 交互中不能跳
        float speed = Input.GetKey(KeyCode.LeftShift) ? runSpeed : walkSpeed;

        Vector3 move = transform.forward * moveInput.y + transform.right * moveInput.x;
        Vector3 targetHorizontalVelocity = move * speed;
        Vector3 currentHorizontalVelocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        Vector3 nextHorizontalVelocity = Vector3.Lerp(
            currentHorizontalVelocity,
            targetHorizontalVelocity,
            moveAcceleration * Time.fixedDeltaTime
        );

        rb.velocity = new Vector3(nextHorizontalVelocity.x, rb.velocity.y, nextHorizontalVelocity.z);
    }
    void Jump()
    {
        if (currentState == PlayerState.Interacting) return; // 交互中不能跳
        if (jumpQueued && IsGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            jumpQueued = false;

            if (animator != null)
            {
                animator.SetTrigger("Jump");
            }
        }

        if (!IsGrounded)
        {
            jumpQueued = false;
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

   void InterRange()
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
        if (currentInteractable == null) return;

        // 设置状态为交互
        currentState = PlayerState.Interacting;

        // 对话进行中时，E 键用于推进对话，不触发场景交互
        if (DialogueManager.Instance != null && DialogueManager.Instance.IsInDialogue)
        {
            return;
        }

        if (TaskChoose.Instance != null && TaskChoose.Instance.IsChoicePanelOpen)
        {
            return;
        }

        if (currentInteractable == null) return;

        currentInteractable.OnInteract();

        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayClick();
        }

    // 交互完成后，再切回 Idle 或其他状态
    StartCoroutine(EndInteraction(3));
}

private IEnumerator EndInteraction(float duration)
{
    yield return new WaitForSeconds(duration);
    currentState = PlayerState.Idle;
}



/*
    private void OnGUI()
    {
        if (currentInteractable == null) return;
        if (DialogueManager.Instance != null && DialogueManager.Instance.IsInDialogue) return;
        if (TaskChoose.Instance != null && TaskChoose.Instance.IsChoicePanelOpen) return;

        EnsurePromptStyle();

        float x = (Screen.width - guiPromptSize.x) * 0.5f + guiPromptOffset.x;
        float y = Screen.height - guiPromptSize.y + guiPromptOffset.y;
        Rect rect = new Rect(x, y, guiPromptSize.x, guiPromptSize.y);
        string displayText = currentInteractable.PromptText;

        float pulse01 = 0.5f + 0.5f * Mathf.Sin(Time.unscaledTime * guiPromptPulseSpeed);
        float pulseMul = Mathf.Lerp(1f - guiPromptPulseStrength, 1f, pulse01);

        Color bgColor = guiPromptBgColor;
        bgColor.a *= pulseMul;
        DrawRect(rect, bgColor);
        DrawRect(new Rect(rect.x - 2f, rect.y - 2f, rect.width + 4f, 2f), guiPromptBorderColor);
        DrawRect(new Rect(rect.x - 2f, rect.yMax, rect.width + 4f, 2f), guiPromptBorderColor);
        DrawRect(new Rect(rect.x - 2f, rect.y, 2f, rect.height), guiPromptBorderColor);
        DrawRect(new Rect(rect.xMax, rect.y, 2f, rect.height), guiPromptBorderColor);

        GUIStyle effectiveStyle = GetEffectivePromptStyle();
        DrawOutlinedLabel(rect, displayText, effectiveStyle, guiPromptOutlineColor, 2f);
    }

    private void EnsurePromptStyle()
    {
        if (runtimePromptStyle != null) return;

        runtimePromptStyle = new GUIStyle(GUI.skin.label);
        runtimePromptStyle.alignment = TextAnchor.MiddleCenter;
        runtimePromptStyle.fontStyle = FontStyle.Bold;
        runtimePromptStyle.fontSize = guiPromptFontSize;
        runtimePromptStyle.normal.textColor = guiPromptTextColor;
        runtimePromptStyle.richText = true;
    }

    private GUIStyle GetEffectivePromptStyle()
    {
        if (guiPromptStyle == null) return runtimePromptStyle;

        if (mergedPromptStyle == null)
        {
            mergedPromptStyle = new GUIStyle(guiPromptStyle);
        }

        mergedPromptStyle.alignment = TextAnchor.MiddleCenter;
        mergedPromptStyle.fontStyle = FontStyle.Bold;
        mergedPromptStyle.fontSize = guiPromptFontSize;
        mergedPromptStyle.normal.textColor = guiPromptTextColor;
        mergedPromptStyle.hover.textColor = guiPromptTextColor;
        mergedPromptStyle.active.textColor = guiPromptTextColor;
        mergedPromptStyle.focused.textColor = guiPromptTextColor;
        return mergedPromptStyle;
    }

    private void DrawOutlinedLabel(Rect rect, string text, GUIStyle style, Color outlineColor, float outlineSize)
    {
        Color oldColor = GUI.color;

        GUI.color = outlineColor;
        GUI.Label(new Rect(rect.x - outlineSize, rect.y, rect.width, rect.height), text, style);
        GUI.Label(new Rect(rect.x + outlineSize, rect.y, rect.width, rect.height), text, style);
        GUI.Label(new Rect(rect.x, rect.y - outlineSize, rect.width, rect.height), text, style);
        GUI.Label(new Rect(rect.x, rect.y + outlineSize, rect.width, rect.height), text, style);

        GUI.color = oldColor;
        GUI.Label(rect, text, style);
    }

    private void DrawRect(Rect rect, Color color)
    {
        Color oldColor = GUI.color;
        GUI.color = color;
        GUI.DrawTexture(rect, Texture2D.whiteTexture);
        GUI.color = oldColor;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, detectRange);
    }
*/
}
