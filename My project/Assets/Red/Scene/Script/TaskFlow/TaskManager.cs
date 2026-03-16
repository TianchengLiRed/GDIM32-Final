using System.Collections.Generic;
using UnityEngine;

public class TaskManager : MonoBehaviour
{
    public static TaskManager Instance;

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

    private List<TaskData> activeTasks = new List<TaskData>();

    public bool HasAcceptedTask { get; private set; }
    public bool AllTasksCompleted { get; private set; }

    private List<TaskType> allPossibleTasks = new List<TaskType>()
    {
        TaskType.CheckComputer,
        TaskType.TalkBoatman,
        TaskType.VisitDock,
        TaskType.TalkDrugDealer,
        TaskType.CheckApartment,
        TaskType.ReadRecord
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
            case TaskType.CheckComputer: return "Check the computer";
            case TaskType.TalkBoatman: return "Talk to the boatman";
            case TaskType.VisitDock: return "Go to the dock";
            case TaskType.TalkDrugDealer: return "Talk to the drug dealer";
            case TaskType.CheckApartment: return "Investigate the apartment";
            case TaskType.ReadRecord: return "Read the voyage records";
        }

        return "未知任务";
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
        
    }

public void AcceptYes()
{
    TaskListUI.Instance.ShowPanel();
    Debug.Log("Accepted Yes");
    AcceptMainTask(3);
    ChoicePanel.SetActive(false);
}

public void AcceptMore()
{
    TaskListUI.Instance.ShowPanel();
    Debug.Log("Accepted More");
    AcceptMainTask(5);
    ChoicePanel.SetActive(false);
}
}
