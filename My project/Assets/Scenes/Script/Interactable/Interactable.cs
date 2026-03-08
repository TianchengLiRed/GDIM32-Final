using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class Interactable : MonoBehaviour
{
    [Header("Highlight Ring")]
    [SerializeField] private bool includeChildRenderers = true;
    [SerializeField] private Color ringColor = new Color(1f, 0.9f, 0.2f, 1f);
    [SerializeField] private float ringWidth = 0.08f;
    [SerializeField] private float ringPadding = 0.2f;
    [SerializeField] private float ringYOffset = 0.12f;
    [SerializeField] private int ringSegments = 48;

    [Header("GUI Prompt")]
    [SerializeField] private string promptText = "Press E";
    [SerializeField] private Vector3 promptOffset = new Vector3(0f, 2f, 0f);

    [Header("Guide Marker")]
    [SerializeField] private Vector3 guideOffset = new Vector3(0f, 2.6f, 0f);

    private Renderer[] _renderers;
    private LineRenderer _highlightRing;
    private Collider _mainCollider;

    protected virtual void Awake()
    {
        CacheRenderers();
        BuildHighlightRing();
    }

    public void SetHighlight(bool state)
    {
        if (_highlightRing != null) _highlightRing.gameObject.SetActive(state);
    }
    public virtual void OnInteract()
    {
        NotifyTaskObjectiveInteracted();
        Debug.Log($"[交互成功] 你点击了物品: {gameObject.name}");
    }

    public virtual string PromptText => promptText;
    public virtual Vector3 PromptWorldPosition => transform.position + promptOffset;
    public virtual Vector3 GuideWorldPosition => transform.position + guideOffset;

    protected void NotifyTaskObjectiveInteracted()
    {
        if (TaskFlowManager.HasInstance)
        {
            TaskFlowManager.Instance.NotifyInteracted(this);
        }
    }

    private void CacheRenderers()
    {
        _renderers = includeChildRenderers ? GetComponentsInChildren<Renderer>() : GetComponents<Renderer>();
        _mainCollider = GetComponent<Collider>();
    }

    private void BuildHighlightRing()
    {
        Bounds bounds;
        bool hasBounds = false;

        if (_mainCollider != null)
        {
            bounds = _mainCollider.bounds;
            hasBounds = true;
        }
        else
        {
            bounds = default;
        }

        if (!hasBounds && _renderers != null && _renderers.Length > 0)
        {
            for (int i = 0; i < _renderers.Length; i++)
            {
                if (_renderers[i] == null) continue;
                if (!hasBounds)
                {
                    bounds = _renderers[i].bounds;
                    hasBounds = true;
                }
                else
                {
                    bounds.Encapsulate(_renderers[i].bounds);
                }
            }
        }

        if (!hasBounds) return;

        Vector3 lossyScale = transform.lossyScale;
        float scaleX = Mathf.Approximately(lossyScale.x, 0f) ? 1f : lossyScale.x;
        float scaleZ = Mathf.Approximately(lossyScale.z, 0f) ? 1f : lossyScale.z;
        float maxHorizontalScale = Mathf.Max(Mathf.Abs(scaleX), Mathf.Abs(scaleZ));

        float radius = Mathf.Max(bounds.extents.x / Mathf.Abs(scaleX), bounds.extents.z / Mathf.Abs(scaleZ)) + ringPadding;
        Vector3 worldCenter = new Vector3(bounds.center.x, bounds.min.y + ringYOffset, bounds.center.z);
        Vector3 localCenter = transform.InverseTransformPoint(worldCenter);

        GameObject ringObj = new GameObject("HighlightRing");
        ringObj.transform.SetParent(transform, false);
        ringObj.transform.localPosition = localCenter;

        _highlightRing = ringObj.AddComponent<LineRenderer>();
        _highlightRing.loop = true;
        _highlightRing.useWorldSpace = false;
        _highlightRing.positionCount = Mathf.Max(16, ringSegments);
        _highlightRing.widthMultiplier = ringWidth / maxHorizontalScale;
        _highlightRing.numCornerVertices = 8;
        _highlightRing.numCapVertices = 8;
        _highlightRing.shadowCastingMode = ShadowCastingMode.Off;
        _highlightRing.receiveShadows = false;
        _highlightRing.material = new Material(Shader.Find("Sprites/Default"));
        _highlightRing.startColor = ringColor;
        _highlightRing.endColor = ringColor;
        _highlightRing.textureMode = LineTextureMode.Stretch;

        for (int i = 0; i < _highlightRing.positionCount; i++)
        {
            float t = (float)i / _highlightRing.positionCount;
            float angle = t * Mathf.PI * 2f;
            float x = Mathf.Cos(angle) * radius;
            float z = Mathf.Sin(angle) * radius;
            _highlightRing.SetPosition(i, new Vector3(x, 0f, z));
        }

        ringObj.SetActive(false);
    }
}
