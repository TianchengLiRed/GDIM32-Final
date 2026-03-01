using UnityEngine;

public class Boss : LookAtPlayerInteractable
{
    [SerializeField] private DialogueData myDialogue;

    private bool introDialoguePlayed = false;

    public override void OnInteract()
    {
        if (TaskChoose.Instance != null && TaskChoose.Instance.IsChoicePanelOpen)
        {
            return;
        }

        if (TaskChoose.Instance != null && TaskChoose.Instance.HasTaskBeenChosen)
        {
            return;
        }

        if (introDialoguePlayed)
        {
            return;
        }

        if (myDialogue != null)
        {
            introDialoguePlayed = true;
            DialogueManager.Instance.StartDialogue(myDialogue, ShowChoice);
        }
    }

    private void ShowChoice()
    {
        if (TaskChoose.Instance != null)
        {
            TaskChoose.Instance.ShowChoicePanel();
        }
    }
}
