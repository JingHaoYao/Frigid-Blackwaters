using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OpenBookTile : MonoBehaviour
{
    [SerializeField]
    Image background;
    [SerializeField]
    Image icon;
    [SerializeField]
    Button button;
    EncyclopediaEntry entry;
    OpenBookMenu bookMenu;
    [SerializeField]
    Image border;
    [SerializeField]
    Sprite[] borderList;

    public void Initialize(EncyclopediaEntry entry, OpenBookMenu bookMenu)
    {
        gameObject.SetActive(true);
        this.entry = entry;
        background.sprite = entry.GetBackground;
        icon.sprite = entry.GetIcon;
        this.bookMenu = bookMenu;
        border.sprite = borderList[entry.GetTier - 1];
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(() => { this.bookMenu.SetRightPage(this.entry); });
    }
}
