using System.Collections.Generic;
using UnityEngine;

public class TaskManager : MonoBehaviour
{
    public static TaskManager Instance;
    private int pendingTaskCount = 0;

    private class TaskData
    {
        public TaskType type;
        public string text;
        public bool completed;

        public TaskData(TaskType type, string text)
        {
            this.type = type;
            this.text = text;
            this.completed = false;
        }
    }
    [SerializeField] private GameObject ChoicePanel;
    [SerializeField] private DialogueData yesDialogue;
    [SerializeField] private DialogueData moreDialogue;


    private List<TaskData> activeTasks = new List<TaskData>();

    public bool HasAcceptedTask { get; private set; }
    public bool AllTasksCompleted { get; private set; }
    public bool OpentheDoor = false;

    private List<TaskType> allPossibleTasks = new List<TaskType>()
    {
        TaskType.FinishEmail,
        TaskType.TakeTelephone,
        TaskType.TalkWithCoworker,
        TaskType.FinishForm,
        TaskType.FinishEmail,
        TaskType.CheckNotes
    };

    private void Awake()
    {
        Instance = this;
        ChoicePanel.SetActive(false);
    }

  public void AcceptMainTask(int taskCount)
{
    if (HasAcceptedTask) return;

    HasAcceptedTask = true;
    GenerateRandomTasks(taskCount);
}

    private void GenerateRandomTasks(int count)
{
    activeTasks.Clear();
    AllTasksCompleted = false;

    List<TaskType> pool = new List<TaskType>(allPossibleTasks);

    count = Mathf.Clamp(count, 1, pool.Count);

    for (int i = 0; i < count; i++)
    {
        int randomIndex = Random.Range(0, pool.Count);
        TaskType selected = pool[randomIndex];
        pool.RemoveAt(randomIndex);

        activeTasks.Add(new TaskData(selected, GetTaskText(selected)));
    }

    List<string> texts = new List<string>();
    foreach (var task in activeTasks)
    {
        texts.Add(task.text);
    }

    TaskListUI.Instance.ShowTasks(texts);
}
    private string GetTaskText(TaskType type)
    {
        switch (type)
        {
            case TaskType.TakeTelephone: return "take the telephone";
            case TaskType.TalkWithCoworker: return "talk with your coworker";
            case TaskType.FinishForm: return "finish the form";
            case TaskType.PrintReport: return "print the report";
            case TaskType.FinishEmail: return "send the email";
            case TaskType.CheckNotes: return "check your notes";
        }

        return "none";
    }

    public void CompleteTask(TaskType type)
    {
        if (!HasAcceptedTask) return;
        if (AllTasksCompleted) return;

        for (int i = 0; i < activeTasks.Count; i++)
        {
            if (activeTasks[i].type == type && !activeTasks[i].completed)
            {
                activeTasks[i].completed = true;
                TaskListUI.Instance.CompleteTaskUI(i);
                CheckAllTasksCompleted();
                return;
            }
        }
    }

    private void CheckAllTasksCompleted()
    {
        foreach (var task in activeTasks)
        {
            if (!task.completed)
            {
                AllTasksCompleted = false;
                return;
            }
        }

        AllTasksCompleted = true;
        Debug.Log("所有任务完成，可以回去找 Boss 了");
    }

    public void ShowChoice(){
        ChoicePanel.SetActive(true);
        DialogueManager.Instance.ShowCursor();
        
    }

public void AcceptYes()
{
    Debug.Log("Accepted Yes");
    pendingTaskCount = 3;
    ChoicePanel.SetActive(false);

    if (DialogueManager.Instance != null && yesDialogue != null)
    {
        DialogueManager.Instance.StartDialogue(yesDialogue);
    }
}

public void AcceptMore()
{
    Debug.Log("Accepted More");
    pendingTaskCount = 5;
    ChoicePanel.SetActive(false);

    if (DialogueManager.Instance != null && moreDialogue != null)
    {
        DialogueManager.Instance.StartDialogue(moreDialogue);
    }
}

public void StartPendingTask()
{
    if (HasAcceptedTask) return;
    if (pendingTaskCount <= 0) return;

    AcceptMainTask(pendingTaskCount);
}

public void OnEndClicked()
{
    Application.Quit();
}
}
