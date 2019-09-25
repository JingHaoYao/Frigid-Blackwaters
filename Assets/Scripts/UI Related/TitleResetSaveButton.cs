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
        SaveSystem.DeleteSave();
        areYouSureMenu.SetActive(false);
    }

    public void GoBack()
    {
        areYouSureMenu.SetActive(false);
    }
}
