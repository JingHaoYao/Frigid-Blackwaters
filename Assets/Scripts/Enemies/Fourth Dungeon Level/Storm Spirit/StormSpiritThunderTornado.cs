using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StormSpiritThunderTornado : MonoBehaviour
{
    [SerializeField] Animator animator;
    [SerializeField] Collider2D damageCollider;
    [SerializeField] float speed;
    [SerializeField] ProjectileParent projectileParent;

    Vector3 travelVector;
    private int numberBounces = 0;

    [SerializeField] LayerMask collisionLayerMask;
    Coroutine mainLoopInstant;
    Vector3 lastPositionHit;

    public void Initialize(float angleTravel, GameObject parent)
    {
        travelVector = new Vector3(Mathf.Cos(angleTravel), Mathf.Sin(angleTravel)) * speed;
        mainLoopInstant = StartCoroutine(mainLoop());
        lastPositionHit = Vector3.one;
        projectileParent.instantiater = parent;
    }

    IEnumerator mainLoop()
    {
        while (numberBounces < 3)
        {
            transform.position += travelVector * Time.deltaTime;
            yield return null;
        }

        damageCollider.enabled = false;
        animator.SetTrigger("Impact");
        yield return new WaitForSeconds(5 / 12f);
        Destroy(this.gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 12 /*&& (Vector2.Distance(lastPositionHit, transform.position) > 1.5f || lastPositionHit.z == 1)*/)
        {
            Vector3 normalVector = collision.GetContact(0).normal;
            travelVector = Vector3.Reflect(travelVector, normalVector);
            lastPositionHit = new Vector3(transform.position.x, transform.position.y, 0);
            numberBounces++;
        }
    }
}
