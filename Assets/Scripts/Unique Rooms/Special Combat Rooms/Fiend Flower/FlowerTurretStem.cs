using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowerTurretStem : MonoBehaviour
{
    [SerializeField] FlowerTurretHead turretHead;
    [SerializeField] Animator animator;
    public GameObject fiendFlowerBoss;

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
        this.turretHead.fiendFlowerBoss = fiendFlowerBoss;
        this.turretHead.initializeFlowerHead();
    }
}
