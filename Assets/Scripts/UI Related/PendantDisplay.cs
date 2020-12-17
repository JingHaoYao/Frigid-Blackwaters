using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PendantDisplay : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    GameObject toolTip;
    Inventory inventory;
    DisplayItem displayInfo;

    Image artifactIcon;

	void Start () {
        inventory = GameObject.Find("PlayerShip").GetComponent<Inventory>();
        toolTip = inventory.toolTip;
        displayInfo = GetComponent<DisplayItem>();

        artifactIcon = GetComponent<Image>();
	}

    public void OnPointerExit(PointerEventData eventData)
    {
        if (displayInfo != null)
        {
            toolTip.SetActive(false);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (displayInfo != null)
        {
            toolTip.SetActive(true);
            toolTip.transform.position = this.transform.position;
            toolTip.GetComponentInChildren<Text>().text = this.GetComponentInChildren<Text>().text;
        }
    }
}
