using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoddessSpearImpact : MonoBehaviour
{
    [SerializeField] CircleCollider2D circCol;
    [SerializeField] AudioSource stabAudio;

    private void Start()
    {
        circCol.enabled = false;
        StartCoroutine(goddessSpearImpact());
    }

    IEnumerator goddessSpearImpact()
    {
        yield return new WaitForSeconds(10 / 12f);
        stabAudio.Play();
        circCol.enabled = true;
        yield return new WaitForSeconds(3 / 12f);
        circCol.enabled = false;
        yield return new WaitForSeconds(12 / 12f);
        Destroy(this.gameObject);
    }
}
