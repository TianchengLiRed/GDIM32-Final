using UnityEngine;

public class PhoneInteractable : Interactable
{

   public override void OnInteract()
    {
        base.OnInteract();
        if (TaskManager.Instance != null)
        {
            TaskManager.Instance.CompleteTask(TaskType.TakeTelephone);
            AudioManager.Instance.PlayerTelephone();
        }
    }
}