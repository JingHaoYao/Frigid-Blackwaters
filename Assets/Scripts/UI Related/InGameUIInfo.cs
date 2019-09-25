using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InGameUIInfo : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject info;
    public void OnPointerExit(PointerEventData eventData)
    {
        info.SetActive(false);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        info.SetActive(true);
    }
}
