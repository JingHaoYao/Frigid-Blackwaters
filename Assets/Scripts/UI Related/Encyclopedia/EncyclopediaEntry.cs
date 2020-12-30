using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/CreateEncyclopediaEntry", order = 1)]
public class EncyclopediaEntry : ScriptableObject
{
    [SerializeField]
    string enemyName;
    [SerializeField]
    int health;
    [SerializeField]
    int armour;
    [SerializeField]
    string range;
    [SerializeField]
    int damage;
    [SerializeField]
    string movementType;
    [SerializeField]
    float speed;
    [SerializeField]
    string description;
    [SerializeField]
    string dungeonInteraction;
    [SerializeField]
    Sprite icon;
    [SerializeField]
    List<TutorialEntry> quip;
    [SerializeField]
    Sprite background;
    [SerializeField]
    int tier;

    public string GetEnemyName
    {
        get { return enemyName; }
    }

    public int GetHealth
    {
        get { return health; }
    }

    public int GetArmour
    {
        get { return armour; }
    }

    public string GetRange
    {
        get { return range; }
    }

    public int GetDamage
    {
        get { return damage; }
    }

    public string GetMovementType
    {
        get { return movementType; }
    }

    public float GetSpeed
    {
        get { return speed; }
    }

    public string GetDescription
    {
        get { return description; }
    }

    public string GetDungeonInteraction
    {
        get { return dungeonInteraction; }
    }

    public Sprite GetIcon
    {
        get { return icon; }
    }

    public List<TutorialEntry> GetQuip
    {
        get { return quip; }
    }

    public Sprite GetBackground
    {
        get { return background; }
    }

    public int GetTier
    {
        get { return tier; }
    }
}
