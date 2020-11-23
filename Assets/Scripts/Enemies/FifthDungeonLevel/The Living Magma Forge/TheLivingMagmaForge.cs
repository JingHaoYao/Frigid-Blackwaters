using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TheLivingMagmaForge : Enemy
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Collider2D takeDamageHitBox;
    public BossManager bossManager;
    private BossHealthBar healthBar;
    [SerializeField] Animator animator;
    [SerializeField] Rigidbody2D rigidBody2D;
    [SerializeField] AudioSource takeDamageAudio, forgeAudio, deathAudio, swipeAudio;
    [SerializeField] GameObject magmaBall;
    Camera mainCamera;
    bool isAttacking = false;
    private float attackPeriod = 2;

    Vector3 centerPosition;

    [SerializeField] Animator axeAnimator, spearAnimator, swordAnimator;
    [SerializeField] Collider2D axeCollider, spearCollider, swordCollider;
    [SerializeField] GameObject upperArm, lowerArm;
    [SerializeField] Animator clawAnimator;
    [SerializeField] Transform parentRoom;

    private float angleToShip(Transform sourceTransform)
    {
        return (360 + Mathf.Atan2(PlayerProperties.playerShipPosition.y - sourceTransform.position.y, PlayerProperties.playerShipPosition.x - sourceTransform.position.x) * Mathf.Rad2Deg) % 360;
    }

    void Initialize()
    {
        axeCollider.enabled = false;
        spearCollider.enabled = false;
        swordCollider.enabled = false;
        StartCoroutine(attackLoop());
    }

    IEnumerator attackLoop()
    {
        while (true)
        {
            if (isAttacking == false)
            {
                if (attackPeriod > 0)
                {
                    attackPeriod -= Time.deltaTime;
                }
                else
                {
                    int whichAttack = Random.Range(0, 3);

                    switch (whichAttack)
                    {
                        case 0:
                            StartCoroutine(summonAxeAndPerformAxeAttack());
                            break;
                        case 1:
                            StartCoroutine(summonSpearAndPerformSpearAttack());
                            break;
                        case 2:
                            StartCoroutine(summonSwordAndPerformSwordAttack());
                            break;
                    }
                    attackPeriod = 0.5f;
                }
            }
            yield return null;
        }
    }

    IEnumerator summonAxeAndPerformAxeAttack()
    {
        isAttacking = true;

        int whichDirection = -1 + 2 * Random.Range(0, 2);

        LeanTween.rotateLocal(upperArm, new Vector3(0, 0, 90), 0.75f).setEaseOutQuad();
        LeanTween.rotateLocal(lowerArm, new Vector3(0, 0, 180), 0.75f).setEaseOutQuad();

        yield return new WaitForSeconds(0.75f);

        animator.Play("Forge Weapon");
        axeAnimator.gameObject.SetActive(true);
        axeAnimator.Play("Rise");
        axeAnimator.transform.localScale = new Vector3(1, 1 * whichDirection);
        forgeAudio.Play();

        yield return new WaitForSeconds(9 / 12f);

        clawAnimator.Play("Claw Close");

        yield return new WaitForSeconds(3 / 12f);

        if(whichDirection == 1)
        {
            LeanTween.rotateLocal(upperArm, new Vector3(0, 0, 310), 0.75f);
            LeanTween.rotateLocal(lowerArm, new Vector3(0, 0, 0), 0.75f);

            yield return new WaitForSeconds(0.75f);

            StartCoroutine(blinkWhite(axeAnimator.GetComponent<SpriteRenderer>()));

            yield return new WaitForSeconds(0.6f);

            swipeAudio.Play();
            axeCollider.enabled = true;

            LeanTween.rotateLocal(upperArm, new Vector3(0, 0, 220), 0.5f);

            yield return new WaitForSeconds(0.5f);
        }
        else
        {
            LeanTween.rotateLocal(upperArm, new Vector3(0, 0, 230), 0.75f);
            LeanTween.rotateLocal(lowerArm, new Vector3(0, 0, 0), 0.75f);

            yield return new WaitForSeconds(0.75f);

            StartCoroutine(blinkWhite(axeAnimator.GetComponent<SpriteRenderer>()));

            yield return new WaitForSeconds(0.6f);

            swipeAudio.Play();
            axeCollider.enabled = true;

            LeanTween.rotateLocal(upperArm, new Vector3(0, 0, 320), 0.5f);

            yield return new WaitForSeconds(0.5f);
        }

        axeCollider.enabled = false;
        LeanTween.rotateLocal(upperArm, new Vector3(0, 0, 90), 0.75f).setEaseOutQuad();
        LeanTween.rotateLocal(lowerArm, new Vector3(0, 0, 180), 0.75f).setEaseOutQuad();

        yield return new WaitForSeconds(0.75f);

        clawAnimator.Play("Claw Open");
        axeAnimator.Play("Sink");

        summonMagmaBalls();

        yield return new WaitForSeconds(9 / 12f);

        axeAnimator.gameObject.SetActive(false);
        isAttacking = false;
    }

    IEnumerator blinkWhite(SpriteRenderer spriteRendererToFocus)
    {
        for (int i = 0; i < 3; i++)
        {
            spriteRendererToFocus.material.SetFloat("_FlashAmount", 1);
            yield return new WaitForSeconds(0.1f);
            spriteRendererToFocus.material.SetFloat("_FlashAmount", 0);
            yield return new WaitForSeconds(0.1f);
        }
    }

    IEnumerator summonSwordAndPerformSwordAttack()
    {
        isAttacking = true;

        int whichDirection = -1 + 2 * Random.Range(0, 2);

        LeanTween.rotateLocal(upperArm, new Vector3(0, 0, 90), 0.75f).setEaseOutQuad();
        LeanTween.rotateLocal(lowerArm, new Vector3(0, 0, 180), 0.75f).setEaseOutQuad();

        yield return new WaitForSeconds(0.75f);

        animator.Play("Forge Weapon");
        swordAnimator.gameObject.SetActive(true);
        swordAnimator.Play("Rise");
        forgeAudio.Play();

        yield return new WaitForSeconds(9 / 12f);

        clawAnimator.Play("Claw Close");

        yield return new WaitForSeconds(3 / 12f);

        if (whichDirection == 1)
        {
            LeanTween.rotateLocal(upperArm, new Vector3(0, 0, 310), 0.75f);
            LeanTween.rotateLocal(lowerArm, new Vector3(0, 0, 0), 0.75f);

            yield return new WaitForSeconds(0.75f);

            StartCoroutine(blinkWhite(swordAnimator.GetComponent<SpriteRenderer>()));

            yield return new WaitForSeconds(0.6f);

            swipeAudio.Play();
            swordCollider.enabled = true;

            LeanTween.rotateLocal(upperArm, new Vector3(0, 0, 270), 0.45f).setEaseOutCirc();

            yield return new WaitForSeconds(0.55f);

            LeanTween.rotateLocal(upperArm, new Vector3(0, 0, 320), 0.25f).setEaseInCirc();

            yield return new WaitForSeconds(0.35f);
        }
        else
        {
            LeanTween.rotateLocal(upperArm, new Vector3(0, 0, 230), 0.75f);
            LeanTween.rotateLocal(lowerArm, new Vector3(0, 0, 0), 0.75f);

            yield return new WaitForSeconds(0.75f);

            StartCoroutine(blinkWhite(swordAnimator.GetComponent<SpriteRenderer>()));

            yield return new WaitForSeconds(0.6f);

            swipeAudio.Play();
            swordCollider.enabled = true;

            LeanTween.rotateLocal(upperArm, new Vector3(0, 0, 270), 0.45f).setEaseOutCirc();

            yield return new WaitForSeconds(0.55f);

            LeanTween.rotateLocal(upperArm, new Vector3(0, 0, 220), 0.25f).setEaseInCirc();

            yield return new WaitForSeconds(0.35f);
        }

        swordCollider.enabled = false;

        LeanTween.rotateLocal(upperArm, new Vector3(0, 0, 90), 0.75f).setEaseOutQuad();
        LeanTween.rotateLocal(lowerArm, new Vector3(0, 0, 180), 0.75f).setEaseOutQuad();

        yield return new WaitForSeconds(0.75f);

        clawAnimator.Play("Claw Open");
        swordAnimator.Play("Sink");

        summonMagmaBalls();

        yield return new WaitForSeconds(9 / 12f);

        swordAnimator.gameObject.SetActive(false);
        isAttacking = false;
    }

    IEnumerator summonSpearAndPerformSpearAttack()
    {
        isAttacking = true;

        int whichDirection = -1 + 2 * Random.Range(0, 2);

        LeanTween.rotateLocal(upperArm, new Vector3(0, 0, 90), 0.75f).setEaseOutQuad();
        LeanTween.rotateLocal(lowerArm, new Vector3(0, 0, 180), 0.75f).setEaseOutQuad();

        yield return new WaitForSeconds(0.75f);

        animator.Play("Forge Weapon");
        spearAnimator.gameObject.SetActive(true);
        spearAnimator.Play("Rise");
        forgeAudio.Play();

        yield return new WaitForSeconds(9 / 12f);

        clawAnimator.Play("Claw Close");

        yield return new WaitForSeconds(3 / 12f);

        LeanTween.rotateLocal(upperArm, new Vector3(0, 0, 270), 0.5f);
        LeanTween.rotateLocal(lowerArm, new Vector3(0, 0, 0), 0.5f);

        yield return new WaitForSeconds(0.5f);

        for(int i = 0; i < 3; i++)
        {
            LeanTween.rotate(lowerArm, new Vector3(0, 0, angleToShip(lowerArm.transform)), 0.25f);

            yield return new WaitForSeconds(0.25f);

            Vector3 directionToMove = new Vector3(Mathf.Cos((angleToShip(lowerArm.transform) + 180) * Mathf.Deg2Rad), Mathf.Sin((angleToShip(lowerArm.transform) + 180) * Mathf.Deg2Rad));
            LeanTween.move(lowerArm, (directionToMove * 4) + lowerArm.transform.position, 0.5f);

            yield return new WaitForSeconds(0.5f);

            StartCoroutine(blinkWhite(spearAnimator.GetComponent<SpriteRenderer>()));

            yield return new WaitForSeconds(0.6f);

            spearCollider.enabled = true;
            swipeAudio.Play();
            LeanTween.move(lowerArm, (directionToMove * -4) + lowerArm.transform.position, 0.1f);

            yield return new WaitForSeconds(0.25f);
            spearCollider.enabled = false;
        }

        spearCollider.enabled = false;

        LeanTween.rotateLocal(upperArm, new Vector3(0, 0, 90), 0.75f).setEaseOutQuad();
        LeanTween.rotateLocal(lowerArm, new Vector3(0, 0, 180), 0.75f).setEaseOutQuad();

        yield return new WaitForSeconds(0.75f);

        clawAnimator.Play("Claw Open");
        spearAnimator.Play("Sink");
        summonMagmaBalls();

        yield return new WaitForSeconds(9 / 12f);

        spearAnimator.gameObject.SetActive(false);
        isAttacking = false;
    }

    private void Start()
    {
        mainCamera = Camera.main;
        Initialize();
        healthBar = FindObjectOfType<BossHealthBar>();
        healthBar.bossStartUp("The Living Magma Forge");
        healthBar.targetEnemy = this;
        EnemyPool.addEnemy(this);
        centerPosition = mainCamera.transform.position;
    }

    void summonMagmaBalls()
    {
        StartCoroutine(magmaBallAttack());
    }

    IEnumerator magmaBallAttack()
    {
        yield return new WaitForSeconds(0.25f);
        for (int i = 0; i < 7; i++)
        {
            int rotationIndex = Mathf.RoundToInt(parentRoom.rotation.eulerAngles.z / 90);
            GameObject magmaBallInstant = Instantiate(magmaBall, transform.position + new Vector3(Mathf.Cos(parentRoom.rotation.eulerAngles.z * Mathf.Deg2Rad + Mathf.PI/2), Mathf.Sin(parentRoom.rotation.eulerAngles.z * Mathf.Deg2Rad + Mathf.PI/2)) * 2.5f, Quaternion.Euler(0, 0, -90));
            Vector3 targetPosition = Vector3.zero;

            switch (rotationIndex)
            {
                case 0:
                    targetPosition = new Vector3(mainCamera.transform.position.x + Random.Range(-7.5f, 7.5f), mainCamera.transform.position.y - Random.Range(3.5f, 8.5f));
                    while (Physics2D.OverlapCircle(targetPosition, 0.25f, 12))
                    {
                        targetPosition = new Vector3(mainCamera.transform.position.x + Random.Range(-7.5f, 7.5f), mainCamera.transform.position.y - Random.Range(3.5f, 8.5f));
                    }

                    break;
                case 1:
                    targetPosition = new Vector3(mainCamera.transform.position.x + Random.Range(3.5f, 8.5f), mainCamera.transform.position.y + Random.Range(-7.5f, 7.5f));
                    while (Physics2D.OverlapCircle(targetPosition, 0.25f, 12))
                    {
                        targetPosition = new Vector3(mainCamera.transform.position.x + Random.Range(3.5f, 8.5f), mainCamera.transform.position.y + Random.Range(-7.5f, 7.5f));
                    }
                    break;
                case 2:
                    targetPosition = new Vector3(mainCamera.transform.position.x + Random.Range(-7.5f, 7.5f), mainCamera.transform.position.y + Random.Range(3.5f, 8.5f));
                    while (Physics2D.OverlapCircle(targetPosition, 0.25f, 12))
                    {
                        targetPosition = new Vector3(mainCamera.transform.position.x + Random.Range(-7.5f, 7.5f), mainCamera.transform.position.y + Random.Range(3.5f, 8.5f));
                    }
                    break;
                case 3:
                    targetPosition = new Vector3(mainCamera.transform.position.x - Random.Range(3.5f, 8.5f), mainCamera.transform.position.y + Random.Range(-7.5f, 7.5f));
                    while (Physics2D.OverlapCircle(targetPosition, 0.25f, 12))
                    {
                        targetPosition = new Vector3(mainCamera.transform.position.x - Random.Range(3.5f, 8.5f), mainCamera.transform.position.y + Random.Range(-7.5f, 7.5f));
                    }
                    break;
            }
            magmaBallInstant.GetComponent<TheLivingMagmaForgeLavaBall>().Initialize(this.gameObject, targetPosition);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<DamageAmount>())
        {
            if (health > 0)
            {
                dealDamage(collision.gameObject.GetComponent<DamageAmount>().damage);
            }
        }
    }

    public override void deathProcedure()
    {
        StopAllCoroutines();
        LeanTween.cancel(this.gameObject);
        takeDamageHitBox.enabled = false;
        //bossManager.bossBeaten(nameID, 1.5f);
        PlayerProperties.playerScript.enemiesDefeated = true;
        healthBar.bossEnd();
        deathAudio.Play();
        animator.enabled = true;
        animator.Play("Death");
        axeAnimator.gameObject.SetActive(false);
        spearAnimator.gameObject.SetActive(false);
        swordAnimator.gameObject.SetActive(false);
        LeanTween.rotateLocal(upperArm, new Vector3(0, 0, 227), 0.5f).setEaseOutQuad();
        LeanTween.rotateLocal(lowerArm, new Vector3(0, 0, 42), 0.5f).setEaseOutQuad();
        SaveSystem.SaveGame();
    }

    public override void damageProcedure(int damage)
    {
        StartCoroutine(hitFrame());
        takeDamageAudio.Play();
        SpawnArtifactKillsAndGoOnCooldown();
    }

    IEnumerator hitFrame()
    {
        spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(.1f);
        spriteRenderer.color = Color.white;
    }
}
