using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActiveSlot : MonoBehaviour {
    GameObject activeIcon;
    public DisplayItem activeItem;
    public Sprite unglow, glow;
    public Image cooldownCircle;
    public Text killText;
    Artifacts artifacts;
    public Color activated = new Color(0, 253, 255, 255);
    public Color unactivated = new Color(75, 75, 75, 255);

	void Awake () {
        artifacts = FindObjectOfType<Artifacts>();
        activeIcon = transform.GetChild(1).gameObject;
        activeIcon.SetActive(false);
	}

    void Update()
    {
        if (activeItem != null)
        {
            if (GameObject.Find("PlayerShip").GetComponent<Artifacts>().numKills >= activeItem.gameObject.GetComponent<ArtifactBonus>().killRequirement && activeItem.hasActive == true)
            {
                GetComponent<Image>().sprite = glow;
                killText.color = activated;
                cooldownCircle.color = activated;
            }
            else
            {
                GetComponent<Image>().sprite = unglow;
                killText.color = Color.white;
                cooldownCircle.color = Color.white;
            }

            if(activeItem.hasActive == true)
            {
                killText.enabled = true;
                cooldownCircle.enabled = true;
                killText.text = activeItem.gameObject.GetComponent<ArtifactBonus>().killRequirement.ToString();
                cooldownCircle.fillAmount = Mathf.Clamp(artifacts.numKills / (float)activeItem.gameObject.GetComponent<ArtifactBonus>().killRequirement, 0, 1);
            }
            else
            {
                killText.enabled = false;
                cooldownCircle.color = unactivated;
            }
        }
        else
        {
            GetComponent<Image>().sprite = unglow;
            killText.enabled = false;
            cooldownCircle.color = unactivated;
        }
    }

    public void addSlot(DisplayItem _displayInfo)
    {
        activeItem = _displayInfo;
        activeIcon.SetActive(true);
        activeIcon.GetComponent<Image>().sprite = _displayInfo.displayIcon;
    }
    
    public void deleteSlot()
    {
        activeItem = null;
        activeIcon.SetActive(false);
    }
}
