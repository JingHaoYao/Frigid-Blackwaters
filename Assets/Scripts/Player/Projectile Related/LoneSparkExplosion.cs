using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoneSparkExplosion : MonoBehaviour
{
    [SerializeField] Animator animator;
    [SerializeField] CircleCollider2D circCol;
    [SerializeField] int whatExplosionType = 0;
    // 0 - normal
    // 1 - additional explode
    // 2 - electric

    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] AudioSource audioSource;

    [Header("Electric Field Properties")]
    [SerializeField] GameObject electricField;
    [SerializeField] private int attackSteroid;
    [SerializeField] private int electricFieldDamage;

    [Header("Additional Explosions Properties")]
    [SerializeField] GameObject additionalExplosion;

    bool shouldSpawnExplosion = false;

    public void Initialize(float delayBeforeExplosion, int numberExplosionsAlready, int additionalDamage)
    {
        spriteRenderer.enabled = false;
        shouldSpawnExplosion = whatExplosionType == 1 && numberExplosionsAlready > 0;
        StartCoroutine(explosionProcess(delayBeforeExplosion, numberExplosionsAlready));
        GetComponent<DamageAmount>().addDamage(additionalDamage);
    }

    IEnumerator explosionProcess(float delay, int prevExplosions)
    {
        circCol.enabled = false;

        yield return new WaitForSeconds(delay);

        spriteRenderer.enabled = true;
        animator.SetTrigger("Explode");
        audioSource.Play();

        yield return new WaitForSeconds(4 / 12f);

        circCol.enabled = true;

        yield return new WaitForSeconds(5 / 12f);

        // probably wanna spawn here

        if (whatExplosionType == 2)
        {
            GameObject field = Instantiate(electricField, transform.position, Quaternion.identity);
            field.GetComponent<LoneSparkElectricField>().Initialize(electricFieldDamage, 4, attackSteroid);
        }

        circCol.enabled = true;

        yield return new WaitForSeconds(4 / 12f);

        if (shouldSpawnExplosion)
        {
            GameObject explosionInstant = Instantiate(additionalExplosion, transform.position, Quaternion.identity);
            explosionInstant.GetComponent<LoneSparkExplosion>().Initialize(0.5f, prevExplosions - 1, 0);
        }

        Destroy(this.gameObject);
    }


}
