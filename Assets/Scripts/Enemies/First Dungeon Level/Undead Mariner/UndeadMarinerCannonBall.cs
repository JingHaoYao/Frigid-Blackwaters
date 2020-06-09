using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UndeadMarinerCannonBall : MonoBehaviour
{
    public float speed = 20;
    public GameObject wallSlamParticles, smokePlume;
    public float angleTravel;
    public GameObject bulletTrail;
    GameObject playerShip;

    void Start()
    {
        playerShip = GameObject.Find("PlayerShip");
    }

    void Update()
    {
        transform.position += Time.deltaTime * speed * new Vector3(Mathf.Cos(angleTravel), Mathf.Sin(angleTravel), 0);
        Instantiate(bulletTrail, transform.position, Quaternion.Euler(0, 0, angleTravel * Mathf.Rad2Deg + 90));
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "playerHitBox")
        {
            PlayerScript playerScript = playerShip.GetComponent<PlayerScript>();
            playerScript.dealDamageToShip(350, this.gameObject);
            playerScript.enemyMomentumVector = new Vector3(Mathf.Cos(angleTravel), Mathf.Sin(angleTravel)) * 8;
            playerScript.enemyMomentumMagnitude = 8;
            playerScript.enemyMomentumDuration = 1f;
            Destroy(this.gameObject);
        }

        if (collision.gameObject.tag == "RoomHitbox" || collision.gameObject.tag == "RoomWall")
        {
            Instantiate(wallSlamParticles, transform.position, Quaternion.Euler(0, 0, (angleTravel * Mathf.Rad2Deg) + 90));
            Instantiate(smokePlume, transform.position, Quaternion.Euler(0, 0, (angleTravel * Mathf.Rad2Deg) + 90));
            Destroy(this.gameObject);
        }
    }
}
