using UnityEngine;

public class PearlAttack : MonoBehaviour
{
    public GameObject pearlPrefab;
    public float baseInterval = 0.8f;
    public float attackRange = 18f;
    public float bulletSpeed = 8f;
    public int baseDamage = 4;

    float timer;

    void Update()
    {
        timer += Time.deltaTime;

        float interval = GetInterval();
        if (timer >= interval)
        {
            Fire();
            timer = 0f;
        }
    }

    float GetInterval()
    {
        return Mathf.Max(
            0.2f,
            baseInterval * (1f - PlayerBattleData.pearlFireRateLv * 0.1f)
        );
    }

    void Fire()
    {
        if (pearlPrefab == null)
        {
            Debug.LogError("Pearl prefab missing!");
            return;
        }

        Enemy target = FindNearestEnemy();
        if (target == null)
            return;

        Vector2 dir =
            ((Vector2)target.transform.position - (Vector2)transform.position).normalized;

        GameObject bullet = Instantiate(
            pearlPrefab,
            transform.position,
            Quaternion.identity
        );

        PearlProjectile projectile = bullet.GetComponent<PearlProjectile>();
        projectile.speed = bulletSpeed;
        projectile.damage = baseDamage + PlayerBattleData.pearlDamageLv * 2;
        projectile.bounceCount = PlayerBattleData.pearlBounceLv;
        projectile.Init(dir);
    }

    Enemy FindNearestEnemy()
    {
        return EnemyRegistry.GetNearest(transform.position, attackRange);
    }
}
