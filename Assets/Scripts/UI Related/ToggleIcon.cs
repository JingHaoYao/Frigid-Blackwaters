using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToggleIcon : MonoBehaviour {
    public Sprite on, off;

    public void switchOn()
    {
        this.GetComponent<Image>().sprite = on;
    }

    public void switchOff()
    {
        this.GetComponent<Image>().sprite = off;
    }
}
