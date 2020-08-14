using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GadgetShotBlast : WeaponFireScript
{
    void pickRendererLayer()
    {
        spriteRenderer.sortingOrder = 200 - (int)(transform.position.y * 10) + 4;
    }

    IEnumerator waitForAudio()
    {
        yield return new WaitForSeconds(animLength);
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
        initShipPos = PlayerProperties.playerShipPosition;
        initFirePos = transform.position;
        fireGadgetShot();
    }

    float angleToCursor()
    {
        return Mathf.Atan2(PlayerProperties.cursorPosition.y - transform.position.y, PlayerProperties.cursorPosition.x - transform.position.x);
    }

    void fireGadgetShot()
    {
        GameObject gadgetShotInstant = Instantiate(bullet, transform.position, Quaternion.identity);
        gadgetShotInstant.GetComponent<GadgetShotProjectile>().Initialize(angleToCursor());

        triggerWeaponFireFlag(new GameObject[] { gadgetShotInstant }, transform.position, angleToCursor());
    }

    void Update()
    {
        transform.position = initFirePos + (PlayerProperties.playerShipPosition - initShipPos);
        pickRendererLayer();
    }
}
