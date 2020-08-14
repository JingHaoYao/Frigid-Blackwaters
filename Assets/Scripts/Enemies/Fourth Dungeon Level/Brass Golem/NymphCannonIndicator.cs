using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NymphCannonIndicator : MonoBehaviour
{
    [SerializeField] SpriteRenderer spriteRenderer;
    public void StartFollowCannon(Vector3 position)
    {
        this.gameObject.SetActive(true);
        StartCoroutine(pulseLoop());
        StartCoroutine(followProcedure(position));
    }

    public void StopFollowCannon()
    {
        StopAllCoroutines();
        this.gameObject.SetActive(false);
    }

    IEnumerator pulseLoop()
    {
        spriteRenderer.color = Color.white;
        while (true)
        {
            LeanTween.alpha(this.gameObject, 0, 0.5f);
            yield return new WaitForSeconds(0.5f);
            LeanTween.alpha(this.gameObject, 1, 0.5f);
            yield return new WaitForSeconds(0.5f);
        }
    }

    IEnumerator followProcedure(Vector3 positionToFollow)
    {
        while (true)
        {
            if (Vector2.Distance(positionToFollow, PlayerProperties.playerShipPosition) > 20)
            {
                float angleToPosition = Mathf.Atan2(positionToFollow.y - transform.position.y, positionToFollow.x - transform.position.x) * Mathf.Rad2Deg;

                transform.position = PlayerProperties.playerShipPosition + new Vector3(Mathf.Cos(angleToPosition * Mathf.Deg2Rad), Mathf.Sin(angleToPosition * Mathf.Deg2Rad)) * 8;
                transform.rotation = Quaternion.Euler(0, 0, angleToPosition + 180);
            }
            else
            {
                transform.position = positionToFollow + Vector3.up * 2;
                transform.rotation = Quaternion.Euler(0, 0, 90);
            }

            yield return null;
        }
    }
}
