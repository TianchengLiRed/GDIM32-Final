using UnityEngine;

public class Door : Interactable
{
    [SerializeField] private Animator animator;
    [SerializeField] private string openTriggerName = "Open";
    [SerializeField] private bool isOpened = false;
    [SerializeField] private GameObject FinalPanel;

    public override void OnInteract()
    {
        if (isOpened) return;

        if (TaskManager.Instance != null && TaskManager.Instance.OpentheDoor)
        {
            OpenDoor();
        }
        else
        {
            
        }
    }
}
