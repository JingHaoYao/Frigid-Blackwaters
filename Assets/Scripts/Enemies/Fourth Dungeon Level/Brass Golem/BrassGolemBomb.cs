using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrassGolemBomb : MonoBehaviour
{
    public Vector3 targetLocation = Vector3.zero;
    [SerializeField] private GameObject shadow;
    [SerializeField] private float speed;
    [SerializeField] private SpriteRenderer spriteRenderer;
    private float totalTime;
    private float currProgress = 0;
    Vector3 unitVector;
    private Vector3 tempTransform = Vector3.zero;
    private float currentTime = 0;
    [SerializeField] AudioSource chargeUp, explosion;
    [SerializeField] ProjectileParent projectileParent;
    [SerializeField] Animator animator;
    [SerializeField] Collider2D collider;
    bool exploded = false;

    public void Initialize(GameObject boss, Vector3 targetPosition)
    {
        projectileParent.instantiater = boss;
        this.targetLocation = targetPosition;

        totalTime = Vector2.Distance(targetLocation, transform.position) / speed;
        LeanTween.move(shadow, targetLocation, totalTime);
        LeanTween.value(0, 1, totalTime / 2).setOnUpdate((float val) => { currProgress = val; }).setEaseOutQuad().setOnComplete(() => { LeanTween.value(1, 0, totalTime / 2).setOnUpdate((float val) => { currProgress = val; }).setEaseInQuad(); });
        tempTransform = transform.position;
        unitVector = (targetLocation - transform.position).normalized;

        StartCoroutine(throwProcedure());
    }

    IEnumerator explosionProcedure()
    {
        animator.SetTrigger("Explode");
        Destroy(this.gameObject, 1.167f);
        chargeUp.Play();
        yield return new WaitForSeconds(9 / 12f);
        collider.enabled = true;
        explosion.Play();
        yield return new WaitForSeconds(3 / 12f);
        collider.enabled = false;
        yield return new WaitForSeconds(2 / 12f);
    }

    IEnumerator throwProcedure()
    {
        while (currentTime < totalTime)
        {
            tempTransform += unitVector * Time.deltaTime * speed;
            shadow.transform.localScale = new Vector3(0.05f, 0.05f) * currProgress;
            transform.position = tempTransform + new Vector3(0, 8 * currProgress);

            currentTime += Time.deltaTime;

            spriteRenderer.sortingOrder = (200 - (int)((transform.position.y - 8 * currProgress) * 10));

            if(currentTime / totalTime > 0.7f && exploded == false)
            {
                exploded = true;
                StartCoroutine(explosionProcedure());
            }

            yield return null;
        }
    }
}
