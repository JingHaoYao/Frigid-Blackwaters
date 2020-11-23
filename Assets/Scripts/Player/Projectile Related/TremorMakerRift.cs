using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TremorMakerRift : PlayerProjectile
{
    [SerializeField] Collider2D collider2D;
    [SerializeField] DamageAmount damageAmount;
    [SerializeField] int whatSpikeType;
    // 0 - small
    // 1 - medium
    // 2 - large
    [SerializeField] Animator animator;
    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] GameObject burnRadius;
    string breakString;
    bool spikeAttack;

    List<Enemy> stunnedEnemies = new List<Enemy>();

    private void Start()
    {
        pickRendererLayer();
        applyUpgrades();
        StartCoroutine(riftRoutine());
    }

    void pickRendererLayer()
    {
        spriteRenderer.sortingOrder = 200 - Mathf.RoundToInt(transform.position.y * 10);
    }

    IEnumerator riftRoutine()
    {
        collider2D.enabled = false;
        yield return new WaitForSeconds(1 / 12f);
        collider2D.enabled = true;
        yield return new WaitForSeconds(2 / 12f);
        collider2D.enabled = false;
        yield return new WaitForSeconds(3 / 12f);

        yield return new WaitForSeconds(15 / 12f);

        animator.Play(breakString);
        yield return new WaitForSeconds(5 / 12f);
        Destroy(this.gameObject);
    }

    void applyUpgrades()
    {
        spikeAttack = false;
        int bonusDamage = 0;

        switch (whatSpikeType)
        {
            case 0:
                animator.Play("Small Rift Emerge");
                breakString = "Small Rift Break";
                break;
            case 1:
                animator.Play("Medium Rift Emerge");
                breakString = "Medium Rift Break";
                break;
            case 2:
                animator.Play("Large Rift Emerge");
                breakString = "Large Rift Break";
                break;
        }

        if (PlayerUpgrades.tremorMakerUpgrades.Count >= 2)
        {
            bonusDamage += 1;
            if (PlayerUpgrades.tremorMakerUpgrades.Count > 3)
            {
                if (PlayerUpgrades.tremorMakerUpgrades[3] == "burn_radius_upgrade")
                {
                    spikeAttack = false;
                    int bonusBurnDamage = 0;
                    bool shouldSlow = false;

                    switch (whatSpikeType)
                    {
                        case 0:
                            animator.Play("Small Rift Emerge");
                            breakString = "Small Rift Break";
                            break;
                        case 1:
                            animator.Play("Medium Rift Emerge");
                            breakString = "Medium Rift Break";
                            break;
                        case 2:
                            animator.Play("Large Rift Emerge");
                            breakString = "Large Rift Break";
                            break;
                    }

                    switch (PlayerUpgrades.tremorMakerUpgrades.Count)
                    {
                        case 5:
                            bonusBurnDamage = 1;
                            break;
                        case 6:
                            bonusBurnDamage = 2;
                            shouldSlow = true;
                            break;
                    }

                    GameObject burnTick = Instantiate(burnRadius, transform.position, Quaternion.identity);
                    burnTick.GetComponent<TremorBurnRadius>().Initialize(3, spriteRenderer.sortingOrder - 2, bonusBurnDamage, shouldSlow);
                }
                else
                {
                    spikeAttack = true;
                    switch (whatSpikeType)
                    {
                        case 0:
                            animator.Play("Small Rift Spike Emerge");
                            breakString = "Small Rift Spike Break";
                            break;
                        case 1:
                            animator.Play("Medium Rift Spike Emerge");
                            breakString = "Medium Rift Spike Break";
                            break;
                        case 2:
                            if (PlayerUpgrades.tremorMakerUpgrades.Count >= 6)
                            {
                                animator.Play("Magma Rift Spike Emerge");
                                breakString = "Magma Rift Spike Break";
                                bonusDamage += 3;
                            }
                            else
                            {
                                animator.Play("Large Rift Spike Emerge");
                                breakString = "Large Rift Spike Break";
                            }
                            break;
                    }

                    switch (PlayerUpgrades.tremorMakerUpgrades.Count)
                    {
                        case 5:
                            bonusDamage += 1;
                            break;
                        case 6:
                            bonusDamage += 2;
                            break;
                    }
                }
            }
        }

        damageAmount.originDamage += bonusDamage;
        damageAmount.updateDamage();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "StrongEnemy" || collision.tag == "MeleeEnemy" || collision.tag == "RangedEnemy" && spikeAttack)
        {
            Enemy enemy = collision.GetComponent<Enemy>();
            if (enemy != null && !stunnedEnemies.Contains(enemy))
            {
                enemy.stunEnemy(1.5f);
                stunnedEnemies.Add(enemy);
            }
        }
    }
}
