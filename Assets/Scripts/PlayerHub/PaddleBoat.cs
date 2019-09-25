using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaddleBoat : MonoBehaviour
{
    public Vector3 start, end;
    bool movingUp = false;
    private float foamTimer = 0;
    public GameObject waterFoam;
    Rigidbody2D rigidBody2D;

    void spawnFoam()
    {
        if (rigidBody2D.velocity.magnitude != 0)
        {
            float whatAngle = Mathf.Atan2(rigidBody2D.velocity.y, rigidBody2D.velocity.x) * Mathf.Rad2Deg;
            foamTimer += Time.deltaTime;
            if (foamTimer >= 0.5f)
            {
                foamTimer = 0;
                Instantiate(waterFoam, transform.position + new Vector3(0, -0.5f, 0), Quaternion.Euler(0, 0, whatAngle + 90));
            }
        }
    }

    private void Start()
    {
        rigidBody2D = GetComponent<Rigidbody2D>();
        transform.position = start;
    }

    void Update()
    {
        spawnFoam();
        if(movingUp == false)
        {
            rigidBody2D.velocity = Vector2.down;
            if(Vector2.Distance(end, transform.position) < 0.2f)
            {
                movingUp = true;
            }
        }
        else
        {
            rigidBody2D.velocity = Vector2.up;
            if (Vector2.Distance(start, transform.position) < 0.2f)
            {
                movingUp = false;
            }
        }
    }
}
