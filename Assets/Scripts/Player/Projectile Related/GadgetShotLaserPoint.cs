using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GadgetShotLaserPoint : MonoBehaviour
{
    [SerializeField] LineRenderer lineRenderer;
    [SerializeField] Animator animator;
    [SerializeField] GameObject hitBox;
    [SerializeField] DamageAmount hitBoxDamageAmount;
    [SerializeField] AudioSource laserFlashAudio;
    [SerializeField] SpriteRenderer spriteRenderer;

    bool shouldActive = false;
    GadgetShotLaserPoint prevPoint;

    public void Initialize(bool shouldActivate, GadgetShotLaserPoint previousPoint, int damageToDeal)
    {
        this.shouldActive = shouldActivate;
        this.prevPoint = previousPoint;
        this.hitBoxDamageAmount.originDamage = damageToDeal;
        this.hitBoxDamageAmount.updateDamage();
    }

    public void Activate()
    {
        if (shouldActive)
        {
            lineRenderer.enabled = true;
            StartCoroutine(activateLaser());
        }
        else
        {
            animator.SetTrigger("Activate");
            Destroy(this.gameObject, 10 / 12f);
        }
    }

    IEnumerator activateLaser()
    {
        animator.SetTrigger("Activate");
        lineRenderer.enabled = true;
        lineRenderer.sortingOrder = spriteRenderer.sortingOrder;
        lineRenderer.startWidth = 0;
        lineRenderer.endWidth = 0;
        lineRenderer.SetPosition(1, prevPoint.transform.position);
        lineRenderer.SetPosition(0, transform.position);

        LeanTween.value(0, 0.2f, 4 / 12f).setOnUpdate((float val) => { lineRenderer.startWidth = val; lineRenderer.endWidth = val; });

        float distance = Vector2.Distance(transform.position, prevPoint.transform.position);
        float angle = Mathf.Atan2(prevPoint.transform.position.y - transform.position.y, prevPoint.transform.position.x - transform.position.x);

        hitBox.transform.position = transform.position + new Vector3(Mathf.Cos(angle), Mathf.Sin(angle)) * distance / 2f;
        hitBox.transform.rotation = Quaternion.Euler(0, 0, angle * Mathf.Rad2Deg);
        hitBox.GetComponent<BoxCollider2D>().size = new Vector3(distance / 2, 0.1f);

        yield return new WaitForSeconds(4 / 12f);

        laserFlashAudio.Play();

        hitBox.SetActive(true);

        LeanTween.value(0.2f, 0, 4 / 12f).setOnUpdate((float val) => { lineRenderer.startWidth = val; lineRenderer.endWidth = val; }).setOnComplete(() => { lineRenderer.enabled = false; });

        yield return new WaitForSeconds(2 / 12f);

        hitBox.SetActive(false);

        yield return new WaitForSeconds(4 / 12f);

        Destroy(this.gameObject);
    } 
}
