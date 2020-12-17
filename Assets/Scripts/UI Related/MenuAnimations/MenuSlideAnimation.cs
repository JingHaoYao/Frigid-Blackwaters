using UnityEngine;
using UnityEngine.Events;

public class MenuSlideAnimation : MonoBehaviour
{
    Animation openAnimation;
    Animation endAnimation;

    bool isAnimating = false;

    public MenuSlideAnimation()
    {
        openAnimation = new Animation();
        endAnimation = new Animation();
    }

    public void SetOpenAnimation(Vector3 startPosition, Vector3 endPosition, float time)
    {
        openAnimation.startingPosition = startPosition;
        openAnimation.endPosition = endPosition;
        openAnimation.animationTime = time;
    }

    public void SetCloseAnimation(Vector3 startPosition, Vector3 endPosition, float time)
    {
        endAnimation.startingPosition = startPosition;
        endAnimation.endPosition = endPosition;
        endAnimation.animationTime = time;
    }

    [System.Serializable]
    struct Animation
    {
        public Vector3 startingPosition;
        public Vector3 endPosition;
        public float animationTime;
    }

    public bool IsAnimating
    {
        get
        {
            return isAnimating;
        }
    }

    public void PlayOpeningAnimation(GameObject objectToAnimate, UnityAction onComplete = null)
    {
        isAnimating = true;
        objectToAnimate.transform.localPosition = openAnimation.startingPosition;
        LeanTween.moveLocal(objectToAnimate, openAnimation.endPosition, openAnimation.animationTime).setEaseOutQuad().setIgnoreTimeScale(true).setOnComplete(() => { isAnimating = false; onComplete?.Invoke(); } );
    }

    public void PlayEndingAnimation(GameObject objectToAnimate, UnityAction action)
    {
        isAnimating = true;
        objectToAnimate.transform.localPosition = endAnimation.startingPosition;
        LeanTween.moveLocal(objectToAnimate, endAnimation.endPosition, endAnimation.animationTime).setEaseOutQuad().setIgnoreTimeScale(true).setOnComplete(() => { action.Invoke(); isAnimating = false; });
    }
}
