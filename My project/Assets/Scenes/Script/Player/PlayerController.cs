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

    [SerializeField] private LayerMask interactLayer;
    [SerializeField] private float detectRange = 3f;//检测范围
    private List<Interactable> interObjs = new List<Interactable>();//范围内

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
        if(Input.GetMouseButtonDown(0)){
            ClickInteract();

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
        // 上一帧高亮的物体全部熄灭
        foreach (var obj in interObjs)
        {
            if (obj != null) obj.SetHighlight(false);
        }
        interObjs.Clear();
        // 扫描玩家周围 detectRange 距离内的所有碰撞体
        Collider[] hits = Physics.OverlapSphere(transform.position, detectRange, interactLayer);
        foreach (var hit in hits)
        {
            //如果是interactable
            Interactable interactable = hit.GetComponent<Interactable>();
            if (interactable != null)
            {
               //高亮
                interactable.SetHighlight(true);
                interObjs.Add(interactable);
            }
        }
    }

    private void ClickInteract()
    {
        // 从摄像机发射一条射线到鼠标位置
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        //满足layer
        if (Physics.Raycast(ray, out hit, 100f, interactLayer))
        {
            //满足交互
            Interactable clickedObj = hit.collider.GetComponent<Interactable>();

            if (clickedObj != null && interObjs.Contains(clickedObj))
            {
                clickedObj.OnInteract();
            }
            else if (clickedObj != null)
            {
                Debug.Log("看见了，但够不到（超出交互范围）");
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, detectRange);
    }

}
