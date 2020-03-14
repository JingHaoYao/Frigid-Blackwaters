using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotgunPlume : WeaponFireScript {
    GameObject playerShip;
    float baseAngle = 0;
    public int numberBullets = 3;
    public bool concentrated = false;

    void pickRendererLayer()
    {
        spriteRenderer.sortingOrder = 200 - (int)(transform.position.y * 10) + 4;
    }

    float pickDirectionTravel()
    {
        GameObject cursor = FindObjectOfType<CursorTarget>().gameObject;
        return (360 + Mathf.Atan2(cursor.transform.position.y - transform.position.y, cursor.transform.position.x - transform.position.x) * Mathf.Rad2Deg) % 360;
    }

    void summonBullets(int numberBullets)
    {
        GameObject[] bulletList = new GameObject[numberBullets];
        if (concentrated == false)
        {
            int start = (numberBullets / 2) * 10;
            for (int i = 0; i < numberBullets; i++)
            {

                GameObject instant = Instantiate(bullet, transform.position, Quaternion.identity);
                instant.GetComponent<ShotgunRound>().angleTravel = baseAngle - start + 10 * i;
                bulletList[i] = instant;
            }
            triggerWeaponFireFlag(bulletList, transform.position, baseAngle - start + 10);
        }
        else
        {
            for (int i = 0; i < 3; i++)
            {
                GameObject instant = Instantiate(bullet, transform.position, Quaternion.identity);
                instant.GetComponent<ShotgunRound>().angleTravel = baseAngle - 2 + 2 * i;
                bulletList[i] = instant;
            }
            triggerWeaponFireFlag(bulletList, transform.position, baseAngle);
        }
    }

    IEnumerator waitForAudio()
    {
        yield return new WaitForSeconds(animLength / 3f);
        spriteRenderer.enabled = false;
        yield return new WaitForSeconds(2f);
        Destroy(this.gameObject);
    }

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        animLength = weaponFire.length;
        StartCoroutine(waitForAudio());
        initShipPos = GameObject.Find("PlayerShip").transform.position;
        initFirePos = transform.position;
        playerShip = GameObject.Find("PlayerShip");
        baseAngle = pickDirectionTravel();
        summonBullets(numberBullets);
    }

    void Update()
    {
        transform.position = initFirePos + (GameObject.Find("PlayerShip").transform.position - initShipPos);
        pickRendererLayer();
    }
}
