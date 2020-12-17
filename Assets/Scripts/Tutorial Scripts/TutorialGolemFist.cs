using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TutorialGolemFist : MonoBehaviour
{
    [SerializeField] Animator animator;
    [SerializeField] GameObject shadow;
    [SerializeField] Sprite sunkenFist;
    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] GameObject damageHitbox;
    [SerializeField] GameObject obstacleHitbox;
    [SerializeField] AudioSource splashAudio;

    public void ResetFist()
    {
        StopAllCoroutines();
        shadow.transform.localScale = Vector3.zero;
        animator.Play("New State");
        spriteRenderer.sprite = sunkenFist;
        damageHitbox.SetActive(false);
        obstacleHitbox.SetActive(true);
    }

    public void SlamFist(Vector3 toPosition, bool turnOffDamageHitBox, UnityAction unityAction)
    {
        StartCoroutine(SlamFistProcedure(toPosition, turnOffDamageHitBox, unityAction));
    }

    IEnumerator followPlayer()
    {
        animator.Play("Fist Rise");
        obstacleHitbox.SetActive(false);
        while(true)
        {
            transform.position = PlayerProperties.playerShipPosition;
            yield return null;
        }
    }

    public void StartFollowPlayer()
    {
        StartCoroutine(followPlayer());
    }

    public void FistSlam()
    {
        StartCoroutine(fistSlam());
    }

    IEnumerator fistSlam()
    {
        animator.Play("Fist Slam");

        LeanTween.value(0.06f, 0f, 2 / 12f).setOnUpdate((float val) => { shadow.transform.localScale = new Vector3(val, val); });

        yield return new WaitForSeconds(2 / 12f);
        splashAudio.Play();
        damageHitbox.SetActive(true);

        yield return new WaitForSeconds(1 / 12f);
        damageHitbox.SetActive(false);
        yield return new WaitForSeconds(4 / 12f);
    }

    IEnumerator SlamFistProcedure(Vector3 toPosition, bool turnOffDamageHitbox, UnityAction betweenAction)
    {
        animator.Play("Fist Rise");
        LeanTween.value(0, 0.06f, 8 / 12f).setOnUpdate((float val) => { shadow.transform.localScale = new Vector3(val, val); });
        yield return new WaitForSeconds(8 / 12f);
        obstacleHitbox.SetActive(false);
        
        while(Vector2.Distance(transform.position, PlayerProperties.playerShipPosition) > 0.2f)
        {
            transform.position += (PlayerProperties.playerShipPosition - transform.position).normalized * Time.deltaTime * 12f;
            yield return null;
        }

        PlayerProperties.playerScript.windowAlreadyOpen = true;

        betweenAction.Invoke();

        animator.Play("Fist Slam");

        LeanTween.value(0.06f, 0f, 2 / 12f).setOnUpdate((float val) => { shadow.transform.localScale = new Vector3(val, val); });

        yield return new WaitForSeconds(2 / 12f);
        obstacleHitbox.SetActive(true);
        splashAudio.Play();
        if (!turnOffDamageHitbox)
        {
            damageHitbox.SetActive(true);
        }

        yield return new WaitForSeconds(1 / 12f);
        damageHitbox.SetActive(false);
        yield return new WaitForSeconds(4 / 12f);
    }
}
