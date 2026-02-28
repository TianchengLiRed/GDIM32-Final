using UnityEngine;

public class Boss : LookAtPlayerInteractable
{
    [SerializeField] private DialogueData myDialogue;

    public override void OnInteract()
    {
        if (myDialogue != null)
        {
            DialogueManager.Instance.StartDialogue(myDialogue, ShowChoice);
        }
    }

    private void ShowChoice()
    {
        if (TaskChoose.Instance != null)
        {
            TaskChoose.Instance.ActivePanel();
        }
    }
}
