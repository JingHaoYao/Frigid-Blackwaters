using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragonCannon : MonoBehaviour
{
    [SerializeField] Animator animator;
    [SerializeField] AudioSource fireAudio, dragonRoarAudio;
    [SerializeField] GameObject fireBall;
    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] Sprite basicSprite;
    Coroutine followRoutine;
    bool canFire = false;

    IEnumerator followCursorAndShoot()
    {
        LeanTween.rotateZ(this.gameObject, angleToCursor() + 90, 0.5f);

        yield return new WaitForSeconds(0.5f);

        while (true)
        { 
            transform.eulerAngles = new Vector3(0, 0, angleToCursor() + 90);
            canFire = true;

            yield return null;
        }
    }

    public float angleToCursor()
    {
        return Mathf.Atan2(PlayerProperties.cursorPosition.y - transform.position.y, PlayerProperties.cursorPosition.x - transform.position.x) * Mathf.Rad2Deg;
    }

    public void ShootProjectile()
    {
        if (canFire)
        {
            StartCoroutine(ShootProjectileCoroutine());
        }
    }

    IEnumerator ShootProjectileCoroutine()
    {
        animator.enabled = true;
        animator.SetTrigger("Fire");
        fireAudio.Play();
        dragonRoarAudio.Play();
        yield return new WaitForSeconds(3 / 12f);
        for (int i = 0; i < 3; i++)
        {
            GameObject fireBallInstant = Instantiate(fireBall, transform.position + new Vector3(Mathf.Cos(angleToCursor() * Mathf.Deg2Rad), Mathf.Sin(angleToCursor() * Mathf.Deg2Rad)) * 1.5f, Quaternion.identity);
            fireBallInstant.GetComponent<DragonCannonFireball>().Initialize((angleToCursor() - 7.5f + 7.5f * i) * Mathf.Deg2Rad);
        }
        yield return new WaitForSeconds(4 / 12f);
        animator.enabled = false;
        spriteRenderer.sprite = basicSprite;
    }

    public void relocateCannon(Vector3 position)
    {
        if (followRoutine != null)
        {
            StopCoroutine(followRoutine);
        }
        transform.rotation = Quaternion.Euler(0, 0, 0);
        transform.position = position;
        animator.SetTrigger("Rise");
        StartCoroutine(waitAndStartCoroutine());
    }

    IEnumerator waitAndStartCoroutine()
    {
        animator.enabled = true;
        yield return new WaitForSeconds(1f);
        followRoutine = StartCoroutine(followCursorAndShoot());
        animator.enabled = false;
    }
}
