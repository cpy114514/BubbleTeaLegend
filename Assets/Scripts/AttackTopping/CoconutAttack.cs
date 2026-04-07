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
            ((Vector2)target.transform.position - (Vector2)transform.position).normalized;

        GameObject coconut = Instantiate(
            coconutPrefab,
            transform.position,
            Quaternion.identity
        );

        CoconutProjectile projectile = coconut.GetComponent<CoconutProjectile>();
        if (projectile != null)
        {
            projectile.SetInitialDirection(dir);
            projectile.damage += PlayerBattleData.coconutDamageLv * 2;
            projectile.pierceCount += PlayerBattleData.coconutPierceLv;
        }
    }

    Enemy FindNearestEnemyInRange()
    {
        return EnemyRegistry.GetNearest(transform.position, attackRange);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
