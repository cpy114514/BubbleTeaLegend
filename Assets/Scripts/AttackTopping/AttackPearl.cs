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
            Transform next = FindNextEnemy(hit.transform);
            if (next != null)
            {
                bounceCount--;

                // ⭐ 只改 direction，不碰 Rigidbody
                direction = (next.position - transform.position).normalized;
                return;
            }
        }

        Destroy(gameObject);
    }

    Transform FindNextEnemy(Transform current)
    {
        Enemy[] enemies = FindObjectsOfType<Enemy>();
        float minDist = float.MaxValue;
        Enemy nearest = null;

        foreach (var e in enemies)
        {
            if (e.transform == current) continue;

            float d = Vector2.Distance(
                transform.position,
                e.transform.position
            );

            if (d < bounceSearchRadius && d < minDist)
            {
                minDist = d;
                nearest = e;
            }
        }

        return nearest ? nearest.transform : null;
    }
}
