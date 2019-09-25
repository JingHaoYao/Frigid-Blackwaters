using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NotificationBell : MonoBehaviour
{
    Animator bellAnimator, animator;
    Text notificationText;
    Image image;
    public bool dialogueNotifications = false;
    public List<string> dialoguesAvailable = new List<string>();

    void Awake()
    {
        animator = this.GetComponent<Animator>();
        bellAnimator = GetComponentsInChildren<Animator>()[1];
        notificationText = GetComponentInChildren<Text>();
        image = GetComponent<Image>();
        image.enabled = false;
        notificationText.enabled = false;
    }

    private void LateUpdate()
    {
        if(dialogueNotifications == true)
        {
            if (dialoguesAvailable.Count > 0)
            {
                string message = "New Dialogues Available at:";
                for (int i = 0; i < dialoguesAvailable.Count; i++)
                {
                    message = message + " " + dialoguesAvailable[i];
                    if (i < dialoguesAvailable.Count - 1)
                    {
                        message += ',';
                    }
                    else
                    {
                        message += '.';
                    }
                }
                startNotification(message);
            }
            else
            {
                this.gameObject.SetActive(false);
            }
        }
    }

    public void startNotification(string text)
    {
        image.enabled = true;
        notificationText.enabled = true;
        notificationText.text = text;
    }
}
