using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElderFrostMageActivateBlast : MonoBehaviour
{
    public List<GameObject> pillarsToActivate;
    public float speed = 30;

    private void Start()
    {
    }

    void Update()
    {
        if (pillarsToActivate.Count > 0)
        {
            if(Vector2.Distance(transform.position, pillarsToActivate[0].transform.position) > 0.5f)
            {
                Vector3 dirVector = new Vector3(pillarsToActivate[0].transform.position.x - transform.position.x, pillarsToActivate[0].transform.position.y - transform.position.y).normalized;
                float angleOrientation = Mathf.Atan2(dirVector.y, dirVector.x) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.Euler(0, 0, angleOrientation + 90);
                transform.position += dirVector * Time.deltaTime * speed;
            }
            else
            {
                pillarsToActivate[0].GetComponent<ElderFrostMageIcePillar>().explodeIcePillar = true;
                pillarsToActivate.Remove(pillarsToActivate[0]);
            }
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
}
