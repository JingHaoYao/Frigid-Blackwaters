using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SerpentFinWave : MonoBehaviour {
    Animator animator;
    public GameObject water;
    float foamTimer = 0;
    public float angleTravel = 0;
    float speed = 6;
    bool collided = false;
    Vector3 startCamPos;
    public GameObject waterFoam;

    void spawnFoam()
    {
        if (speed > 0)
        {
            float whatAngle = angleTravel;
            foamTimer += Time.deltaTime;
            if (foamTimer >= 0.05f * speed / 3f)
            {
                foamTimer = 0;
                GameObject foam = Instantiate(waterFoam, transform.position, Quaternion.Euler(0, 0, whatAngle + 90));
            }
        }
    }

    void Start () {
        animator = GetComponent<Animator>();
        startCamPos = Camera.main.transform.position;
	}

	void Update () {
        transform.position += new Vector3(Mathf.Cos(angleTravel * Mathf.Deg2Rad), Mathf.Sin(angleTravel * Mathf.Deg2Rad), 0) * Time.deltaTime * speed;
        spawnFoam();

        if(collided == false && (Mathf.Abs(transform.position.y - startCamPos.y) > 8 || Mathf.Abs(transform.position.x - startCamPos.x) > 8))
        {
            this.GetComponent<Collider2D>().enabled = false;
            collided = true;
            animator.SetTrigger("Submerge");
            Destroy(this.gameObject, 0.5f);
            speed = 0;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collided == false)
        {
            this.GetComponent<Collider2D>().enabled = false;
            collided = true;
            animator.SetTrigger("Submerge");
            Destroy(this.gameObject, 0.5f);
            speed = 0;
        }
    }
}
