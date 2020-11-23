using System.Collections;
using UnityEngine;

public class PyrotheumBlasterProjectile : MonoBehaviour
{
    public float speed;

    [SerializeField] Animator animator;
    bool impacted = false;
    GameObject playerShip;

    [SerializeField] float rotationOffset;
    [SerializeField] AudioSource impactAudio;
    [SerializeField] ProjectileParent projectileParent;
    [SerializeField] Collider2D col;

    private Vector3 centerPosition;
    float angleOffset = 0;
    float angleTravel;


    private float radius = 0;

    public void Initialize(GameObject instantiater, Vector3 centerPosition, int whichBlast, float angleTravel)
    {
        this.projectileParent.instantiater = instantiater;
        this.centerPosition = centerPosition;

        angleOffset = whichBlast * 120;
        this.angleTravel = angleTravel;

        StartCoroutine(projectileProcedure());
    }

    IEnumerator projectileProcedure()
    {
        LeanTween.value(0, 1, 1f).setOnUpdate((float val) => { radius = val; });
        while (true)
        {
            angleOffset += Time.deltaTime * 360;

            if(angleOffset >= 360)
            {
                angleOffset = 0;
            }

            transform.rotation = Quaternion.Euler(0, 0, angleOffset + 180);

            transform.position = centerPosition + new Vector3(Mathf.Cos(angleOffset * Mathf.Deg2Rad), Mathf.Sin(angleOffset * Mathf.Deg2Rad)) * radius;
            centerPosition += new Vector3(Mathf.Cos(angleTravel), Mathf.Sin(angleTravel)) * Time.deltaTime * speed;
            yield return null;
        }
    }

    void impactProcedure()
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

    void Start()
    {
        animator = GetComponent<Animator>();
        playerShip = PlayerProperties.playerShip;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (impacted == false && (collision.gameObject == PlayerProperties.playerShip || collision.gameObject.layer == 12))
        {
            StopAllCoroutines();
            impactProcedure();
        }
    }
}
