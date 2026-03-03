using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioManager : MonoBehaviour
{
    
    // 单例实例
    public static AudioManager Instance;

    [Header("Button Sounds")]
    // private AudioSource audioSource;
    public AudioClip clickSound;
    public AudioClip hoverSound;
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
        // 确保场景中只有一个 AudioManager
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

        // Load saved audio settings when switch scenes
        // LoadAudioSettings();
    }

    // 提供一个通用的播放接口
    public void PlaySound(AudioClip clip, float volume = 1f)
    {
        if (clip != null)
        {
            sourceSFX.PlayOneShot(clip, volume);
        }
    }

    public void PlayClick()
    {
        if (clickSound != null)
            sourceSFX.PlayOneShot(clickSound);
    }

    public void PlayHover()
    {
        if (hoverSound != null)
            sourceSFX.PlayOneShot(hoverSound);
    }

    public void PlayerMove()
    {
        if (clickSound != null)
            sourceSFX.PlayOneShot(moveSound);

    }
    public void PlayerDrink()
    {
         if (clickSound != null)
            sourceSFX.PlayOneShot(drinkSound);

    }
    public void PlayerComputer()
    {
        if (clickSound != null)
            sourceSFX.PlayOneShot(computerSound);

    }
    public void PlayerTelephone()
    {
        if (clickSound != null)
            sourceSFX.PlayOneShot(phoneSound);

    }
    public void PlayerPrinter()
    {
        if (clickSound != null)
            sourceSFX.PlayOneShot(printerSound);

    }
    public void PlayerShit()
    {
        if (clickSound != null)
            sourceSFX.PlayOneShot(shitSound);

    }
}

