using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstBossFinalDeath : MonoBehaviour
{
    public GameObject deadHead;
    IEnumerator spawnHead()
    {
        yield return new WaitForSeconds(1.583f / 0.667f);
        Instantiate(deadHead, transform.position + new Vector3(0, 3.08f, 0), Quaternion.identity);
        FindObjectOfType<AudioManager>().PlaySound("First Boss Defeated Music");
        FindObjectOfType<AudioManager>().FadeIn("First Boss Defeated Music", 0.1f, 0.5f);
        GameObject[] swordMen = GameObject.FindGameObjectsWithTag("RangedEnemy");
        foreach (GameObject enemy in swordMen)
        {
            Destroy(enemy);
        }
        Destroy(this.gameObject);
    }

    private void Start()
    {
        StartCoroutine(spawnHead());
        StartCoroutine(fadeOut(this.GetComponent<AudioSource>(), 0.0417f, 0.1f));
        GameObject[] swordMen = GameObject.FindGameObjectsWithTag("RangedEnemy");
        foreach (GameObject enemy in swordMen)
        {
            Destroy(enemy);
        }
    }

    IEnumerator fadeOut(AudioSource source, float speed, float wait)
    {
        while (source.volume > 0)
        {
            source.volume -= speed;
            yield return new WaitForSeconds(wait);
        }
    }
}
