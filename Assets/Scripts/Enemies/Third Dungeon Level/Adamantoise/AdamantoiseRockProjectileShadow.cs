using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdamantoiseRockProjectileShadow : MonoBehaviour
{
    public GameObject[] rockProjectiles;
    [SerializeField] Collider2D damageCollider;
    public GameObject splash;

    private void Start()
    {
        startRockProcedure();
    }

    void pickRendererLayer(SpriteRenderer rend)
    {
        rend.sortingOrder = (200 - (int)(transform.position.y * 10));
    }

    void startRockProcedure()
    {
        GameObject rock = Instantiate(rockProjectiles[Random.Range(0, rockProjectiles.Length)], transform.position + Vector3.up * 21, Quaternion.Euler(0, 0, Random.Range(0, 360)));
        pickRendererLayer(rock.GetComponent<SpriteRenderer>());
        LeanTween.moveY(rock, transform.position.y, 1f).setEaseInQuad();
        LeanTween.rotateZ(rock, rock.transform.rotation.eulerAngles.z + 270, 1f);
        LeanTween.scale(this.gameObject, new Vector3(0.22f, 0.22f), 1f).setOnComplete(() => { Destroy(this.gameObject); Destroy(rock); Instantiate(splash, transform.position, Quaternion.identity); });
        LeanTween.value(0, 1, 1).setOnUpdate((float val) => { turnOnCollider(val); });
    }

    void turnOnCollider(float val)
    {
        if(val >= 0.8f)
        {
            damageCollider.enabled = true;
        }
    }

}
