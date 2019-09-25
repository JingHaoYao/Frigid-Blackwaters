using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class HoverToolTip : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject toolTip;
    public Text text;

    public string alternateMessage;
    public bool useAlternateMessage = false;
    public bool findToolTip = false;

    void Start()
    {
        if (findToolTip)
        {
            toolTip = FindObjectOfType<Inventory>().toolTip;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        toolTip.SetActive(false);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        toolTip.SetActive(true);
        toolTip.transform.position = this.transform.position;
        if (useAlternateMessage == false)
        {
            toolTip.GetComponentInChildren<Text>().text = text.text;
        }
        else
        {
            toolTip.GetComponentInChildren<Text>().text = alternateMessage;
        }
    }
}
