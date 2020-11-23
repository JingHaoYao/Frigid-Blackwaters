using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleResetSaveButton : MonoBehaviour
{
    public GameObject areYouSureMenu;

    public void openMenu()
    {
        areYouSureMenu.SetActive(true);
    }

    public void DeleteSave()
    {
        FindObjectOfType<AudioManager>().PlaySound("Generic Button Click");
        SaveSystem.DeleteSave();
        areYouSureMenu.SetActive(false);
    }

    public void GoBack()
    {
        FindObjectOfType<AudioManager>().PlaySound("Generic Button Click");
        areYouSureMenu.SetActive(false);
    }
}
