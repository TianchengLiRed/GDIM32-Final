using UnityEngine;

public class Boss : LookAtPlayerInteractable
{
    [SerializeField] private Animator Animator;
    [SerializeField] private DialogueData bossDialogue;
    [SerializeField] private DialogueData completeDialogue;
    [SerializeField] private DialogueData scoldDialogue;
    public override void OnInteract()
    {
        base.OnInteract();
        Animator.SetBool("Istalking", true);
         if (!TaskManager.Instance.HasAcceptedTask)
        {
            // 还没接任务
            DialogueManager.Instance.StartDialogue(bossDialogue, true);
        }
        else if (!TaskManager.Instance.AllTasksCompleted)
        {
            // 已接任务但没完成
            DialogueManager.Instance.StartDialogue(scoldDialogue, false);
        }
        else
        {
            // 全部完成
            DialogueManager.Instance.StartDialogue(completeDialogue, false);
            TaskManager.Instance.OpentheDoor = true;
        }
    }

    private void ShowChoice()
    {
        if (TaskChoose.Instance != null)
        {
            TaskChoose.Instance.ShowChoicePanel();
        }
    }
    public void StopTalking()
    {
        Animator.SetBool("istalking", false);
    }
    
}
