using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrogLanternFireProjectile : MonoBehaviour
{
    public float speed;
    bool impacted = false;
    GameObject playerShip;
    public float angleTravel;
    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] LightAuraController auraController;

    void Start()
    {
        spriteRenderer.color = new Color(1, 1, 1, 0);
        StartCoroutine(fadeIn());
        auraController.fadeInLights();
        playerShip = PlayerProperties.playerShip;
    }

    IEnumerator fadeIn()
    {
        while(spriteRenderer.color.a < 1)
        {
            spriteRenderer.color = new Color(1, 1, 1, spriteRenderer.color.a + Time.deltaTime * 4);
            yield return null;
        }
    }

    IEnumerator fadeOut()
    {
        while (spriteRenderer.color.a > 0)
        {
            spriteRenderer.color = new Color(1, 1, 1, spriteRenderer.color.a - Time.deltaTime * 2);
            yield return null;
        }
    }

    void Update()
    {
        if (impacted == false)
        {
            transform.rotation = Quaternion.Euler(0, 0, angleTravel + 90);
            transform.position += new Vector3(Mathf.Cos(angleTravel * Mathf.Deg2Rad), Mathf.Sin(angleTravel * Mathf.Deg2Rad)) * Time.deltaTime * speed;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (impacted == false && collision.gameObject.layer != 15)
        {
            impacted = true;
            StartCoroutine(fadeOut());
            auraController.fadeOutLights();
            this.GetComponent<Collider2D>().enabled = false;
        }
    }
}
