using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AncientLavaCannonHead : MonoBehaviour
{
    [SerializeField] Animator animator;
    [SerializeField] ParticleSystem flameThrowerParticles;
    [SerializeField] GameObject flamethrowerHitbox;
    [SerializeField] GameObject pyrotheumSpitterProjectile;
    [SerializeField] AudioSource flameThrowerLoop;
    [SerializeField] AudioSource pyrotheumSpitAudio;
    [SerializeField] AncientLavaCannon ancientLavaCannonInstant;
    [SerializeField] SpriteRenderer spriteRenderer;

    IEnumerator hitFrame()
    {
        spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(.1f);
        spriteRenderer.color = Color.white;
    }

    public void StartHitFrame()
    {
        StartCoroutine(hitFrame());
    }

    public void StartUp()
    {
        animator.Play("Boss Start Up Head");
        LeanTween.move(this.gameObject, transform.position + Vector3.up, 8 / 12f).setEaseOutCirc();
    }

    public void StartFlameThrowerRoutine(float from, float to)
    {
        StartCoroutine(flameThrowerRoutine(from, to));
    }

    IEnumerator flameThrowerRoutine(float from, float to)
    {
        animator.Play("Cannon Down View Flamethrower Start Up");
        LeanTween.rotate(this.gameObject, new Vector3(0, 0, from), 1f).setEaseOutQuad();
        flameThrowerParticles.Play();
        flameThrowerLoop.Play();

        yield return new WaitForSeconds(1f);

        float time = Mathf.Abs(to - from) / 60;
        LeanTween.rotate(this.gameObject, new Vector3(0, 0, to), time);
        StartCoroutine(toggleHitBoxRoutine(time));

        yield return new WaitForSeconds(time);
        flameThrowerLoop.Stop();

        animator.Play("Cannon Down View Flamethrower End");
        flameThrowerParticles.Stop();
    }

    IEnumerator toggleHitBoxRoutine(float totalTime)
    {
        float timeElapsed = 0;
        while(timeElapsed < totalTime)
        {
            timeElapsed += 0.2f;
            yield return new WaitForSeconds(0.2f);
            flamethrowerHitbox.SetActive(true);
            yield return new WaitForSeconds(0.2f);
            timeElapsed += 0.2f;
            flamethrowerHitbox.SetActive(false);
        }
    }

    float angleToShip()
    {
        return (Mathf.Atan2(PlayerProperties.playerShipPosition.y - transform.position.y, PlayerProperties.playerShipPosition.x - transform.position.x) * Mathf.Rad2Deg + 360) % 360;
    }

    public void StartSpitAttack()
    {
        StartCoroutine(spitterRoutine());
    }

    IEnumerator spitterRoutine()
    {
        animator.Play("Cannon Down View Spit Start Up");
        LeanTween.rotate(this.gameObject, new Vector3(0, 0, angleToShip() + 90), 1f);
        yield return new WaitForSeconds(1f);

        animator.Play("Cannon Down View Spit Idle");

        float angleBase = angleToShip();

        for(int i = 0; i < 15; i++)
        {
            for(int k = 0; k < 3; k++)
            {
                GameObject instant = Instantiate(pyrotheumSpitterProjectile, transform.position, Quaternion.identity);
                instant.GetComponent<PyrotheumProjectile>().angleTravel = angleBase + k * 120;
                instant.GetComponent<ProjectileParent>().instantiater = ancientLavaCannonInstant.gameObject;
                pyrotheumSpitAudio.Play();
            }
            angleBase += 5;
            yield return new WaitForSeconds(0.2f);
        }

        animator.Play("Cannon Down View Spit End");
    }

    public void DeathRoutine()
    {
        StopAllCoroutines();
        flameThrowerParticles.Stop();
        flamethrowerHitbox.SetActive(false);
        spriteRenderer.color = Color.white;
        LeanTween.rotate(this.gameObject, new Vector3(0, 0, 0), 0.5f);
        LeanTween.move(this.gameObject, transform.position - Vector3.up, 0.5f).setEaseOutCirc();
        animator.Play("Head Death");
    }

}
