using System.Collections;
using UnityEngine;

public class RockBubblerHomingProjectile : MonoBehaviour
{
    public float speed;

    [SerializeField] Animator animator;
    bool impacted = false;
    GameObject playerShip;

    [SerializeField] AudioSource impactAudio;
    [SerializeField] ProjectileParent projectileParent;
    [SerializeField] Collider2D col;

    [SerializeField] GameObject pyrotheumProjectile;

    public void Initialize(GameObject instantiater)
    {
        this.projectileParent.instantiater = instantiater;

        StartCoroutine(projectileProcedure());
    }

    IEnumerator projectileProcedure()
    {
        while (speed > 0)
        {
            speed -= Time.deltaTime * 2;
            transform.position += (PlayerProperties.playerShipPosition - transform.position).normalized * Time.deltaTime * speed;
            yield return null;
        }

        impactProcedure();
    }

    void impactProcedure()
    {
        if (impacted == false)
        {
            impacted = true;
            animator.SetTrigger("Burst");

            for(int i = 0; i < 4; i++)
            {
                float angleAttack = i * 90;
                GameObject projectileInstant = Instantiate(pyrotheumProjectile, transform.position, Quaternion.identity);
                projectileInstant.GetComponent<PyrotheumProjectile>().angleTravel = angleAttack;
                projectileInstant.GetComponent<ProjectileParent>().instantiater = projectileParent.instantiater;

            }

            impactAudio.Play();
            Destroy(this.gameObject, 5 / 12f);
            col.enabled = false;
        }
    }

    void Start()
    {
        animator = GetComponent<Animator>();
        playerShip = PlayerProperties.playerShip;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (impacted == false && (collision.gameObject == PlayerProperties.playerShip || collision.gameObject.layer == 12))
        {
            StopAllCoroutines();
            impactProcedure();
        }
    }
}
