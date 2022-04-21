using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public float hoverSpeed = 3;
    public float scaleMultiplier = 1.5f;

    private Vector2 defaultScale;

    private Vector2 scaleTo = Vector2.zero;

    private RectTransform rect;

    void Start()
    {
        rect = GetComponent<RectTransform>();
        defaultScale = rect.sizeDelta;
        scaleTo = defaultScale;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("hovering");
        scaleTo = defaultScale * scaleMultiplier;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        scaleTo = defaultScale;
    }

    void Update()
    {
        rect.sizeDelta = Vector2.MoveTowards(rect.sizeDelta, scaleTo, Time.deltaTime * hoverSpeed);
    }
}
