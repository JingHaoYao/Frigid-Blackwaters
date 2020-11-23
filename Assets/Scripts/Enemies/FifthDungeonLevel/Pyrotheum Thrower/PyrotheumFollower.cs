using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PyrotheumFollower : MonoBehaviour
{
    [SerializeField] Animator animator;
    bool impacted = false;
    GameObject playerShip;

    [SerializeField] AudioSource impactAudio;
    [SerializeField] ProjectileParent projectileParent;
    [SerializeField] Collider2D col;

    GameObject enemyToFollow;
    float angleOffset = 0;
    float angleRotation = 0;

    public void Initialize(GameObject instantiater, int whichType)
    {
        angleOffset = whichType * 120;
        projectileParent.instantiater = instantiater;
        enemyToFollow = instantiater;
        StartCoroutine(projectileProcedure());
    }

    IEnumerator projectileProcedure()
    {
        while (true)
        {
            angleRotation += Time.deltaTime * 120;
            transform.position = enemyToFollow.transform.position + new Vector3(Mathf.Cos((angleOffset + angleRotation) * Mathf.Deg2Rad), Mathf.Sin((angleOffset + angleRotation) * Mathf.Deg2Rad)) * (1.5f + Mathf.Sin(angleRotation * Mathf.Deg2Rad));
            if(angleRotation >= 360)
            {
                angleRotation = 0;
            }

            yield return null;
        }
    }

    public void dissapear()
    {
        if (impacted == false)
        {
            impacted = true;
            animator.SetTrigger("Impact");
            impactAudio.Play();
            Destroy(this.gameObject, 7 / 12f);
            col.enabled = false;
        }
    }
}
