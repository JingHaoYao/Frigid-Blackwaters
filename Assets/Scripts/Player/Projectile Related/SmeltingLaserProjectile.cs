using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmeltingLaserProjectile : PlayerProjectile
{
    int damageCap = 0;
    bool focusingBeam = false;
    [SerializeField] Animator animator;
    [SerializeField] AudioSource audioSource;
    [SerializeField] DamageAmount damageAmount;
    [SerializeField] Collider2D collider;
    SmeltingLaserUpgradeManager upgradeManager;
    int baseDamage = 2;
    List<Enemy> hitEnemies = new List<Enemy>();

    public void Initialize(Vector3 spawnPosition, Vector3 targetPosition, float time, int extraBonus, SmeltingLaserUpgradeManager manager)
    {
        collider.enabled = false;
        transform.position = spawnPosition;
        baseDamage += extraBonus; // for the upgrade stuff
        applyUpgrades();
        StartCoroutine(laserRoutine(targetPosition, time));
        upgradeManager = manager;
    }

    IEnumerator laserRoutine(Vector3 targetPosition, float time)
    {
        if(focusingBeam)
        {
            animator.Play("Blue Laser Start");
        }

        audioSource.volume = 0;
        audioSource.Play();
        LeanTween.value(0, 1, 5 / 12f).setOnUpdate((float val) => { audioSource.volume = val; });

        yield return new WaitForSeconds(5 / 12f);

        collider.enabled = true;
        animator.Play(focusingBeam ? "Blue Laser Idle" : "Smelting Laser Idle");

        LeanTween.move(this.gameObject, targetPosition, time).setOnUpdate(UpdateDamage);

        yield return new WaitForSeconds(time);

        collider.enabled = false;
        animator.Play(focusingBeam ? "Blue Laser End" : "Smelting Laser End");
        LeanTween.value(1, 0, 6 / 12f).setOnUpdate((float val) => { audioSource.volume = val; });

        yield return new WaitForSeconds(6 / 12f);

        Destroy(this.gameObject);
    }

    void UpdateDamage(float val)
    {
        if(focusingBeam)
        {
            damageAmount.originDamage = Mathf.RoundToInt(val * (damageCap) + baseDamage);
            transform.localScale = new Vector3(5 + 3 * val, 5 + 3 * val);
            damageAmount.updateDamage();
        }
    }
    
    void applyUpgrades()
    {
        if(PlayerUpgrades.smeltingLaserUpgrades.Count >= 2)
        {
            baseDamage++;
            if(PlayerUpgrades.smeltingLaserUpgrades.Count >= 4)
            {
                if(PlayerUpgrades.smeltingLaserUpgrades[3] == "focusing_laser_upgrade")
                {
                    focusingBeam = true;
                    switch(PlayerUpgrades.smeltingLaserUpgrades.Count)
                    {
                        case 4:
                            damageCap = 6;
                            break;
                        case 5:
                            damageCap = 8;
                            break;
                        case 6:
                            damageCap = 12;
                            break;
                    }
                }
            }
        }
        damageAmount.originDamage = baseDamage;
        damageAmount.updateDamage();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "StrongEnemy" || collision.tag == "MeleeEnemy" || collision.tag == "RangedEnemy")
        {
            Enemy collidedEnemy = collision.GetComponent<Enemy>();

            if(collidedEnemy != null && !hitEnemies.Contains(collidedEnemy))
            {
                upgradeManager.SpawnMetalChunk(transform.position);
                hitEnemies.Add(collidedEnemy);
            }
        }
    }
}
