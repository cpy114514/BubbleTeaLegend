using UnityEngine;

public class PuddingProjectile : MonoBehaviour
{
    public float speed = 6f;
    public int damage = 2;
    public float lifeTime = 2f;

    Vector2 dir;

    public void Init(Vector2 d)
    {
        dir = d;
        Destroy(gameObject, lifeTime);
    }

    void Update()
    {
        transform.position += (Vector3)(dir * speed * Time.deltaTime);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            other.GetComponent<Enemy>()?.TakeDamage(damage);
            Destroy(gameObject);
        }
    }
}
