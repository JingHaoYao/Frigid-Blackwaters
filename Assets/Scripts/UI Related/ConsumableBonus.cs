using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConsumableBonus : MonoBehaviour {
    public int restoredHealth;
    public int attackBonus;
    public float defenseBonus;
    public int speedBonus;
    public int duration;
    public int priceBase;
    public bool consumableActivated = false;

    PlayerScript playerScript;

    private void Start()
    {
        playerScript = GameObject.Find("PlayerShip").GetComponent<PlayerScript>();
    }

    public void consumeItem()
    {
        playerScript.trueDamage -= restoredHealth;
        if(playerScript.trueDamage <= 0)
        {
            playerScript.trueDamage = 0;
        }
        consumableActivated = true;
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
        Destroy(this.gameObject);
    }
}
