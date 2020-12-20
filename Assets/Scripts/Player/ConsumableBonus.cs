using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ConsumableBonus : MonoBehaviour {
    public string consumableName;
    public int restoredHealth;
    public int attackBonus;
    public float defenseBonus;
    public int speedBonus;
    public int duration;
    public int priceBase;

    public Text loreText;
    public Text effectText;

    public bool destroyConsumable = true;
    UnityAction consumableAction;

    PlayerScript playerScript;

    private void Start()
    {
        playerScript = GameObject.Find("PlayerShip").GetComponent<PlayerScript>();
    }

    public void SetAction(UnityAction consumableAction)
    {
        this.consumableAction = consumableAction;
    }

    public void consumeItem()
    {
        playerScript.healPlayer(restoredHealth);

        consumableAction?.Invoke();

        StartCoroutine(activateConsumable());
    }

    IEnumerator activateConsumable()
    {
        playerScript.conAttackBonus += attackBonus;
        playerScript.conDefenseBonus += defenseBonus;
        playerScript.conSpeedBonus += speedBonus;
        if (duration > 0)
        {
            FindObjectOfType<DurationUI>().addTile(this.GetComponent<DisplayItem>().displayIcon, duration);
        }
        yield return new WaitForSeconds(duration);
        playerScript.conAttackBonus -= attackBonus;
        playerScript.conDefenseBonus -= defenseBonus;
        playerScript.conSpeedBonus -= speedBonus;

        if (destroyConsumable)
        {
            Destroy(this.gameObject);
        }
    }
}
