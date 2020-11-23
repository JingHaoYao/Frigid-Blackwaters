using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructableObject : MonoBehaviour
{
    [SerializeField] Animator animator;
    [SerializeField] string destroyString;
    [SerializeField] float destroyLength;

    [SerializeField] List<GameObject> debris;
    [SerializeField] AudioSource audioSource;

    bool exploded = false;

    void Explode()
    {
        animator.SetTrigger(destroyString);
        audioSource.Play();
        Destroy(this.gameObject, destroyLength);

        int numberDebris = Random.Range(2, 4);
        for(int i = 0; i < numberDebris; i++)
        {
            GameObject debrisInstant = Instantiate(debris[Random.Range(0, debris.Count)], transform.position + new Vector3(Random.Range(-0.5f, 0.5f), Random.Range(-0.5f, 0.5f)), Quaternion.identity);
            if (Random.Range(0, 2) == 1)
            {
                Vector3 scale = debrisInstant.transform.localScale;
                debrisInstant.transform.localScale = new Vector3(scale.x * -1, scale.y);
            }
            debrisInstant.transform.SetParent(transform.parent);

            float angle = Mathf.Atan2(debrisInstant.transform.position.y - transform.position.y, debrisInstant.transform.position.x - transform.position.x);

            debrisInstant.GetComponent<Rigidbody2D>().AddForceAtPosition(new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * 200, debrisInstant.transform.position);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!exploded)
        {
            exploded = true;
            Explode();
        }
    }

}
