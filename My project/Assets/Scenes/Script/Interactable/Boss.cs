using UnityEngine;

public class Boss : LookAtPlayerInteractable
{
    [SerializeField] private Animator Animator;
    [SerializeField] private DialogueData bossDialogue;
    public override void OnInteract()
    {
        base.OnInteract();
        Animator.SetBool("Istalking", true);
        DialogueManager.Instance.StartDialogue(bossDialogue, true);
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
