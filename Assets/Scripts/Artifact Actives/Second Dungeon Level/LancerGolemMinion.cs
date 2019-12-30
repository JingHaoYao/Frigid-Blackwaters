using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LancerGolemMinion : MonoBehaviour
{
    Animator animator;
    SpriteRenderer spriteRenderer;
    WhichRoomManager whichRoomManager;
    Rigidbody2D rigidBody2D;
    public GameObject lanceHitBox;
    int whatView = 1;
    int prevView = 1;
    bool attacking = false;
    float speed = 3f;
    List<AStarNode> path;
    PlayerScript playerScript;

    float pickAnimPeriod = 0;

    float duration = 0;
    Enemy targetEnemy;

    void Start()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        whichRoomManager = GetComponent<WhichRoomManager>();
        rigidBody2D = GetComponent<Rigidbody2D>();
        playerScript = FindObjectOfType<PlayerScript>();
    }

    IEnumerator attack()
    {
        attacking = true;
        rigidBody2D.velocity = Vector3.zero;
        animator.SetTrigger("Attack" + whatView.ToString());
        yield return new WaitForSeconds(2f / 12f);
        GetComponents<AudioSource>()[0].Play();
        yield return new WaitForSeconds(4f / 12f);
        lanceHitBox.GetComponents<PolygonCollider2D>()[whatView - 1].enabled = true;
        yield return new WaitForSeconds(1f / 12f);
        lanceHitBox.GetComponents<PolygonCollider2D>()[whatView - 1].enabled = false;
        yield return new WaitForSeconds(2f / 12f);
        attacking = false;
    }

    void pickView(float angleOrientation)
    {
        if (angleOrientation >= 0 && angleOrientation < 60)
        {
            whatView = 3;
        }
        else if (angleOrientation >= 60 && angleOrientation < 120)
        {
            whatView = 4;
        }
        else if (angleOrientation >= 120 && angleOrientation < 180)
        {
            whatView = 5;
        }
        else if (angleOrientation >= 180 && angleOrientation < 240)
        {
            whatView = 6;
        }
        else if (angleOrientation >= 240 && angleOrientation < 300)
        {
            whatView = 1;
        }
        else
        {
            whatView = 2;
        }
    }

    void pickIdleAnimation()
    {
        pickAnimPeriod += Time.deltaTime;
        if (pickAnimPeriod > 0.1f)
        {
            if (prevView != whatView)
            {
                animator.SetTrigger("Idle" + whatView.ToString());
                prevView = whatView;
            }
            pickAnimPeriod = 0;
        }
    }

    void pickTargetEnemy()
    {
        if(targetEnemy == null)
        {
            Enemy currEnemy = null;
            float dist = float.MaxValue;
            Enemy[] enemyList = FindObjectsOfType<Enemy>();
            enemyList = FindObjectsOfType<Enemy>();

            if (enemyList.Length > 0)
            {
                foreach (Enemy enemy in enemyList)
                {
                    if (Vector2.Distance(playerScript.transform.position, enemy.transform.position) < dist)
                    {
                        dist = Vector2.Distance(playerScript.transform.position, enemy.transform.position);
                        currEnemy = enemy;
                    }
                }

                targetEnemy = currEnemy;
            }
            else
            {
                if (duration < 20)
                {
                    duration = 30;
                }
            }
        }
    }

    float cardinalizeDirections(float angle)
    {
        if (angle > 22.5f && angle <= 67.5f)
        {
            return 45;
        }
        else if (angle > 67.5f && angle <= 112.5f)
        {
            return 90;
        }
        else if (angle > 112.5f && angle <= 157.5f)
        {
            return 135;
        }
        else if (angle > 157.5f && angle <= 202.5f)
        {
            return 180;
        }
        else if (angle > 202.5f && angle <= 247.5f)
        {
            return 225;
        }
        else if (angle > 247.5 && angle < 292.5)
        {
            return 270;
        }
        else if (angle > 292.5 && angle < 337.5)
        {
            return 315;
        }
        else
        {
            return 0;
        }
    }

    private void Update()
    {
        pickTargetEnemy();
        if (!animator.GetCurrentAnimatorStateInfo(0).IsName("Golem Minion Rise"))
        {
            if (duration < 20)
            {
                if (GetComponent<AStarPathfinding>().grid != null)
                {
                    this.GetComponent<AStarPathfinding>().target = targetEnemy.transform.position;
                    path = GetComponent<AStarPathfinding>().seekPath;
                    AStarNode pathNode = path[0];
                    Vector3 targetPos = pathNode.nodePosition;

                    float angleToShip = (360 + Mathf.Atan2(targetPos.y - transform.position.y, targetPos.x - transform.position.x) * Mathf.Rad2Deg) % 360;
                    pickView(angleToShip);
                    if (Vector2.Distance(targetEnemy.transform.position, transform.position) < 1.7f && attacking == false)
                    {
                        StartCoroutine(attack());
                    }
                    else
                    {
                        if (attacking == false)
                        {

                            pickIdleAnimation();
                            float targetAngle = (360 + Mathf.Atan2(targetPos.y - (transform.position.y + 0.5f), targetPos.x - transform.position.x) * Mathf.Rad2Deg) % 360;
                            rigidBody2D.velocity = new Vector2(Mathf.Cos(cardinalizeDirections(targetAngle) * Mathf.Deg2Rad), Mathf.Sin(cardinalizeDirections(targetAngle) * Mathf.Deg2Rad)) * speed;
                        }
                    }
                }
                else
                {

                    float angleToShip = (360 + Mathf.Atan2(targetEnemy.transform.position.y - transform.position.y, targetEnemy.transform.position.x - transform.position.x) * Mathf.Rad2Deg) % 360;
                    pickView(angleToShip);
                    if (Vector2.Distance(targetEnemy.transform.position, transform.position) < 1.7f && attacking == false)
                    {
                        StartCoroutine(attack());
                    }
                    else
                    {
                        if (attacking == false)
                        {

                            pickIdleAnimation();
                            rigidBody2D.velocity = (targetEnemy.transform.position - transform.position).normalized * speed;
                        }
                    }
                }

                duration += Time.deltaTime;
            }
            else
            {
                if (duration != 256)
                {
                    duration = 256;
                    animator.SetTrigger("Explode");
                    Destroy(this.gameObject, 0.667f);
                    StopAllCoroutines();
                    foreach (PolygonCollider2D col in lanceHitBox.GetComponents<PolygonCollider2D>())
                    {
                        col.enabled = false;
                    }
                }

                rigidBody2D.velocity = Vector3.zero;
            }
        }
    }
}
