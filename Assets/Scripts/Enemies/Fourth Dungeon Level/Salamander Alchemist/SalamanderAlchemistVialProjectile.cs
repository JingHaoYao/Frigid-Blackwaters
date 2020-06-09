using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SalamanderAlchemistVialProjectile : MonoBehaviour
{
    [SerializeField] ProjectileParent projectileParent;
    [SerializeField] GameObject damagingFog;
    public float travelAngle;
    public float speed = 20;
    bool hitShip;
    [SerializeField] AudioSource glassShatterSound;
    [SerializeField] Collider2D collider;
    [SerializeField] Animator animator;

    void Update()
    {
        if (hitShip == false)
        {
            transform.position += new Vector3(Mathf.Cos(travelAngle * Mathf.Deg2Rad), Mathf.Sin(travelAngle * Mathf.Deg2Rad), 0) * Time.deltaTime * speed;
            if (collider.enabled == true)
            {
                LeanTween.rotateZ(this.gameObject, transform.rotation.eulerAngles.z + 270, 0.2f);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        collider.enabled = false;
        
        if (hitShip == false)
        {
            speed = 0;
            glassShatterSound.Play();
            animator.SetTrigger("Explode");
            GameObject fogInstant = Instantiate(damagingFog, transform.position, Quaternion.identity);
            fogInstant.GetComponent<ProjectileParent>().instantiater = projectileParent.instantiater;
            Destroy(this.gameObject, 7f / 12f);
        }
    }
}
