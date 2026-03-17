using UnityEngine;

public class TimerManager : MonoBehaviour
{
    public static TimerManager Instance;

    public float timeRemaining;
    private bool isRunning = false;

    [SerializeField]private GameObject endPanel;

    private void Awake()
    {
        Instance = this;
        endPanel.SetActive(false);
    }

    void Update()
    {
        if (isRunning)
        {
            timeRemaining -= Time.deltaTime;

            if (timeRemaining <= 0)
            {
                timeRemaining = 0;
                isRunning = false;
                Debug.Log("Game Over");
                endPanel.SetActive(true);
            }
        }
    }

    public void StartTimer(float duration)
    {
        timeRemaining = duration;
        isRunning = true;
    }

    public void ResetTimer()
    {
        timeRemaining = 0;
        isRunning = false;
    }

    void OnGUI()
    {
        int totalSeconds = Mathf.CeilToInt(timeRemaining);
        int minutes = totalSeconds / 60;
        int seconds = totalSeconds % 60;
        GUI.Box(new Rect(12, 12, 120, 32), $"Time {minutes:00}:{seconds:00}");
    }
}

