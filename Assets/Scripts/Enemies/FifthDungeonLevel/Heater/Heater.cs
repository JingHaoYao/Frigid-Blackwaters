using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heater : Enemy
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Rigidbody2D rigidBody2D;
    [SerializeField] private Animator animator;
    private float travelAngle;
    public GameObject deadSpearman;
    private float foamTimer = 0;
    [SerializeField] private AudioSource damageAudio;
    [SerializeField] private AudioSource attackAudio;
    [SerializeField] GameObject fireProjectile;
    bool isAttacking = false;
    Camera mainCamera;

    private void Start()
    {
        mainCamera = Camera.main;
        StartCoroutine(movementLoop());
    }

    private float angleToShip()
    {
        return (360 + Mathf.Atan2(PlayerProperties.playerShipPosition.y - transform.position.y, PlayerProperties.playerShipPosition.x - transform.position.x) * Mathf.Rad2Deg) % 360;
    }

    float MovementOnce()
    {
        Vector3 directionVector = new Vector3(Mathf.Cos(angleToShip() * Mathf.Deg2Rad), Mathf.Sin(angleToShip() * Mathf.Deg2Rad));
        Vector3 potentialPosition = transform.position + directionVector * 6;

        Vector3 targetPos = new Vector3(Mathf.Clamp(potentialPosition.x, mainCamera.transform.position.x - 8, mainCamera.transform.position.x + 8), Mathf.Clamp(potentialPosition.y, mainCamera.transform.position.y - 8, mainCamera.transform.position.y + 8));

        float time = Vector2.Distance(transform.position, targetPos) / speed;

        LeanTween.move(this.gameObject, targetPos, time).setEaseInOutQuad();

        return time;
    }

    IEnumerator shootFlames()
    {
        animator.SetTrigger("FlameBurst");
        attackAudio.Play();
        isAttacking = true;
        yield return new WaitForSeconds(3 / 12f);

        for(int i = 0; i < 6; i++)
        {
            GameObject fireProjectileInstant = Instantiate(fireProjectile, transform.position + Vector3.up, Quaternion.identity);
            fireProjectileInstant.GetComponent<BasicProjectile>().angleTravel = i * 60;
            fireProjectileInstant.GetComponent<ProjectileParent>().instantiater = this.gameObject;
        }

        yield return new WaitForSeconds(7 / 12f);

        animator.SetTrigger("Idle");
        isAttacking = false;
    }

    IEnumerator movementLoop()
    {
        while (true)
        {
            float waitTime = MovementOnce();

            yield return new WaitForSeconds(waitTime);

            while(stopAttacking == true)
            {
                yield return null;
            }
        }
    }

    IEnumerator hitFrame()
    {
        spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(.1f);
        spriteRenderer.color = Color.white;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<DamageAmount>())
        {
            StartCoroutine(shootFlames());
            dealDamage(collision.gameObject.GetComponent<DamageAmount>().damage);
        }
    }

    public override void deathProcedure()
    {
        GameObject deadPirate = Instantiate(deadSpearman, transform.position, Quaternion.identity);
        Destroy(this.gameObject);
    }

    public override void damageProcedure(int damage)
    {
        StartCoroutine(hitFrame());
        damageAudio.Play();
    }
}
