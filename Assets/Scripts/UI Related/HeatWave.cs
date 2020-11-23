using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeatWave : MonoBehaviour
{
    private float rate = 1;
    private void OnEnable()
    {

        rate = Random.Range(0.75f, 1.5f);
        transform.localScale = new Vector3(0.01f, 0.01f, 0.01f);
    }

    void Update()
    {
        transform.localScale += new Vector3(0.01f, 0.01f, 0.01f) * rate;

        if (transform.localScale.x > 10f)
        {
            this.gameObject.SetActive(false);
        }
    }

}
