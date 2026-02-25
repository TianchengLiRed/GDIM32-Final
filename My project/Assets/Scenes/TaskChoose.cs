using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class TaskChoose : MonoBehaviour
{
    public static TaskChoose Instance;

    public GameObject choosePanel;
    public Button left;
    public Button right;
    [SerializeField] private DialogueData DialogueLeft;
    [SerializeField] private DialogueData DialogueRight;

    public event Action OnChooseLeft; 
    public event Action OnChooseRight; 
    private bool intasking = false;
    // Start is called before the first frame update
    void Start()
    {
        choosePanel.SetActive(false);
        left.onClick.AddListener(()=>LeftTaskChoosed());
        right.onClick.AddListener(()=>RightTaskChoosed());

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ActivePanel()
    {
        choosePanel.SetActive(true);

    }
    public void LeftTaskChoosed()
    {
        choosePanel.SetActive(false);
        DialogueManager.Instance.StartDialogue(DialogueLeft);
        OnChooseLeft?.Invoke();

    }
    public void RightTaskChoosed()
    {
        choosePanel.SetActive(false);
        DialogueManager.Instance.StartDialogue(DialogueRight);
        OnChooseRight?.Invoke();

    }
}
