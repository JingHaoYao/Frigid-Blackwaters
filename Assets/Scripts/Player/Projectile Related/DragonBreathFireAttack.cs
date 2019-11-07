using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragonBreathFireAttack : PlayerProjectile {
    PolygonCollider2D polyCol;
    Vector3 initPos, initShipPos;
    PlayerScript playerScript;
    DamageAmount damageAmount;
    int bonusDamage = 0;
    public int upgradeDamage;

    float pickDirectionTravel()
    {
        GameObject cursor = FindObjectOfType<CursorTarget>().gameObject;
        return (360 + Mathf.Atan2(cursor.transform.position.y - transform.position.y, cursor.transform.position.x - transform.position.x) * Mathf.Rad2Deg) % 360;
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
        damageAmount = GetComponent<DamageAmount>();
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

        if (whichWeaponFrom == 1)
        {
            foreach (ArtifactSlot slot in FindObjectOfType<Artifacts>().artifactSlots)
            {
                if (slot.displayInfo != null && slot.displayInfo.GetComponent<ArtifactEffect>())
                    slot.displayInfo.GetComponent<ArtifactEffect>().firedFrontWeapon(new GameObject[1] { this.gameObject });
            }
        }
        else if (whichWeaponFrom == 2)
        {
            foreach (ArtifactSlot slot in FindObjectOfType<Artifacts>().artifactSlots)
            {
                if (slot.displayInfo != null && slot.displayInfo.GetComponent<ArtifactEffect>())
                    slot.displayInfo.GetComponent<ArtifactEffect>().firedLeftWeapon(new GameObject[1] { this.gameObject });
            }
        }
        else
        {
            foreach (ArtifactSlot slot in FindObjectOfType<Artifacts>().artifactSlots)
            {
                if (slot.displayInfo != null && slot.displayInfo.GetComponent<ArtifactEffect>())
                    slot.displayInfo.GetComponent<ArtifactEffect>().firedRightWeapon(new GameObject[1] { this.gameObject });
            }
        }
    }

	void Update () {
        transform.position = initPos + (GameObject.Find("PlayerShip").transform.position - initShipPos);
        this.GetComponent<SpriteRenderer>().sortingOrder = GameObject.Find("PlayerShip").GetComponent<SpriteRenderer>().sortingOrder + 1;
	}
}
