using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NymphCannon : MonoBehaviour
{
    [SerializeField] Animator animator;
    [SerializeField] LineRenderer lineRenderer;
    [SerializeField] AudioSource chargeAudio, fireAudio;
    [SerializeField] GameObject laserImpact;
    [SerializeField] GameObject eIndicator;
    [SerializeField] SpriteRenderer spriteRenderer;
    Coroutine flashRoutine;

    public TheBrassGolem boss;
    int leanTweenId = 0;

    IEnumerator flashLoop()
    {
        while (true)
        {
            leanTweenId = LeanTween.color(this.gameObject, Color.gray, 0.5f).id;
            yield return new WaitForSeconds(0.5f);
            leanTweenId = LeanTween.color(this.gameObject, Color.white, 0.5f).id;
            yield return new WaitForSeconds(0.5f);
        }
    }

    IEnumerator fireLaser()
    {
        animator.SetTrigger("Fire");
        chargeAudio.Play();

        yield return new WaitForSeconds(6 / 12f);

        fireAudio.Play();
        lineRenderer.enabled = true;

        boss.StartRemoveArmor();
        boss.dealDamage(20);

        Vector3 spawnPosition = lineRenderer.GetPosition(1);

        float angle = Mathf.Atan2(spawnPosition.y - transform.position.y, spawnPosition.x - transform.position.x) * Mathf.Rad2Deg;

        Instantiate(laserImpact, spawnPosition, Quaternion.Euler(0, 0, angle + 180));

        yield return new WaitForSeconds(2 / 12f);
        lineRenderer.enabled = false;

        yield return new WaitForSeconds(2 / 12f);
    }

    public void StartCannonProcedure()
    {
        StartCoroutine(procCannon());
        flashRoutine = StartCoroutine(flashLoop());
    }

    IEnumerator procCannon()
    {
        float period = 0;

        while(period < 8)
        {
            period += Time.deltaTime;

            if(Vector2.Distance(transform.position, PlayerProperties.playerShipPosition) < 4.5f)
            {
                eIndicator.SetActive(true);

                if (Input.GetKeyDown(KeyCode.E))
                {
                    eIndicator.SetActive(false);
                    StartCoroutine(fireLaser());
                    break;
                }
            }
            yield return null;
        }


        eIndicator.SetActive(false);
        StopCoroutine(flashRoutine);
        boss.StopIndicator();
        LeanTween.cancel(leanTweenId);
        spriteRenderer.color = Color.white;
    }
    
}
