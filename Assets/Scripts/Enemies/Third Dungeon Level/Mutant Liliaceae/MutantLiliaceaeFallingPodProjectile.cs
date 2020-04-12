using UnityEngine;

public class MutantLiliaceaeFallingPodProjectile : MonoBehaviour
{
    [SerializeField] Animator animator;
    [SerializeField] AudioSource audio;

    public void shatterPod()
    {
        animator.SetTrigger("Shatter");
        Destroy(this.gameObject, 7 / 12f);
        audio.Play();
    }
}
