using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletalFlameJugglerFireBall : MonoBehaviour
{
    private bool impacted = false;
    [SerializeField] Animator animator;
    [SerializeField] Collider2D collider2D;
    [SerializeField] ProjectileParent projectileParent;
    [SerializeField] float speed = 6;
    [SerializeField] AudioSource impactAudio;
    [SerializeField] float timerLimit = 2;

    private Vector3 center;
    private float timer = 0;

    public void Initialize(Vector3 center)
    {
        this.center = center;
    }

    void Impact()
    {
        if (impacted == false)
        {
            impacted = true;
            animator.SetTrigger("Impact");
            Destroy(this.gameObject, 0.5f);
            collider2D.enabled = false;
            impactAudio.Play();
        }
    }


    private void Update()
    {
        if (impacted == false)
        {
            float angleFromCenter = Mathf.Atan2(transform.position.y - center.y, transform.position.x - center.x);
            Vector3 directionVector = (new Vector3(Mathf.Cos(angleFromCenter), Mathf.Sin(angleFromCenter)) + new Vector3(Mathf.Cos(angleFromCenter + Mathf.PI / 2), Mathf.Sin(angleFromCenter + Mathf.PI / 2)) * 2).normalized;
            transform.rotation = Quaternion.Euler(0, 0, Mathf.Atan2(directionVector.y, directionVector.x) * Mathf.Rad2Deg + 180);
            transform.position += directionVector * Time.deltaTime * speed;
            timer += Time.deltaTime;

            if(timer > timerLimit)
            {
                Impact();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer != 15)
        {
            Impact();
        }
    }
}
