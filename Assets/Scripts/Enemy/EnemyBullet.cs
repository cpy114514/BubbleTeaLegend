using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    public int damage = 5;
    public float lifeTime = 3f;

    void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerHealthParticles hp =
                FindObjectOfType<PlayerHealthParticles>();

            if (hp != null)
                hp.TakeDamage(damage);

            Destroy(gameObject);
        }

        if (other.CompareTag("Wall"))
        {
            Destroy(gameObject);
        }
    }
}
