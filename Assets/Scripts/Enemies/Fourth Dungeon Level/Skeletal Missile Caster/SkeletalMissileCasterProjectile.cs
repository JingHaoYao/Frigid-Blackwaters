using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletalMissileCasterProjectile : MonoBehaviour
{
    public float speed;

    [SerializeField] Animator animator;
    [SerializeField] AudioSource audioSource;
    [SerializeField] Collider2D collider2D;
    bool impacted = false;
    GameObject playerShip;
    private float angleTravel;

    void Start()
    {
        playerShip = PlayerProperties.playerShip;
        StartCoroutine(mainRoutine());
    }

    void Update()
    {
        if(impacted == false)
        {
            LeanTween.rotateZ(this.gameObject, transform.rotation.eulerAngles.z + 270, 0.2f);
        }
    }

    IEnumerator mainRoutine()
    {
        yield return new WaitForSeconds(8 / 12f);
        angleTravel = Mathf.Atan2(playerShip.transform.position.y - transform.position.y, playerShip.transform.position.x - transform.position.x) * Mathf.Rad2Deg;
        while(impacted == false)
        {
            transform.position += new Vector3(Mathf.Cos(angleTravel * Mathf.Deg2Rad), Mathf.Sin(angleTravel * Mathf.Deg2Rad)) * Time.deltaTime * speed;
            yield return null;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (impacted == false && collision.gameObject.layer != 15)
        {
            impacted = true;
            animator.SetTrigger("Explode");
            audioSource.Play();
            Destroy(this.gameObject, 0.5f);
            collider2D.enabled = false;
        }
    }
}
