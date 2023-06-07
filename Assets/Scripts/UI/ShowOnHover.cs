using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ShowOnHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private RectTransform HoverRect;

    public void OnPointerEnter(PointerEventData eventData)
    {
        Show();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Hide();
    }

    // Does what it says, shows the object specified by Hover Rect
    private void Show()
    {
        HoverRect.gameObject.SetActive(true);
    }

    // Hides HoverRect
    private void Hide()
    {
        HoverRect.gameObject.SetActive(false);
    }

    // Start is called before the first frame update
    void Start()
    {
        Hide();   
    }
}
