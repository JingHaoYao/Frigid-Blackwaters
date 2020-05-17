using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrogmanOrbCasterProjectile : MonoBehaviour
{
    public float speed;

    bool impacted = false;
    GameObject playerShip;
    // In degrees
    public float angleTravel;
    [SerializeField] Animator animator;
    [SerializeField] AudioSource explodeAudio;

    void Start()
    {
        playerShip = PlayerProperties.playerShip;
    }

    void Update()
    {
        if (impacted == false)
        {
            transform.position += new Vector3(Mathf.Cos(angleTravel * Mathf.Deg2Rad), Mathf.Sin(angleTravel * Mathf.Deg2Rad)) * Time.deltaTime * speed;
        }
        LeanTween.rotateZ(transform.gameObject, transform.rotation.eulerAngles.z + 270, 0.1f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (impacted == false && collision.gameObject.layer != 15)
        {
            impacted = true;
            LeanTween.alpha(this.gameObject, 0, 0.5f).setOnComplete(() => Destroy(this.gameObject));
            this.GetComponent<Collider2D>().enabled = false;
            animator.SetTrigger("Explode");
            explodeAudio.Play();
        }
    }
}
