using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ArtifactUIBloodMeter : MonoBehaviour {
    Image backgroundImage;
    Artifacts artifacts;
    public Sprite noBlood, blood1, blood2, blood3, blood4, blood5;

    void Start () {
        artifacts = GameObject.Find("PlayerShip").GetComponent<Artifacts>();
        backgroundImage = this.GetComponent<Image>();
	}

	void Update () {
        if (artifacts.numKills == 0)
        {
            backgroundImage.sprite = noBlood;
        }
        else if (artifacts.numKills > 0 && artifacts.numKills <= 4)
        {
            backgroundImage.sprite = blood1;
        }
        else if (artifacts.numKills > 4 && artifacts.numKills <= 8)
        {
            backgroundImage.sprite = blood2;
        }
        else if (artifacts.numKills > 8 && artifacts.numKills <= 12)
        {
            backgroundImage.sprite = blood3;
        }
        else if (artifacts.numKills > 12 && artifacts.numKills <= 16)
        {
            backgroundImage.sprite = blood4;
        }
        else
        {
            backgroundImage.sprite = blood5;
        }
	}
}
