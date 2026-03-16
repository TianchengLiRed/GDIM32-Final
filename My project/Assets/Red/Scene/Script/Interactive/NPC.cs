using UnityEngine;

public class NPC : LookAtPlayerInteractable
{
    [SerializeField] private DialogueData myDialogue;
    [SerializeField] private Animator Animator;


    public override void OnInteract()
    {
        base.OnInteract();

        if (myDialogue != null)
        {
            Animator.SetBool("Istalking", true);
            DialogueManager.Instance.StartDialogue(myDialogue);
        }
    }
    public void StopTalking()
    {
        Animator.SetBool("istalking", false);
    }
}
