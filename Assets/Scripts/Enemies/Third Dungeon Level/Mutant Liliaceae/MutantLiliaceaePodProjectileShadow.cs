using UnityEngine;

public class MutantLiliaceaePodProjectileShadow : MonoBehaviour
{
    public GameObject pod;
    [SerializeField] Collider2D damageCollider;
    public GameObject splash;
    [SerializeField] LayerMask playerLayerMask;

    bool isUnderMushroom()
    {
        Collider2D colliders = Physics2D.OverlapCircle(transform.position, 0.5f, layerMask: playerLayerMask);
        if (colliders != null && (colliders.gameObject.name.Contains("Blue Mushroom Guard") || colliders.gameObject.name.Contains("Green Mushroom Guard") || colliders.gameObject.name.Contains("Red Mushroom Guard")))
        {
            return true;
        }
        return false;
    }

    private void Start()
    {
        if (isUnderMushroom())
        {
            startBlockedRockProcedure();
        }
        else
        {
            startRockProcedure();
        }
    }

    void pickRendererLayer(SpriteRenderer rend)
    {
        rend.sortingOrder = (200 - (int)(transform.position.y * 10));
    }

    void startRockProcedure()
    {
        GameObject rock = Instantiate(pod, transform.position + Vector3.up * 21, Quaternion.Euler(0, 0, Random.Range(0, 360)));
        pickRendererLayer(rock.GetComponent<SpriteRenderer>());
        LeanTween.moveY(rock, transform.position.y, 1f).setEaseInQuad();
        LeanTween.rotateZ(rock, rock.transform.rotation.eulerAngles.z + 270, 1f);
        LeanTween.scale(this.gameObject, new Vector3(0.22f, 0.22f), 1f).setOnComplete(() => { Destroy(this.gameObject); Destroy(rock); Instantiate(splash, transform.position, Quaternion.identity); });
        LeanTween.value(0, 1, 1).setOnUpdate((float val) => { turnOnCollider(val); });
    }

    void startBlockedRockProcedure()
    {
        GameObject rock = Instantiate(pod, transform.position + Vector3.up * 21, Quaternion.Euler(0, 0, Random.Range(0, 360)));
        pickRendererLayer(rock.GetComponent<SpriteRenderer>());
        LeanTween.moveY(rock, transform.position.y + 3.5f, 0.75f).setEaseInQuad();
        LeanTween.rotateZ(rock, rock.transform.rotation.eulerAngles.z + 270, 1f);
        LeanTween.scale(this.gameObject, new Vector3(0.15f, 0.15f), 0.75f).setOnComplete(() => { Destroy(this.gameObject); rock.GetComponent<MutantLiliaceaeFallingPodProjectile>().shatterPod(); });
    }

    void turnOnCollider(float val)
    {
        if (val >= 0.8f)
        {
            damageCollider.enabled = true;
        }
    }
}
