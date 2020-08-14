using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GadgetShotProjectile : PlayerProjectile
{
    [SerializeField] Animator animator;
    [SerializeField] AudioSource bounceAudio;
    [SerializeField] Transform trans;
    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] Sprite primedBomb, laserBomb;
    [SerializeField] DamageAmount damageAmount;
    private int numberBounces = 0;
    Vector3 travelVector;
    [SerializeField] float speed;

    [SerializeField] GameObject explosion;
    [SerializeField] GameObject laserPoint;
    List<GadgetShotLaserPoint> spawnedLaserPoints = new List<GadgetShotLaserPoint>();
    [SerializeField] GameObject particles;
    GameObject particlesInstant;

    bool canCollide = true;
    bool impacted = false;

    IEnumerator canCollideDelay()
    {
        canCollide = false;
        yield return new WaitForSeconds(0.1f);
        canCollide = true;
    }

    int whatShotType = 0;
    // 0 - regular
    // 1 - bounce explosions
    // 2 - laser beams
    int tier = 0;

    Coroutine rotateRoutineInstant;

    string extraAnimPrefix = "";

    void pickTierAndType()
    {
        animator.enabled = false;
        numberBounces = 3;
        switch (PlayerUpgrades.gadgetShotUpgrades.Count)
        {
            case 0:
                tier = 1;
                whatShotType = 0;
                break;
            case 1:
                tier = 1;
                whatShotType = 0;
                break;
            case 2:
                tier = 2;
                whatShotType = 0;
                damageAmount.originDamage += 1;
                damageAmount.updateDamage();
                break;
            case 3:
                tier = 3;
                whatShotType = 0;
                damageAmount.originDamage += 1;
                damageAmount.updateDamage();
                break;
            case 4:
                tier = 4;
                damageAmount.originDamage += 1;
                damageAmount.updateDamage();
                if (PlayerUpgrades.gadgetShotUpgrades[3] == "bounce_explosions_upgrade")
                {
                    spriteRenderer.sprite = primedBomb;
                    extraAnimPrefix = "Primed";
                    whatShotType = 1;
                }
                else
                {
                    spriteRenderer.sprite = laserBomb;
                    extraAnimPrefix = "Laser";
                    whatShotType = 2;
                }
                break;
            case 5:
                tier = 5;
                damageAmount.originDamage += 1;
                damageAmount.updateDamage();
                if (PlayerUpgrades.gadgetShotUpgrades[3] == "bounce_explosions_upgrade")
                {
                    spriteRenderer.sprite = primedBomb;
                    extraAnimPrefix = "Primed";
                    whatShotType = 1;
                }
                else
                {
                    spriteRenderer.sprite = laserBomb;
                    extraAnimPrefix = "Laser";
                    whatShotType = 2;
                }
                break;
            case 6:
                tier = 6;
                damageAmount.originDamage += 1;
                damageAmount.updateDamage();
                if (PlayerUpgrades.gadgetShotUpgrades[3] == "bounce_explosions_upgrade")
                {
                    spriteRenderer.sprite = primedBomb;
                    extraAnimPrefix = "Primed";
                    whatShotType = 1;
                }
                else
                {
                    spriteRenderer.sprite = laserBomb;
                    numberBounces = 5;
                    extraAnimPrefix = "Laser";
                    whatShotType = 2;
                }
                break;
        }
        animator.enabled = true;
        animator.SetTrigger("Impact" + extraAnimPrefix);
    }

    public void Initialize(float angleTravel)
    {
        pickTierAndType();
        rotateRoutineInstant = StartCoroutine(rotateLoop());
        travelVector = new Vector3(Mathf.Cos(angleTravel), Mathf.Sin(angleTravel)) * speed;
        particlesInstant = Instantiate(particles, transform.position, Quaternion.Euler(0, 0, angleTravel * Mathf.Rad2Deg + 90));
        StartCoroutine(mainLoop());
    }

    IEnumerator mainLoop()
    {
        while (numberBounces > 0)
        {
            trans.position += travelVector * Time.deltaTime;
            particlesInstant.transform.position = transform.position;
            yield return null;
        }

        if(whatShotType == 2)
        {
            foreach(var gadget in spawnedLaserPoints)
            {
                gadget.Activate();
            }
        }

        impacted = true;

        particlesInstant.GetComponent<ParticleSystem>().Stop();
        Destroy(particlesInstant, 1f);
        StopCoroutine(rotateRoutineInstant);
        animator.SetTrigger("Dissipate" + extraAnimPrefix);

        Destroy(this.gameObject, 6/12f);
    }

    IEnumerator rotateLoop()
    {
        while (true)
        {
            transform.Rotate(0, 0, 6 * 120 * Time.deltaTime);
            yield return null;
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 12 && canCollide && !impacted)
        {
            StartCoroutine(canCollideDelay());
            Vector3 normalVector = collision.GetContact(0).normal;
            travelVector = Vector3.Reflect(travelVector, normalVector);

            numberBounces--;

            if (numberBounces > 0)
            {
                particlesInstant.transform.rotation = Quaternion.Euler(0, 0, Mathf.Atan2(travelVector.y, travelVector.x) * Mathf.Rad2Deg + 90);
            }

            bounceAudio.Play();

            animator.SetTrigger("Impact" + extraAnimPrefix);

            damageAmount.originDamage += 1;
            damageAmount.updateDamage();

            if(whatShotType == 1)
            {
                GameObject explosionInstant = Instantiate(explosion, transform.position, Quaternion.identity);

                switch (tier)
                {
                    case 4:
                        explosionInstant.GetComponent<GadgetShotExplosion>().Initialize(4, 2);
                        break;
                    case 5:
                        explosionInstant.GetComponent<GadgetShotExplosion>().Initialize(8, 2);
                        break;
                    case 6:
                        explosionInstant.GetComponent<GadgetShotExplosion>().Initialize(8, 4);
                        break;
                }
            }
            else if(whatShotType == 2)
            {
                GameObject laserPointInstant = Instantiate(laserPoint, transform.position, Quaternion.identity);
                GadgetShotLaserPoint laserPointScript = laserPointInstant.GetComponent<GadgetShotLaserPoint>();

                switch (tier)
                {
                    case 4:
                        if(spawnedLaserPoints.Count == 0) {
                            laserPointScript.Initialize(false, null, 4);
                        }
                        else
                        {
                            laserPointScript.Initialize(true, spawnedLaserPoints[spawnedLaserPoints.Count - 1], 4);
                        }
                        break;
                    case 5:
                        if (spawnedLaserPoints.Count == 0)
                        {
                            laserPointScript.Initialize(false, null, 8);
                        }
                        else
                        {
                            laserPointScript.Initialize(true, spawnedLaserPoints[spawnedLaserPoints.Count - 1], 8);
                        }
                        break;
                    case 6:
                        if (spawnedLaserPoints.Count == 0)
                        {
                            laserPointScript.Initialize(false, null, 8);
                        }
                        else
                        {
                            laserPointScript.Initialize(true, spawnedLaserPoints[spawnedLaserPoints.Count - 1], 8);
                        }
                        break;
                }

                spawnedLaserPoints.Add(laserPointScript);
            }
        }
    }

}
