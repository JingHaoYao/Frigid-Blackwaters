using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class FireReaperScythe : MonoBehaviour
{
    [SerializeField] Animator animator;
    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] GameObject swipe;
    [SerializeField] PolygonCollider2D collider;
    [SerializeField] AudioSource swipeAudio;
    GameObject bossEnemy;

    public void StartUp(GameObject bossEnemy)
    {
        animator.Play("Fire Scythe Ignite");
        this.bossEnemy = bossEnemy;
    }

    IEnumerator blinkWhite()
    {
        for(int i = 0; i < 3; i++)
        {
            spriteRenderer.material.SetFloat("_FlashAmount", 1);
            yield return new WaitForSeconds(0.1f);
            spriteRenderer.material.SetFloat("_FlashAmount", 0);
            yield return new WaitForSeconds(0.1f);
        }
    }

    public void UpdatePosition(Vector3 position, int sortingLayer) 
    {
        transform.position = position;
        spriteRenderer.sortingOrder = sortingLayer;
    }

    public void swipeAttack(float startingAngle, float endingAngle, UnityAction onComplete)
    {
        StartCoroutine(swipeAttackRoutine(startingAngle, endingAngle, onComplete));
    }

    IEnumerator swipeAttackRoutine(float startingAngle, float endingAngle, UnityAction onComplete)
    {
        LeanTween.rotate(this.gameObject, new Vector3(0, 0, startingAngle), 0.2f);

        yield return new WaitForSeconds(0.2f);

        GameObject swipeInstant = Instantiate(swipe, transform.position, Quaternion.Euler(0, 0, ((startingAngle + endingAngle) / 2) + 90));
        swipeInstant.GetComponent<ProjectileParent>().instantiater = bossEnemy;
        LeanTween.rotate(this.gameObject, new Vector3(0, 0, endingAngle), 9 / 12f).setEaseOutQuad();

        yield return new WaitForSeconds(9 / 12f);

        LeanTween.rotate(this.gameObject, new Vector3(0, 0, 90), 0.2f);

        yield return new WaitForSeconds(0.2f);

        onComplete.Invoke();
    }

    public void hookAttack(float startingAngle, UnityAction onComplete)
    {
        StartCoroutine(swipeHookRoutine(startingAngle, onComplete));
    }

    IEnumerator swipeHookRoutine(float startingAngle, UnityAction onComplete)
    {
        LeanTween.rotate(this.gameObject, new Vector3(0, 0, startingAngle), 0.2f);

        yield return new WaitForSeconds(0.2f);

        StartCoroutine(blinkWhite());

        yield return new WaitForSeconds(0.6f);
        swipeAudio.Play();

        collider.enabled = true;

        LeanTween.rotate(this.gameObject, new Vector3(0, 0, startingAngle + 179), 0.25f).setEaseOutQuad();

        yield return new WaitForSeconds(0.25f);

        collider.enabled = false;
        LeanTween.rotate(this.gameObject, new Vector3(0, 0, 90), 0.2f);

        yield return new WaitForSeconds(0.2f);

        onComplete.Invoke();
    }

    public void DieDown()
    {
        StopAllCoroutines();
        LeanTween.rotate(this.gameObject, new Vector3(0, 0, 90), 0.5f);
        collider.enabled = false;
        animator.Play("Fire Scythe Burn Out");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "playerHitBox")
        {
            float angleToTravel = (transform.rotation.eulerAngles.z + 90) * Mathf.Deg2Rad;
            PlayerProperties.playerScript.setPlayerEnemyMomentum(new Vector3(Mathf.Cos(angleToTravel), Mathf.Sin(angleToTravel)) * 18, 1.5f);
        }
    }
}
