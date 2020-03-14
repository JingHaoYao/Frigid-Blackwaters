using UnityEngine;

public class LeafFanSkeletonWave : MonoBehaviour
{
    public float speed;

    bool impacted = false;
    public float angleTravel;

    float foamTimer = 0;
    public GameObject waterFoam;

    [SerializeField] private Collider2D collider2D;

    void Update()
    {
        if (impacted == false)
        {
            transform.position += new Vector3(Mathf.Cos(angleTravel * Mathf.Deg2Rad), Mathf.Sin(angleTravel * Mathf.Deg2Rad)) * Time.deltaTime * speed;
        }

        spawnFoam();
    }

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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (impacted == false && collision.gameObject.layer != 15)
        {
            impacted = true;
            this.collider2D.enabled = false;
            LeanTween.alpha(this.gameObject, 0, 0.7f).setOnComplete(() => Destroy(this.gameObject));
        }
    }
}
