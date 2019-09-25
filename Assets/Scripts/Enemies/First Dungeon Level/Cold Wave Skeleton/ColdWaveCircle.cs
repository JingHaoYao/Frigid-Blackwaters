using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColdWaveCircle : MonoBehaviour
{
    Animator animator;
    public GameObject coldWave;
    public float angleAttack = 0;

    IEnumerator summonColdWave(float duration)
    {
        yield return new WaitForSeconds(0.583f);
        GameObject wave = Instantiate(coldWave, transform.position + new Vector3(Mathf.Cos(angleAttack * Mathf.Deg2Rad), Mathf.Sin(angleAttack * Mathf.Deg2Rad)) * 0.3f, Quaternion.Euler(0, 0, angleAttack + 90));
        wave.GetComponent<ProjectileParent>().instantiater = this.GetComponent<ProjectileParent>().instantiater;
        yield return new WaitForSeconds(duration);
        animator.SetTrigger("Spawn Out");
        yield return new WaitForSeconds(0.75f);
        Destroy(this.gameObject);
    }

    void Start()
    {
        animator = GetComponent<Animator>();
        transform.rotation = Quaternion.Euler(0, 0, angleAttack);
        StartCoroutine(summonColdWave(4f));
    }
}
