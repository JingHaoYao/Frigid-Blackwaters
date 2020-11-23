using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShortPyrotheumBlast : MonoBehaviour
{
    [SerializeField] Animator animator;
    bool impacted = false;
    GameObject playerShip;

    [SerializeField] AudioSource impactAudio;
    [SerializeField] ProjectileParent projectileParent;
    [SerializeField] Collider2D col;
    [SerializeField] float timeToWait = 0.5f;
    [SerializeField] float speed = 6;
    float angleTravel;

    public void Initialize(GameObject instantiater, float angleTravel)
    {
        transform.localScale = new Vector3(0.5f, 0.5f);
        LeanTween.scale(this.gameObject, new Vector3(2.5f, 2.5f), 0.75f).setEaseOutCirc();
        projectileParent.instantiater = instantiater;
        this.angleTravel = angleTravel;
        StartCoroutine(projectileProcedure());
    }

    IEnumerator projectileProcedure()
    {
        float period = 0;
        transform.rotation = Quaternion.Euler(0, 0, angleTravel + 90);
        while (period < 0.5f)
        {
            transform.position += new Vector3(Mathf.Cos(angleTravel * Mathf.Deg2Rad), Mathf.Sin(angleTravel * Mathf.Deg2Rad)) * Time.deltaTime * speed;
            period += Time.deltaTime;

            yield return null;
        }
        dissapear();
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
