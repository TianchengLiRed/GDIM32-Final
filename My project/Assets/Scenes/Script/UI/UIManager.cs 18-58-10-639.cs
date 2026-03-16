/*
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
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

    [Header("Task Panels")]
    [SerializeField] private GameObject computerPanel;
    [SerializeField] private GameObject leftPanel;
    [SerializeField] private GameObject rightPanel;

    [Header("Checklist")]
    [SerializeField] private ChecklistBinding[] leftChecklist;
    [SerializeField] private ChecklistBinding[] rightChecklist;

    private Coroutine typingCoroutine;
    private readonly HashSet<Interactable> completedInteractables = new HashSet<Interactable>();
    private readonly Dictionary<TextMeshProUGUI, string> baseChecklistText = new Dictionary<TextMeshProUGUI, string>();
    private ChecklistBinding[] activeChecklist;

    [Serializable]
    private class ChecklistBinding
    {
        public Interactable target;
        public TextMeshProUGUI text;
        public string pendingPrefix = "[ ] ";
        public string donePrefix = "[x] ";
    }

    private void Awake()
    {
        Instance = this;
    }

    private IEnumerator Start()
    {
        if (dialoguePanel != null) dialoguePanel.SetActive(false);
        if (leftPanel != null) leftPanel.SetActive(false);
        if (rightPanel != null) rightPanel.SetActive(false);
        if (computerPanel != null) computerPanel.SetActive(false);

        if (_branchButtons != null)
        {
            for (int i = 0; i < _branchButtons.Count; i++)
            {
                if (_branchButtons[i] != null)
                {
                    _branchButtons[i].gameObject.SetActive(false);
                }
            }
        }

        yield return null;

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

        Interactable.OnInteracted += HandleInteractableCompleted;
        CacheChecklistBaseTexts(leftChecklist);
        CacheChecklistBaseTexts(rightChecklist);
        ResetChecklistVisuals(leftChecklist);
        ResetChecklistVisuals(rightChecklist);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E)
            && DialogueManager.Instance != null
            && DialogueManager.Instance.CanAdvanceDialogue)
        {
            OnClickNext();
        }
    }

    private void OnDestroy()
    {
        if (DialogueManager.Instance != null)
        {
            DialogueManager.Instance.OnLineStarted -= ShowLine;
            DialogueManager.Instance.OnDialogueEnded -= HideUI;
            DialogueManager.Instance.OnDialogueReplied -= ShowChoice;
        }

        if (TaskChoose.Instance != null)
        {
            TaskChoose.Instance.OnChoicePanelShown -= HideTaskResultPanels;
            TaskChoose.Instance.OnChooseLeft -= ShowLeftPanel;
            TaskChoose.Instance.OnChooseRight -= ShowRightPanel;
        }

        Interactable.OnInteracted -= HandleInteractableCompleted;
    }

    private void ShowLine(DialogueLine line)
    {
        if (line == null) return;

        if (dialoguePanel != null) dialoguePanel.SetActive(true);
        if (_nameText != null) _nameText.text = line.speakerName;
        if (_portraitImage != null) _portraitImage.sprite = line.portrait;

        ShowText(line.content);

        if (_branchButtons == null) return;
        for (int i = 0; i < _branchButtons.Count; i++)
        {
            if (_branchButtons[i] != null)
            {
                _branchButtons[i].gameObject.SetActive(false);
            }
        }
    }

    public void ShowText(string content)
    {
        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
        }

        typingCoroutine = StartCoroutine(TypeText(content));
    }

    private IEnumerator TypeText(string content)
    {
        if (_contentText == null) yield break;

        _contentText.text = string.Empty;
        if (string.IsNullOrEmpty(content))
        {
            typingCoroutine = null;
            yield break;
        }

        foreach (char c in content)
        {
            _contentText.text += c;
            yield return new WaitForSeconds(typeSpeed);
        }

        typingCoroutine = null;
    }

    private void ShowChoice(DialogueLine line)
    {
        if (_branchButtons == null || _branchTexts == null || line == null) return;

        for (int i = 0; i < _branchButtons.Count; i++)
        {
            if (_branchButtons[i] == null) continue;

            bool hasOption = line.replyOptions != null && i < line.replyOptions.Count;
            _branchButtons[i].gameObject.SetActive(hasOption);

            if (!hasOption) continue;

            if (i < _branchTexts.Count && _branchTexts[i] != null)
            {
                _branchTexts[i].text = line.replyOptions[i];
            }

            int index = i;
            _branchButtons[i].onClick.RemoveAllListeners();
            _branchButtons[i].onClick.AddListener(() =>
            {
                if (DialogueManager.Instance != null)
                {
                    DialogueManager.Instance.SelectReply(index);
                }
            });
        }
    }

    private void HideUI()
    {
        if (dialoguePanel != null) dialoguePanel.SetActive(false);

        if (_branchButtons == null) return;
        for (int i = 0; i < _branchButtons.Count; i++)
        {
            if (_branchButtons[i] != null)
            {
                _branchButtons[i].gameObject.SetActive(false);
            }
        }
    }

    public void OnClickNext()
    {
        if (DialogueManager.Instance == null) return;

        if (DialogueManager.Instance.IsWaitingLoopChoice)
        {
            DialogueManager.Instance.DisplayNextLine();
            return;
        }

        if (_branchButtons != null)
        {
            for (int i = 0; i < _branchButtons.Count; i++)
            {
                if (_branchButtons[i] != null && _branchButtons[i].gameObject.activeSelf)
                {
                    return;
                }
            }
        }

        DialogueManager.Instance.DisplayNextLine();
    }

    private void ShowLeftPanel()
    {
        HideTaskResultPanels();
        BeginChecklist(leftChecklist);
        if (leftPanel != null) leftPanel.SetActive(true);
    }

    private void ShowRightPanel()
    {
        HideTaskResultPanels();
        BeginChecklist(rightChecklist);
        if (rightPanel != null) rightPanel.SetActive(true);
    }

    private void HideTaskResultPanels()
    {
        if (leftPanel != null) leftPanel.SetActive(false);
        if (rightPanel != null) rightPanel.SetActive(false);
    }

    public void OpenComputerPanel()
    {
        if (computerPanel != null) computerPanel.SetActive(true);
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
        if (computerPanel != null) computerPanel.SetActive(false);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = false;
    }

    private void BeginChecklist(ChecklistBinding[] checklist)
    {
        completedInteractables.Clear();
        activeChecklist = checklist;
        RefreshChecklist(checklist);
    }

    private void HandleInteractableCompleted(Interactable interactable)
    {
        if (interactable == null || activeChecklist == null || activeChecklist.Length == 0) return;

        for (int i = 0; i < activeChecklist.Length; i++)
        {
            if (activeChecklist[i] != null && activeChecklist[i].target == interactable)
            {
                completedInteractables.Add(interactable);
                RefreshChecklist(activeChecklist);
                return;
            }
        }
    }

    private void CacheChecklistBaseTexts(ChecklistBinding[] checklist)
    {
        if (checklist == null) return;

        for (int i = 0; i < checklist.Length; i++)
        {
            ChecklistBinding binding = checklist[i];
            if (binding == null || binding.text == null) continue;
            if (baseChecklistText.ContainsKey(binding.text)) continue;
            baseChecklistText[binding.text] = binding.text.text;
        }
    }

    private void ResetChecklistVisuals(ChecklistBinding[] checklist)
    {
        if (checklist == null) return;

        for (int i = 0; i < checklist.Length; i++)
        {
            ChecklistBinding binding = checklist[i];
            if (binding == null || binding.text == null) continue;

            string baseText = GetBaseText(binding.text);
            binding.text.text = $"{binding.pendingPrefix}{baseText}";
        }
    }

    private void RefreshChecklist(ChecklistBinding[] checklist)
    {
        if (checklist == null) return;

        for (int i = 0; i < checklist.Length; i++)
        {
            ChecklistBinding binding = checklist[i];
            if (binding == null || binding.text == null) continue;

            string baseText = GetBaseText(binding.text);
            bool isDone = binding.target != null && completedInteractables.Contains(binding.target);
            binding.text.text = isDone
                ? $"{binding.donePrefix}<s>{baseText}</s>"
                : $"{binding.pendingPrefix}{baseText}";
        }
    }

    private string GetBaseText(TextMeshProUGUI textComp)
    {
        if (textComp == null) return string.Empty;
        if (baseChecklistText.TryGetValue(textComp, out string value)) return value;

        value = textComp.text;
        baseChecklistText[textComp] = value;
        return value;
    }
}
*/