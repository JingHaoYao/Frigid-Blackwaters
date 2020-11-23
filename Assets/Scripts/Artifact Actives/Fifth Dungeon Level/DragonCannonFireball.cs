using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragonCannonFireball : MonoBehaviour
{
    [SerializeField] GameObject explosionImpact;
    [SerializeField] Collider2D collider2D;
    private float angleTravel;
    [SerializeField] float speed = 8;

    public void Initialize(float angle)
    {
        this.angleTravel = angle;
    }

    private void Update()
    {
        transform.position += new Vector3(Mathf.Cos(angleTravel), Mathf.Sin(angleTravel)) * Time.deltaTime * speed;
        transform.rotation = Quaternion.Euler(0, 0, angleTravel * Mathf.Rad2Deg + 90);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 12)
        {
            Destroy(this.gameObject);
            Instantiate(explosionImpact, transform.position, Quaternion.identity);
        }
    }
}
