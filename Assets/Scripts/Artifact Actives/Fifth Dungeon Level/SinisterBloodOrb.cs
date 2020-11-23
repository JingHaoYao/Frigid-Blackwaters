using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SinisterBloodOrb : MonoBehaviour
{
    float speed = 0.5f;
    [SerializeField] Animator animator;
    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] AudioSource burstAudio;
    Vector3 basePosition;
    float offSet = 1;

    private void Start()
    {
        basePosition = transform.position;
        LeanTween.value(0, 1, 0.5f).setEaseInOutQuad().setLoopPingPong().setOnUpdate((float val) => { offSet = val; });
        StartCoroutine(mainLoop());
    }

    IEnumerator mainLoop()
    {
        yield return new WaitForSeconds(5 / 12f);
        while(Vector2.Distance(transform.position, PlayerProperties.playerShipPosition) > 0.5f)
        {
            Vector3 directionVector = PlayerProperties.playerShipPosition - basePosition;
            basePosition += directionVector.normalized * speed * Time.deltaTime;
            transform.position = basePosition + Vector3.up * offSet;

            speed += Time.deltaTime * 0.5f;

            yield return null;
        }

        PlayerProperties.playerScript.healPlayer(600);
        animator.SetTrigger("Burst");
        burstAudio.Play();
        StartCoroutine(speedBonus());

        yield return new WaitForSeconds(6 / 12f);
        spriteRenderer.enabled = false;
    }

    IEnumerator speedBonus()
    {
        PlayerProperties.playerScript.conSpeedBonus += 3;

        yield return new WaitForSeconds(3f);

        PlayerProperties.playerScript.conSpeedBonus -= 3;
        Destroy(this.gameObject);
    }

}
