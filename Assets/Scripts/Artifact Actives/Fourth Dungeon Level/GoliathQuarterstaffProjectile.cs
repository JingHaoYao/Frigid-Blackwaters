using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoliathQuarterstaffProjectile : MonoBehaviour
{
    public float speed;

    [SerializeField] Animator animator;
    [SerializeField] AudioSource audioSource;
    [SerializeField] Collider2D collider2D;
    bool impacted = false;
    GameObject playerShip;
    Vector3 centerPosition;

    public void Initialize(Vector3 centerPosition)
    {
        this.centerPosition = centerPosition;
    }

    void Start()
    {
        playerShip = PlayerProperties.playerShip;
        StartCoroutine(mainRoutine());
    }

    void Update()
    {
        if (impacted == false)
        {
            LeanTween.rotateZ(this.gameObject, transform.rotation.eulerAngles.z + 270, 0.2f);
        }
    }

    IEnumerator mainRoutine()
    {
        while (impacted == false)
        {
            Vector3 vectorAway = (transform.position - centerPosition).normalized * 0.25f;
            float angleAway = Mathf.Atan2(transform.position.y - centerPosition.y, transform.position.x - centerPosition.x) + Mathf.PI / 2;
            transform.position += (new Vector3(Mathf.Cos(angleAway), Mathf.Sin(angleAway)) + vectorAway) * Time.deltaTime * speed;
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
