using UnityEngine;

public class Boss : Interactable
{
    [SerializeField] private DialogueData myDialogue;
    [SerializeField] private Transform player;
    [SerializeField] private float rotationSpeed = 5f;

    private void Update()
    {
        NpcLookAtPlayer();
    }

    void NpcLookAtPlayer()
    {
        if (player != null)
        {
            Vector3 direction = player.position - transform.position;
            direction.y = 0;

            if (direction.magnitude > 0.1f)
            {
                Quaternion targetRotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.Slerp(
                    transform.rotation,
                    targetRotation,
                    rotationSpeed * Time.deltaTime
                );
            }
        }
    }

    public override void OnInteract()
    {
        if (myDialogue != null)
        {
            // 订阅“对话结束”事件
            DialogueManager.Instance.OnDialogueEnded += OnBossDialogueEnd;

            // 开始对话
            DialogueManager.Instance.StartDialogue(myDialogue);
        }
    }

    private void OnBossDialogueEnd()
    {
        // 弹出选择面板
        ShowChoice();

        // 取消订阅（非常重要，防止重复触发）
        DialogueManager.Instance.OnDialogueEnded -= OnBossDialogueEnd;
    }

    private void ShowChoice()
    {
        if (TaskChoose.Instance != null)
        {
            TaskChoose.Instance.ActivePanel();
        }
    }
}