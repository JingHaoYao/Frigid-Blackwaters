using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlaisesTomeFireball : MonoBehaviour
{
    public float speed = 5;
    GameObject cursorToFollow;
    float prevAngle;
    public GameObject impact;
    Vector3 targetScale = Vector3.zero;

    void Start()
    {
        cursorToFollow = FindObjectOfType<CursorTarget>().gameObject;
    }

    void Update()
    {
        float angleToCursor = (360 + Mathf.Atan2(cursorToFollow.transform.position.y - transform.position.y, cursorToFollow.transform.position.x - transform.position.x) * Mathf.Rad2Deg) % 360;
        targetScale = new Vector3(speed / 2f, speed / 2f);

        if (Mathf.Abs(prevAngle - angleToCursor) < 10 && Vector2.Distance(transform.position, cursorToFollow.transform.position) > 0.2f)
        {
            speed += Time.deltaTime * 4;
            GetComponent<DamageAmount>().originDamage = 1 + (Mathf.FloorToInt(speed / 2f));
            GetComponent<DamageAmount>().updateDamage();
        }
        else
        {
            speed = 5;
        }

        if (Mathf.Abs(transform.localScale.x - targetScale.x) > 0.1f)
        {
            if (transform.localScale.x > targetScale.x)
            {
                transform.localScale = transform.localScale - new Vector3(1, 1, 1) * Time.deltaTime * 5;
            }
            else if (transform.localScale.x < targetScale.x)
            {
                transform.localScale = transform.localScale + new Vector3(1, 1, 1) * Time.deltaTime * 5;
            }
        }

        prevAngle = angleToCursor;

        if (Vector2.Distance(transform.position, cursorToFollow.transform.position) > 0.2f)
        {
            transform.position += (cursorToFollow.transform.position - transform.position).normalized * speed * Time.deltaTime;
            transform.rotation = Quaternion.Euler(0, 0, angleToCursor + 90);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Instantiate(impact, transform.position, Quaternion.identity);
    }
}
