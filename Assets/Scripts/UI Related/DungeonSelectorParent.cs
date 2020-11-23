using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DungeonSelectorParent : MonoBehaviour
{
    public Animator blackFadeOutAnimator;
    [SerializeField] private Image backGroundImage;
    [SerializeField] private GameObject levelSelector;
    Coroutine returnRoutine;

    IEnumerator fadeInFadeOut()
    {
        blackFadeOutAnimator.gameObject.SetActive(true);
        blackFadeOutAnimator.SetTrigger("FadeOut");
        yield return new WaitForSeconds(1f);
        FindObjectOfType<AudioManager>().FadeIn("Background Music", 0.2f, 1f);
        FindObjectOfType<AudioManager>().FadeIn("Background Waves", 0.2f, 0.05f);
        this.levelSelector.SetActive(false);
        backGroundImage.enabled = false;
        PlayerProperties.playerShip.transform.position = Vector3.zero;
        blackFadeOutAnimator.SetTrigger("FadeIn");
        yield return new WaitForSeconds(1f);
        backGroundImage.enabled = true;
        this.levelSelector.SetActive(true);
        blackFadeOutAnimator.gameObject.SetActive(false);
        PlayerProperties.playerScript.removeRootingObject();
        PlayerProperties.playerScript.windowAlreadyOpen = false;
        returnRoutine = null;
        this.gameObject.SetActive(false);
    }

    public void returnToHub()
    {
        if (returnRoutine == null)
        {
            returnRoutine = StartCoroutine(fadeInFadeOut());
        }
    }
}
