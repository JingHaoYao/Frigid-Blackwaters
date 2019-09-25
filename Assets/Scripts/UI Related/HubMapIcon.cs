using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class HubMapIcon : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    GameObject toolTip;
    Text text;
    public string unlockText;
    public GameObject shipIcon;
    public string buildingID;

    void Start()
    {
        toolTip = GameObject.Find("PlayerShip").GetComponent<Inventory>().toolTip;
        text = GetComponentInChildren<Text>();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        toolTip.SetActive(false);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        toolTip.SetActive(true);
        toolTip.transform.position = this.transform.position;
        if (!MiscData.unlockedBuildings.Contains(buildingID))
        {
            toolTip.GetComponentInChildren<Text>().text = unlockText;
        }
        else
        {
            toolTip.GetComponentInChildren<Text>().text = text.text;
        }
    }

    void Update()
    {
        if (!MiscData.unlockedBuildings.Contains(buildingID))
        {
            this.GetComponent<Image>().color = Color.grey;
        }
        else
        {
            this.GetComponent<Image>().color = Color.white;
        }
    }
}