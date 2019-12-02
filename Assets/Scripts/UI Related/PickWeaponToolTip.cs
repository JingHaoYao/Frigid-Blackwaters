using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PickWeaponToolTip : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject weaponToolTip;

    public void OnPointerExit(PointerEventData eventData)
    {
        weaponToolTip.SetActive(false);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        weaponToolTip.SetActive(true);
        weaponToolTip.transform.position = transform.position + Vector3.up * 40;
        // Find the template in TrainingHouseWeaponIcon that corresponds to the tile's position in the child hierarchy, and then display the text 
        weaponToolTip.GetComponentInChildren<Text>().text = FindObjectOfType<TrainingHouseWeaponIcon>().templates[transform.GetSiblingIndex()].GetComponent<Text>().text;
    }
}
