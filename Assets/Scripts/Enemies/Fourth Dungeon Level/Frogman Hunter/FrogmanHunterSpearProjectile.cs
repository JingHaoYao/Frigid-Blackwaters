using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrogmanHunterSpearProjectile : MonoBehaviour
{
    [SerializeField] ProjectileParent projectileParent;
    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] GameObject explosion;
    [SerializeField] float speed;
    private bool collided = false;
    private float angleTravel;

    public void Initialize(float angleTravelInDeg, GameObject parent, bool fadeIn)
    {
        projectileParent.instantiater = parent;
        angleTravel = angleTravelInDeg * Mathf.Deg2Rad;
        transform.rotation = Quaternion.Euler(0, 0, angleTravelInDeg);
        if (fadeIn)
        {
            spriteRenderer.color = new Color(1, 1, 1, 0);
            LeanTween.alpha(this.gameObject, 1, 0.5f);
        }
    }

    private void Update()
    {
        if(collided == false)
        {
            transform.position += new Vector3(Mathf.Cos(angleTravel), Mathf.Sin(angleTravel)) * speed * Time.deltaTime;
        }
    }

    void explode()
    {
        collided = true;
        GameObject explosionInstant = Instantiate(explosion, transform.position, Quaternion.identity);
        explosionInstant.GetComponent<ProjectileParent>().instantiater = projectileParent.instantiater;
        Destroy(this.gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        explode();
    }
}
