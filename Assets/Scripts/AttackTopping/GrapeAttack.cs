using UnityEngine;

public class GrapeAttack : MonoBehaviour
{
    [Header("Projectile")]
    public GameObject projectilePrefab;   // ⭐ 由 BattleManager 注入

    [Header("Base Stats")]
    public float baseFireInterval = 1f;
    public float attackRange = 10f;
    public float bulletSpeed = 6f;
    public int baseDamage = 3;

    [Header("Scatter")]
    public int baseScatterCount = 3;
    public float scatterAngle = 45f;

    float fireTimer;

    void Update()
    {
        fireTimer += Time.deltaTime;

        float interval = GetFireInterval();
        if (fireTimer < interval)
            return;

        Enemy target = FindNearestEnemy();
        if (target == null)
            return;

        Fire(target);
        fireTimer = 0f;
    }

    // =====================
    // Fire
    // =====================
    void Fire(Enemy target)
    {
        if (projectilePrefab == null)
        {
            Debug.LogError("Grape projectile prefab missing!");
            return;
        }

        Vector2 baseDir =
            (target.transform.position - transform.position).normalized;

        int count = GetScatterCount();
        float step = count <= 1 ? 0 : scatterAngle / (count - 1);
        float start = -scatterAngle / 2f;

        for (int i = 0; i < count; i++)
        {
            float angle = start + step * i;
            Vector2 dir = Rotate(baseDir, angle);

            GameObject b = Instantiate(
                projectilePrefab,
                transform.position,
                Quaternion.identity
            );

            PearlProjectile proj = b.GetComponent<PearlProjectile>();
            proj.speed = bulletSpeed;
            proj.damage = GetDamage();
            proj.bounceCount = 0; // 葡萄不弹射
            proj.Init(dir);
        }
    }

    // =====================
    // Stats from upgrades
    // =====================
    float GetFireInterval()
    {
        int lv = PlayerBattleData.grapeFireRateLv;
        float factor = 1f - lv * 0.12f;
        return Mathf.Max(0.25f, baseFireInterval * factor);
    }

    int GetDamage()
    {
        return baseDamage + PlayerBattleData.grapeDamageLv * 2;
    }

    int GetScatterCount()
    {
        return baseScatterCount + PlayerBattleData.grapeScatterCountLv;
    }

    // =====================
    // Utils
    // =====================
    Enemy FindNearestEnemy()
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

    Vector2 Rotate(Vector2 v, float deg)
    {
        float rad = deg * Mathf.Deg2Rad;
        float cos = Mathf.Cos(rad);
        float sin = Mathf.Sin(rad);

        return new Vector2(
            v.x * cos - v.y * sin,
            v.x * sin + v.y * cos
        ).normalized;
    }
}
