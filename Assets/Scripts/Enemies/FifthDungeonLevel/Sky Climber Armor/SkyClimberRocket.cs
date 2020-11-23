using System.Collections;
using UnityEngine;

public class SkyClimberRocket : MonoBehaviour
{
    [SerializeField] GameObject explosion;
    private GameObject instantiatingObject;
    [SerializeField] ParticleSystem rocketParticles;
    [SerializeField] SpriteRenderer spriteRenderer;

    public void Initialize(GameObject instantiatingObject, Vector3 targetPosition)
    {
        this.instantiatingObject = instantiatingObject;
        StartCoroutine(followProcedure(targetPosition));
    }

    IEnumerator followProcedure(Vector3 targetPosition)
    {
        float randomVelocityAngle = Random.Range(75, 105);
        Vector3 velocity = new Vector3(Mathf.Cos(randomVelocityAngle * Mathf.Deg2Rad), Mathf.Sin(randomVelocityAngle * Mathf.Deg2Rad)) * 8;
        float rateIncrease = 0;

        if(Random.Range(0, 2) == 1)
        {
            randomVelocityAngle -= 360;
        }

        while(Vector2.Distance(transform.position, targetPosition) > 0.25f)
        {
            float angleToPosition = Mathf.Atan2(targetPosition.y - transform.position.y, targetPosition.x - transform.position.x);
            velocity = (new Vector3(Mathf.Cos(randomVelocityAngle * Mathf.Deg2Rad), Mathf.Sin(randomVelocityAngle * Mathf.Deg2Rad)) + new Vector3(Mathf.Cos(angleToPosition), Mathf.Sin(angleToPosition)) * 2).normalized * 12;
            transform.position += velocity * Time.deltaTime;
         
            transform.rotation = Quaternion.Euler(0, 0, Mathf.Atan2(velocity.y, velocity.x) * Mathf.Rad2Deg);

            randomVelocityAngle += 60 * Time.deltaTime;
            randomVelocityAngle = randomVelocityAngle % 360;

            yield return null;
        }

        GameObject explosionInstant = Instantiate(explosion, targetPosition, Quaternion.identity);
        explosionInstant.GetComponent<SkyClimberRocketExplosion>().Initialize(instantiatingObject);

        spriteRenderer.enabled = false;
        rocketParticles.Stop();
        Destroy(this.gameObject, 5f);
    }
}
