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

        if (TaskManager.Instance != null && TaskManager.Instance.AllTasksCompleted)
        {
            OpenDoor();
        }
        else
        {
            Debug.Log("The door won't open yet. Finish all tasks first.");
            // 这里也可以放提示UI或音效
        }
    }

    private void OpenDoor()
    {
        isOpened = true;

        if (animator != null)
        {
            animator.SetTrigger(openTriggerName);
        }
        FinalPanel.SetActive(true);

        Debug.Log("Door opened.");
    }
}
