using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrassGolemFlameThrowerArm : MonoBehaviour
{
    // Component is attached onto the left arm joint of the brass golem

    [SerializeField] Transform upperArm;
    [SerializeField] Transform flameThrowerForeArm;
    [SerializeField] BrassGolemFlameThrowerForearm flameThrowerScript;

    [SerializeField] float defaultUpperArmRotation, defaultForeArmRotation;
    [SerializeField] float swipeDuration = 4;
    [SerializeField] Animator jointAnimator, upperArmAnimator, foreArmAnimator;

    [SerializeField] AudioSource loadOutAudio, heavyLoadOutAudio;

    [SerializeField] List<SpriteRenderer> renderers;

    IEnumerator hitFrame()
    {
        foreach (SpriteRenderer spriteRenderer in renderers)
        {
            spriteRenderer.color = Color.red;
        }
        yield return new WaitForSeconds(.1f);
        foreach (SpriteRenderer spriteRenderer in renderers)
        {
            spriteRenderer.color = Color.white;
        }
    }

    public void startHitFrame()
    {
        StartCoroutine(hitFrame());
    }

    void backToWhite()
    {
        foreach (SpriteRenderer spriteRenderer in renderers)
        {
            spriteRenderer.color = Color.white;
        }
    }

    void rotateBackToOriginalPosition(float duration)
    {
        LeanTween.rotateLocal(upperArm.gameObject, new Vector3(0, 0, defaultUpperArmRotation), duration).setEaseOutCirc();
        LeanTween.rotateLocal(flameThrowerForeArm.gameObject, new Vector3(0, 0, defaultForeArmRotation), duration).setEaseOutCirc();
    }

    public void setToUnactive()
    {
        upperArm.gameObject.SetActive(false);
        flameThrowerForeArm.gameObject.SetActive(false);
        this.gameObject.SetActive(false);
    }

    public void StartLoadOut()
    {
        this.gameObject.SetActive(true);
        StartCoroutine(loadOut());
    }

    public void rightSwipe()
    {
        StartCoroutine(swipeRightProcedure());
    }

    public void leftSwipe()
    {
        StartCoroutine(swipeLeftProcedure());
    }

    IEnumerator loadOut()
    {
        transform.localPosition = new Vector3(0, 0.96f);
        LeanTween.moveLocalX(this.gameObject, -0.45f, 0.75f);
        
        upperArm.localRotation = Quaternion.Euler(0, 0, defaultUpperArmRotation);
        flameThrowerForeArm.localRotation = Quaternion.Euler(0, 0, defaultForeArmRotation);
        upperArm.gameObject.SetActive(false);
        flameThrowerForeArm.gameObject.SetActive(false);

        loadOutAudio.Play();
        jointAnimator.SetTrigger("LoadOut");

        yield return new WaitForSeconds(8 / 12f);

        upperArm.gameObject.SetActive(true);
        loadOutAudio.Play();
        upperArmAnimator.SetTrigger("LoadOut");

        yield return new WaitForSeconds(16 / 12f);

        flameThrowerForeArm.gameObject.SetActive(true);
        heavyLoadOutAudio.Play();
        foreArmAnimator.SetTrigger("LoadOut");
        yield return new WaitForSeconds(18 / 12f);
        // done
    }

    IEnumerator swipeRightProcedure()
    {
        LeanTween.rotateLocal(upperArm.gameObject, new Vector3(0, 0, 180), 1.5f).setEaseOutCirc();
        LeanTween.rotateLocal(flameThrowerForeArm.gameObject, new Vector3(0, 0, 0), 1.5f).setEaseOutCirc();

        yield return new WaitForSeconds(1.5f);

        flameThrowerScript.StartFlameThrowerSequence(swipeDuration - 1f);

        LeanTween.rotateLocal(upperArm.gameObject, new Vector3(0, 0, 280), swipeDuration);

        yield return new WaitForSeconds(swipeDuration);

        rotateBackToOriginalPosition(2f);
    }

    IEnumerator swipeLeftProcedure()
    {
        LeanTween.rotateLocal(upperArm.gameObject, new Vector3(0, 0, 350), 1.5f).setEaseOutCirc();
        LeanTween.rotateLocal(flameThrowerForeArm.gameObject, new Vector3(0, 0, 0), 1.5f).setEaseOutCirc();

        yield return new WaitForSeconds(1.5f);

        flameThrowerScript.StartFlameThrowerSequence(swipeDuration - 1f);

        LeanTween.rotateLocal(upperArm.gameObject, new Vector3(0, 0, 284), swipeDuration);

        yield return new WaitForSeconds(swipeDuration);

        rotateBackToOriginalPosition(2f);
    }

    IEnumerator testLoop()
    {
        while (true)
        {
            StartCoroutine(swipeRightProcedure());
            yield return new WaitForSeconds(8f);
            StartCoroutine(swipeLeftProcedure());
            yield return new WaitForSeconds(8f);
        }
    }

    public void armDieDown()
    {
        StopAllCoroutines();
        flameThrowerScript.stopProcedures();
        backToWhite();
        LeanTween.rotateLocal(upperArm.gameObject, new Vector3(0, 0, 274), 1.5f).setEaseOutCirc();
        LeanTween.rotateLocal(flameThrowerForeArm.gameObject, new Vector3(0, 0, 35), 1.5f).setEaseOutCirc();
    }
}
