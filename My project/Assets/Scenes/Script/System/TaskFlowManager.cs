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

    [Header("Guide")]
    [SerializeField] private bool requireCoffeeBeforeTask = true;
    [SerializeField] private Transform player;
    [SerializeField] private Transform coffeeTarget;
    [SerializeField] private Transform worldArrow;
    [SerializeField] private Vector3 arrowOffset = new Vector3(0f, 2.4f, 0f);

    [Header("Guide UI")]
    [SerializeField] private Color guideColor = new Color(1f, 0.93f, 0.2f, 1f);
    [SerializeField] private int guideFontSize = 28;

    public FlowState State { get; private set; } = FlowState.None;

    public bool NeedCoffee => State == FlowState.NeedCoffee;
    public bool CanStartWork => State == FlowState.CoffeeDone;
    public bool RequireCoffeeBeforeTask => requireCoffeeBeforeTask;

    public event Action OnCoffeeReadyToWork;

    private GUIStyle _guideStyle;

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }

        _instance = this;
        ResolveReferences();
        EnsureWorldArrow();
        SetArrowVisible(false);
    }

    private void Update()
    {
        if (!NeedCoffee)
        {
            SetArrowVisible(false);
            return;
        }

        ResolveReferences();
        UpdateArrow();
    }

    public void BeginCoffeeStep()
    {
        State = FlowState.NeedCoffee;
        ResolveReferences();
        EnsureWorldArrow();
        SetArrowVisible(true);
    }

    public void OnCoffeeDrank()
    {
        if (!NeedCoffee) return;

        State = FlowState.CoffeeDone;
        SetArrowVisible(false);
        OnCoffeeReadyToWork?.Invoke();
    }

    private void ResolveReferences()
    {
        if (player == null)
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null)
            {
                player = playerObj.transform;
            }
            else
            {
                PlayerController controller = FindObjectOfType<PlayerController>();
                if (controller != null) player = controller.transform;
            }
        }

        if (coffeeTarget == null)
        {
            Coffee coffee = FindObjectOfType<Coffee>();
            if (coffee != null) coffeeTarget = coffee.transform;
        }
    }

    private void UpdateArrow()
    {
        if (worldArrow == null || player == null || coffeeTarget == null) return;

        worldArrow.position = player.position + arrowOffset;
        Vector3 dir = coffeeTarget.position - player.position;
        dir.y = 0f;
        if (dir.sqrMagnitude < 0.001f) return;

        worldArrow.rotation = Quaternion.LookRotation(dir.normalized, Vector3.up);
    }

    private void EnsureWorldArrow()
    {
        if (worldArrow != null) return;

        GameObject arrowObj = new GameObject("AutoGuideArrow");
        LineRenderer lr = arrowObj.AddComponent<LineRenderer>();
        lr.useWorldSpace = false;
        lr.positionCount = 5;
        lr.widthMultiplier = 0.06f;
        lr.numCornerVertices = 4;
        lr.numCapVertices = 4;
        lr.loop = false;
        lr.material = new Material(Shader.Find("Sprites/Default"));
        lr.startColor = new Color(1f, 0.93f, 0.2f, 1f);
        lr.endColor = new Color(1f, 0.93f, 0.2f, 1f);
        lr.SetPosition(0, new Vector3(0f, 0f, 0f));
        lr.SetPosition(1, new Vector3(0f, 0f, 1f));
        lr.SetPosition(2, new Vector3(-0.24f, 0f, 0.74f));
        lr.SetPosition(3, new Vector3(0f, 0f, 1f));
        lr.SetPosition(4, new Vector3(0.24f, 0f, 0.74f));

        worldArrow = arrowObj.transform;
    }

    private void SetArrowVisible(bool visible)
    {
        if (worldArrow != null)
        {
            worldArrow.gameObject.SetActive(visible);
        }
    }

    private void OnGUI()
    {
        if (!NeedCoffee) return;

        EnsureStyle();

        Rect rect = new Rect(Screen.width * 0.5f - 260f, 24f, 520f, 46f);
        GUI.Label(rect, "-> 前往咖啡机喝咖啡，开始倒计时后才能做任务", _guideStyle);
    }

    private void EnsureStyle()
    {
        if (_guideStyle != null) return;

        _guideStyle = new GUIStyle(GUI.skin.label);
        _guideStyle.alignment = TextAnchor.MiddleCenter;
        _guideStyle.fontStyle = FontStyle.Bold;
        _guideStyle.fontSize = guideFontSize;
        _guideStyle.normal.textColor = guideColor;
    }
}
