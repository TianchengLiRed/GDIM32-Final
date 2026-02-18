using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private float speed = 5f;
    private Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }

    void Move()
    {
       float h = Input.GetAxis("Horizontal");
       float v = Input.GetAxis("Vertical");

        Vector3 move = transform.forward * v + transform.right * h;

        rb.velocity = move * speed + new Vector3(0, rb.velocity.y, 0);
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        Vector3 origin = transform.position + Vector3.up * 1.6f; // 头的高度
        Vector3 direction = transform.forward;

        Gizmos.DrawRay(origin, direction * 5f);
    }
}
