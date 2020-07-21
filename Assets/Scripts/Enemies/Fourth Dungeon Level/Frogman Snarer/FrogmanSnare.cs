using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrogmanSnare : MonoBehaviour
{
    bool shouldSnare = true;
    [SerializeField] Animator animator;
    [SerializeField] GameObject damageBox;
    [SerializeField] AudioSource snapAudio;
    public FrogmanSnarer snarer;
    bool snaredAlready = false;
    // need reference to enemy

    private void Start()
    {
        StartCoroutine(waitUntilDecay());
    }

    IEnumerator waitUntilDecay()
    {
        yield return new WaitForSeconds(10f);
        snarer.removeMine(this.gameObject);
        goAway();
    }

    public void goAway()
    {
        shouldSnare = false;
        LeanTween.alpha(this.gameObject, 0, 0.5f).setOnComplete(() => Destroy(this.gameObject));
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject == PlayerProperties.playerShip && shouldSnare && snaredAlready == false)
        {
            snaredAlready = true;
            StartCoroutine(snapAndOpen());
        }
    }

    IEnumerator snapAndOpen()
    {
        snapAudio.Play();
        animator.SetTrigger("Snap");
        PlayerProperties.playerScript.addRootingObject();
        damageBox.SetActive(true);
        yield return new WaitForSeconds(1 / 12f);
        damageBox.SetActive(false);
        yield return new WaitForSeconds(1.5f);
        PlayerProperties.playerScript.removeRootingObject();
        animator.SetTrigger("Open");
        yield return new WaitForSeconds(8 / 12f);
        Destroy(this.gameObject);
    }
}
