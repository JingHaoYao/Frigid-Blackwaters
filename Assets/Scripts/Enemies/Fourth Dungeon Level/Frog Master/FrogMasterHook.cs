using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrogMasterHook : MonoBehaviour
{
    private FrogMaster bossObject;
    private float angleTravel;

    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] Sprite closedSprite;
    [SerializeField] float speed;
    [SerializeField] LineRenderer lineRenderer;
    [SerializeField] ProjectileParent projectileParent;
    Coroutine throwHookRoutine;
    Camera mainCamera;
    bool closed = false;
    private Vector3 handPosition;
    Coroutine throwBack;

    public void Initialize(float angleTravel, FrogMaster master, Vector3 handPosition)
    {
        this.bossObject = master;
        this.angleTravel = angleTravel;
        this.projectileParent.instantiater = master.gameObject;
        LeanTween.value(20, 8, 1.5f).setOnUpdate((float val) => { speed = val; }).setEaseOutCirc();
        this.handPosition = handPosition;
    }

    private void Start()
    {
        mainCamera = Camera.main;
        transform.rotation = Quaternion.Euler(0, 0, angleTravel * Mathf.Rad2Deg);
        throwHookRoutine = StartCoroutine(throwHook());

        if(angleTravel > Mathf.PI)
        {
            lineRenderer.sortingOrder = bossObject.GetComponent<SpriteRenderer>().sortingOrder + 5;
        }
        else
        {
            lineRenderer.sortingOrder = bossObject.GetComponent<SpriteRenderer>().sortingOrder - 5;
        }
    }

    bool CheckOutOfBoundsRange()
    {
        return Mathf.Abs(mainCamera.transform.position.x - transform.position.x) > 8 || Mathf.Abs(mainCamera.transform.position.y - transform.position.y) > 8;
    }

    IEnumerator throwHook()
    {
        while(Vector2.Distance(transform.position, handPosition) < 8 && !CheckOutOfBoundsRange())
        {
            lineRenderer.SetPosition(0, transform.position);
            lineRenderer.SetPosition(1, handPosition);
            transform.position += new Vector3(Mathf.Cos(angleTravel), Mathf.Sin(angleTravel)) * speed * Time.deltaTime;
            yield return null;
        }

        if (throwBack == null)
        {
            throwBack = StartCoroutine(returnHook());
        }
    }

    IEnumerator returnHook()
    {
        LeanTween.value(8, 20, 0.75f).setOnUpdate((float val) => { speed = val; }).setEaseOutCirc();
        angleTravel += Mathf.PI;
        bossObject.TriggerThrowback();
        while (Vector2.Distance(transform.position, handPosition) > 1)
        {
            if (closed)
            {
                PlayerProperties.playerShip.transform.position += new Vector3(Mathf.Cos(angleTravel), Mathf.Sin(angleTravel)) * speed * Time.deltaTime;
            }

            lineRenderer.SetPosition(0, transform.position);
            lineRenderer.SetPosition(1, handPosition);
            transform.position += (this.handPosition - transform.position).normalized * speed * Time.deltaTime;
            yield return null;
        }

        if (closed)
        {
            PlayerProperties.playerScript.removeRootingObject();
        }
        Destroy(this.gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "playerHitBox" && !PlayerProperties.playerScript.isShipRooted())
        {
            StopCoroutine(throwHookRoutine);
            spriteRenderer.sprite = closedSprite;
            PlayerProperties.playerScript.addRootingObject();
            closed = true;
            if (throwBack == null)
            {
                throwBack = StartCoroutine(returnHook());
            }
        }
    }
}
