using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RevolvingCannonPlume : WeaponFireScript
{
    [SerializeField] AudioSource audioSource;

    void pickRendererLayer()
    {
        spriteRenderer.sortingOrder = 200 - (int)(transform.position.y * 10) + 4;
    }

    float pickDirectionTravel()
    {
        GameObject cursor = FindObjectOfType<CursorTarget>().gameObject;
        return (360 + Mathf.Atan2(cursor.transform.position.y - transform.position.y, cursor.transform.position.x - transform.position.x) * Mathf.Rad2Deg) % 360;
    }

    IEnumerator waitForAudio()
    {
        yield return new WaitForSeconds(animLength);
        spriteRenderer.enabled = false;
        yield return new WaitForSeconds(audioSource.clip.length);
        Destroy(this.gameObject);
    }

    void Start()
    {
        animLength = weaponFire.length;
        StartCoroutine(waitForAudio());
        initShipPos = PlayerProperties.playerShipPosition;
        initFirePos = transform.position;
        StartCoroutine(waitForAudio());
        summonBulletBasedOnUpgrades();
    }

    void summonBulletBasedOnUpgrades()
    {
        int bonusDamage = 0;
        float angleAttack = pickDirectionTravel() * Mathf.Deg2Rad;
        if(PlayerUpgrades.revolvingCannonUpgrades.Count >= 2)
        {
            bonusDamage += 1;
            if (PlayerUpgrades.revolvingCannonUpgrades.Count > 3) {
                if (PlayerUpgrades.revolvingCannonUpgrades[3] == "bullet_cartridge_upgrade")
                {
                    if (PlayerUpgrades.revolvingCannonUpgrades.Count == 5)
                    {
                        bonusDamage += 3;
                    }
                    else
                    {
                        bonusDamage += 5;
                    }
                }
                else
                {   if(PlayerUpgrades.revolvingCannonUpgrades.Count == 4)
                    {
                        StartCoroutine(summonAdditionalBullets(1, bonusDamage, angleAttack));
                    }
                    else if (PlayerUpgrades.revolvingCannonUpgrades.Count == 5)
                    {
                        StartCoroutine(summonAdditionalBullets(2, bonusDamage, angleAttack));
                    }
                    else
                    {
                        StartCoroutine(summonAdditionalBullets(3, bonusDamage, angleAttack));
                    }
                }
            }
        }
        GameObject bulletInstant = Instantiate(bullet, transform.position, Quaternion.identity);
        bulletInstant.GetComponent<RevolvingBullet>().Initialize(angleAttack, 1 + bonusDamage, false);
    }
    
    IEnumerator summonAdditionalBullets(int numberBullets, int bonusDamage, float angleTravel)
    {
        float offset = 0.25f / (numberBullets + 1);

        for(int i = 0; i < numberBullets; i++)
        {
            yield return new WaitForSeconds(offset);
            GameObject bulletInstant = Instantiate(bullet, transform.position, Quaternion.identity);
            bulletInstant.GetComponent<RevolvingBullet>().Initialize(angleTravel, 1 + bonusDamage, true);
        }
    }

    void Update()
    {
        transform.position = initFirePos + (GameObject.Find("PlayerShip").transform.position - initShipPos);
        pickRendererLayer();
    }
}
