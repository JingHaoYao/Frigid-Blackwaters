using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragonBreathFireAttack : PlayerProjectile {
    PolygonCollider2D polyCol;
    Vector3 initPos, initShipPos;
    PlayerScript playerScript;
    int bonusDamage = 0;
    public int upgradeDamage;

    float pickDirectionTravel()
    {
        Vector3 cursorPosition = PlayerProperties.cursorPosition;
        return (360 + Mathf.Atan2(cursorPosition.y - transform.position.y, cursorPosition.x - transform.position.x) * Mathf.Rad2Deg) % 360;
    }

    IEnumerator hitFlame()
    {
        for (int i = 0; i < 2 + bonusDamage; i++)
        {
            yield return new WaitForSeconds(0.1f);
            polyCol.enabled = true;
            yield return new WaitForSeconds(0.1f);
            polyCol.enabled = false;
        }
        Destroy(this.gameObject);
    }

	void Start () {
        playerScript = GameObject.Find("PlayerShip").GetComponent<PlayerScript>();
        bonusDamage = 1 + playerScript.attackBonus + playerScript.conAttackBonus + upgradeDamage;
        initPos = transform.position;
        initShipPos = GameObject.Find("PlayerShip").transform.position;
        polyCol = GetComponent<PolygonCollider2D>();
        polyCol.enabled = false;
        StartCoroutine(hitFlame());
        float stopRotatePeriod = (bonusDamage + 2) * 0.2f + 0.4f;
        transform.rotation = Quaternion.Euler(0, 0, pickDirectionTravel() + 180);
        if(playerScript.stopRotatePeriod == 0)
        {
            playerScript.stopRotatePeriod += stopRotatePeriod;
        }
        else if(playerScript.stopRotatePeriod < stopRotatePeriod)
        {
            playerScript.stopRotatePeriod = stopRotatePeriod;
        }

        triggerWeaponFireFlag();
    }

	void Update () {
        transform.position = initPos + (GameObject.Find("PlayerShip").transform.position - initShipPos);
        this.GetComponent<SpriteRenderer>().sortingOrder = GameObject.Find("PlayerShip").GetComponent<SpriteRenderer>().sortingOrder + 1;
	}
}
