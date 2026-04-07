using UnityEngine;

public class CoconutProjectile : MonoBehaviour
{
    public float speed = 8f;
    public float turnSpeed = 720f;

    public int damage = 2;
    public int pierceCount = 2;

    Vector2 direction;
    Enemy target;

    public void SetInitialDirection(Vector2 dir)
    {
        direction = dir.normalized;
        transform.right = direction;
        FindTarget();
    }

    void Update()
    {
        if (target == null)
            FindTarget();

        if (target != null)
        {
            Vector2 toTarget =
                ((Vector2)target.transform.position - (Vector2)transform.position).normalized;

            direction = Vector2.Lerp(
                direction,
                toTarget,
                turnSpeed * Time.deltaTime / 360f
            ).normalized;

            transform.right = direction;
        }

        transform.position +=
            (Vector3)(direction * speed * Time.deltaTime);
    }

    void FindTarget()
    {
        target = EnemyRegistry.GetNearest(transform.position);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        Enemy enemy = other.GetComponent<Enemy>();
        if (enemy == null)
            return;

        enemy.TakeDamage(damage);
        pierceCount--;

        if (pierceCount <= 0)
            Destroy(gameObject);
    }
}
