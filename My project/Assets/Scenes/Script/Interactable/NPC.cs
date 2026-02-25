using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : Interactable
{
   [SerializeField] private DialogueData myDialogue;
    [SerializeField] private Transform Player;
    [SerializeField] private float rotationSpeed = 5f;

    private void Update()
    {
        NpclookAtPlayer();
    }

    void NpclookAtPlayer()
    {
        if (Player != null)
        {
            Vector3 direction = Player.position - transform.position;
            direction.y = 0; // 保持水平旋转
            if (direction.magnitude > 0.1f) // 避免过于接近时的抖动
            {
                Quaternion targetRotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            }
        }
    }

    public override void OnInteract()
    {
        // 触发交互时，告诉管理器开始这段对话
        if (myDialogue != null)
        {
            //调用scriptobject的对话信息
            DialogueManager.Instance.StartDialogue(myDialogue);
        }
    }
}
