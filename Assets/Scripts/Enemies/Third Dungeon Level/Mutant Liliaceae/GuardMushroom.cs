using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardMushroom : MonoBehaviour
{
    [SerializeField] Animator animator;

    public void Sink()
    {
        animator.SetTrigger("Sink");
        Destroy(this.gameObject, 9 / 12f);
    }
}
