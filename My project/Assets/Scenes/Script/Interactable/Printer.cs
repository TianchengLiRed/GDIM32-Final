using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PrinterInteractable : Interactable
{
    [SerializeField] private Animator printerAnimator;
    [SerializeField] private GameObject paperPrefab;
    [SerializeField] private Transform spawnPoint;

    public override void OnInteract()
    {
        printerAnimator.SetTrigger("Print");
    }

    public void SpawnPaper()
    {
        Instantiate(paperPrefab, spawnPoint.position, spawnPoint.rotation);

        base.OnInteract();
    }
}