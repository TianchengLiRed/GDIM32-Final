using UnityEngine;

public class RestroomDoor : Interactable
{
    [SerializeField] private Animator animator;
    [SerializeField] private bool isOpen = false;
    [SerializeField] private GameObject FinalPanel;

    public override void OnInteract()
    {
        if (isOpen) return;

        if (TaskManager.Instance != null && TaskManager.Instance.OpentheDoor)
        {
            isOpen = !isOpen;
            animator.SetBool("IsOpen", isOpen);

        }
        else
        {
            
        }
    }
}
