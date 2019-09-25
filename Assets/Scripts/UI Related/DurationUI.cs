using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DurationUI : MonoBehaviour
{
    public GameObject durationUITile;

    public void addTile(Sprite icon, float maxDuration)
    {
        GameObject tile = Instantiate(durationUITile);
        tile.transform.SetParent(this.transform);
        tile.GetComponent<DurationTile>().itemIcon = icon;
        tile.GetComponent<DurationTile>().maxDuration = maxDuration;
    }
}
