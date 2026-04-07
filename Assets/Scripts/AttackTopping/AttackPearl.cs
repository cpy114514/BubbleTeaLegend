using UnityEngine;

public class PearlProjectile : MonoBehaviour
{
    public float speed = 15f;
    public int damage = 5;
    public float lifeTime = 3f;

    public int bounceCount = 0;
    public float bounceSearchRadius = 25f;

    Vector2 direction;

    public void Init(Vector2 dir)
    {
        direction = dir.normalized;
        Destroy(gameObject, lifeTime);
    }

    void Update()
    {
        transform.position +=
            (Vector3)(direction * speed * Time.deltaTime);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Enemy"))
            return;

        Enemy hit = other.GetComponent<Enemy>();
        if (hit == null)
            return;

        hit.TakeDamage(damage);

        if (bounceCount > 0)
        {
            Enemy next = FindNextEnemy(hit);
            if (next != null)
            {
                bounceCount--;
                direction =
                    ((Vector2)next.transform.position - (Vector2)transform.position).normalized;
                return;
            }
        }

        Destroy(gameObject);
    }

    Enemy FindNextEnemy(Enemy current)
    {
        return EnemyRegistry.GetNearest(
            transform.position,
            bounceSearchRadius,
            current
        );
    }
}
