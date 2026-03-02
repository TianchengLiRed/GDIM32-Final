using System;
using UnityEngine;

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
    [SerializeField] private string coffeePromptMessage = "先去喝咖啡，准备开始工作";
    [SerializeField] private Color promptTextColor = new Color(1f, 0.93f, 0.2f, 1f);
    [SerializeField] private int promptFontSize = 28;
    [SerializeField] private Vector2 promptBoxSize = new Vector2(760f, 84f);
    [SerializeField] private Vector2 promptScreenOffset = new Vector2(0f, 72f);
    [SerializeField] private Color promptBackgroundColor = new Color(0f, 0f, 0f, 0.45f);

    public FlowState State { get; private set; } = FlowState.None;

    public bool NeedCoffee => State == FlowState.NeedCoffee;
    public bool CanStartWork => State == FlowState.CoffeeDone;
    public bool RequireCoffeeBeforeTask => requireCoffeeBeforeTask;

    public event Action OnCoffeeReadyToWork;
    private GUIStyle _promptStyle;

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
    }

    public void OnCoffeeDrank()
    {
        if (!NeedCoffee) return;

        State = FlowState.CoffeeDone;
        OnCoffeeReadyToWork?.Invoke();
    }

    private void OnGUI()
    {
        if (!NeedCoffee) return;

        EnsurePromptStyle();

        float x = (Screen.width - promptBoxSize.x) * 0.5f + promptScreenOffset.x;
        float y = promptScreenOffset.y;
        Rect rect = new Rect(x, y, promptBoxSize.x, promptBoxSize.y);
        DrawRect(rect, promptBackgroundColor);
        GUI.Label(rect, coffeePromptMessage, _promptStyle);
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

    private void DrawRect(Rect rect, Color color)
    {
        Color oldColor = GUI.color;
        GUI.color = color;
        GUI.DrawTexture(rect, Texture2D.whiteTexture);
        GUI.color = oldColor;
    }
}
