using UnityEngine;

public class RestroomDoor : Interactable
{
    [SerializeField] private Animator animator;
    [SerializeField] private bool isOpened = false;
    [SerializeField] private GameObject FinalPanel;

    public override void OnInteract()
    {
        if (isOpened) return;

        if (TaskManager.Instance != null && TaskManager.Instance.OpentheDoor)
        {
            
        }
        else
        {
            
        }
    }
}
