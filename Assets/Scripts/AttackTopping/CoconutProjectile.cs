using UnityEngine;

public class CoconutProjectile : MonoBehaviour
{
    public float speed = 8f;
    public float turnSpeed = 720f;

    public int damage = 2;
    public int pierceCount = 2;

    Vector2 direction;
    Enemy target;

    void Start()
    {
        FindTarget();
    }

    public void SetInitialDirection(Vector2 dir)
    {
        direction = dir.normalized;
        transform.right = direction;
    }

    void Update()
    {
        if (target == null)
        {
            FindTarget();
        }

        if (target != null)
        {
            Vector2 toTarget =
                (target.transform.position - transform.position).normalized;

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
        Enemy[] enemies = FindObjectsOfType<Enemy>();
        float minDist = float.MaxValue;

        foreach (var e in enemies)
        {
            float d = Vector2.Distance(transform.position, e.transform.position);
            if (d < minDist)
            {
                minDist = d;
                target = e;
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        Enemy enemy = other.GetComponent<Enemy>();
        if (enemy == null)
            return;

        enemy.TakeDamage(damage);
        pierceCount--;

        if (pierceCount <= 0)
        {
            Destroy(gameObject);
        }
    }
}
