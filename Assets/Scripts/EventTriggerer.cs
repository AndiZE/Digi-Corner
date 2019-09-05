using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class EventTriggerer : MonoBehaviour, IBeginDragHandler, IEndDragHandler
{
    [SerializeField]
    private TouchInput input;

    public void OnBeginDrag(PointerEventData eventData)
    {
        input.ActivateInput(true);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        input.ActivateInput(false);
    }
}
