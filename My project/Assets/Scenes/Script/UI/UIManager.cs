using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class UIManager: MonoBehaviour
{
    public static UIManager Instance;
    
    [SerializeField] private GameObject dialogPanel;
    [SerializeField] private GameObject computerPanel;
    [SerializeField] private GameObject leftPanel;
    [SerializeField] private GameObject rightPanel;
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI contentText;

    void Awake()
    {
        Instance = this;
    }
   IEnumerator Start()
{
    dialogPanel.SetActive(false);
    leftPanel.SetActive(false);
    rightPanel.SetActive(false);
    computerPanel.SetActive(false);

    yield return null; // 等一帧，保证所有 Awake 完成

    if (DialogueManager.Instance != null)
    {
        DialogueManager.Instance.OnLineStarted += ShowLine;
        DialogueManager.Instance.OnDialogueEnded += HideUI;
    }

    if (TaskChoose.Instance != null)
    {
        TaskChoose.Instance.OnChoicePanelShown += HideTaskResultPanels;
        TaskChoose.Instance.OnChooseLeft += ShowLeftPanel;
        TaskChoose.Instance.OnChooseRight += ShowRightPanel;
    }
}

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E)
            && DialogueManager.Instance != null
            && DialogueManager.Instance.CanAdvanceDialogue)
        {
            OnClickNext();

        }
        
    }

    private void ShowLine(DialogueLine line)//展示对应line的文字和name
    {
        dialogPanel.SetActive(true);
        nameText.text = line.name;
        contentText.text = line.content; // 这里之后可以加打字机效果
    }

    private void HideUI() => dialogPanel.SetActive(false);

    // 玩家点击对话框推进下一句
    public void OnClickNext()
    {
        DialogueManager.Instance.DisplayNextLine();
    }

    private void ShowLeftPanel()
    {
        HideTaskResultPanels();
        leftPanel.SetActive(true);
    }

    private void ShowRightPanel()
    {
        HideTaskResultPanels();
        rightPanel.SetActive(true);
    }

    private void HideTaskResultPanels()
    {
        leftPanel.SetActive(false);
        rightPanel.SetActive(false);
    }

    public void OpenComputerPanel()
    {
        computerPanel.SetActive(true);
         Cursor.lockState = CursorLockMode.None;
    Cursor.visible = true;
    }

    public void Email()
    {

    }

    public void Form()
    {

    }
    public void Close()
    {
        computerPanel.SetActive(false);
         Cursor.lockState = CursorLockMode.None;
    Cursor.visible = false;

    }
}
