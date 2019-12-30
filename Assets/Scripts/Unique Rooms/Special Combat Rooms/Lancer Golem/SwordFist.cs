using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordFist : MonoBehaviour
{
    Animator animator;
    public GameObject damageHitBox;
    PlayerScript playerScript;
    Rigidbody2D rigidBody2D;
    public LayerMask impactLayerMask;
    public GameObject waterFoam;
    float foamTimer = 0;

    void spawnFoam()
    {
        if (rigidBody2D.velocity.magnitude != 0)
        {
            float whatAngle = Mathf.Atan2(rigidBody2D.velocity.y, rigidBody2D.velocity.x) * Mathf.Rad2Deg;
            foamTimer += Time.deltaTime;
            if (foamTimer >= 0.01f)
            {
                foamTimer = 0;
                GameObject foam = Instantiate(waterFoam, transform.position, Quaternion.Euler(0, 0, whatAngle + 90));
            }
        }
    }

    IEnumerator dash()
    {
        GetComponent<AudioSource>().Play();
        animator.SetTrigger("Charge");
        yield return new WaitForSeconds(8f / 12f);
        damageHitBox.SetActive(true);
        Vector2 unit = (playerScript.transform.position - transform.position).normalized;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, unit, 20, impactLayerMask);

        Vector2 pointOfContact = hit.point;

        for(int i = 0; i < 5; i++)
        {
            rigidBody2D.velocity = unit * i * 3f;
            yield return new WaitForSeconds(0.05f);
        }

        while(Vector3.Distance(pointOfContact, transform.position) > 1.5f)
        {
            yield return null;
        }

        animator.SetTrigger("Charge Down");

        for (int i = 0; i < 5; i++)
        {
            rigidBody2D.velocity = unit * (1 / (3f * (i+1)));
            yield return new WaitForSeconds(0.05f);
        }

        damageHitBox.SetActive(false);
        rigidBody2D.velocity = Vector3.zero;
    }

    public void dashAttack()
    {
        StartCoroutine(dash());
    }

    private void Update()
    {
        spawnFoam();
    }

    void Start()
    {
        animator = GetComponent<Animator>();
        rigidBody2D = GetComponent<Rigidbody2D>();
        playerScript = FindObjectOfType<PlayerScript>();
    }
}
