using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

  [Header("BGM")]
    public AudioClip bgmMusic;

    [Header("Game Sounds")]
    public AudioClip moveSound;
    public AudioClip drinkSound;
    public AudioClip computerSound;
    public AudioClip printerSound;
    public AudioClip phoneSound;
    public AudioClip shitSound;
    public AudioClip doorSound;
    public AudioClip TalkSound;

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

    private void Start()
    {
        PlayBGM();   // 游戏开始自动播放BGM
    }

    // 播放BGM（循环）
    public void PlayBGM()
    {
        if (bgmMusic == null) return;

        sourceBGM.clip = bgmMusic;
        sourceBGM.loop = true;
        sourceBGM.Play();
    } 
    public void PlaySound(AudioClip clip, float volume = 1f)
    {
        if (clip != null)
        {
            sourceSFX.PlayOneShot(clip, volume);
        }
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
     public void PlayerTalk()
    {
        PlaySound(TalkSound);
    }
}

