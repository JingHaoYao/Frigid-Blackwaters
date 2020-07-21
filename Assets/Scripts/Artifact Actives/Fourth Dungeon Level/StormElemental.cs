using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StormElemental : MonoBehaviour
{
    List<Vector3> shipPositions = new List<Vector3>();
    float previousTravelAngle;
    [SerializeField] float speed;
    [SerializeField] CircleCollider2D circCol;
    [SerializeField] Animator animator;
    Coroutine damageRoutine;

    IEnumerator damageProcedure()
    {
        while (true)
        {
            circCol.enabled = false;
            yield return new WaitForSeconds(0.2f);
            circCol.enabled = true;
            yield return new WaitForSeconds(0.2f);
        }
    }

    private void Start()
    {
        previousTravelAngle = currentTravelAngle();
        damageRoutine = StartCoroutine(damageProcedure());
        shipPositions.Clear();
    }

    float currentTravelAngle()
    {
        return Mathf.Atan2(PlayerProperties.shipTravellingVector.y, PlayerProperties.shipTravellingVector.x);
    }

    void Update()
    {
        if (Mathf.Abs(previousTravelAngle - currentTravelAngle()) > 0.001f)
        {
            shipPositions.Add(PlayerProperties.playerShipPosition);
            previousTravelAngle = currentTravelAngle();
        }

        if (shipPositions.Count > 0)
        {
            Vector3 velocityVector = (shipPositions[0] - transform.position).normalized * speed;

            transform.position += velocityVector * Time.deltaTime;

            if (Vector2.Distance(transform.position, shipPositions[0]) < 0.2f)
            {
                shipPositions.Remove(shipPositions[0]);
            }
        }
        else
        {
            if (Vector2.Distance(transform.position, PlayerProperties.playerShipPosition) > 2f)
            {
                Vector3 velocityVector = (PlayerProperties.playerShipPosition - transform.position).normalized * speed;

                transform.position += velocityVector * Time.deltaTime;
            }
        }
    }

    public void removeTornado()
    {
        StopCoroutine(damageRoutine);
        animator.SetTrigger("Death");
        Destroy(this.gameObject, 0.667f);
    }
}
