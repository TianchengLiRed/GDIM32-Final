using UnityEngine;

public class TimerManager : MonoBehaviour
{
    public static TimerManager Instance;

    public float timeRemaining;
    private bool isRunning = false;

    private void Awake()
    {
        Instance = this;
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
        GUI.Box(new Rect(12, 12, 120, 32), $"时间 {minutes:00}:{seconds:00}");
    }
}

