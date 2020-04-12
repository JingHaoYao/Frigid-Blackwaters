using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarnivorousTangleFiendVineSpike : MonoBehaviour
{
    [SerializeField] CircleCollider2D damageCollider;
    [SerializeField] AudioSource splashAudio;

    private void Start()
    {
        StartCoroutine(spikeAttack());
    }

    IEnumerator spikeAttack()
    {
        damageCollider.enabled = false;
        splashAudio.Play();
        yield return new WaitForSeconds(2 / 12f);
        damageCollider.enabled = true;
        yield return new WaitForSeconds(3 / 12f);
        damageCollider.enabled = false;
        yield return new WaitForSeconds(8 / 12f);
        Destroy(this.gameObject);
    }
}
