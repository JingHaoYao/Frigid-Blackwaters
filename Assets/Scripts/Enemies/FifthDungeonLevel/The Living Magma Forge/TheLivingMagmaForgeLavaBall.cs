using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TheLivingMagmaForgeLavaBall : MonoBehaviour
{
    public Vector3 targetLocation = Vector3.zero;
    [SerializeField] private CircleCollider2D circCol;
    [SerializeField] private GameObject shadow;
    [SerializeField] private float speed;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] Animator animator;
    [SerializeField] AudioSource impactAudio;
    [SerializeField] ProjectileParent projectileParent;

    private float totalTime;
    private float currProgress = 0;
    Vector3 unitVector;
    private Vector3 tempTransform = Vector3.zero;
    private float currentTime = 0;

    public void Initialize(GameObject instantiater, Vector3 targetPosition)
    {
        projectileParent.instantiater = instantiater;
        this.targetLocation = targetPosition;
        StartCoroutine(MovementLoop());
    }

    IEnumerator MovementLoop()
    {
        circCol.enabled = false;
        totalTime = Vector2.Distance(targetLocation, transform.position) / speed;
        LeanTween.move(shadow, targetLocation, totalTime);
        LeanTween.value(0, 1, totalTime / 2).setOnUpdate((float val) => { currProgress = val; }).setEaseOutQuad().setOnComplete(() => { LeanTween.value(1, 0, totalTime / 2).setOnUpdate((float val) => { currProgress = val; }).setEaseInQuad(); });
        tempTransform = transform.position;
        unitVector = (targetLocation - transform.position).normalized;

        while (true)
        {
            tempTransform += unitVector * Time.deltaTime * speed;
            shadow.transform.localScale = new Vector3(0.05f, 0.05f) * currProgress;
            transform.position = tempTransform + new Vector3(0, 8 * currProgress);

            currentTime += Time.deltaTime;

            circCol.enabled = currProgress <= 0.2f;

            spriteRenderer.sortingOrder = (200 - (int)((transform.position.y - 5 * currProgress) * 10));

            if (currentTime >= totalTime)
            {
                animator.SetTrigger("Impact");
                impactAudio.Play();
                Destroy(this.gameObject, 5 / 12f);

                break;
            }

            yield return null;
        }
    }
}
