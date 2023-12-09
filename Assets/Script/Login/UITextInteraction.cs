using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class UITextInteraction : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [System.Serializable]
    private class OnClickEvent : UnityEvent { }

    [SerializeField] private OnClickEvent onClickEvent;

    private TextMeshProUGUI text;
    private IPointerClickHandler _pointerClickHandlerImplementation;
    private IPointerEnterHandler _pointerEnterHandlerImplementation;
    private IPointerExitHandler _pointerExitHandlerImplementation;

    private void Awake()
    {
        text = GetComponent<TextMeshProUGUI>();
    }
    
    public void OnPointerEnter(PointerEventData eventData)
    {
        text.fontStyle = FontStyles.Bold;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        text.fontStyle = FontStyles.Normal;
    }
    
    public void OnPointerClick(PointerEventData eventData)
    {
        onClickEvent?.Invoke();
    }
}
