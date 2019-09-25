using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuHelperText : MonoBehaviour
{
    public Text informationText;
    public string[] info;
    int dialogueIndex = 0;
    GameObject playerShip;
    PlayerScript playerScript;

    void Start()
    {
        playerShip = GameObject.Find("PlayerShip");
        playerScript = playerShip.GetComponent<PlayerScript>();
    }

    void Update()
    {
        if (dialogueIndex < info.Length && informationText.transform.parent.gameObject.activeSelf == true)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                dialogueIndex++;
                if (dialogueIndex == info.Length)
                {
                    informationText.transform.parent.gameObject.SetActive(false);
                }
                else
                {
                    informationText.text = info[dialogueIndex];
                }
            }
        }
    }

    public void turnOnInfo()
    {
        informationText.transform.parent.gameObject.SetActive(true);
        dialogueIndex = 0;
        informationText.text = info[0];
    }

    private void OnEnable()
    {
        if(informationText.transform.parent.gameObject.activeSelf == true)
        {
            informationText.transform.parent.gameObject.SetActive(false);
        }
    }
}
