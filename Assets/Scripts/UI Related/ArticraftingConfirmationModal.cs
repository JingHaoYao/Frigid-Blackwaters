using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class ArticraftingConfirmationModal : MonoBehaviour
{
    [SerializeField] Button confirmButton;
    [SerializeField] Button cancelButton;
    [SerializeField] Text mainText;

    private void Awake()
    {
        cancelButton.onClick.AddListener(() => { this.gameObject.SetActive(false); PlayGenericButtonClick(); });
        this.gameObject.SetActive(false);
    }

    public void Disable()
    {
        this.gameObject.SetActive(false);
    }

    void PlayGenericButtonClick()
    {
        PlayerProperties.audioManager.PlaySound("Generic Button Click");
    }

    public void Initialize(string mainText, bool disableConfirm, UnityAction confirmAction)
    {
        this.mainText.text = mainText;
        if(disableConfirm)
        {
            confirmButton.interactable = false;
        }
        else
        {
            confirmButton.interactable = true;
            confirmButton.onClick.RemoveAllListeners();
            confirmButton.onClick.AddListener(confirmAction);
            confirmButton.onClick.AddListener(PlayGenericButtonClick);
        }
        this.gameObject.SetActive(true);
    }

}
