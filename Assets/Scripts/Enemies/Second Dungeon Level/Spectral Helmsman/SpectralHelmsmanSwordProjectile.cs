using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpectralHelmsmanSwordProjectile : MonoBehaviour
{
    [SerializeField] PolygonCollider2D polyCol;
    Rigidbody2D rigidBody2D;
    [SerializeField] Animator animator;
    float angleToShip = 0;

    private void Start()
    {
        rigidBody2D = GetComponent<Rigidbody2D>();
        polyCol.enabled = false;
        angleToShip = (360 + Mathf.Atan2(PlayerProperties.playerShipPosition.y - transform.position.y, PlayerProperties.playerShipPosition.x - transform.position.x) * Mathf.Rad2Deg) % 360;
        transform.rotation = Quaternion.Euler(0, 0, angleToShip);
        StartCoroutine(swordDash(angleToShip));
    }

    void moveTowards(float directionInDeg, float speed)
    {
        transform.position += new Vector3(Mathf.Cos(directionInDeg * Mathf.Deg2Rad), Mathf.Sin(directionInDeg * Mathf.Deg2Rad)) * speed * Time.deltaTime;
    }

    IEnumerator swordDash(float angleAttack)
    {
        yield return new WaitForSeconds(4 / 12f);

        polyCol.enabled = true;

        rigidBody2D = GetComponent<Rigidbody2D>();

        LeanTween.value(0, 12, 0.8f).setOnUpdate((float val) => { moveTowards(angleAttack, val); });

        float period = 0;

        while(period < 1.6f)
        {
            moveTowards(angleAttack, 12);
            period += Time.deltaTime;
            yield return null;
        }

        polyCol.enabled = false;
        LeanTween.value(12, 0, 0.5f).setOnUpdate((float val) => { moveTowards(angleAttack, val); });
        animator.SetTrigger("Shatter");
        Destroy(this.gameObject, 4 / 12f);
    }
}
