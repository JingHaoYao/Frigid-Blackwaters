using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FiendFlower : Enemy
{
    [SerializeField] Collider2D takeDamageHitBox;
    [SerializeField] WhichRoomManager roomManager;
    [SerializeField] Animator animator;
    [SerializeField] AudioSource takeDamageAudio;
    [SerializeField] SpriteRenderer spriteRenderer;
    BossHealthBar bossHealthBar;
    public GameObject petalChest;
    public GameObject bluePetalTurret;
    public GameObject redPetalTurret;
    public GameObject vineWhipAttack;
    Camera mainCamera;
    bool dormant = true;
    private float vineWhipPeriod = 2;
    private float turretPeriod = 3;


    IEnumerator awakenRoutine()
    {
        bossHealthBar.bossStartUp("Fiend Flower");
        bossHealthBar.targetEnemy = this;
        animator.SetTrigger("WakeUp");
        EnemyPool.addEnemy(this);
        roomManager.antiSpawnSpaceDetailer.spawnDoorSeals();
        PlayerProperties.playerScript.enemiesDefeated = false;
        yield return new WaitForSeconds(11 / 12f);
        dormant = false;
        StartCoroutine(attackLoop());
    }

    IEnumerator attackLoop()
    {
        while (true)
        {
            if (stopAttacking == false)
            {
                if (vineWhipPeriod > 0)
                {
                    vineWhipPeriod -= Time.deltaTime;
                }
                else
                {
                    summonVine();
                    vineWhipPeriod = 4;
                }

                if (turretPeriod > 0)
                {
                    turretPeriod -= Time.deltaTime;
                }
                else
                {
                    for (int i = 0; i < 3; i++)
                    {
                        summonTurret();
                    }
                    turretPeriod = 6;
                }
            }

            yield return null;
        }
    }

    Vector3 pickRandPosition()
    {
        Vector3 posToReturn =
            new Vector3(
                Random.Range(mainCamera.transform.position.x - 8, mainCamera.transform.position.y + 8),
                Random.Range(mainCamera.transform.position.y - 8, mainCamera.transform.position.y + 8));
        while(Vector2.Distance(posToReturn, transform.position) < 3 || Vector2.Distance(posToReturn, PlayerProperties.playerShipPosition) < 3)
        {
            posToReturn =
            new Vector3(
                Random.Range(mainCamera.transform.position.x - 8, mainCamera.transform.position.y + 8),
                Random.Range(mainCamera.transform.position.y - 8, mainCamera.transform.position.y + 8));
        }
        return posToReturn;
    }

    bool checkIfPositionIsValid(Vector3 pos)
    {
        return Mathf.Abs(pos.x - mainCamera.transform.position.x) < 8.5f && Mathf.Abs(pos.y - mainCamera.transform.position.y) < 8.5f;
    }

    void summonTurret()
    {
        GameObject turretInstant = Instantiate(Random.Range(0, 2) == 1 ? redPetalTurret : bluePetalTurret, pickRandPosition(), Quaternion.identity);
        turretInstant.GetComponent<FlowerTurretStem>().fiendFlowerBoss = this.gameObject;
    }

    void summonVine()
    {
        float randAngle = Random.Range(0, Mathf.PI * 2);
        Vector3 proposedPosition = PlayerProperties.playerShipPosition + new Vector3(Mathf.Cos(randAngle), Mathf.Sin(randAngle)) * 1.5f;
        while (!checkIfPositionIsValid(proposedPosition))
        {
            randAngle = Random.Range(0, Mathf.PI * 2);
            proposedPosition = PlayerProperties.playerShipPosition + new Vector3(Mathf.Cos(randAngle), Mathf.Sin(randAngle)) * 1.5f;
        }
        GameObject vineInstant = Instantiate(vineWhipAttack, proposedPosition, Quaternion.identity);
        vineInstant.GetComponent<ProjectileParent>().instantiater = this.gameObject;
    }

    private void Start()
    {
        bossHealthBar = FindObjectOfType<BossHealthBar>();
        mainCamera = Camera.main;
    }

    public override void deathProcedure()
    {
        StopAllCoroutines();
        this.gameObject.GetComponent<BoxCollider2D>().enabled = false;
        roomManager.antiSpawnSpaceDetailer.trialDefeated = true;
        PlayerProperties.playerScript.enemiesDefeated = true;
        animator.SetTrigger("Death");
        bossHealthBar.bossEnd();
        takeDamageHitBox.enabled = false;

        Instantiate(petalChest, Camera.main.transform.position + new Vector3(0, -2, 0), Quaternion.identity);
    }

    public override void damageProcedure(int damage)
    {
        StartCoroutine(hitFrame());
        takeDamageAudio.Play();
    }

    IEnumerator hitFrame()
    {
        spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(.1f);
        spriteRenderer.color = Color.white;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<DamageAmount>())
        {
            if (dormant == true && Vector2.Distance(mainCamera.transform.position, transform.position) < 4)
            {
                StartCoroutine(awakenRoutine());
                return;
            }

            if (health > 0)
            {
                dealDamage(collision.gameObject.GetComponent<DamageAmount>().damage);
            }
        }
    }
}
