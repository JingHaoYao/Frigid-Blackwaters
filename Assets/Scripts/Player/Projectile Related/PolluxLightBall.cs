using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PolluxLightBall : PlayerProjectile
{
    float timerUntilExplode = 0;
    float speed = 0;
    [SerializeField] DamageAmount damageAmount;
    [SerializeField] Animator animator;
    [SerializeField] Collider2D damagingCollider;
    [SerializeField] AudioSource explodeAudio, lightAudio;
    private float angleTravel;
    bool impacted = false;
    [SerializeField] int bonusDamage = 0;

    [Header("Pollux Spread Upgrades")]
    [SerializeField] int numberSmallLightBalls = 0;
    [SerializeField] GameObject smallLightBall;

    [Header("Stagnant Upgrades")]
    [SerializeField] bool waves = false;
    [SerializeField] bool spawnInstantExplosions = false;
    [SerializeField] GameObject explosion;
    List<Enemy> enemiesToExplode = new List<Enemy>();

    [SerializeField] ParticleSystem polluxParticles;

    private float percentComplete = 0;

    public void Initialize(float percentComplete)
    {
        transform.localScale = new Vector3(3 * percentComplete, 3 * percentComplete);
        timerUntilExplode = Mathf.Clamp(2.5f * percentComplete, 0.75f, 3f);
        speed = Mathf.Clamp(14 * percentComplete, 3, 14);
        damageAmount.originDamage = Mathf.Clamp(Mathf.FloorToInt(6 * percentComplete), 0, 7) + bonusDamage;
        damageAmount.updateDamage();
        rotate();
        angleTravel = angletoCursor();
        StartCoroutine(explodeAfterTime());
        this.percentComplete = percentComplete;
        polluxParticles.startSize = 0.2f * percentComplete;
        polluxParticles.startLifetime = 0.5f * percentComplete;
        LeanTween.value(0, 0.65f, 0.5f).setOnUpdate((float val) => { lightAudio.volume = val; });
    }

    void rotate()
    {
        LeanTween.rotateZ(this.gameObject, transform.rotation.eulerAngles.z + 270, 1f).setOnComplete(rotate);
    }

    float angletoCursor()
    {
        return Mathf.Atan2(PlayerProperties.cursorPosition.y - transform.position.y, PlayerProperties.cursorPosition.x - transform.position.x);
    }

    IEnumerator explodeAfterTime()
    {

        yield return new WaitForSeconds(timerUntilExplode);
        if (impacted == false)
        {
            explodeProcedure();
        }
    }

    void Update()
    {   
        if (impacted == false)
        {
            transform.position += new Vector3(Mathf.Cos(angleTravel), Mathf.Sin(angleTravel)) * Time.deltaTime * speed;
        }
        polluxParticles.transform.localRotation = Quaternion.Euler(0, 0, (angleTravel * Mathf.Rad2Deg) + 90 - transform.rotation.eulerAngles.z);
    }

    void explodeProcedure()
    {
        impacted = true;
        polluxParticles.loop = false;
        if (waves == false)
        {
            animator.SetTrigger("Explode");
            lightAudio.Stop();
            explodeAudio.Play();
            damagingCollider.enabled = false;
            StartCoroutine(turnOffSpriteRenderer());
            Destroy(this.gameObject, 1f);

            if (numberSmallLightBalls > 0 && percentComplete >= 0.5f)
            {
                float angleIncrement = 360 / numberSmallLightBalls;
                for (int i = 0; i < numberSmallLightBalls; i++)
                {
                    GameObject lightBallInstant = Instantiate(smallLightBall, transform.position, Quaternion.identity);
                    lightBallInstant.GetComponent<SmallPolluxLightBall>().InitializeProjectile(i * angleIncrement);
                }
            }
        }
        else
        {
            if (percentComplete >= 0.5f)
            {
                StartCoroutine(wavesProcedure());
            }
            else
            {
                lightAudio.Stop();
                animator.SetTrigger("Explode");
                explodeAudio.Play();
                damagingCollider.enabled = false;
                StartCoroutine(turnOffSpriteRenderer());
                Destroy(this.gameObject, 1f);
            }
        }
    }

    IEnumerator turnOffSpriteRenderer()
    {
        yield return new WaitForSeconds(0.5f);
        GetComponent<SpriteRenderer>().enabled = false;
    }

    IEnumerator wavesProcedure()
    {
        animator.SetTrigger("Waves");
        for(int i = 0; i < 10; i++)
        {
            damagingCollider.enabled = false;
            yield return new WaitForSeconds(0.1f);
            damagingCollider.enabled = true;
            yield return new WaitForSeconds(0.1f);
        }
        animator.SetTrigger("Explode");
        lightAudio.Stop();
        explodeAudio.Play();
        damagingCollider.enabled = false;
        StartCoroutine(turnOffSpriteRenderer());
        Destroy(this.gameObject, 1f);
        if (spawnInstantExplosions)
        {
            foreach(Enemy enemy in enemiesToExplode)
            {
                if (enemy != null)
                {
                    enemy.dealDamage(5);
                    Instantiate(explosion, enemy.transform.position, Quaternion.identity);
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (waves == false)
        {
            if (impacted == false && collision.gameObject.layer != 15)
            {
                explodeProcedure();
            }
        }
        else
        {
            if (impacted == false)
            {
                if (collision.gameObject.layer == 10 && spawnInstantExplosions)
                {
                    Enemy enemy = collision.gameObject.GetComponent<Enemy>();
                    if (enemy != null)
                    {
                        enemiesToExplode.Add(enemy);
                    }
                }
                else if (collision.gameObject.layer == 12)
                {
                    explodeProcedure();
                }
            }
        }
    }
}
