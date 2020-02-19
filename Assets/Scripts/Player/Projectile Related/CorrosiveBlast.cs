using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CorrosiveBlast : PlayerProjectile
{
    [SerializeField] private CircleCollider2D damageCollider;
    public GameObject corrosiveEffect;
    public bool explosiveGas = false;
    public GameObject corrosiveExplosion;

    private void Start()
    {
        transform.rotation = Quaternion.Euler(0, 0, pickDirectionTravel());
        triggerWeaponFireFlag();
        destroy();
    }

    private float pickDirectionTravel()
    {
        Vector3 cursorPosition = PlayerProperties.cursorPosition;
        return (360 + Mathf.Atan2(cursorPosition.y - transform.position.y, cursorPosition.x - transform.position.x) * Mathf.Rad2Deg) % 360;
    }

    public void destroy()
    {
        Destroy(this.gameObject, 1.083f);
    }

    public void turnOffCollider()
    {
        damageCollider.enabled = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.layer == 16 /* is player projectile*/)
        {
            PlayerProjectile projectileObject = collision.gameObject.GetComponent<PlayerProjectile>();
            if (projectileObject != null)
            {
                if (explosiveGas == true && projectileObject.weaponElements.Contains(WeaponProperties.WeaponElement.FireElement))
                {
                    Instantiate(corrosiveExplosion, transform.position 
                        + new Vector3(
                            Mathf.Cos(transform.rotation.eulerAngles.z * Mathf.Deg2Rad), 
                            Mathf.Sin(transform.rotation.eulerAngles.z * Mathf.Deg2Rad)) * 
                            (Mathf.Sqrt(Mathf.Pow(damageCollider.offset.x, 2) + Mathf.Pow(damageCollider.offset.y, 2)) * 
                            transform.localScale.x), Quaternion.identity);
                    Destroy(this.gameObject);
                }
            }
        }
        else if (collision.gameObject.GetComponent<Enemy>())
        {
            GameObject chemicalDecay = Instantiate(corrosiveEffect, collision.gameObject.transform.position, Quaternion.identity);
            collision.gameObject.GetComponent<Enemy>().addStatus(chemicalDecay.GetComponent<EnemyStatusEffect>());
        }
    }
}
