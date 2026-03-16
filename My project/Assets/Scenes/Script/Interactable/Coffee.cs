using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coffee : Interactable
{
    private bool hasDrankCoffee = false;

    public override void OnInteract()
    {
        if (hasDrankCoffee) return;

        base.OnInteract();
        hasDrankCoffee = true;
        TimerManager.Instance.StartTimer(30);
        AudioManager.Instance.PlayerDrink();
        if (TaskFlowManager.Instance != null)
        {
            TaskFlowManager.Instance.OnCoffeeDrank();
        }

    }
}
