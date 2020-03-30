using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FiendFlowerBudStem : MonoBehaviour
{
    [SerializeField] FiendFlowerBudTurretHead turretHead;
    [SerializeField] Animator animator;

    public void destroyStem()
    {
        animator.SetTrigger("Sink");
        Destroy(this.gameObject, 0.333f);
    }

    private void Start()
    {
        StartCoroutine(startUpRoutine());
    }

    IEnumerator startUpRoutine()
    {
        yield return new WaitForSeconds(0.333f);
        this.turretHead.initializeFlowerHead();
    }
}
