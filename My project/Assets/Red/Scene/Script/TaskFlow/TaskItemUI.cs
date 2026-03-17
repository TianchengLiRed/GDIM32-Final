using TMPro;
using UnityEngine;

public enum TaskType
{
    None,
    FinishEmail,
    FinishForm,
    TalkWithCoworker,
    PrintReport,
    CheckNotes,
    TakeTelephone
}
public class TaskItemUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI taskText;

    private string originalText;
    private bool completed;

    public void Setup(string text)
    {
        originalText = text;
        completed = false;
        Refresh();
    }

    public void Complete()
    {
        completed = true;
        Refresh();
    }

    private void Refresh()
    {
        if (completed)
        {
            taskText.text = originalText;
            taskText.color = Color.green;
        }
        else
        {
            taskText.text = originalText;
            taskText.color = Color.black;
        }
    }
}