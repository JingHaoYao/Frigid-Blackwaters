using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TectonicChanneler : Enemy
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Rigidbody2D rigidBody2D;
    [SerializeField] private Animator animator;
    private float travelAngle;
    List<AStarNode> path;
    [SerializeField] private AudioSource damageAudio;
    [SerializeField] private AudioSource chargeAudio;
    [SerializeField] private AudioSource channelingLoopAudio;
    [SerializeField] private AudioSource flameAudio;
    [SerializeField] private GameObject tectonicChannelerDeath;
    [SerializeField] AStarPathfinding aStarPathfinding;
    [SerializeField] GameObject flameProjectile;
    Vector3 randomPos;
    bool isAttacking = false;
    int prevView = -1;
    int whatView = 1;
    int mirror = 1;
    [SerializeField] GameObject pyrotheumProjectile;
    float attackPeriod = 0;
    Vector3 targetPosition;
    Camera mainCamera;

    void pickIdleAnimation(float angleToFace)
    {
        pickView(angleToFace);
        if (prevView != whatView)
        {
            prevView = whatView;
            animator.SetTrigger("Idle" + whatView);
        }
    }

    void pickRendererLayer()
    {
        spriteRenderer.sortingOrder = 200 - (int)(transform.position.y * 10);
    }

    void moveTowards(float direction)
    {
        rigidBody2D.velocity = new Vector3(Mathf.Cos(direction * Mathf.Deg2Rad), Mathf.Sin(direction * Mathf.Deg2Rad), 0) * speed;
    }

    void pickView(float angleOrientation)
    {
        if (angleOrientation >= 0 && angleOrientation < 60)
        {
            whatView = 4;
            mirror = -1;
        }
        else if (angleOrientation >= 60 && angleOrientation < 120)
        {
            whatView = 3;
            mirror = 1;
        }
        else if (angleOrientation >= 120 && angleOrientation < 180)
        {
            whatView = 4;
            mirror = 1;
        }
        else if (angleOrientation >= 180 && angleOrientation < 240)
        {
            whatView = 2;
            mirror = -1;
        }
        else if (angleOrientation >= 240 && angleOrientation < 300)
        {
            whatView = 1;
            mirror = 1;
        }
        else
        {
            whatView = 2;
            mirror = 1;
        }
    }

    void moveTowards(float direction, float speedToTravel)
    {
        rigidBody2D.velocity = new Vector3(Mathf.Cos(direction * Mathf.Deg2Rad), Mathf.Sin(direction * Mathf.Deg2Rad), 0) * speedToTravel;
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

    void travelLocation()
    {
        path = aStarPathfinding.seekPath;
        aStarPathfinding.target = randomPos;
        Vector3 targetPos = Vector3.zero;

        if (path.Count > 0)
        {
            AStarNode pathNode = path[0];
            targetPos = pathNode.nodePosition;
        }

        float travelAngle = cardinalizeDirections((360 + Mathf.Atan2(targetPos.y - (transform.position.y + 0.4f), targetPos.x - transform.position.x) * Mathf.Rad2Deg) % 360);

        if (path != null && path.Count > 0 && Vector2.Distance(path[path.Count - 1].nodePosition, transform.position) > 0.5f)
        {
            moveTowards(travelAngle);
            pickIdleAnimation(travelAngle);
        }
        else
        {
            rigidBody2D.velocity = Vector3.zero;
            if (isAttacking == false && Vector2.Distance(transform.position, randomPos) < 1f && stopAttacking == false)
            {
                StartCoroutine(ChannelAndRelease());
            }
        }
    }

    IEnumerator ChannelAndRelease()
    {
        animator.SetTrigger("CallRise");
        isAttacking = true;
        yield return new WaitForSeconds(7 / 12f);
        channelingLoopAudio.Play();
        if (stopAttacking == false)
        {
            for (int i = 0; i < 4; i++)
            {
                GameObject projectileInstant = Instantiate(pyrotheumProjectile, new Vector3(mainCamera.transform.position.x - 8, mainCamera.transform.position.y + 1.25f), Quaternion.identity);
                projectileInstant.GetComponent<WallSpawnedPyrotheumProjectile>().Initialize(this.gameObject, transform.position + Vector3.up * 1.25f);
                projectileInstant = Instantiate(pyrotheumProjectile, new Vector3(mainCamera.transform.position.x + 8, mainCamera.transform.position.y + 1.25f), Quaternion.identity);
                projectileInstant.GetComponent<WallSpawnedPyrotheumProjectile>().Initialize(this.gameObject, transform.position + Vector3.up * 1.25f);
                projectileInstant = Instantiate(pyrotheumProjectile, new Vector3(mainCamera.transform.position.x, mainCamera.transform.position.y + 8), Quaternion.identity);
                projectileInstant.GetComponent<WallSpawnedPyrotheumProjectile>().Initialize(this.gameObject, transform.position + Vector3.up * 1.25f);
                projectileInstant = Instantiate(pyrotheumProjectile, new Vector3(mainCamera.transform.position.x, mainCamera.transform.position.y - 8), Quaternion.identity);
                projectileInstant.GetComponent<WallSpawnedPyrotheumProjectile>().Initialize(this.gameObject, transform.position + Vector3.up * 1.25f);
                projectileInstant = Instantiate(pyrotheumProjectile, new Vector3(mainCamera.transform.position.x - 8, mainCamera.transform.position.y + 8), Quaternion.identity);
                projectileInstant.GetComponent<WallSpawnedPyrotheumProjectile>().Initialize(this.gameObject, transform.position + Vector3.up * 1.25f);
                projectileInstant = Instantiate(pyrotheumProjectile, new Vector3(mainCamera.transform.position.x + 8, mainCamera.transform.position.y + 8), Quaternion.identity);
                projectileInstant.GetComponent<WallSpawnedPyrotheumProjectile>().Initialize(this.gameObject, transform.position + Vector3.up * 1.25f);
                projectileInstant = Instantiate(pyrotheumProjectile, new Vector3(mainCamera.transform.position.x + 8, mainCamera.transform.position.y - 8), Quaternion.identity);
                projectileInstant.GetComponent<WallSpawnedPyrotheumProjectile>().Initialize(this.gameObject, transform.position + Vector3.up * 1.25f);
                projectileInstant = Instantiate(pyrotheumProjectile, new Vector3(mainCamera.transform.position.x - 8, mainCamera.transform.position.y - 8), Quaternion.identity);
                projectileInstant.GetComponent<WallSpawnedPyrotheumProjectile>().Initialize(this.gameObject, transform.position + Vector3.up * 1.25f);
                yield return new WaitForSeconds(1.5f);
            }
        }
        flameAudio.Play();
        animator.SetTrigger("FlameDrop");
        channelingLoopAudio.Stop();
        yield return new WaitForSeconds(7 / 12f);

        if (stopAttacking == false)
        {
            for (int i = 0; i < 6; i++)
            {
                float angle = i * 60;
                GameObject fireBallInstant = Instantiate(flameProjectile, transform.position, Quaternion.identity);
                fireBallInstant.GetComponent<BasicProjectile>().angleTravel = angle;
                fireBallInstant.GetComponent<ProjectileParent>().instantiater = this.gameObject;
            }
        }

        yield return new WaitForSeconds(4 / 12f);

        if (stopAttacking == false)
        {
            for (int i = 0; i < 6; i++)
            {
                float angle = i * 60 + 30;
                GameObject fireBallInstant = Instantiate(flameProjectile, transform.position, Quaternion.identity);
                fireBallInstant.GetComponent<BasicProjectile>().angleTravel = angle;
                fireBallInstant.GetComponent<ProjectileParent>().instantiater = this.gameObject;
            }
        }

        yield return new WaitForSeconds(4 / 12f);
        isAttacking = false;
        randomPos = pickRandPos();
    }

    private void Start()
    {
        StartCoroutine(mainLoop());
        mainCamera = Camera.main;
        randomPos = pickRandPos();
    }

    Vector3 pickRandPos()
    {
        float randX;
        float randY;
        if (Random.Range(0, 2) == 1)
        {
            randX = transform.position.x + Random.Range(4.0f, 5.0f);
            if (Random.Range(0, 2) == 1)
            {
                randY = transform.position.y + Random.Range(4.0f, 5.0f);
            }
            else
            {
                randY = transform.position.y + Random.Range(-5.0f, -4.0f);
            }
        }
        else
        {
            randX = transform.position.x + Random.Range(-5.0f, -4.0f);
            if (Random.Range(0, 2) == 1)
            {
                randY = transform.position.y + Random.Range(4.0f, 5.0f);
            }
            else
            {
                randY = transform.position.y + Random.Range(-5.0f, -4.0f);
            }
        }

        Vector3 randPos = new Vector3(Mathf.Clamp(randX, Camera.main.transform.position.x - 7, Camera.main.transform.position.x + 7), Mathf.Clamp(randY, Camera.main.transform.position.y - 7, Camera.main.transform.position.y + 7), 0);
        while (Physics2D.OverlapCircle(randPos, .5f) || Vector2.Distance(randPos, transform.position) < 2)
        {
            if (Random.Range(0, 2) == 1)
            {
                randX = transform.position.x + Random.Range(4.0f, 5.0f);
                if (Random.Range(0, 2) == 1)
                {
                    randY = transform.position.y + Random.Range(4.0f, 5.0f);
                }
                else
                {
                    randY = transform.position.y + Random.Range(-5.0f, -4.0f);
                }
            }
            else
            {
                randX = transform.position.x + Random.Range(-5.0f, -4.0f);
                if (Random.Range(0, 2) == 1)
                {
                    randY = transform.position.y + Random.Range(4.0f, 5.0f);
                }
                else
                {
                    randY = transform.position.y + Random.Range(-5.0f, -4.0f);
                }
            }
            randPos = new Vector3(Mathf.Clamp(randX, Camera.main.transform.position.x - 7, Camera.main.transform.position.x + 7), Mathf.Clamp(randY, Camera.main.transform.position.y - 7, Camera.main.transform.position.y + 7), 0);
        }
        return randPos;
    }

    IEnumerator mainLoop()
    {
        while (true)
        {
            travelLocation();
            yield return null;
        }
    }

    private float angleToShip()
    {
        return (360 + Mathf.Atan2(PlayerProperties.playerShipPosition.y - transform.position.y, PlayerProperties.playerShipPosition.x - transform.position.x) * Mathf.Rad2Deg) % 360;
    }

    IEnumerator hitFrame()
    {
        spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(.1f);
        spriteRenderer.color = Color.white;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<DamageAmount>())
        {
            dealDamage(collision.gameObject.GetComponent<DamageAmount>().damage);
        }
    }

    public override void deathProcedure()
    {
        Instantiate(tectonicChannelerDeath, transform.position, Quaternion.identity);
        Destroy(this.gameObject);
    }

    public override void damageProcedure(int damage)
    {
        StartCoroutine(hitFrame());
        damageAudio.Play();
    }
}
