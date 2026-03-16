using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Doors : Interactable
{
    [SerializeField] private Animator doorAnimator;
    private bool opened = false;

    public override void OnInteract()
    {
        if (opened) return;

        base.OnInteract();

        if (doorAnimator != null)
        {
            doorAnimator.SetTrigger("OpenDoor");
            opened = true;
        }
    }
}
