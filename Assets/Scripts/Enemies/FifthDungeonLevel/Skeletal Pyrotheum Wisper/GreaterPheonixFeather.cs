using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GreaterPheonixFeather : MonoBehaviour
{
    bool impacted = false;
    [SerializeField] ProjectileParent projectileParent;
    Vector3 centerPosition;
    
    public void Initialize(float initialAngleTravel, GameObject instantiater, Vector3 centerPosition)
    {
        projectileParent.instantiater = instantiater;
        StartCoroutine(projectileProcedure(initialAngleTravel));
        this.centerPosition = centerPosition;
    }

    IEnumerator projectileProcedure(float initialAngleTravel)
    {
        transform.rotation = Quaternion.Euler(0, 0, initialAngleTravel);
        Vector3 endPosition = transform.position + new Vector3(Mathf.Cos(initialAngleTravel * Mathf.Deg2Rad), Mathf.Sin(initialAngleTravel * Mathf.Deg2Rad)) * 2;
        LeanTween.move(this.gameObject, endPosition, 0.75f).setEaseOutQuad();

        yield return new WaitForSeconds(0.75f);

        float angleToShip = Mathf.Atan2(PlayerProperties.playerShipPosition.y - transform.position.y, PlayerProperties.playerShipPosition.x - transform.position.x) * Mathf.Rad2Deg;

        LeanTween.rotate(this.gameObject, new Vector3(0, 0, angleToShip), 0.25f);

        yield return new WaitForSeconds(0.25f);

        while(true)
        {
            transform.position += new Vector3(Mathf.Cos(angleToShip * Mathf.Deg2Rad), Mathf.Sin(angleToShip * Mathf.Deg2Rad)) * Time.deltaTime * 14;
            if(Vector2.Distance(transform.position, centerPosition) > 15)
            {
                Destroy(this.gameObject);
                break;
            }
            yield return null;
        }
    }
}
