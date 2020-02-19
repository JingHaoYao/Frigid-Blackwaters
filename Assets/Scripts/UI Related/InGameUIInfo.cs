using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InGameUIInfo : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject[] allHovers;
    public GameObject infoPointer;
    public void OnPointerExit(PointerEventData eventData)
    {
        foreach(GameObject hover in allHovers)
        {
            hover.SetActive(false);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if(infoPointer != null && infoPointer.activeSelf == true)
        {
            infoPointer.SetActive(false);
        }

        foreach (GameObject hover in allHovers)
        {
            hover.SetActive(true);
        }
    }
}
