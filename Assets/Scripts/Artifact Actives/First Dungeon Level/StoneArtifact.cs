using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoneArtifact : MonoBehaviour
{
    public GameObject pulse;
    public GameObject waterSplash;
    Animator animator;

    IEnumerator pulseWaves(int numberPulses)
    {
        for(int i = 0; i < numberPulses; i++)
        {
            Instantiate(pulse, transform.position, Quaternion.identity);
            yield return new WaitForSeconds(0.5f);
        }
        animator.SetTrigger("Sink");
        yield return new WaitForSeconds(1 / 12f);
        Instantiate(waterSplash, transform.position, Quaternion.identity);
        yield return new WaitForSeconds(3 / 12f);
        Destroy(this.gameObject);
    }

    void Start()
    {
        animator = GetComponent<Animator>();
        StartCoroutine(pulseWaves(5));
    }
}
