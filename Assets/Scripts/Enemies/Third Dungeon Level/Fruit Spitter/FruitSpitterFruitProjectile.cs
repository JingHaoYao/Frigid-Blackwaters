using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FruitSpitterFruitProjectile : MonoBehaviour
{
    public float speed;

    Animator animator;
    bool impacted = false;
    GameObject playerShip;
    // In degrees
    public float angleTravel;
    [SerializeField] private AudioSource splatAudio;

    void Start()
    {
        animator = GetComponent<Animator>();
        playerShip = PlayerProperties.playerShip;
        transform.rotation = Quaternion.Euler(0, 0, Random.Range(0, 360));
    }

    void Update()
    {
        if (impacted == false)
        {
            transform.Rotate(new Vector3(0, 0, 1) * Time.deltaTime * 120);
            transform.position += new Vector3(Mathf.Cos(angleTravel * Mathf.Deg2Rad), Mathf.Sin(angleTravel * Mathf.Deg2Rad)) * Time.deltaTime * speed;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (impacted == false && collision.gameObject.layer != 15)
        {
            impacted = true;
            animator.SetTrigger("Explode");
            splatAudio.Play();
            Destroy(this.gameObject, 0.333f);
            this.GetComponent<Collider2D>().enabled = false;
        }
    }
}
