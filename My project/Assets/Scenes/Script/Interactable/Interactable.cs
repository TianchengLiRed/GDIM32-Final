using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    [Header("Highlight")]
    [SerializeField] private Color highlightColor = Color.yellow;
    [SerializeField] private bool includeChildRenderers = true;

    [Header("GUI Prompt")]
    [SerializeField] private string promptText = "按 E 交互";
    [SerializeField] private Vector3 promptOffset = new Vector3(0f, 2f, 0f);

    private Renderer[] _renderers;
    private Color[] _originColors;
    private string[] _colorPropertyNames;

    protected virtual void Awake()
    {
        CacheRenderers();
    }

    public void SetHighlight(bool state)
    {
        for (int i = 0; i < _renderers.Length; i++)
        {
            var renderer = _renderers[i];
            if (renderer == null) continue;

            string propertyName = _colorPropertyNames[i];
            if (string.IsNullOrEmpty(propertyName)) continue;

            renderer.material.SetColor(propertyName, state ? highlightColor : _originColors[i]);
        }
    }
    public virtual void OnInteract()
    {
        Debug.Log($"[交互成功] 你点击了物品: {gameObject.name}");
    }

    public string PromptText => promptText;
    public Vector3 PromptWorldPosition => transform.position + promptOffset;

    private void CacheRenderers()
    {
        _renderers = includeChildRenderers ? GetComponentsInChildren<Renderer>() : GetComponents<Renderer>();
        _originColors = new Color[_renderers.Length];
        _colorPropertyNames = new string[_renderers.Length];

        for (int i = 0; i < _renderers.Length; i++)
        {
            var renderer = _renderers[i];
            if (renderer == null) continue;

            Material material = renderer.material;
            if (material == null) continue;

            if (material.HasProperty("_BaseColor"))
            {
                _colorPropertyNames[i] = "_BaseColor";
            }
            else if (material.HasProperty("_Color"))
            {
                _colorPropertyNames[i] = "_Color";
            }

            if (!string.IsNullOrEmpty(_colorPropertyNames[i]))
            {
                _originColors[i] = material.GetColor(_colorPropertyNames[i]);
            }
        }
    }
}
