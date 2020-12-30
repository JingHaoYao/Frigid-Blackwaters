using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OpenBookMenu : MonoBehaviour
{
    [SerializeField]
    Image background;
    [SerializeField]
    Image icon;
    [SerializeField]
    Text title;
    [SerializeField]
    Text health;
    [SerializeField]
    Text armour;
    [SerializeField]
    Text range;
    [SerializeField]
    Text damage;
    [SerializeField]
    Text movementType;
    [SerializeField]
    Text speed;
    [SerializeField]
    Text description;
    [SerializeField]
    Text dungeonInteraction;
    [SerializeField]
    Text killText;
    [SerializeField]
    Button quipButton;
    [SerializeField]
    GameObject openBookTile;
    [SerializeField]
    Transform gridTransform;
    List<OpenBookTile> allOpenBookTiles = new List<OpenBookTile>();
    [SerializeField]
    Button returnButton;
    [SerializeField]
    Text dungeonName;
    [SerializeField]
    Color[] dungeonColours;
    [SerializeField]
    GameObject infoContainer;
    [SerializeField]
    Button bossButton;
    [SerializeField]
    Image bossImage;
    [SerializeField]
    Sprite bossSprite;
    [SerializeField]
    Sprite enemySprite;

    public void SetRightPage(EncyclopediaEntry entry)
    {
        background.sprite = entry.GetBackground;
        icon.sprite = entry.GetIcon;
        title.text = entry.GetEnemyName;
        health.text = entry.GetHealth.ToString();
        armour.text = entry.GetArmour.ToString();
        range.text = entry.GetRange;
        damage.text = entry.GetDamage.ToString();
        movementType.text = entry.GetMovementType;
        speed.text = entry.GetSpeed.ToString();
        description.text = entry.GetDescription;
        dungeonInteraction.text = entry.GetDungeonInteraction;
        // implement killText
        quipButton.onClick.RemoveAllListeners();
        quipButton.onClick.AddListener(()=> { PlayerProperties.tutorialWidgetMenu.Initialize(entry.GetQuip); });
    }

    EncyclopediaEntry[] LoadEncyclopediaEntries(int dungeonLevel)
    {
        switch (dungeonLevel)
        {
            case 1:
                return Resources.LoadAll<EncyclopediaEntry>("EncyclopediaEntries/FirstDungeon");
            case 2:
                return Resources.LoadAll<EncyclopediaEntry>("EncyclopediaEntries/SecondDungeon");
            case 3:
                return Resources.LoadAll<EncyclopediaEntry>("EncyclopediaEntries/ThirdDungeon");
            case 4:
                return Resources.LoadAll<EncyclopediaEntry>("EncyclopediaEntries/FourthDungeon");
            case 5:
                return Resources.LoadAll<EncyclopediaEntry>("EncyclopediaEntries/FifthDungeon");
            default:
                return new EncyclopediaEntry[0];
        }
    }

    public void SetOpenBookMenu(int dungeonLevel, bool bossMode)
    {
        bossButton.onClick.RemoveAllListeners();
        bossButton.onClick.AddListener(() => { SetOpenBookMenu(dungeonLevel, !bossMode); });
        if (bossMode == true)
        {
            bossImage.sprite = enemySprite;
        }
        else
        {
            bossImage.sprite = bossSprite;
        }
        switch (dungeonLevel)
        {
            case 1:
                dungeonName.text = "THE SURFACE LEVEL";
                break;
            case 2:
                dungeonName.text = "THE ICE LEVEL";
                break;
            case 3:
                dungeonName.text = "THE FLORA LEVEL";
                break;
            case 4:
                dungeonName.text = "THE ILLUSION LEVEL";
                break;
            case 5:
                dungeonName.text = "THE INFERNO LEVEL";
                break;
            default:
                dungeonName.text = "THE PRASAD PRASAD FLOOR";
                break;
        }
        dungeonName.color = dungeonColours[dungeonLevel-1];
        EncyclopediaEntry[] entries = LoadEncyclopediaEntries(dungeonLevel);
        List<EncyclopediaEntry> sortList = new List<EncyclopediaEntry>();
        foreach (EncyclopediaEntry entry in entries)
        {
            sortList.Add(entry);
        }
        sortList.Sort((e1, e2) => e1.GetTier.CompareTo(e2.GetTier));
        List<EncyclopediaEntry> unlockedList = new List<EncyclopediaEntry>();
        foreach (EncyclopediaEntry entry in sortList)
        {
            if (MiscData.seenEnemies.Contains(entry.GetEnemyName))
            {
                if (bossMode == true && entry.GetTier >= 5) 
                {
                    unlockedList.Add(entry);
                }
                else if (bossMode == false && entry.GetTier <= 4)
                {
                    unlockedList.Add(entry);
                }
            }
        }
        for (int i = 0; i < unlockedList.Count; i++)
        {
            if (i < allOpenBookTiles.Count)
            {
                allOpenBookTiles[i].Initialize(unlockedList[i], this);
            }
            else
            {
                OpenBookTile newTile = Instantiate(openBookTile).GetComponent<OpenBookTile>();
                allOpenBookTiles.Add(newTile);
                newTile.Initialize(unlockedList[i], this);
                newTile.transform.SetParent(gridTransform);
                newTile.transform.localScale = Vector3.one;
            }
        }
        for (int i = unlockedList.Count; i < allOpenBookTiles.Count; i++)
        {
            allOpenBookTiles[i].gameObject.SetActive(false);
        }
        if (unlockedList.Count > 0)
        {
            SetRightPage(unlockedList[0]);
            infoContainer.SetActive(true);
        }
        else
        {
            infoContainer.SetActive(false);
        }
    }

    public void Initialize(EnemyEncyclopedia encyclopedia)
    {
        returnButton.onClick.AddListener(()=> { encyclopedia.CloseDungeonPage(); });
    }
}
