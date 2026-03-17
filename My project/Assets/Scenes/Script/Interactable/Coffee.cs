using UnityEngine;

public class Coffee : Interactable
{
    private bool hasDrankCoffee = false;

    public override void OnInteract()
    {
        if (hasDrankCoffee) return;

        base.OnInteract();
        hasDrankCoffee = true;

        if (AudioManager.Instance != null)
            AudioManager.Instance.PlayerDrink();

        if (TimerManager.Instance != null)
            TimerManager.Instance.StartTimer(300);

        if (TaskManager.Instance != null)
            TaskManager.Instance.StartPendingTask();
    }
}
