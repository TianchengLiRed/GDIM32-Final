using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coffee : Interactable
{
    public override void OnInteract()
    {
        TimerManager.Instance.StartTimer(30);
        AudioManager.Instance.PlayerDrink();

    }
}
