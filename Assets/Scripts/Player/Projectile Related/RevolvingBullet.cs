using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RevolvingBullet : PlayerProjectile
{
    [SerializeField] GameObject bulletSparks;
    private float angleTravel;
    [SerializeField] float speed;
    [SerializeField] Animator animator;
    [SerializeField] DamageAmount damageAmount;
    [SerializeField] ParticleSystem particlesTrail;
    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] Collider2D damageCollider;

    public void Initialize(float angleTravel, int damage, bool rapidBullet = false)
    {
        this.angleTravel = angleTravel;
        StartCoroutine(travelRoutine());
        damageAmount.originDamage = damage;
        damageAmount.updateDamage();
        if(rapidBullet)
        {
            animator.Play("Rapid Bullet Animation");
        }
        transform.rotation = Quaternion.Euler(0, 0, angleTravel * Mathf.Rad2Deg);
    }

    IEnumerator travelRoutine()
    {
        while(true)
        {
            transform.position += new Vector3(Mathf.Cos(angleTravel), Mathf.Sin(angleTravel)) * Time.deltaTime * speed;
            yield return null;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "RoomHitbox" || collision.gameObject.tag == "RoomWall" || collision.gameObject.tag == "EnemyShield")
        {
            Instantiate(bulletSparks, transform.position, Quaternion.Euler(0, 0, (angleTravel * Mathf.Rad2Deg) + 90));
            Destroy(this.gameObject, 2f);
            spriteRenderer.enabled = false;
            damageCollider.enabled = false;
            particlesTrail.Stop();
        }
    }
}
