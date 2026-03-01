using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.Serialization;

public class TaskChoose : MonoBehaviour
{
    public static TaskChoose Instance;

    private enum TaskBranch
    {
        None,
        Left,
        Right
    }

    [FormerlySerializedAs("choosePanel")]
    [SerializeField] private GameObject choiceCanvasRoot;
    [FormerlySerializedAs("left")]
    [SerializeField] private Button leftChoiceButton;
    [FormerlySerializedAs("right")]
    [SerializeField] private Button rightChoiceButton;
    [SerializeField] private DialogueData dialogueLeft;
    [SerializeField] private DialogueData dialogueRight;

    private TaskBranch pendingTask = TaskBranch.None;
    private bool taskLockedIn = false;

    public bool IsChoicePanelOpen => choiceCanvasRoot != null && choiceCanvasRoot.activeSelf;
    public bool HasPendingTask => pendingTask != TaskBranch.None;
    public bool HasTaskBeenChosen => taskLockedIn || pendingTask != TaskBranch.None;

    public event Action OnChoicePanelShown;
    public event Action OnChooseLeft; 
    public event Action OnChooseRight; 
    // Start is called before the first frame update
    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        if (choiceCanvasRoot != null) choiceCanvasRoot.SetActive(false);
        if (leftChoiceButton != null) leftChoiceButton.onClick.AddListener(() => SelectTask(TaskBranch.Left));
        if (rightChoiceButton != null) rightChoiceButton.onClick.AddListener(() => SelectTask(TaskBranch.Right));

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
        ShowChoicePanel();
    }

    public void ShowChoicePanel()
    {
        if (taskLockedIn || pendingTask != TaskBranch.None) return;
        if (choiceCanvasRoot == null) return;
        if (choiceCanvasRoot != null) choiceCanvasRoot.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        OnChoicePanelShown?.Invoke();
    }

    private void SelectTask(TaskBranch branch)
    {
        if (taskLockedIn) return;

        HideChoicePanel();
        pendingTask = branch;
        taskLockedIn = true;
        TryStartOrGuideCoffee();
    }

    private void HideChoicePanel()
    {
        if (choiceCanvasRoot != null) choiceCanvasRoot.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void TryStartOrGuideCoffee()
    {
        if (TaskFlowManager.Instance != null
            && TaskFlowManager.Instance.RequireCoffeeBeforeTask
            && !TaskFlowManager.Instance.CanStartWork)
        {
            TaskFlowManager.Instance.BeginCoffeeStep();
            Debug.Log("已选择任务，先去喝咖啡，喝完后正式开始任务。");
            return;
        }

        StartPendingTask();
    }

    private void StartPendingTask()
    {
        switch (pendingTask)
        {
            case TaskBranch.Left:
                OnChooseLeft?.Invoke();
                break;
            case TaskBranch.Right:
                OnChooseRight?.Invoke();
                break;
        }

        pendingTask = TaskBranch.None;
    }
}
