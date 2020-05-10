using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrogmanBola : MonoBehaviour
{
    [SerializeField] Collider2D collider;
    [SerializeField] Animator animator;
    [SerializeField] SpriteRenderer spriteRenderer;
    GameObject playerShip;
    public float travelAngle;
    public float bolaSpeed = 20;
    bool hitShip;
    Vector3 hitPos;
    Vector3 hitShipPos;
    [SerializeField] AudioSource popSound;

    void destroySpear()
    {
        animator.SetTrigger("Pop");
        LeanTween.alpha(this.gameObject, 0, 0.5f);
        Destroy(this.gameObject, 6f / 12f);
    }

    IEnumerator slowShip(float duration)
    {
        PlayerScript playerScript = FindObjectOfType<PlayerScript>();

        playerScript.enemySpeedModifier -= 2;

        yield return new WaitForSeconds(duration);
        playerScript.enemySpeedModifier += 2;
        destroySpear();
    }

    void Start()
    {
        playerShip = PlayerProperties.playerShip;
        transform.rotation = Quaternion.Euler(0, 0, travelAngle);
    }

    void Update()
    {
        if (hitShip == false)
        {
            transform.position += new Vector3(Mathf.Cos(travelAngle * Mathf.Deg2Rad), Mathf.Sin(travelAngle * Mathf.Deg2Rad), 0) * Time.deltaTime * bolaSpeed;
            if (collider.enabled == true)
            {
                LeanTween.rotateZ(this.gameObject, transform.rotation.eulerAngles.z + 270, 0.2f);
            }
        }
        else
        {
            transform.position = hitPos + (playerShip.transform.position - hitShipPos);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        collider.enabled = false;
        LeanTween.cancelAll(this.gameObject);
        if (collision.gameObject.tag == "playerHitBox" && hitShip == false)
        {
            hitShip = true;
            animator.SetTrigger("Pop");
            popSound.Play();
            bolaSpeed = 0;
            collider.enabled = false;
            transform.position = playerShip.transform.position;
            hitShipPos = playerShip.transform.position;
            hitPos = transform.position;
            StartCoroutine(slowShip(6));
        }
        else
        {
            if (hitShip == false)
            {
                bolaSpeed = 0;
                animator.SetTrigger("Pop");
                popSound.Play();
                LeanTween.alpha(this.gameObject, 0, 0.5f);
                Destroy(this.gameObject, 6f / 12f);
            }
        }
    }
}
