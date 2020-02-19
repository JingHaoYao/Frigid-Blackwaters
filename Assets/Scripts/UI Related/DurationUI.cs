using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DurationUI : MonoBehaviour
{
    public GameObject durationUITile;

    private void Awake()
    {
        PlayerProperties.durationUI = this;
    }

    public void addTile(Sprite icon, float maxDuration)
    {
        GameObject tile = Instantiate(durationUITile);
        tile.transform.SetParent(this.transform);
        tile.transform.localScale = Vector2.one;
        tile.GetComponent<DurationTile>().itemIcon = icon;
        tile.GetComponent<DurationTile>().maxDuration = maxDuration;
    }
}
