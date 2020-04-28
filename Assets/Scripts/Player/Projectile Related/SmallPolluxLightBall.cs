using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmallPolluxLightBall : PlayerProjectile
{
    private float angleTravel;
    [SerializeField] float speed;
    [SerializeField] Animator animator;
    [SerializeField] AudioSource explodeAudio;
    [SerializeField] Collider2D damagingCollider;
    bool impacted = false;

    public void InitializeProjectile(float angleTravel)
    {
        this.angleTravel = angleTravel;
        rotate();
    }

    void rotate()
    {
        LeanTween.rotateZ(this.gameObject, transform.rotation.eulerAngles.z + 270, 0.5f).setOnComplete(rotate);
    }

    void Start()
    {
        
    }

    void Update()
    {
        if (impacted == false)
        {
            transform.rotation = Quaternion.Euler(0, 0, angleTravel);
            transform.position += new Vector3(Mathf.Cos(angleTravel * Mathf.Deg2Rad), Mathf.Sin(angleTravel * Mathf.Deg2Rad)) * Time.deltaTime * speed;
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (impacted == false && collision.gameObject.layer != 15)
        {
            impacted = true;
            animator.SetTrigger("Explode");
            explodeAudio.Play();
            damagingCollider.enabled = false;
            Destroy(this.gameObject, 5 / 12f);
        }
    }

}
