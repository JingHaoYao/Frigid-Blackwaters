using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrogmanPelletCasterPellet : MonoBehaviour
{
    public float speed;

    [SerializeField] Animator animator;
    [SerializeField] AudioSource audioSource;
    [SerializeField] Collider2D collider2D;
    bool impacted = false;
    public float angleTravel;

    void Start()
    {
        StartCoroutine(mainRoutine());
        rotate();
    }

    void rotate()
    {
        LeanTween.rotateZ(this.gameObject, transform.rotation.eulerAngles.z + 270, 0.2f).setOnComplete(rotate);
    }

    IEnumerator mainRoutine()
    {
        yield return new WaitForSeconds(6 / 12f);
        while (impacted == false)
        {
            transform.position += new Vector3(Mathf.Cos(angleTravel * Mathf.Deg2Rad), Mathf.Sin(angleTravel * Mathf.Deg2Rad)) * Time.deltaTime * speed;
            yield return null;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (impacted == false && collision.gameObject.layer != 15)
        {
            impacted = true;
            animator.SetTrigger("Impact");
            audioSource.Play();
            Destroy(this.gameObject, 0.5f);
            collider2D.enabled = false;
        }
    }
}
