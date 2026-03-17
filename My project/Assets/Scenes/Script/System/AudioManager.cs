using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("Button Sounds")]
    public AudioClip clickSound;
    public AudioClip hoverSound;

    [Header("Game Sounds")]
    public AudioClip moveSound;
    public AudioClip drinkSound;
    public AudioClip computerSound;
    public AudioClip printerSound;
    public AudioClip phoneSound;
    public AudioClip shitSound;

    public AudioSource sourceSFX;
    public AudioSource sourceBGM;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            transform.SetParent(null);
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        sourceSFX.playOnAwake = false;
        sourceBGM.playOnAwake = false;
    }

    public void PlaySound(AudioClip clip, float volume = 1f)
    {
        if (clip != null)
        {
            sourceSFX.PlayOneShot(clip, volume);
        }
    }

    public void PlayClick()
    {
        PlaySound(clickSound);
    }

    public void PlayHover()
    {
        PlaySound(hoverSound);
    }

    public void PlayerMove()
    {
        PlaySound(moveSound);
    }

    public void PlayerDrink()
    {
        PlaySound(drinkSound);
    }

    public void PlayerComputer()
    {
        PlaySound(computerSound);
    }

    public void PlayerTelephone()
    {
        PlaySound(phoneSound);
    }

    public void PlayerPrinter()
    {
        PlaySound(printerSound);
    }

    public void PlayerShit()
    {
        PlaySound(shitSound);
    }
}
