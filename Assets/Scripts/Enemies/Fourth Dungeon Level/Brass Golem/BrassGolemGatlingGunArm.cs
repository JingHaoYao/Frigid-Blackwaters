using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrassGolemGatlingGunArm : MonoBehaviour
{
    [SerializeField] Transform upperArm;
    [SerializeField] Transform gatlingGunForearm;

    [SerializeField] float defaultUpperArmRotation, defaultForeArmRotation;
    [SerializeField] Animator jointAnimator, upperArmAnimator, foreArmAnimator;

    [SerializeField] AudioSource loadOutAudio, heavyLoadOutAudio;
    [SerializeField] AudioSource gatlingGunAudio;

    [SerializeField] GameObject splash;
    [SerializeField] GameObject boss;

    [SerializeField] Sprite gatlingGunNeutral;
    [SerializeField] List<LineRenderer> lineRenderers;

    [SerializeField] List<SpriteRenderer> renderers;


    IEnumerator hitFrame()
    {
        foreach (SpriteRenderer spriteRenderer in renderers)
        {
            spriteRenderer.color = Color.red;
        }
        yield return new WaitForSeconds(.1f);
        foreach (SpriteRenderer spriteRenderer in renderers)
        {
            spriteRenderer.color = Color.white;
        }
    }

    public void startHitFrame()
    {
        StartCoroutine(hitFrame());
    }

    void backToWhite()
    {
        foreach (SpriteRenderer spriteRenderer in renderers)
        {
            spriteRenderer.color = Color.white;
        }
    }

    Vector3 centerOfRoom = new Vector3(1400, 0, 0);

    void rotateBackToOriginalPosition(float duration)
    {
        LeanTween.rotateLocal(upperArm.gameObject, new Vector3(0, 0, defaultUpperArmRotation), duration).setEaseOutCirc();
        LeanTween.rotateLocal(gatlingGunForearm.gameObject, new Vector3(0, 0, defaultForeArmRotation), duration).setEaseOutCirc();
    }

    public void setToUnactive()
    {
        upperArm.gameObject.SetActive(false);
        gatlingGunForearm.gameObject.SetActive(false);
        this.gameObject.SetActive(false);
    }

    public void StartLoadOut()
    {
        this.gameObject.SetActive(true);
        StartCoroutine(loadOut());
    }

    public void leftGatlingGunAttack()
    {
        StartCoroutine(gatlingGunAttackLeft());
    }

    public void rightGatlingGunAttack()
    {
        StartCoroutine(gatlingGunAttackRight());
    }

    public void centerGatlingGunAttack()
    {
        StartCoroutine(gatlingGunAttackCenter());
    }

    IEnumerator gatlingGunAttackLeft()
    {
        LeanTween.rotateLocal(upperArm.gameObject, new Vector3(0, 0, -42), 1.5f).setEaseOutCirc();
        LeanTween.rotateLocal(gatlingGunForearm.gameObject, new Vector3(0, 0, -90), 1.5f).setEaseOutCirc();

        yield return new WaitForSeconds(1.5f);

        gatlingGunAudio.Play();

        yield return new WaitForSeconds(0.5f);

        foreArmAnimator.enabled = true;
        foreArmAnimator.SetTrigger("Fire");

        for(int i = 0; i < 15; i++)
        {
            for(int k = 0; k < 5; k++)
            {
                float summonAngle = 202.5f + k * 11.25f;
                Vector3 positionToSpawn = (new Vector3(Mathf.Cos(summonAngle * Mathf.Deg2Rad), Mathf.Sin(summonAngle * Mathf.Deg2Rad)) * (i * 2.25f)) + centerOfRoom;
                StartCoroutine(briefLineRendererShow(positionToSpawn, k));
                GameObject splashInstant = Instantiate(splash, positionToSpawn, Quaternion.identity);
                splashInstant.GetComponent<ProjectileParent>().instantiater = boss;
                yield return new WaitForSeconds(0.05f);
            }
        }
        // In total waits five seconds

        foreArmAnimator.enabled = false;
        gatlingGunForearm.GetComponent<SpriteRenderer>().sprite = gatlingGunNeutral;

        rotateBackToOriginalPosition(2f);
    }

    IEnumerator briefLineRendererShow(Vector3 position, int index)
    {
        lineRenderers[index].gameObject.SetActive(true);
        lineRenderers[index].SetPositions(new Vector3[2] { lineRenderers[index].transform.position, position + Vector3.up * 0.25f });
        yield return new WaitForSeconds(0.15f);
        lineRenderers[index].gameObject.SetActive(false);
    }

    IEnumerator gatlingGunAttackCenter()
    {
        LeanTween.rotateLocal(upperArm.gameObject, new Vector3(0, 0, -113), 1.5f).setEaseOutCirc();
        LeanTween.rotateLocal(gatlingGunForearm.gameObject, new Vector3(0, 0, -3), 1.5f).setEaseOutCirc();

        yield return new WaitForSeconds(1.5f);

        gatlingGunAudio.Play();

        yield return new WaitForSeconds(0.5f);

        foreArmAnimator.enabled = true;
        foreArmAnimator.SetTrigger("Fire");

        for (int i = 0; i < 15; i++)
        {
            for (int k = 0; k < 5; k++)
            {
                float summonAngle = 247.5f + k * 11.25f;
                Vector3 positionToSpawn = (new Vector3(Mathf.Cos(summonAngle * Mathf.Deg2Rad), Mathf.Sin(summonAngle * Mathf.Deg2Rad)) * (i * 2.25f)) + centerOfRoom;
                StartCoroutine(briefLineRendererShow(positionToSpawn, k));
                GameObject splashInstant = Instantiate(splash, positionToSpawn, Quaternion.identity);
                splashInstant.GetComponent<ProjectileParent>().instantiater = boss;
                yield return new WaitForSeconds(0.05f);
            }
        }
        // In total waits five seconds

        foreArmAnimator.enabled = false;
        gatlingGunForearm.GetComponent<SpriteRenderer>().sprite = gatlingGunNeutral;

        rotateBackToOriginalPosition(2f);
    }

    IEnumerator gatlingGunAttackRight()
    {
        LeanTween.rotateLocal(upperArm.gameObject, new Vector3(0, 0, -140), 1.5f).setEaseOutCirc();
        LeanTween.rotateLocal(gatlingGunForearm.gameObject, new Vector3(0, 0, 72), 1.5f).setEaseOutCirc();

        yield return new WaitForSeconds(1.5f);

        gatlingGunAudio.Play();

        yield return new WaitForSeconds(0.5f);

        foreArmAnimator.enabled = true;
        foreArmAnimator.SetTrigger("Fire");

        for (int i = 0; i < 15; i++)
        {
            for (int k = 0; k < 5; k++)
            {
                float summonAngle = 292.5f + k * 11.25f;
                Vector3 positionToSpawn = (new Vector3(Mathf.Cos(summonAngle * Mathf.Deg2Rad), Mathf.Sin(summonAngle * Mathf.Deg2Rad)) * (i * 2.25f)) + centerOfRoom;
                StartCoroutine(briefLineRendererShow(positionToSpawn, k));
                GameObject splashInstant = Instantiate(splash, positionToSpawn, Quaternion.identity);
                splashInstant.GetComponent<ProjectileParent>().instantiater = boss;
                yield return new WaitForSeconds(0.05f);
            }
        }
        // In total waits five seconds

        foreArmAnimator.enabled = false;
        gatlingGunForearm.GetComponent<SpriteRenderer>().sprite = gatlingGunNeutral;

        rotateBackToOriginalPosition(2f);
    }

    IEnumerator loadOut()
    {
        transform.localPosition = new Vector3(0, 0.96f);
        LeanTween.moveLocalX(this.gameObject, 0.45f, 0.75f);
        upperArm.localRotation = Quaternion.Euler(0, 0, defaultUpperArmRotation);
        gatlingGunForearm.localRotation = Quaternion.Euler(0, 0, defaultForeArmRotation);
        upperArm.gameObject.SetActive(false);
        gatlingGunForearm.gameObject.SetActive(false);

        loadOutAudio.Play();
        jointAnimator.SetTrigger("LoadOut");

        yield return new WaitForSeconds(8 / 12f);

        upperArm.gameObject.SetActive(true);
        loadOutAudio.Play();
        upperArmAnimator.SetTrigger("LoadOut");

        yield return new WaitForSeconds(16 / 12f);

        gatlingGunForearm.gameObject.SetActive(true);
        heavyLoadOutAudio.Play();
        foreArmAnimator.SetTrigger("LoadOut");
        yield return new WaitForSeconds(17 / 12f);

        foreArmAnimator.enabled = false;
        // done
    }

    IEnumerator testLoop()
    {
        while (true)
        {
            StartCoroutine(gatlingGunAttackLeft());
            yield return new WaitForSeconds(10f);
            StartCoroutine(gatlingGunAttackCenter());
            yield return new WaitForSeconds(10f);
            StartCoroutine(gatlingGunAttackRight());
            yield return new WaitForSeconds(10f);
        }
    }

    public void armDieDown()
    {
        StopAllCoroutines();

        foreach(LineRenderer lr in lineRenderers)
        {
            lr.enabled = false;
        }

        backToWhite();
        LeanTween.rotateLocal(upperArm.gameObject, new Vector3(0, 0, -100), 1.5f).setEaseOutCirc();
        LeanTween.rotateLocal(gatlingGunForearm.gameObject, new Vector3(0, 0, -32), 1.5f).setEaseOutCirc();
    }
}
