using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireworkExplosion : PlayerProjectile {
    Animator animator;
    CircleCollider2D circCol;

    IEnumerator turnOnCollider()
    {
        yield return new WaitForSeconds(2f / 12f);
        circCol.enabled = true;
        yield return new WaitForSeconds(1f / 12f);
        circCol.enabled = false;
    }

	void Start () {
        circCol = GetComponent<CircleCollider2D>();
        circCol.enabled = false;
        animator = GetComponent<Animator>();
        Destroy(this.gameObject, 0.667f);
        StartCoroutine(turnOnCollider());
        if (PlayerUpgrades.fireworkUpgrades.Count >= 2)
        {
            this.GetComponent<DamageAmount>().addDamage(1);
        }
    }
}
