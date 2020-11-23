using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallSpawnedPyrotheumProjectile : MonoBehaviour
{
    public float speed;

    [SerializeField] Animator animator;
    bool impacted = false;
    GameObject playerShip;

    [SerializeField] float rotationOffset;
    [SerializeField] AudioSource impactAudio;
    [SerializeField] ProjectileParent projectileParent;
    [SerializeField] Collider2D col;

    private Vector3 targetPosition;
    
    public void Initialize(GameObject instantiater, Vector3 targetPosition)
    {
        projectileParent.instantiater = instantiater;
        this.targetPosition = targetPosition;
        float angleTravel = Mathf.Atan2(targetPosition.y - transform.position.y, targetPosition.x - transform.position.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angleTravel + rotationOffset);
        StartCoroutine(projectileProcedure());
    }

    IEnumerator projectileProcedure()
    {
        col.enabled = false;
        yield return new WaitForSeconds(13 / 12f);
        col.enabled = true;
        LeanTween.move(this.gameObject, targetPosition, Vector2.Distance(transform.position, targetPosition) / speed).setEaseInOutQuad().setOnComplete(impactProcedure);
    }

    void impactProcedure()
    {
        if(impacted == false)
        {
            impacted = true;
            animator.SetTrigger("Impact");

            impactAudio.Play();
            Destroy(this.gameObject, 7 / 12f);
            col.enabled = false;
        }
    }

    void Start()
    {
        animator = GetComponent<Animator>();
        playerShip = PlayerProperties.playerShip;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (impacted == false && collision.gameObject == PlayerProperties.playerShip)
        {
            StopAllCoroutines();
            impactProcedure();
        }
    }
}
