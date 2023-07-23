using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InGameBar : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private InGameSystem m_inGameSystem;

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log(gameObject.name);
        m_inGameSystem.SetInGameBar(this);
    }
}
