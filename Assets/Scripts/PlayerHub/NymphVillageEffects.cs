using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NymphVillageEffects : MonoBehaviour
{
    // Script is just used to add more life to the player hub of the fourth level
    // Attached to camera object

    [SerializeField] GameObject nymphShadow;

    private void Start()
    {
        StartCoroutine(spawnNymphs());
    }

    IEnumerator spawnNymphs()
    {
        while (true)
        {
            for (int k = 0; k < Random.Range(1, 3); k++)
            {
                Vector3 basePosition = transform.position + new Vector3(Random.Range(-8.0f, 8.0f), Random.Range(-8.0f, 8.0f));
                float angle = Mathf.Atan2(transform.position.y - basePosition.y, transform.position.x - basePosition.x) * Mathf.Rad2Deg + Random.Range(-45, 45);

                for (int i = 0; i < Random.Range(2, 6); i++)
                {
                    Vector3 from = basePosition + new Vector3(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f));
                    Vector3 to = from + new Vector3(Mathf.Cos(angle), Mathf.Sin(angle)) * Random.Range(7.0f, 8.0f);
                    GameObject nymph = Instantiate(nymphShadow, transform.position, Quaternion.identity);
                    nymph.GetComponent<NymphShadow>().Initialize(from, to);
                }
            }
            yield return new WaitForSeconds(Random.Range(2f, 5f));
        }
    }
}
