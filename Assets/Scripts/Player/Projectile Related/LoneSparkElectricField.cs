using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoneSparkElectricField : MonoBehaviour
{
    [SerializeField] Animator animator;
    [SerializeField] CircleCollider2D circCol;
    [SerializeField] DamageAmount damageAmount;
    [SerializeField] GameObject empowerEffect;

    List<GameObject> weaponsBuffed = new List<GameObject>();
    Coroutine shockLoopRoutine;
    private int attackSteroid = 0;

    public void Initialize(int damageToSet, float duration, int attackSteroid)
    {
        damageAmount.originDamage = damageToSet;
        damageAmount.updateDamage();
        this.attackSteroid = attackSteroid;
        shockLoopRoutine = StartCoroutine(shockLoop());
        StartCoroutine(waitUntilDecay(duration));
    }

    IEnumerator waitUntilDecay(float dur)
    {
        yield return new WaitForSeconds(dur);

        StopCoroutine(shockLoopRoutine);

        animator.SetTrigger("Dissipate");

        Destroy(this.gameObject, 5 / 12f);
    }

    IEnumerator shockLoop()
    {
        while(true)
        {
            circCol.enabled = false;

            yield return new WaitForSeconds(0.2f);

            circCol.enabled = true;

            yield return new WaitForSeconds(0.2f);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.layer == 16) // Is player projectile?
        {
            DamageAmount damageInstant = collision.gameObject.GetComponent<DamageAmount>();

            if(damageInstant != null)
            {
                Instantiate(empowerEffect, collision.transform.position, Quaternion.identity);
                damageInstant.originDamage = damageInstant.originDamage + attackSteroid;
                damageInstant.updateDamage();
            }
        }
    }

}
