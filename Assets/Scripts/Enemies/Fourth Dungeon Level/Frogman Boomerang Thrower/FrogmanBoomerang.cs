using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrogmanBoomerang : MonoBehaviour
{
    [SerializeField] PolygonCollider2D polyCol;
    Vector3 targetLocation;
    [SerializeField] float speed;
    GameObject frogmanBoomeranger;
    bool stopTravelling = false;

    public float Initialize(Vector3 targetLocation, GameObject frogmanBoomeranger, bool shouldFadeIn = false)
    {
        if (shouldFadeIn)
        {
            GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0);
            LeanTween.alpha(this.gameObject, 1, 0.3f);
        }

        this.frogmanBoomeranger = frogmanBoomeranger;
        this.targetLocation = targetLocation;
        float time = Vector2.Distance(targetLocation, transform.position) / speed;
        LeanTween.move(this.gameObject, targetLocation, time).setEaseInOutQuad().setOnComplete(() => returnProcedure(time));
        return time * 2;
    }

    void returnProcedure(float time)
    {
        if (frogmanBoomeranger != null)
        {
            LeanTween.move(this.gameObject, frogmanBoomeranger.transform.position + Vector3.up * 0.5f + (targetLocation - frogmanBoomeranger.transform.position).normalized * 0.25f, time).setEaseInOutQuad().setOnComplete(() => { LeanTween.alpha(this.gameObject, 0, 0.3f).setOnComplete(() => Destroy(this.gameObject)); stopTravelling = true; polyCol.enabled = false; });
        }
        else
        {
            LeanTween.alpha(this.gameObject, 0, 0.3f).setOnComplete(() => Destroy(this.gameObject));
            stopTravelling = true;
            polyCol.enabled = false;
        }
    }


    private void Update()
    {
        if (stopTravelling == false) {
            LeanTween.rotateZ(this.gameObject, transform.rotation.eulerAngles.z + 270, 0.1f);
        }
    }
}
