using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Doors : Interactable
{
    [SerializeField] private Animator doorAnimator;
    private bool isOpen = false;

    public override void OnInteract()
    {
        base.OnInteract();

        isOpen = !isOpen;
        doorAnimator.SetBool("IsOpen", isOpen);
    }
}
