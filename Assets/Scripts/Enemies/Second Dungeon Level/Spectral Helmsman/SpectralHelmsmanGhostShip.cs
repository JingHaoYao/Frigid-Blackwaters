using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpectralHelmsmanGhostShip : MonoBehaviour
{
    public bool leftFiring;
    public GameObject cannonBall;
    [SerializeField] private Animator animator;
    [SerializeField] private AudioSource audioSource;
    private float waitUntilFirePeriod;
    public GameObject spectralHelmsman;

    IEnumerator fadeOut()
    {
        yield return new WaitForSeconds(2.7f);
        animator.SetTrigger("FadeOut");
    }

    void Start()
    {
        waitUntilFirePeriod = Random.Range(0.0f, 2.5f);
        StartCoroutine(fireCannons());
        StartCoroutine(fadeOut());
        LeanTween.moveY(this.gameObject, 10f, 3.5f).setOnComplete(() => { Destroy(this.gameObject); });
    }

    IEnumerator fireCannons()
    {
        yield return new WaitForSeconds(waitUntilFirePeriod);
        animator.SetTrigger("Fire");
        yield return new WaitForSeconds(4f / 12f);
        audioSource.Play();
        if (leftFiring) {
        
            GameObject cannonBallInstant = Instantiate(cannonBall, transform.position + new Vector3(-1.2f, 0.6f), Quaternion.identity);
            cannonBallInstant.GetComponent<ProjectileParent>().instantiater = spectralHelmsman;
            cannonBallInstant.GetComponent<SkeletalMusketRound>().angleTravel = 180 * Mathf.Deg2Rad;
            cannonBallInstant = Instantiate(cannonBall, transform.position + new Vector3(-1.2f, -0.5f), Quaternion.identity);
            cannonBallInstant.GetComponent<ProjectileParent>().instantiater = spectralHelmsman;
            cannonBallInstant.GetComponent<SkeletalMusketRound>().angleTravel = 180 * Mathf.Deg2Rad;
        }
        else
        {
            GameObject cannonBallInstant = Instantiate(cannonBall, transform.position + new Vector3(1.2f, 0.6f), Quaternion.identity);
            cannonBallInstant.GetComponent<ProjectileParent>().instantiater = spectralHelmsman;
            cannonBallInstant = Instantiate(cannonBall, transform.position + new Vector3(1.2f, -0.5f), Quaternion.identity);
            cannonBallInstant.GetComponent<ProjectileParent>().instantiater = spectralHelmsman;
        }
    }
}
