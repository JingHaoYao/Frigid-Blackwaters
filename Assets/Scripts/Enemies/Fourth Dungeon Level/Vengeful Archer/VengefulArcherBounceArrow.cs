using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VengefulArcherBounceArrow : MonoBehaviour
{
    [SerializeField] Animator animator;
    [SerializeField] Collider2D damageCollider;
    [SerializeField] float speed;
    [SerializeField] ProjectileParent projectileParent;
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioSource impactAudio;
    [SerializeField] GameObject lightningEffect;

    Vector3 travelVector;
    private int numberBounces = 0;

    [SerializeField] LayerMask collisionLayerMask;
    Coroutine mainLoopInstant;
    Vector3 lastPositionHit;

    public void Initialize(float angleTravel, GameObject parent)
    {
        travelVector = new Vector3(Mathf.Cos(angleTravel), Mathf.Sin(angleTravel)) * speed;
        transform.rotation = Quaternion.Euler(0, 0, angleTravel * Mathf.Rad2Deg);
        mainLoopInstant = StartCoroutine(mainLoop());
        lastPositionHit = Vector3.one;
        projectileParent.instantiater = parent;
    }

    IEnumerator mainLoop()
    {
        while (numberBounces < 10)
        {
            transform.position += travelVector * Time.deltaTime;
            yield return null;
        }

        LeanTween.value(14, 0, 1.5f).setEaseOutCirc().setOnUpdate((float val) => { speed = val; });
        LeanTween.alpha(this.gameObject, 0, 1.5f);
        damageCollider.enabled = false;
        yield return new WaitForSeconds(1.5f);
        Destroy(this.gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 12)
        {
            Vector3 normalVector = collision.GetContact(0).normal;
            travelVector = Vector3.Reflect(travelVector, normalVector);
            lastPositionHit = new Vector3(transform.position.x, transform.position.y, 0);
            transform.rotation = Quaternion.Euler(0, 0, Mathf.Atan2(travelVector.y, travelVector.x) * Mathf.Rad2Deg);
            numberBounces++;
            audioSource.Play();
            impactAudio.Play();
            Instantiate(lightningEffect, collision.GetContact(0).point, Quaternion.identity);
        }
    }
}
