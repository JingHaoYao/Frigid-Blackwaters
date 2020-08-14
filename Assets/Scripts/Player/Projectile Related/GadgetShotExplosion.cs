using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GadgetShotExplosion : MonoBehaviour
{

    [SerializeField] AudioSource audioSource;
    [SerializeField] DamageAmount damageAmount;
    [SerializeField] CircleCollider2D circCol;

    private void Start()
    {
        circCol.enabled = false;
        
    }

    public void Initialize(int damage, float size)
    {
        transform.localScale = new Vector3(size, size, 0);
        damageAmount.originDamage = damage;
        damageAmount.updateDamage();
        StartCoroutine(gadgetShotExplosion());
    }


    IEnumerator gadgetShotExplosion()
    {
        circCol.enabled = false;
        audioSource.Play();
        yield return new WaitForSeconds(4 / 12f);
        circCol.enabled = true;
        yield return new WaitForSeconds(4 / 12f);
        circCol.enabled = false;
        yield return new WaitForSeconds(4 / 12f);
        Destroy(this.gameObject);
    }
}
