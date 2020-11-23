using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GreaterPheonixGlideProjectile : MonoBehaviour
{
    [SerializeField] ProjectileParent projectileParent;
    
    public void Initialize(Vector3 startPosition, Vector3 endPosition, GameObject bossEnemy)
    {
        projectileParent.instantiater = bossEnemy;
        transform.position = startPosition;
        transform.rotation = Quaternion.Euler(0, 0, Mathf.Atan2(endPosition.y - startPosition.y, endPosition.x - startPosition.x) * Mathf.Rad2Deg);
        LeanTween.move(this.gameObject, endPosition, Vector2.Distance(startPosition, endPosition) / 12f).setEaseInQuad().setOnComplete(() => Destroy(this.gameObject));
    }
}
