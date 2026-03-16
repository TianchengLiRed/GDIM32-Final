using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class PhoneInteractable : Interactable
{
    [Header("Task Control")]
    [SerializeField] private bool autoStartOnPlay = false;
    [SerializeField] private bool autoStartOnTaskChoice = true;
    [SerializeField] private bool startOnLeftChoice = true;
    [SerializeField] private bool startOnRightChoice = true;
    [SerializeField] private int requiredAnsweredCalls = 1;

    [Header("Call Timing")]
    [SerializeField] private float firstCallDelay = 3f;
    [SerializeField] private Vector2 callIntervalRange = new Vector2(8f, 16f);
    [SerializeField] private float ringDuration = 6f;

    [Header("Prompt")]
    [SerializeField] private string idlePrompt = "Phone is idle";
    [SerializeField] private string ringingPrompt = "Press E to answer";
    [SerializeField] private string guidePrompt = "Phone is ringing! Go answer it";
    [SerializeField] private bool guideWhileRinging = true;

    [Header("Ring SFX")]
    [SerializeField] private AudioSource ringSource;
    [SerializeField] private AudioClip ringClip;
    [SerializeField] private float ringVolume = 1f;

    [Header("Events")]
    [SerializeField] private UnityEvent onCallStarted;
    [SerializeField] private UnityEvent onCallMissed;
    [SerializeField] private UnityEvent onCallAnswered;
    [SerializeField] private UnityEvent onTaskCompleted;

    private Coroutine callLoopRoutine;
    private Coroutine ringRoutine;
    private bool taskActive;
    private bool isRinging;
    private int answeredCount;

    public bool TaskActive => taskActive;
    public bool IsRinging => isRinging;
    public int AnsweredCount => answeredCount;

    public override string PromptText => isRinging ? ringingPrompt : idlePrompt;

    protected override void Awake()
    {
        base.Awake();

        if (ringSource == null)
        {
            ringSource = GetComponent<AudioSource>();
        }

        if (ringSource == null)
        {
            ringSource = gameObject.AddComponent<AudioSource>();
        }

        ringSource.playOnAwake = false;
        ringSource.loop = true;
        ringSource.spatialBlend = 1f;

        if (ringClip == null && AudioManager.Instance != null)
        {
            ringClip = AudioManager.Instance.phoneSound;
        }
    }

    private void Start()
    {
        if (autoStartOnTaskChoice && TaskChoose.Instance != null)
        {
            if (startOnLeftChoice)
            {
                TaskChoose.Instance.OnChooseLeft += StartPhoneTask;
            }

            if (startOnRightChoice)
            {
                TaskChoose.Instance.OnChooseRight += StartPhoneTask;
            }
        }

        if (autoStartOnPlay)
        {
            StartPhoneTask();
        }
    }

    private void OnDestroy()
    {
        if (TaskChoose.Instance != null)
        {
            TaskChoose.Instance.OnChooseLeft -= StartPhoneTask;
            TaskChoose.Instance.OnChooseRight -= StartPhoneTask;
        }
    }

    public void StartPhoneTask()
    {
        if (taskActive) return;

        taskActive = true;
        answeredCount = 0;
        StopAllCallCoroutines();
        callLoopRoutine = StartCoroutine(CallLoop());
    }

    public void StopPhoneTask()
    {
        taskActive = false;
        StopAllCallCoroutines();
        StopRinging();
    }

    public override void OnInteract()
    {
        if (!taskActive || !isRinging)
        {
            return;
        }

        base.OnInteract();
        answeredCount++;
        onCallAnswered?.Invoke();
        StopRinging();

        if (answeredCount >= requiredAnsweredCalls)
        {
            taskActive = false;
            onTaskCompleted?.Invoke();
            return;
        }

        callLoopRoutine = StartCoroutine(CallLoop());
    }

    private IEnumerator CallLoop()
    {
        if (!taskActive) yield break;

        float delay = answeredCount == 0 ? firstCallDelay : Random.Range(callIntervalRange.x, callIntervalRange.y);
        if (delay > 0f)
        {
            yield return new WaitForSeconds(delay);
        }

        if (!taskActive) yield break;

        ringRoutine = StartCoroutine(RingWindow());
    }

    private IEnumerator RingWindow()
    {
        isRinging = true;
        onCallStarted?.Invoke();
        BeginRingAudio();

        if (guideWhileRinging && TaskFlowManager.HasInstance)
        {
            TaskFlowManager.Instance.SetObjective(this, guidePrompt);
        }

        float timer = 0f;
        while (timer < ringDuration && isRinging)
        {
            timer += Time.deltaTime;
            yield return null;
        }

        if (isRinging)
        {
            onCallMissed?.Invoke();
            StopRinging();
            if (taskActive)
            {
                callLoopRoutine = StartCoroutine(CallLoop());
            }
        }

        ringRoutine = null;
    }

    private void BeginRingAudio()
    {
        if (ringSource == null || ringClip == null) return;

        ringSource.clip = ringClip;
        ringSource.volume = ringVolume;
        ringSource.Play();
    }

    private void StopRinging()
    {
        isRinging = false;

        if (ringSource != null && ringSource.isPlaying)
        {
            ringSource.Stop();
        }

        if (guideWhileRinging && TaskFlowManager.HasInstance)
        {
            TaskFlowManager.Instance.ClearObjective(this);
        }

        if (ringRoutine != null)
        {
            StopCoroutine(ringRoutine);
            ringRoutine = null;
        }
    }

    private void StopAllCallCoroutines()
    {
        if (callLoopRoutine != null)
        {
            StopCoroutine(callLoopRoutine);
            callLoopRoutine = null;
        }

        if (ringRoutine != null)
        {
            StopCoroutine(ringRoutine);
            ringRoutine = null;
        }
    }
}
