using UnityEngine;

public class LemonAttack : MonoBehaviour
{
    public GameObject lemonPrefab;

    [Header("Fire")]
    public float baseFireInterval = 2f;
    public float attackRange = 7f;

    float timer;

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= GetFireInterval())
        {
            Fire();
            timer = 0f;
        }
    }

    float GetFireInterval()
    {
        float factor = 1f - PlayerBattleData.lemonFireRateLv * 0.1f;
        return Mathf.Max(0.5f, baseFireInterval * factor);
    }


    void Fire()
    {
        Enemy target = FindNearestEnemy();
        if (target == null)
            return;

        int count = GetLemonCount();

        // ===== 情况 1：只有 1 个（特殊规则）=====
        if (count == 1)
        {
            Vector2 dir = GetAxisDirectionToTarget(target.transform.position);
            FireOne(dir);
            return;
        }

        // ===== 情况 2：>= 2 个，360° 均分 =====
        float angleStep = 360f / count;

        for (int i = 0; i < count; i++)
        {
            float angle = angleStep * i;
            Vector2 dir = Rotate(Vector2.right, angle);
            FireOne(dir);
        }
    }

    int GetLemonCount()
    {
        return 1 + PlayerBattleData.lemonCountLv;
    }


    void FireOne(Vector2 dir)
    {
        GameObject l = Instantiate(
            lemonPrefab,
            transform.position,
            Quaternion.identity
        );

        LemonBoomerang b = l.GetComponent<LemonBoomerang>();
        if (b != null)
        {
            b.damage += PlayerBattleData.lemonDamageLv * 2;
            b.Init(dir);
        }
    }

    // =====================
    // 只在 count == 1 时使用
    // 上下左右取最近方向
    // =====================
    Vector2 GetAxisDirectionToTarget(Vector3 targetPos)
    {
        Vector2 toTarget = (targetPos - transform.position).normalized;

        if (Mathf.Abs(toTarget.x) > Mathf.Abs(toTarget.y))
        {
            return toTarget.x > 0 ? Vector2.right : Vector2.left;
        }
        else
        {
            return toTarget.y > 0 ? Vector2.up : Vector2.down;
        }
    }

    Enemy FindNearestEnemy()
    {
        Enemy[] enemies = FindObjectsOfType<Enemy>();
        float minDist = float.MaxValue;
        Enemy nearest = null;

        foreach (var e in enemies)
        {
            float d = Vector2.Distance(transform.position, e.transform.position);
            if (d <= attackRange && d < minDist)
            {
                minDist = d;
                nearest = e;
            }
        }

        return nearest;
    }

    Vector2 Rotate(Vector2 v, float degrees)
    {
        float rad = degrees * Mathf.Deg2Rad;
        float cos = Mathf.Cos(rad);
        float sin = Mathf.Sin(rad);

        return new Vector2(
            v.x * cos - v.y * sin,
            v.x * sin + v.y * cos
        ).normalized;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
