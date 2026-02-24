using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    private Color highlightColor = Color.yellow;
    private Renderer _renderer;
    private Color originColor;
    private bool isInRange = false;

    protected virtual void Awake()
    {
        _renderer = GetComponent<Renderer>();
        if (_renderer != null) originColor = _renderer.material.color;
    }

    public void SetHighlight(bool state)
    {
        if (isInRange == state) return;
        isInRange = state;

        if (_renderer != null)
        {
            _renderer.material.color = state ? highlightColor : originColor;
        }
    }
    public virtual void OnInteract()
    {
        Debug.Log($"[交互成功] 你点击了物品: {gameObject.name}");
    }
}
