using System.Collections.Generic;
using UnityEngine;

public class TaskListUI : MonoBehaviour
{
    public static TaskListUI Instance;

    [SerializeField] private GameObject panel;
    [SerializeField] private List<TaskItemUI> taskSlots;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        panel.SetActive(false); // 一开始不显示
    }

    public void ShowTasks(List<string> taskTexts)
    {
        panel.SetActive(true);

        for (int i = 0; i < taskSlots.Count; i++)
        {
            if (i < taskTexts.Count)
            {
                taskSlots[i].gameObject.SetActive(true);
                taskSlots[i].Setup(taskTexts[i]);
            }
            else
            {
                taskSlots[i].gameObject.SetActive(false);
            }
        }
    }

    public void CompleteTaskUI(int index)
    {
        if (index >= 0 && index < taskSlots.Count)
        {
            taskSlots[index].Complete();
        }
    }

    public void HideAll()
    {
        panel.SetActive(false);
    }

    public void ShowPanel(){
        panel.SetActive(true);
    }

    
}
