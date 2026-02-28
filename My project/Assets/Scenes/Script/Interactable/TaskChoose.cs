using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class TaskChoose : MonoBehaviour
{
    public static TaskChoose Instance;

    private enum TaskBranch
    {
        None,
        Left,
        Right
    }

    public GameObject choosePanel;
    public Button left;
    public Button right;
    [SerializeField] private DialogueData dialogueLeft;
    [SerializeField] private DialogueData dialogueRight;

    private TaskBranch pendingTask = TaskBranch.None;

    public event Action OnChooseLeft; 
    public event Action OnChooseRight; 
    // Start is called before the first frame update
    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        choosePanel.SetActive(false);
        left.onClick.AddListener(() => SelectTask(TaskBranch.Left));
        right.onClick.AddListener(() => SelectTask(TaskBranch.Right));

        if (TaskFlowManager.Instance != null)
        {
            TaskFlowManager.Instance.OnCoffeeReadyToWork += StartPendingTask;
        }

    }

    private void OnDestroy()
    {
        if (TaskFlowManager.HasInstance)
        {
            TaskFlowManager.Instance.OnCoffeeReadyToWork -= StartPendingTask;
        }
    }

    // Update is called once per frame
    public void ActivePanel()
    {
        choosePanel.SetActive(true);
    }

    private void SelectTask(TaskBranch branch)
    {
        choosePanel.SetActive(false);
        pendingTask = branch;
        TryStartOrGuideCoffee();
    }

    private void TryStartOrGuideCoffee()
    {
        if (TaskFlowManager.Instance != null
            && TaskFlowManager.Instance.RequireCoffeeBeforeTask
            && !TaskFlowManager.Instance.CanStartWork)
        {
            TaskFlowManager.Instance.BeginCoffeeStep();
            Debug.Log("先去喝咖啡，开启倒计时后再开始任务。");
            return;
        }

        StartPendingTask();
    }

    private void StartPendingTask()
    {
        switch (pendingTask)
        {
            case TaskBranch.Left:
                DialogueManager.Instance.StartDialogue(dialogueLeft);
                OnChooseLeft?.Invoke();
                break;
            case TaskBranch.Right:
                DialogueManager.Instance.StartDialogue(dialogueRight);
                OnChooseRight?.Invoke();
                break;
        }

        pendingTask = TaskBranch.None;
    }
}
