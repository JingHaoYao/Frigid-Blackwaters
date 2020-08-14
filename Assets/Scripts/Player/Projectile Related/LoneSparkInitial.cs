using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoneSparkInitial : PlayerProjectile
{
    [SerializeField] GameObject normalExplosion1, normalExplosion2;
    [SerializeField] GameObject electricExplosion1, electricExplosion2, electricExplosion3;
    [SerializeField] GameObject additionalExplosion1, additionalExplosion2;
    [SerializeField] Animator animator;
    [SerializeField] DamageAmount damageAmount;
    [SerializeField] ParticleSystem particleSystem;
    [SerializeField] SpriteRenderer spriteRenderer;

    Vector3 targetPosition;

    void pickIdleAnim()
    {
        if(PlayerUpgrades.loneSparkUpgrades.Count > 3)
        {
            if(PlayerUpgrades.loneSparkUpgrades[3] == "static_field_upgrade")
            {
                animator.SetTrigger("Idle2");
            }
            else
            {
                animator.SetTrigger("Idle1");
            }
        }
    }

    private void Start()
    {
        targetPosition = PlayerProperties.cursorPosition;
        pickIdleAnim();
        StartCoroutine(projectileProcedure());
    }

    IEnumerator projectileProcedure()
    {
        particleSystem.Stop();
        particleSystem.transform.localRotation = Quaternion.Euler(0, 0, angletoCursor() * Mathf.Rad2Deg + 90);  
        float distance = Vector2.Distance(targetPosition, transform.position);
        LeanTween.move(this.gameObject, transform.position + new Vector3(Mathf.Cos(angletoCursor()), Mathf.Sin(angletoCursor())) * distance * 0.2f, 2.5f).setEaseOutQuad();

        yield return new WaitForSeconds(2.5f);

        particleSystem.Play();
        LeanTween.move(this.gameObject, targetPosition, 0.5f).setEaseInQuad();

        yield return new WaitForSeconds(0.5f);

        spawnExplosion();
        particleSystem.Stop();
        spriteRenderer.enabled = false;
        Destroy(this.gameObject, 1f);
    }

    void spawnExplosion()
    {
        string upgradeName = "";
        if(PlayerUpgrades.loneSparkUpgrades.Count > 3)
        {
            upgradeName = PlayerUpgrades.loneSparkUpgrades[3];
        }

        GameObject explosionInstant;

        switch (PlayerUpgrades.loneSparkUpgrades.Count)
        {
            case 0:
                explosionInstant = Instantiate(normalExplosion1, transform.position, Quaternion.identity);
                explosionInstant.GetComponent<LoneSparkExplosion>().Initialize(0, 0, damageAmount.damage);
                break;
            case 1:
                explosionInstant = Instantiate(normalExplosion1, transform.position, Quaternion.identity);
                explosionInstant.GetComponent<LoneSparkExplosion>().Initialize(0, 0, damageAmount.damage);
                break;
            case 2:
                explosionInstant = Instantiate(normalExplosion2, transform.position, Quaternion.identity);
                explosionInstant.GetComponent<LoneSparkExplosion>().Initialize(0, 0, damageAmount.damage);
                break;
            case 3:
                explosionInstant = Instantiate(normalExplosion2, transform.position, Quaternion.identity);
                explosionInstant.GetComponent<LoneSparkExplosion>().Initialize(0, 0, damageAmount.damage);
                break;
            case 4:
                if(upgradeName == "static_field_upgrade")
                {
                    explosionInstant = Instantiate(electricExplosion1, transform.position, Quaternion.identity);
                    explosionInstant.GetComponent<LoneSparkExplosion>().Initialize(0, 0, damageAmount.damage);
                }
                else
                {
                    explosionInstant = Instantiate(additionalExplosion1, transform.position, Quaternion.identity);
                    explosionInstant.GetComponent<LoneSparkExplosion>().Initialize(0, 3, damageAmount.damage);
                }
                break;
            case 5:
                if (upgradeName == "static_field_upgrade")
                {
                    explosionInstant = Instantiate(electricExplosion2, transform.position, Quaternion.identity);
                    explosionInstant.GetComponent<LoneSparkExplosion>().Initialize(0, 0, damageAmount.damage);
                }
                else
                {
                    explosionInstant = Instantiate(additionalExplosion2, transform.position, Quaternion.identity);
                    explosionInstant.GetComponent<LoneSparkExplosion>().Initialize(0, 3, damageAmount.damage);
                }
                break;
            case 6:
                if (upgradeName == "static_field_upgrade")
                {
                    explosionInstant = Instantiate(electricExplosion3, transform.position, Quaternion.identity);
                    explosionInstant.GetComponent<LoneSparkExplosion>().Initialize(0, 0, damageAmount.damage);
                }
                else
                {
                    explosionInstant = Instantiate(additionalExplosion2, transform.position, Quaternion.identity);
                    explosionInstant.GetComponent<LoneSparkExplosion>().Initialize(0, 5, damageAmount.damage);
                }
                break;
        }
    }


    float angletoCursor()
    {
        return Mathf.Atan2(PlayerProperties.cursorPosition.y - transform.position.y, PlayerProperties.cursorPosition.x - transform.position.x);
    }
}
