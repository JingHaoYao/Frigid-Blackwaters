using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConsumableToolTip : MonoBehaviour
{
    [SerializeField] Text nameText;
    [SerializeField] Text loreText;
    [SerializeField] Text effectText;
    [SerializeField] Text attackStat, speedStat, defenseStat, healingStat;
    [SerializeField] GameObject durationIcon;
    [SerializeField] Text durationText;
    [SerializeField] RectTransform rectTransform;
    [SerializeField] RectTransform canvasRectTransform;

    private void Awake()
    {
        PlayerProperties.consumableToolTip = this;
    }

    public void SetTextAndPosition(
        string name,
        string loreDesc,
        string effectDesc,
        int attackBonus,
        float speedBonus,
        float defenseBonus,
        int healingBonus,
        int duration,
        Vector3 position
    )
    {
        this.gameObject.SetActive(true);
        nameText.text = name + " [Consumable]";

        loreText.text = loreDesc;

        effectText.gameObject.SetActive(effectDesc != "");
        effectText.text = effectDesc;

        attackStat.text = (attackBonus > 0 ? "+" : "") + attackBonus.ToString();
        speedStat.text = (speedBonus > 0 ? "+" : "") + speedBonus.ToString();
        defenseStat.text = (defenseBonus > 0 ? "+" : "") + (defenseBonus * 100).ToString() + "%";
        healingStat.text = (healingBonus > 0 ? "+" : "") + healingBonus.ToString();

        durationIcon.SetActive(duration != 0);
        int numberMinutes = Mathf.FloorToInt((float)duration / 60);
        int numberSeconds = duration % 60;

        durationText.text = (numberMinutes != 0 ? numberMinutes.ToString() + " Min" + (numberMinutes == 1 ? " " : "s ") : "") + (numberSeconds == 0 ? "" : numberSeconds.ToString() + " Secs");

        Canvas.ForceUpdateCanvases();

        float minX = (rectTransform.sizeDelta.x - canvasRectTransform.sizeDelta.x) * 0.5f;
        float maxX = (canvasRectTransform.sizeDelta.x - rectTransform.sizeDelta.x) * 0.5f;
        float minY = (-canvasRectTransform.sizeDelta.y) * 0.5f;
        float maxY = (canvasRectTransform.sizeDelta.y * 0.5f - rectTransform.sizeDelta.y);

        rectTransform.position = new Vector3(
            Mathf.Clamp(position.x, minX + canvasRectTransform.position.x, maxX + canvasRectTransform.position.x),
            Mathf.Clamp(position.y, minY + canvasRectTransform.position.y, maxY + canvasRectTransform.position.y));
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) || PlayerProperties.playerScript.windowAlreadyOpen == false)
        {
            this.gameObject.SetActive(false);
        }
    }
}
