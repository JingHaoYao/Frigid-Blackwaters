using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GreaterPheonixPlumeFeather : MonoBehaviour
{
    Vector3 centerPosition;
    float offset;

    public void Initialize(float initialAngleTravel)
    {
        StartCoroutine(projectileProcedure(initialAngleTravel));
        centerPosition = Camera.main.transform.position;
        offset = Camera.main.orthographicSize;
    }

    IEnumerator projectileProcedure(float initialAngleTravel)
    {
        transform.rotation = Quaternion.Euler(0, 0, initialAngleTravel);
        Vector3 endPosition = transform.position + new Vector3(Mathf.Cos(initialAngleTravel * Mathf.Deg2Rad), Mathf.Sin(initialAngleTravel * Mathf.Deg2Rad)) * 2;
        LeanTween.move(this.gameObject, endPosition, 0.75f).setEaseOutQuad();

        yield return new WaitForSeconds(0.75f);

        float angleToCursor = Mathf.Atan2(PlayerProperties.cursorPosition.y - transform.position.y, PlayerProperties.cursorPosition.x - transform.position.x) * Mathf.Rad2Deg;

        LeanTween.rotate(this.gameObject, new Vector3(0, 0, angleToCursor), 0.25f);

        yield return new WaitForSeconds(0.25f);

        float period = 0;

        while (true)
        {
            if (period < 0.5f)
            {
                period += Time.deltaTime;
                angleToCursor = Mathf.Atan2(PlayerProperties.cursorPosition.y - transform.position.y, PlayerProperties.cursorPosition.x - transform.position.x) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.Euler(0, 0, angleToCursor);
            }
            transform.position += new Vector3(Mathf.Cos(angleToCursor * Mathf.Deg2Rad), Mathf.Sin(angleToCursor * Mathf.Deg2Rad)) * Time.deltaTime * 14;

            if (Vector2.Distance(transform.position, centerPosition) > Mathf.Sqrt(2 * Mathf.Pow(offset, 2)) + 5)
            {
                Destroy(this.gameObject);
                break;
            }
            yield return null;
        }
    }
}
