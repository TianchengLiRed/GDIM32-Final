using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class InteractiveItems : MonoBehaviour
{
    [Header("basic Items")]
    public string ItemNames;
    public AudioSource audioSource;
    public AudioClip InteractionAudio;

    public abstract void Interact()
    {
        if (audioSource != null && InteractiveAudio != null)
        {
            audioSource.PlayOneShot(InteractiveAudio);
        }
    }

    
    // Start is called before the first frame update

}
