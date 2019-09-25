using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnchorShot : MonoBehaviour
{
    public float speed = 0;
    public float angleTravel;
    public Vector3 returnPosition;
    Vector3 initCameraPos;
    GameObject playerShip;
    bool returnToSender;
    bool anchorGone;
    float stunPeriod = 3;
    List<GameObject> grabbedEnemyList = new List<GameObject>();
    public GameObject anchorPoof;

    float pickDirectionTravel()
    {
        GameObject cursor = FindObjectOfType<CursorTarget>().gameObject;
        return (360 + Mathf.Atan2(cursor.transform.position.y - transform.position.y, cursor.transform.position.x - transform.position.x) * Mathf.Rad2Deg) % 360;
    }

    void Start()
    {
        playerShip = GameObject.Find("PlayerShip");
        angleTravel = pickDirectionTravel();
        transform.rotation = Quaternion.Euler(0, 0, angleTravel + 90);
        initCameraPos = Camera.main.transform.position;
        returnPosition = playerShip.transform.position;
    }

    void Update()
    {
        if(speed < 50)
        {
            speed += 75 * Time.deltaTime;
        }

        if (returnToSender == false)
        {
            transform.position += Time.deltaTime * speed * new Vector3(Mathf.Cos(angleTravel * Mathf.Deg2Rad), Mathf.Sin(angleTravel * Mathf.Deg2Rad), 0);
            if (Vector2.Distance(returnPosition, transform.position) > 8 || Mathf.Abs(transform.position.x - Camera.main.transform.position.x) > 7 || Mathf.Abs(transform.position.y - Camera.main.transform.position.y) > 7)
            {
                returnToSender = true;
                speed = 0;
            }
        }
        else
        {
            if (anchorGone == false)
            {
                returnPosition = playerShip.transform.position;
                Vector3 toShipVector = new Vector3(playerShip.transform.position.x - transform.position.x, playerShip.transform.position.y - transform.position.y).normalized;
                transform.rotation = Quaternion.Euler(0, 0, Mathf.Atan2(toShipVector.y, toShipVector.x) * Mathf.Rad2Deg + 270);
                transform.position += Time.deltaTime * speed * toShipVector;
                foreach (GameObject enemy in grabbedEnemyList)
                {
                    enemy.transform.position += toShipVector * speed * Time.deltaTime;
                }

                if (Vector2.Distance(returnPosition, transform.position) < 0.5f)
                {
                    this.GetComponent<Collider2D>().enabled = false;
                    this.GetComponent<SpriteRenderer>().enabled = false;
                    this.transform.GetChild(0).gameObject.SetActive(false);
                    Instantiate(anchorPoof, transform.position, Quaternion.identity);
                    anchorGone = true;
                }
            }
        }

        if(anchorGone == true)
        {
            if(stunPeriod > 0)
            {
                stunPeriod -= Time.deltaTime;
            }
            else
            {
                foreach(GameObject enemy in grabbedEnemyList)
                {
                    if (enemy != null)
                    {
                        enemy.GetComponent<Enemy>().stopAttacking = false;
                        enemy.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;
                    }
                }
                Destroy(this.gameObject);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.GetComponent<Enemy>() && collision.gameObject.tag != "StrongEnemy")
        {
            this.GetComponent<AudioSource>().Play();
            collision.gameObject.GetComponent<Enemy>().stopAttacking = true;
            collision.gameObject.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
            grabbedEnemyList.Add(collision.gameObject);
            FindObjectOfType<AudioManager>().PlaySound("Anchor Hit");
            returnToSender = true;
            speed = 0;
        }
    }
}
