using UnityEngine;
using System;
using System.Collections;

public class ChargedObsidianShard : ArtifactEffect
{
    [SerializeField] GameObject chargeBall;
    [SerializeField] DisplayItem displayItem;
    [SerializeField] GameObject lightningEffect;
    GameObject chargeBallInstant;
    bool wasEquipped = false;
    Coroutine mainRoutine;
    int damageToApply = 0;

    private void Update()
    {
        if (displayItem.isEquipped)
        {
            if (!wasEquipped)
            {
                wasEquipped = true;
                mainRoutine = StartCoroutine(mainLoop());
            }
        }
        else
        {
            if (wasEquipped)
            {
                wasEquipped = false;
                Destroy(chargeBallInstant);
            }
        }
    }

    public override void dealtDamage(int damageDealt, Enemy enemy)
    {
        if(damageToApply != 0)
        {
            chargeBallInstant.GetComponent<Animator>().SetTrigger("Explode");
            Destroy(chargeBallInstant, 4 / 12f);
            GameObject effectInstant = Instantiate(lightningEffect, enemy.transform.position, Quaternion.identity);
            effectInstant.transform.localScale = new Vector3(damageToApply / 3f, damageToApply / 3f);
            enemy.dealDamage(damageToApply);
            StopCoroutine(mainRoutine);
            mainRoutine = StartCoroutine(mainLoop());
        }
    }

    IEnumerator mainLoop()
    {
        damageToApply = 0;
        chargeBallInstant = Instantiate(chargeBall, PlayerProperties.playerShipPosition, Quaternion.identity);
        chargeBallInstant.transform.localScale = Vector3.zero;
        float period = 0;
        SpriteRenderer chargeBallRenderer = chargeBallInstant.GetComponent<SpriteRenderer>();

        while (true)
        {
            period = Mathf.Clamp(period + Time.deltaTime, 0, 8);
            damageToApply = Mathf.RoundToInt(period * 2);
            chargeBallInstant.transform.position = PlayerProperties.playerShipPosition + Vector3.up * 2;
            chargeBallInstant.transform.localScale = new Vector3(5 * period / 8f, 5 * period / 8f);
            chargeBallRenderer.sortingOrder = PlayerProperties.spriteRenderer.sortingOrder + 10;

            yield return null;
        }
    }
}
