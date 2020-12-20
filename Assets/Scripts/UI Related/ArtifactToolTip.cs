using UnityEngine;
using UnityEngine.UI;

public class ArtifactToolTip : MonoBehaviour
{
    [SerializeField] Text nameText;
    [SerializeField] Text loreText;
    [SerializeField] Text effectText;
    [SerializeField] GameObject attackIcon, speedIcon, defenseIcon, periodicHealingIcon, healthIcon;
    [SerializeField] Text attackStat, speedStat, defenseStat, periodicHealingStat, healthStat;
    [SerializeField] GameObject artifactKillsIcon;
    [SerializeField] Text artifactKillsCount;
    [SerializeField] RectTransform rectTransform;
    [SerializeField] RectTransform canvasRectTransform;

    [SerializeField] Color commonColor, uncommonColor, rareColor, legendaryColor;
    [SerializeField] string[] rarityTitles;

    private void Awake()
    {
        PlayerProperties.artifactToolTip = this;
    }

    public void SetTextAndPosition(
        string name, 
        string loreDesc, 
        string effectDesc, 
        int attackBonus, 
        float speedBonus,
        int healthBonus,
        float defenseBonus, 
        int periodicHealingBonus,
        bool hasActive,
        bool soulBound,
        int numberKillsforActive,
        int rarity,
        Vector3 position
    )
    {
        this.gameObject.SetActive(true);
        nameText.text = name + " " + rarityTitles[rarity] + (soulBound ? " [Soulbound]" : "");
        
        switch(rarity)
        {
            case 0:
                nameText.color = commonColor;
                break;
            case 1:
                nameText.color = uncommonColor;
                break;
            case 2:
                nameText.color = rareColor;
                break;
            case 3:
                nameText.color = legendaryColor;
                break;
        }

        loreText.text = loreDesc;

        effectText.gameObject.SetActive(effectDesc != "");
        effectText.text = effectDesc;


        attackStat.text = (attackBonus > 0 ? "+" : "") + attackBonus.ToString();
        speedStat.text = (speedBonus > 0 ? "+" : "") + speedBonus.ToString();
        defenseStat.text = (defenseBonus > 0 ? "+" : "") + (defenseBonus * 100).ToString() + "%";
        periodicHealingStat.text = (periodicHealingBonus > 0 ? "+" : "") + periodicHealingBonus.ToString();
        healthStat.text = (healthBonus > 0 ? "+" : "") + healthBonus.ToString();

        artifactKillsIcon.SetActive(hasActive);
        artifactKillsCount.text = numberKillsforActive.ToString() + " Kill Active:";

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
