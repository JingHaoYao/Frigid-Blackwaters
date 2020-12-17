using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TutorialGolem : Enemy
{
    [SerializeField] Animator animator;
    [SerializeField] TutorialGolemFist leftFist, rightFist;
    Vector3 leftFistStartPosition, rightFistStartPosition;
    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] Sprite resetStateSprite;
    [SerializeField] AudioSource golemStartUpAudio;
    [SerializeField] BossHealthBar bossHealthBar;
    [SerializeField] AudioSource golemExplodeAudio;
    [SerializeField] NewTutorialManager tutorialManager;
    [SerializeField] AudioSource chargeFlashAudio;
    [SerializeField] AudioSource flashAudio;

    public void TutorialSlamDash(UnityAction inbetweenAction, int numberSlams, UnityAction onComplete)
    {
        StartCoroutine(slamFistRoutine(numberSlams, inbetweenAction, onComplete));
    }

    public void GolemFlash(bool actuallyFlash, UnityAction inbetweenAction, UnityAction onComplete)
    {
        StartCoroutine(flashRoutine(actuallyFlash, inbetweenAction, onComplete));
    }

    IEnumerator flashRoutine(bool actuallyFlash, UnityAction inbetweenAction, UnityAction onComplete = null)
    {
        animator.Play("Golem Flash");
        chargeFlashAudio.Play();
        yield return new WaitForSeconds(3 / 12f);
        flashAudio.Play();
        if (actuallyFlash)
        {
            tutorialManager.FlashWhite();

            yield return new WaitForSeconds(0.5f);
            leftFist.StartFollowPlayer();
        }
        else
        {
            StartCoroutine(slamFistRoutine(1, inbetweenAction, onComplete));
        }
    }

    public void GuaranteedDeathSlam() { 
        leftFist.FistSlam();
        bossHealthBar.bossEnd();
    }

    IEnumerator slamFistRoutine(int numberSlams, UnityAction inbetweenAction, UnityAction onComplete)
    {
        bool usedLeftFist = false;
        for(int i = 0; i < numberSlams; i++)
        {
            if(usedLeftFist == false)
            {
                leftFist.SlamFist(PlayerProperties.playerShipPosition, true, inbetweenAction);
                usedLeftFist = true;
            }
            else
            {
                rightFist.SlamFist(PlayerProperties.playerShipPosition, true, inbetweenAction);
                usedLeftFist = false;
            }
            yield return new WaitForSeconds(2f);
        }
        onComplete?.Invoke();
    }

    public void ResetBoss()
    {
        leftFist.ResetFist();
        rightFist.ResetFist();
        leftFist.transform.position = leftFistStartPosition;
        rightFist.transform.position = rightFistStartPosition;
        animator.Play("New State");
        spriteRenderer.sprite = resetStateSprite;
    }

    public void StartBoss(UnityAction endAction)
    {
        StartCoroutine(startBoss(endAction));
    }

    IEnumerator startBoss(UnityAction unityAction)
    {
        animator.Play("Golem Startup");
        golemStartUpAudio.Play();
        bossHealthBar.targetEnemy = this;
        bossHealthBar.bossStartUp("The Resting Golem");
        yield return new WaitForSeconds(9 / 12f);
        unityAction.Invoke();
    }

    public override void damageProcedure(int damage)
    {
    }

    public override void deathProcedure()
    {
        animator.Play("Golem Crystal Explode");
        golemExplodeAudio.Play();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.GetComponent<DamageAmount>())
        {
            int damage = collision.gameObject.GetComponent<DamageAmount>().damage;

            if(damage > 2)
            {
                dealDamage(999);
            }
            else
            {
                SpawnArtifactKillsAndGoOnCooldown(0, -1.5f, false);
            }
        }
    }

    private void Start()
    {
        leftFistStartPosition = leftFist.transform.position;
        rightFistStartPosition = rightFist.transform.position;
    }
}
