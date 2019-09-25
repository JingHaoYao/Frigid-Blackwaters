using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClamEnemy : Enemy
{
    Animator animator;
    SpriteRenderer spriteRenderer;
    BoxCollider2D enemyCol;
    public Sprite[] spriteList;
    int whatView, mirror;
    float rotatePeriod = 0;
    float currentAngleOrientation = 0;
    float[] viableAngles;
    public GameObject clamShot, deadClam;
    public GameObject solidHitBox;
    int angleIndex = 0;
    bool cw = false;
    Vector3 randomPos;
    Rigidbody2D rigidBody2D;
    List<AStarNode> path;
    public float travelSpeed = 4;
    float travelAngle = 0;
    private float foamTimer = 0;
    public GameObject waterFoam;

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

    void pickView(float angle) {
        if (angle > 255 && angle <= 285)
        {
            whatView = 1;
            mirror = 1;
        }
        else if (angle > 285 && angle <= 360)
        {
            whatView = 2;
            mirror = 1;
        }
        else if (angle > 180 && angle <= 255)
        {
            whatView = 2;
            mirror = -1;
        }
        else if (angle > 75 && angle <= 105)
        {
            whatView = 4;
            mirror = 1;
        }
        else if (angle >= 0 && angle <= 75)
        {
            whatView = 3;
            mirror = 1;
        }
        else
        {
            whatView = 3;
            mirror = -1;
        }
    }

    Vector3 pickRandPos()
    {
        float randX;
        float randY;
        if (Random.Range(0, 2) == 1)
        {
            randX = transform.position.x + Random.Range(5.0f, 7.0f);
            if (Random.Range(0, 2) == 1)
            {
                randY = transform.position.y + Random.Range(5.0f, 7.0f);
            }
            else
            {
                randY = transform.position.y + Random.Range(-7.0f, -5.0f);
            }
        }
        else
        {
            randX = transform.position.x + Random.Range(-7.0f, -5.0f);
            if (Random.Range(0, 2) == 1)
            {
                randY = transform.position.y + Random.Range(5.0f, 7.0f);
            }
            else
            {
                randY = transform.position.y + Random.Range(-7.0f, -5.0f);
            }
        }
        Vector3 randPos = new Vector3(Mathf.Clamp(randX, Camera.main.transform.position.x - 7, Camera.main.transform.position.x + 7), Mathf.Clamp(randY, Camera.main.transform.position.y - 7, Camera.main.transform.position.y + 7), 0);

        while (Physics2D.OverlapCircle(randPos, .5f))
        {
            if (Random.Range(0, 2) == 1)
            {
                randX = transform.position.x + Random.Range(5.0f, 7.0f);
                if (Random.Range(0, 2) == 1)
                {
                    randY = transform.position.y + Random.Range(5.0f, 7.0f);
                }
                else
                {
                    randY = transform.position.y + Random.Range(-7.0f, -5.0f);
                }
            }
            else
            {
                randX = transform.position.x + Random.Range(-7.0f, -5.0f);
                if (Random.Range(0, 2) == 1)
                {
                    randY = transform.position.y + Random.Range(5.0f, 7.0f);
                }
                else
                {
                    randY = transform.position.y + Random.Range(-7.0f, -5.0f);
                }
            }
            randPos = new Vector3(Mathf.Clamp(randX, Camera.main.transform.position.x - 7, Camera.main.transform.position.x + 7), Mathf.Clamp(randY, Camera.main.transform.position.y - 7, Camera.main.transform.position.y + 7), 0);
        }
        return randPos;
    }

    void travelLocation()
    {
        path = GetComponent<AStarPathfinding>().seekPath;
        this.GetComponent<AStarPathfinding>().target = randomPos;
        AStarNode pathNode = path[0];
        Vector3 targetPos = pathNode.nodePosition;
        travelAngle = cardinalizeDirections((360 + Mathf.Atan2(targetPos.y - (transform.position.y + 0.4f), targetPos.x - transform.position.x) * Mathf.Rad2Deg) % 360);

        moveTowards(travelAngle);

        if (Vector2.Distance(transform.position, randomPos) < 1.5f)
        {
            randomPos = pickRandPos();
        }
    }

    void moveTowards(float direction)
    {
        rigidBody2D.velocity = new Vector3(Mathf.Cos(direction * Mathf.Deg2Rad), Mathf.Sin(direction * Mathf.Deg2Rad), 0) * travelSpeed;
    }

    void rotateView()
    {
        rotatePeriod += Time.deltaTime;
        if(rotatePeriod > 1.3f)
        {
            rotatePeriod = 0;
            if (cw == false)
            {
                angleIndex++;
                if (angleIndex > viableAngles.Length - 1)
                {
                    angleIndex = 0;
                }
            }
            else
            {
                angleIndex--;
                if (angleIndex < 0)
                {
                    angleIndex = viableAngles.Length - 1;
                }
            }
            pickView(viableAngles[angleIndex]);
            transform.localScale = new Vector3(0.08f * mirror, 0.08f);
            spriteRenderer.sprite = spriteList[whatView - 1];
            StartCoroutine(fireShot(whatView, viableAngles[angleIndex]));
        }
    }

    IEnumerator fireShot(int whatView, float angle)
    {
        animator.enabled = true;
        enemyCol.enabled = true;
        solidHitBox.SetActive(false);
        animator.SetTrigger("Attack" + whatView.ToString());
        this.GetComponents<AudioSource>()[1].Play();
        yield return new WaitForSeconds(3f / 8f);
        for (int i = 0; i < 3; i++)
        {
            GameObject shot = Instantiate(clamShot, transform.position + new Vector3(0, 0.4f), Quaternion.Euler(0, 0, Random.Range(0, 360)));
            shot.GetComponent<ClamProjectile>().angleTravel = ((angle - 5) + 5 * i) * Mathf.Deg2Rad;
            shot.GetComponent<ProjectileParent>().instantiater = this.gameObject;
        }
        yield return new WaitForSeconds(6f / 8f);
        animator.enabled = false;
        enemyCol.enabled = false;
        solidHitBox.SetActive(true);
    }

    void spawnFoam()
    {
        if (rigidBody2D.velocity.magnitude != 0)
        {
            float whatAngle = Mathf.Atan2(rigidBody2D.velocity.y, rigidBody2D.velocity.x) * Mathf.Rad2Deg;
            foamTimer += Time.deltaTime;
            if (foamTimer >= 0.05f * travelSpeed / 3f)
            {
                foamTimer = 0;
                Instantiate(waterFoam, transform.position, Quaternion.Euler(0, 0, whatAngle + 90));
            }
        }
    }

    void Start()
    {
        viableAngles = new float[8] { 0, 45, 90, 135, 180, 225, 270, 315 };
        animator = GetComponent<Animator>();
        rigidBody2D = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        enemyCol = GetComponent<BoxCollider2D>();
        randomPos = pickRandPos();
        path = GetComponent<AStarPathfinding>().seekPath;
        enemyCol.enabled = false;
        animator.enabled = false;
        if(Random.Range(0,2) == 0)
        {
            cw = true;
        }
        angleIndex = Random.Range(0, viableAngles.Length);
        currentAngleOrientation = viableAngles[angleIndex];
    }

    void Update()
    {
        if (stopAttacking == false)
        {
            rotateView();
        }
        travelLocation();
        spawnFoam();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<DamageAmount>() && health > 0)
        {
            int damageDealt = collision.gameObject.GetComponent<DamageAmount>().damage;
            health -= damageDealt;
            this.GetComponents<AudioSource>()[0].Play();
            if (health <= 0)
            {
                Instantiate(deadClam, transform.position, Quaternion.identity);
                addKills();
                Destroy(this.gameObject);
            }
            else
            {
                StartCoroutine(hitFrame());
            }
        }
    }

    IEnumerator hitFrame()
    {
        spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(.1f);
        spriteRenderer.color = Color.white;
    }
}
