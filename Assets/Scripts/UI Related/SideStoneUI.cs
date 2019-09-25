using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SideStoneUI : MonoBehaviour
{
    public Image leftStones, rightStones;
    public Sprite[] leftStoneRunes;
    public Sprite[] rightStoneRunes;

    public void updateRunes(int level)
    {
        if (level <= 0)
        {
            leftStones.sprite = leftStoneRunes[0];
            rightStones.sprite = rightStoneRunes[0];
        }
        else if (level >= 9)
        {
            leftStones.sprite = leftStoneRunes[9];
            rightStones.sprite = rightStoneRunes[9];
        }
        else
        {
            leftStones.sprite = leftStoneRunes[level];
            rightStones.sprite = rightStoneRunes[level];
        }
    }

    private void Update()
    {
        int dangerValueCap = Mathf.RoundToInt((Vector3.Magnitude(Camera.main.transform.position) / 20f));
        updateRunes(dangerValueCap);
    }
}
