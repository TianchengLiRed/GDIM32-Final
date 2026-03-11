using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UIManager: MonoBehaviour
{
    public static UIManager Instance;
    [Header("DialogueRelated")]
    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private TextMeshProUGUI _nameText;
    [SerializeField] private Image _portraitImage;

    [SerializeField] private List<TextMeshProUGUI> _branchTexts;
    [SerializeField] private List<Button> _branchButtons;
    [Header("TypeText")]
    [SerializeField] private TextMeshProUGUI _contentText;
    [SerializeField] private float typeSpeed = 0.03f;

    private Coroutine typingCoroutine;


    [SerializeField] private GameObject computerPanel;
    [SerializeField] private GameObject leftPanel;
    [SerializeField] private GameObject rightPanel;


    void Awake()
    {
        Instance = this;
    }
   IEnumerator Start()
{
    dialoguePanel.SetActive(false);
    for(int i =0; i < _branchButtons.Count; i++)
    {
        _branchButtons[i].gameObject.SetActive(false);
    }
    leftPanel.SetActive(false);
    rightPanel.SetActive(false);
    computerPanel.SetActive(false);

    yield return null; // 等一帧，保证所有 Awake 完成

    if (DialogueManager.Instance != null)
    {
        DialogueManager.Instance.OnLineStarted += ShowLine;
        DialogueManager.Instance.OnDialogueEnded += HideUI;
        DialogueManager.Instance.OnDialogueReplied += ShowChoice;
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
        dialoguePanel.SetActive(true);
        _nameText.text = line.speakerName;
        ShowText(line.content); // 这里之后可以加打字机效果
        _portraitImage.sprite = line.portrait;

        for(int i = 0; i < _branchButtons.Count; i++)
        {
            _branchButtons[i].gameObject.SetActive(false);   
        }
    }

    public void ShowText(string content)
    {
        if(typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
        }

        typingCoroutine = StartCoroutine(TypeText(content));
    }

    private IEnumerator TypeText(string content)
    {
        _contentText.text = "";
          foreach (char c in content)
        {
            _contentText.text += c;
            yield return new WaitForSeconds(typeSpeed);
        }

        typingCoroutine = null;
    }

    private void ShowChoice(DialogueLine line){
        for(int i = 0; i < _branchButtons.Count; i++)
        {
            if(line.replyOptions != null && i < line.replyOptions.Count)
            {
                _branchButtons[i].gameObject.SetActive(true);
                _branchTexts[i].text = line.replyOptions[i];

                int index = i;
                _branchButtons[i].onClick.RemoveAllListeners();
                _branchButtons[i].onClick.AddListener(()=>{
                    DialogueManager.Instance.SelectReply(index);
                });
            }
        }
    }

    private void HideUI() 
    {
        dialoguePanel.SetActive(false);

        for (int i = 0; i < _branchButtons.Count; i++)
        {
            _branchButtons[i].gameObject.SetActive(false);
        }
    }

    // 玩家点击对话框推进下一句
    public void OnClickNext()
    {
        if (DialogueManager.Instance.IsWaitingLoopChoice)
        {
            DialogueManager.Instance.DisplayNextLine();
         return;
       }

        for (int i = 0; i < _branchButtons.Count; i++)
        {
            if (_branchButtons[i].gameObject.activeSelf)
               return;
        }

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
