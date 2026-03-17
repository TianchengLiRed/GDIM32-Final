using UnityEngine;

public class Coffee : Interactable
{
    private bool hasDrankCoffee = false;

    public override void OnInteract()
    {
        if (hasDrankCoffee) return;

        base.OnInteract();
        hasDrankCoffee = true;
        AudioManager.Instance.PlayerDrink();

        if (AudioManager.Instance != null)
            AudioManager.Instance.PlayerDrink();

        if (TimerManager.Instance != null)
            TimerManager.Instance.StartTimer(150);

        if (TaskManager.Instance != null)
            TaskManager.Instance.StartPendingTask();
    }
}
