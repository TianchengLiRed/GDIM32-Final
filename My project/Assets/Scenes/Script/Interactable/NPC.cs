using UnityEngine;

public class NPC : LookAtPlayerInteractable
{
    [SerializeField] private DialogueData myDialogue;

    public override void OnInteract()
    {
        if (myDialogue != null)
        {
            DialogueManager.Instance.StartDialogue(myDialogue);
        }
    }
}
