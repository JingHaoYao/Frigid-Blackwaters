using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireFighterProjectile : MonoBehaviour
{
    public float destroyTime;
    public string breakString;
    public float speed;

    Animator animator;
    bool impacted = false;
    GameObject playerShip;
    // In degrees
    public float angleTravel;
    [SerializeField] float rotationOffset;
    [SerializeField] float radiusBurn = 1;
    [SerializeField] Collider2D collider;
    [SerializeField] AudioSource audioSource;
    [SerializeField] ProjectileParent projectileParent;
    float period = 0;
    float offSetDistance = 0;

    void Start()
    {
        animator = GetComponent<Animator>();
        playerShip = PlayerProperties.playerShip;
    }

    public void Initialize(float angleTravel, GameObject instantiater, float periodOffset)
    {
        projectileParent.instantiater = instantiater;
        this.angleTravel = angleTravel;
        period += periodOffset;
    }

    void Update()
    {
        if (impacted == false)
        {
            offSetDistance = Mathf.Sin(period) * 1.5f;
            period += Time.deltaTime * 3;
            Vector3 travelVector = (new Vector3(Mathf.Cos(angleTravel * Mathf.Deg2Rad), Mathf.Sin(angleTravel * Mathf.Deg2Rad)) + new Vector3(Mathf.Cos((angleTravel + 90) * Mathf.Deg2Rad), Mathf.Sin((angleTravel + 90) * Mathf.Deg2Rad)) * offSetDistance);
            transform.position += travelVector * Time.deltaTime * speed;
            float rotationAngle = Mathf.Atan2(travelVector.y, travelVector.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, rotationAngle + rotationOffset);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (impacted == false && collision.gameObject.layer != 15)
        {
            impacted = true;
            animator.SetTrigger(breakString);

            if (collision.gameObject.layer == 12)
            {
                EnemyPool.floorFireSpawner.SpawnFloorFires(transform.position, radiusBurn);
            }
  
            this.audioSource.Play();
            Destroy(this.gameObject, destroyTime);
            this.collider.enabled = false;
        }
    }
}
