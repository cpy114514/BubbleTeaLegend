using UnityEngine;
using System.Collections.Generic;



public class LemonBoomerang : MonoBehaviour
{
    public float speed = 7f;
    public float maxDistance = 6f;
    public int damage = 2;

    HashSet<Enemy> hitEnemies = new HashSet<Enemy>();

    Vector3 startPos;
    Vector3 direction;
    bool returning = false;
    Transform player;

    void Start()
    {
        startPos = transform.position;
        player = FindObjectOfType<PlayerMovement>()?.transform;
    }

    public void Init(Vector2 dir)
    {
        direction = dir.normalized;
        transform.right = direction;

        // ===== Damage =====
        damage += PlayerBattleData.lemonDamageLv * 2;

        // ===== Size =====
        float sizeFactor = 1f + PlayerBattleData.lemonSizeLv * 0.15f;
        transform.localScale *= sizeFactor;
    }


    void Update()
    {
        if (!returning)
        {
            transform.position += direction * speed * Time.deltaTime;

            if (Vector3.Distance(startPos, transform.position) >= maxDistance)
            {
                returning = true;
            }
        }
        else
        {
            if (player == null)
            {
                Destroy(gameObject);
                return;
            }

            Vector2 toPlayer =
                (player.position - transform.position).normalized;

            transform.right = toPlayer;
            transform.position += (Vector3)(toPlayer * speed * Time.deltaTime);

            if (Vector3.Distance(transform.position, player.position) < 0.4f)
            {
                Destroy(gameObject);
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        Enemy enemy = other.GetComponent<Enemy>();
        if (enemy == null)
            return;

        // ⭐ 已经打过这个敌人，就不再打
        if (hitEnemies.Contains(enemy))
            return;

        hitEnemies.Add(enemy);
        enemy.TakeDamage(damage);
    }

}
