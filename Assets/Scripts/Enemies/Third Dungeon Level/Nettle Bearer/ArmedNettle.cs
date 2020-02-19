using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmedNettle : MonoBehaviour
{
    public float angleTravelInDeg;
    [SerializeField] private float travelSpeed;
    [SerializeField] private Animator animator;
    [SerializeField] private ProjectileParent projectileParent;
    [SerializeField] private AudioSource breakAudio;
    private bool impacted = false;
    public GameObject singleNettle;

    void summonNewNettles()
    {
        for(int i = 0; i < 3; i++)
        {
            float angleToInstantiate = angleTravelInDeg + 180 - 5 * i;
            GameObject singleNettleInstant = Instantiate(singleNettle, transform.position + new Vector3(Mathf.Cos(angleToInstantiate * Mathf.Deg2Rad), Mathf.Sin(angleToInstantiate * Mathf.Deg2Rad)) * 0.75f, Quaternion.Euler(0, 0, angleToInstantiate));
            singleNettleInstant.GetComponent<BasicProjectile>().angleTravel = angleToInstantiate;
            singleNettleInstant.GetComponent<ProjectileParent>().instantiater = this.projectileParent.instantiater;
        }
    }

    void Update()
    {
        if (impacted == false)
        {
            transform.rotation = Quaternion.Euler(0, 0, angleTravelInDeg);
            transform.position += new Vector3(Mathf.Cos(angleTravelInDeg * Mathf.Deg2Rad), Mathf.Sin(angleTravelInDeg * Mathf.Deg2Rad)) * Time.deltaTime * travelSpeed;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (impacted == false && collision.gameObject.layer != 15)
        {
            impacted = true;
            animator.SetTrigger("Shatter");
            Destroy(this.gameObject, 0.417f);
            breakAudio.Play();
            summonNewNettles();
            this.GetComponent<Collider2D>().enabled = false;
        }
    }
}
