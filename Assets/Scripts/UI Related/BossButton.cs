using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class BossButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public BossSelectMenu menu;
    public int whatButton;
    public Vector3 toolTipPosition;

    public void OnPointerExit(PointerEventData eventData)
    {
        menu.turnOffToolTip();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        menu.turnOnToolTip(whatButton, toolTipPosition);
    }

}
