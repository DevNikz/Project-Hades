using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class GenericClick : MonoBehaviour, IPointerClickHandler
{
    public UnityEvent OnClickCallback;
    public void OnPointerClick(PointerEventData eventData)
    {
        OnClickCallback?.Invoke();
    }
}
