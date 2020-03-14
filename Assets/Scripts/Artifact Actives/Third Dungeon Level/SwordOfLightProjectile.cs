using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordOfLightProjectile : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private BoxCollider2D boxCol;
    bool impacted = false;
    public float angleTravel;
    [SerializeField] float speed;
    [SerializeField] AudioSource audioSource;

    private void Start()
    {
        angleTravel = Mathf.Atan2(PlayerProperties.cursorPosition.y - transform.position.y, PlayerProperties.cursorPosition.x - transform.position.x);
        transform.rotation = Quaternion.Euler(0, 0, angleTravel * Mathf.Rad2Deg);
        StartCoroutine(launchSword());
    }

    IEnumerator launchSword()
    {
        yield return new WaitForSeconds(5 / 12f);
        while (impacted == false)
        {
            transform.position += new Vector3(Mathf.Cos(angleTravel), Mathf.Sin(angleTravel)) * Time.deltaTime * speed;
            yield return null;
        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (impacted == false && collision.gameObject.layer != 15)
        {
            audioSource.Play();
            impacted = true;
            animator.SetTrigger("FadeAway");
            Destroy(this.gameObject, 5/12f);
            boxCol.enabled = false;
        }
    }
}
