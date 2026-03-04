using System;
using UnityEngine;
using UnityEngine.Serialization;

public class TaskFlowManager : MonoBehaviour
{
    private static TaskFlowManager _instance;
    public static bool HasInstance => _instance != null;
    public static TaskFlowManager Instance
    {
        get
        {
            if (_instance == null)
            {
                GameObject go = new GameObject("TaskFlowManager");
                _instance = go.AddComponent<TaskFlowManager>();
            }
            return _instance;
        }
    }

    public enum FlowState
    {
        None,
        NeedCoffee,
        CoffeeDone
    }

    [SerializeField] private bool requireCoffeeBeforeTask = true;

    [Header("Top Prompt")]
    [FormerlySerializedAs("coffeePromptMessage")]
    [SerializeField] private string defaultCoffeePromptMessage = "Go drink coffee before starting work";
    [SerializeField] private Color promptTextColor = new Color(1f, 0.93f, 0.2f, 1f);
    [SerializeField] private int promptFontSize = 28;
    [SerializeField] private Vector2 promptBoxSize = new Vector2(760f, 84f);
    [SerializeField] private Vector2 promptScreenOffset = new Vector2(0f, 72f);
    [SerializeField] private Color promptBackgroundColor = new Color(0f, 0f, 0f, 0.45f);

    [Header("Objective Guide")]
    [SerializeField] private Interactable coffeeTarget;
    [SerializeField] private string guideArrowText = "▼";
    [SerializeField] private Vector2 guideArrowSize = new Vector2(64f, 48f);
    [SerializeField] private Vector2 guideArrowScreenOffset = new Vector2(0f, -56f);
    [SerializeField] private Vector2 guideArrowScreenMargin = new Vector2(28f, 28f);
    [SerializeField] private int guideArrowFontSize = 40;
    [SerializeField] private Color guideArrowColor = new Color(1f, 0.93f, 0.2f, 1f);
    [SerializeField] private float guideArrowPulseSpeed = 4f;
    [SerializeField] private float guideArrowPulseDistance = 10f;

    public FlowState State { get; private set; } = FlowState.None;

    public bool NeedCoffee => State == FlowState.NeedCoffee;
    public bool CanStartWork => State == FlowState.CoffeeDone;
    public bool RequireCoffeeBeforeTask => requireCoffeeBeforeTask;
    public bool HasObjective => _currentObjective != null;
    public Interactable CurrentObjective => _currentObjective;

    public event Action OnCoffeeReadyToWork;
    private GUIStyle _promptStyle;
    private GUIStyle _arrowStyle;
    private Interactable _currentObjective;
    private string _currentPromptMessage;

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }

        _instance = this;
    }

    public void BeginCoffeeStep()
    {
        State = FlowState.NeedCoffee;
        SetObjective(ResolveCoffeeTarget(), defaultCoffeePromptMessage);
    }

    public void OnCoffeeDrank()
    {
        if (!NeedCoffee) return;

        State = FlowState.CoffeeDone;
        ClearObjective();
        OnCoffeeReadyToWork?.Invoke();
    }

    public void SetObjective(Interactable target, string promptMessage = null)
    {
        _currentObjective = target;
        _currentPromptMessage = string.IsNullOrWhiteSpace(promptMessage) ? _currentPromptMessage : promptMessage;
    }

    public void ClearObjective(Interactable target = null)
    {
        if (target != null && _currentObjective != target) return;

        _currentObjective = null;
        _currentPromptMessage = null;
    }

    public void NotifyInteracted(Interactable target)
    {
        if (target == null) return;
        if (_currentObjective != target) return;

        ClearObjective(target);
    }

    private void OnGUI()
    {
        if (!string.IsNullOrWhiteSpace(_currentPromptMessage))
        {
            DrawTopPrompt(_currentPromptMessage);
        }

        if (_currentObjective != null)
        {
            DrawObjectiveArrow();
        }
    }

    private void EnsurePromptStyle()
    {
        if (_promptStyle != null) return;

        _promptStyle = new GUIStyle(GUI.skin.label);
        _promptStyle.alignment = TextAnchor.MiddleCenter;
        _promptStyle.fontStyle = FontStyle.Bold;
        _promptStyle.fontSize = promptFontSize;
        _promptStyle.normal.textColor = promptTextColor;
        _promptStyle.wordWrap = true;
    }

    private void EnsureArrowStyle()
    {
        if (_arrowStyle != null) return;

        _arrowStyle = new GUIStyle(GUI.skin.label);
        _arrowStyle.alignment = TextAnchor.MiddleCenter;
        _arrowStyle.fontStyle = FontStyle.Bold;
        _arrowStyle.fontSize = guideArrowFontSize;
        _arrowStyle.normal.textColor = guideArrowColor;
    }

    private void DrawTopPrompt(string message)
    {
        EnsurePromptStyle();

        float x = (Screen.width - promptBoxSize.x) * 0.5f + promptScreenOffset.x;
        float y = promptScreenOffset.y;
        Rect rect = new Rect(x, y, promptBoxSize.x, promptBoxSize.y);
        DrawRect(rect, promptBackgroundColor);
        GUI.Label(rect, message, _promptStyle);
    }

    private void DrawObjectiveArrow()
    {
        Camera cam = Camera.main;
        if (cam == null) return;

        Vector3 screenPoint = cam.WorldToScreenPoint(_currentObjective.GuideWorldPosition);
        EnsureArrowStyle();

        float pulse = Mathf.Sin(Time.unscaledTime * guideArrowPulseSpeed) * guideArrowPulseDistance;
        Vector2 screenCenter = new Vector2(Screen.width * 0.5f, Screen.height * 0.5f);
        Vector2 targetScreenPos = new Vector2(screenPoint.x, screenPoint.y);
        Vector2 dirFromCenter = targetScreenPos - screenCenter;

        if (screenPoint.z <= 0f)
        {
            dirFromCenter = -dirFromCenter;
        }

        if (dirFromCenter.sqrMagnitude < 0.001f)
        {
            dirFromCenter = Vector2.up;
        }

        bool isInsideScreen =
            screenPoint.z > 0f &&
            targetScreenPos.x >= 0f && targetScreenPos.x <= Screen.width &&
            targetScreenPos.y >= 0f && targetScreenPos.y <= Screen.height;

        Vector2 finalScreenPos;
        if (isInsideScreen)
        {
            finalScreenPos = targetScreenPos;
        }
        else
        {
            float halfWidth = Screen.width * 0.5f - guideArrowScreenMargin.x - guideArrowSize.x * 0.5f;
            float halfHeight = Screen.height * 0.5f - guideArrowScreenMargin.y - guideArrowSize.y * 0.5f;

            float scaleToEdgeX = Mathf.Approximately(dirFromCenter.x, 0f) ? float.MaxValue : halfWidth / Mathf.Abs(dirFromCenter.x);
            float scaleToEdgeY = Mathf.Approximately(dirFromCenter.y, 0f) ? float.MaxValue : halfHeight / Mathf.Abs(dirFromCenter.y);
            float scale = Mathf.Min(scaleToEdgeX, scaleToEdgeY);

            finalScreenPos = screenCenter + dirFromCenter * scale;
        }

        float x = finalScreenPos.x - guideArrowSize.x * 0.5f + guideArrowScreenOffset.x;
        float y = (Screen.height - finalScreenPos.y) - guideArrowSize.y * 0.5f + guideArrowScreenOffset.y + pulse;
        Rect rect = new Rect(x, y, guideArrowSize.x, guideArrowSize.y);
        GUI.Label(rect, guideArrowText, _arrowStyle);
    }

    private Interactable ResolveCoffeeTarget()
    {
        if (coffeeTarget != null) return coffeeTarget;

        Coffee foundCoffee = FindObjectOfType<Coffee>();
        if (foundCoffee != null)
        {
            coffeeTarget = foundCoffee;
        }

        return coffeeTarget;
    }

    private void DrawRect(Rect rect, Color color)
    {
        Color oldColor = GUI.color;
        GUI.color = color;
        GUI.DrawTexture(rect, Texture2D.whiteTexture);
        GUI.color = oldColor;
    }
}
