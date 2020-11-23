using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplashOfRainDrop : MonoBehaviour
{
    [SerializeField] Animator animator;
    [SerializeField] AudioSource audioSource;
    [SerializeField] Collider2D col;

    float angleToCursor()
    {
        return Mathf.Atan2(PlayerProperties.cursorPosition.y - PlayerProperties.playerShipPosition.y, PlayerProperties.cursorPosition.x - PlayerProperties.playerShipPosition.x);
    }

    private void Start()
    {
        StartCoroutine(mainLoop());
    }

    IEnumerator mainLoop()
    {
        float waitPeriod = 0;

        while(waitPeriod < 0.5f)
        {
            waitPeriod += Time.deltaTime;
            transform.rotation = Quaternion.Euler(0, 0, angleToCursor() * Mathf.Rad2Deg);
            
            yield return null;
        }

        float angleAttack = angleToCursor();
        transform.rotation = Quaternion.Euler(0, 0, angleToCursor() * Mathf.Rad2Deg);

        float speed = 12;

        while(true)
        {
            transform.position += new Vector3(Mathf.Cos(angleAttack), Mathf.Sin(angleAttack)) * Time.deltaTime * speed;
            speed += Time.deltaTime * 2;
            yield return null;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        col.enabled = false;
        StopAllCoroutines();
        animator.SetTrigger("Impact");
        audioSource.Play();
        Destroy(this.gameObject, 7 / 12f);
    }
}
