using UnityEngine;

public class CoconutAttack : MonoBehaviour
{
    public GameObject coconutPrefab;

    [Header("Fire")]
    public float baseFireInterval = 0.9f;
    public float attackRange = 15f;

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
        float factor = 1f - PlayerBattleData.coconutFireRateLv * 0.1f;
        return Mathf.Max(0.3f, baseFireInterval * factor);
    }

    void Fire()
    {
        Enemy target = FindNearestEnemyInRange();
        if (target == null)
            return;

        Vector2 dir =
            (target.transform.position - transform.position).normalized;

        GameObject c = Instantiate(
            coconutPrefab,
            transform.position,
            Quaternion.identity
        );

        CoconutProjectile proj = c.GetComponent<CoconutProjectile>();
        if (proj != null)
        {
            // ⭐ 初始方向
            proj.SetInitialDirection(dir);

            // ⭐ 升级数据
            proj.damage += PlayerBattleData.coconutDamageLv * 2;
            proj.pierceCount += PlayerBattleData.coconutPierceLv;
        }
    }

    // =====================
    // Enemy search
    // =====================
    Enemy FindNearestEnemyInRange()
    {
        Enemy[] enemies = FindObjectsOfType<Enemy>();
        float minDist = float.MaxValue;
        Enemy nearest = null;

        foreach (var e in enemies)
        {
            float d = Vector2.Distance(
                transform.position,
                e.transform.position
            );

            if (d > attackRange)
                continue;

            if (d < minDist)
            {
                minDist = d;
                nearest = e;
            }
        }

        return nearest;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
