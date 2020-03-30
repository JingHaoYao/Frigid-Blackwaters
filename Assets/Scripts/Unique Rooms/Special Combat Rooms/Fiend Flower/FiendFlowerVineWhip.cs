using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FiendFlowerVineWhip : MonoBehaviour
{
    [SerializeField] CircleCollider2D damageCollider;
    [SerializeField] AudioSource whipAudio;

    IEnumerator vineProcess()
    {
        damageCollider.enabled = false;
        yield return new WaitForSeconds(9 / 12f);
        whipAudio.Play();
        damageCollider.enabled = true;
        yield return new WaitForSeconds(1 / 12f);
        damageCollider.enabled = false;
        yield return new WaitForSeconds(2 / 12f);
        damageCollider.enabled = true;
        yield return new WaitForSeconds(1 / 12f);
        damageCollider.enabled = false;
        yield return new WaitForSeconds(9 / 12f);
        Destroy(this.gameObject);
    }

    private void Start()
    {
        StartCoroutine(vineProcess());
    }
}
