using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KeyBindingsSlot : MonoBehaviour
{
    GameObject currentButton;
    Dictionary<string, KeyCode> keys = new Dictionary<string, KeyCode>();
    public GameObject[] buttonList;
    public Text active1Display, active2Display, active3Display;
    public Text targetConesText;

    void updateDict()
    {
        keys.Clear();
        keys.Add("moveUp", (KeyCode)System.Enum.Parse(typeof(KeyCode), SavedKeyBindings.moveUp));
        keys.Add("moveRight", (KeyCode)System.Enum.Parse(typeof(KeyCode), SavedKeyBindings.moveRight));
        keys.Add("moveLeft", (KeyCode)System.Enum.Parse(typeof(KeyCode), SavedKeyBindings.moveLeft));
        keys.Add("moveDown", (KeyCode)System.Enum.Parse(typeof(KeyCode), SavedKeyBindings.moveDown));
        keys.Add("dash", (KeyCode)System.Enum.Parse(typeof(KeyCode), SavedKeyBindings.dash));
        keys.Add("firstArtifact", (KeyCode)System.Enum.Parse(typeof(KeyCode), SavedKeyBindings.firstArtifact));
        keys.Add("secondArtifact", (KeyCode)System.Enum.Parse(typeof(KeyCode), SavedKeyBindings.secondArtifact));
        keys.Add("thirdArtifact", (KeyCode)System.Enum.Parse(typeof(KeyCode), SavedKeyBindings.thirdArtifact));
    }

    void updateSavedKeyBindings()
    {
        SavedKeyBindings.moveUp = keys["moveUp"].ToString();
        SavedKeyBindings.moveRight = keys["moveRight"].ToString();
        SavedKeyBindings.moveLeft = keys["moveLeft"].ToString();
        SavedKeyBindings.moveDown = keys["moveDown"].ToString();
        SavedKeyBindings.dash = keys["dash"].ToString();
        SavedKeyBindings.firstArtifact = keys["firstArtifact"].ToString();
        SavedKeyBindings.secondArtifact = keys["secondArtifact"].ToString();
        SavedKeyBindings.thirdArtifact = keys["thirdArtifact"].ToString();
    }

    private void Start()
    {
        updateDict();
        UpdateUI();
        if (SavedKeyBindings.targetConesEnabled == true)
        {
            targetConesText.text = "Disable Target Cones";
        }
        else
        {
            targetConesText.text = "Enable Target Cones";
        }
        this.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && this.gameObject.activeSelf == true)
        {
            this.gameObject.SetActive(false);
            Time.timeScale = 1;
            GameObject.Find("PlayerShip").GetComponent<PlayerScript>().windowAlreadyOpen = false;
        }
    }

    private void OnGUI()
    {
        if(currentButton != null)
        {
            Event targetEvent = Event.current;
            currentButton.GetComponentInChildren<Text>().text = "Pick Key";
            if (targetEvent.isKey && !keys.ContainsValue(targetEvent.keyCode))
            {
                keys[currentButton.name] = targetEvent.keyCode;
                updateSavedKeyBindings();
                UpdateUI();
                currentButton = null;
            }
        }
    }

    public void changeKey(GameObject clicked)
    {
        currentButton = clicked;
    }

    public void targetCones()
    {
        if(SavedKeyBindings.targetConesEnabled == true)
        {
            SavedKeyBindings.targetConesEnabled = false;
            targetConesText.text = "Enable Target Cones";
        }
        else
        {
            SavedKeyBindings.targetConesEnabled = true;
            targetConesText.text = "Disable Target Cones";
        }
        SaveSystem.SaveGame();
    }

    public void UpdateUI()
    {
        foreach(GameObject button in buttonList)
        {
            button.GetComponentInChildren<Text>().text = keys[button.name].ToString();
        }
        if (SavedKeyBindings.firstArtifact.Length >= 6)
        {
            active1Display.text = SavedKeyBindings.firstArtifact[5].ToString();
        }
        else
        {
            active1Display.text = SavedKeyBindings.firstArtifact;
        }

        if (SavedKeyBindings.secondArtifact.Length >= 6)
        {
            active2Display.text = SavedKeyBindings.secondArtifact[5].ToString();
        }
        else
        {
            active2Display.text = SavedKeyBindings.secondArtifact;
        }

        if (SavedKeyBindings.thirdArtifact.Length >= 6)
        {
            active3Display.text = SavedKeyBindings.thirdArtifact[5].ToString();
        }
        else
        {
            active3Display.text = SavedKeyBindings.thirdArtifact;
        }
    }
}
