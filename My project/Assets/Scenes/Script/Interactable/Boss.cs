using UnityEngine;

public class Boss : LookAtPlayerInteractable
{
    [SerializeField] private Animator Animator;
    [SerializeField] private DialogueData bossDialogue;
    [SerializeField] private DialogueData completeDialogue;
    public override void OnInteract()
    {
        base.OnInteract();
        Animator.SetBool("Istalking", true);
        if (TaskManager.Instance.AllTasksCompleted)
        {
            DialogueManager.Instance.StartDialogue(completeDialogue, false);
            TaskManager.Instance.OpentheDoor = true;
        }
        else
        {
            DialogueManager.Instance.StartDialogue(bossDialogue, true);
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
