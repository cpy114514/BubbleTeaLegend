using System.Collections.Generic;
using UnityEngine;

public class PuddingControl : MonoBehaviour
{
    static readonly List<PuddingControl> ActivePuddings = new();

    [Header("Separation")]
    public float separationRadius = 0.5f;
    public float separationStrength = 1.2f;

    [Header("Follow Threshold")]
    public float nearRadius = 3.0f;
    public float farRadius = 8.0f;

    [Header("Movement")]
    public float followSpeed = 6.5f;
    public float guardSpeed = 3.0f;
    public float guardRadius = 2.3f;
    public float wanderChangeInterval = 1.5f;

    [Header("Melee")]
    public float meleeRange = 3f;
    public int meleeDamage = 4;
    public float meleeCooldown = 0.8f;
    public float meleeChargeDistance = 1.5f;
    public float meleeChargeSpeed = 6f;
    public float knockbackForce = 5f;
    public float meleeFreezeTime = 0.1f;

    [Header("Ranged")]
    public float rangedRange = 15f;
    public int rangedDamage = 2;
    public float rangedCooldown = 1f;
    public GameObject projectilePrefab;

    Transform player;

    bool isFollowing;
    bool isMeleeCharging;

    Vector2 meleeDir;
    float meleeMoveLeft;
    float freezeTimer;

    Vector2 wanderDir;
    float wanderTimer;

    float meleeTimer;
    float rangedTimer;

    void OnEnable()
    {
        if (!ActivePuddings.Contains(this))
            ActivePuddings.Add(this);
    }

    void OnDisable()
    {
        ActivePuddings.Remove(this);
    }

    Vector2 ComputeSeparation()
    {
        Vector2 force = Vector2.zero;
        int count = 0;
        float separationRadiusSqr = separationRadius * separationRadius;
        Vector2 myPosition = transform.position;

        for (int i = 0; i < ActivePuddings.Count; i++)
        {
            PuddingControl other = ActivePuddings[i];
            if (other == null || other == this)
                continue;

            Vector2 diff = myPosition - (Vector2)other.transform.position;
            float distSqr = diff.sqrMagnitude;
            if (distSqr <= 0f || distSqr >= separationRadiusSqr)
                continue;

            float dist = Mathf.Sqrt(distSqr);
            force += diff.normalized * (separationRadius - dist);
            count++;
        }

        if (count > 0)
            force /= count;

        return force;
    }

    void Start()
    {
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject != null)
            player = playerObject.transform;
        else
            Debug.LogError("Player tag not found.");

        PickNewWanderDir();
    }

    void Update()
    {
        if (player == null)
            return;

        if (isMeleeCharging)
        {
            float move = meleeChargeSpeed * Time.deltaTime;
            float step = Mathf.Min(move, meleeMoveLeft);

            transform.position += (Vector3)(meleeDir * step);
            meleeMoveLeft -= step;

            if (meleeMoveLeft <= 0f)
            {
                isMeleeCharging = false;
                freezeTimer = meleeFreezeTime;
            }

            return;
        }

        if (freezeTimer > 0f)
        {
            freezeTimer -= Time.deltaTime;
            return;
        }

        Move();

        meleeTimer += Time.deltaTime;
        rangedTimer += Time.deltaTime;

        Enemy enemy = FindNearestEnemy();
        if (enemy == null)
            return;

        float dist = Vector2.Distance(transform.position, enemy.transform.position);
        if (dist <= meleeRange)
            TryMelee(enemy);
        else if (dist <= rangedRange)
            TryRanged(enemy);
    }

    void Move()
    {
        float distToPlayer = Vector2.Distance(transform.position, player.position);

        if (isFollowing)
        {
            if (distToPlayer <= nearRadius)
                isFollowing = false;
        }
        else if (distToPlayer >= farRadius)
        {
            isFollowing = true;
        }

        Vector2 sep = ComputeSeparation();

        if (isFollowing)
        {
            Vector2 dir =
                ((Vector2)player.position - (Vector2)transform.position).normalized;

            Vector2 move =
                dir * followSpeed + sep * separationStrength;

            transform.position +=
                (Vector3)(move * Time.deltaTime);
        }
        else
        {
            wanderTimer += Time.deltaTime;
            if (wanderTimer >= wanderChangeInterval)
            {
                PickNewWanderDir();
                wanderTimer = 0f;
            }

            Vector2 targetPos =
                (Vector2)player.position + wanderDir * guardRadius;

            Vector2 dir = targetPos - (Vector2)transform.position;
            if (dir.sqrMagnitude <= 0.01f)
                return;

            Vector2 move =
                dir.normalized * guardSpeed + sep * separationStrength;

            transform.position +=
                (Vector3)(move * Time.deltaTime);
        }
    }

    void PickNewWanderDir()
    {
        if (EnemyRegistry.ActiveEnemies.Count == 0)
        {
            wanderDir = Random.insideUnitCircle.normalized;
            return;
        }

        Vector2 center = player.position;
        Vector2 sumDir = Vector2.zero;
        int nearbyEnemyCount = 0;
        float scanRadius = farRadius * 1.2f;
        float scanRadiusSqr = scanRadius * scanRadius;

        for (int i = 0; i < EnemyRegistry.ActiveEnemies.Count; i++)
        {
            Enemy enemy = EnemyRegistry.ActiveEnemies[i];
            if (enemy == null)
                continue;

            Vector2 toEnemy = (Vector2)enemy.transform.position - center;
            if (toEnemy.sqrMagnitude >= scanRadiusSqr)
                continue;

            sumDir += toEnemy.normalized;
            nearbyEnemyCount++;
        }

        if (nearbyEnemyCount == 0)
        {
            wanderDir = Random.insideUnitCircle.normalized;
            return;
        }

        Vector2 enemyBiasDir = sumDir.normalized;
        wanderDir = Random.value < 0.7f
            ? enemyBiasDir
            : Random.insideUnitCircle.normalized;
    }

    void TryMelee(Enemy enemy)
    {
        if (meleeTimer < GetMeleeCooldown())
            return;

        meleeDir =
            ((Vector2)enemy.transform.position - (Vector2)transform.position).normalized;

        isMeleeCharging = true;
        meleeMoveLeft = meleeChargeDistance;

        enemy.TakeDamage(GetMeleeDamage());
        enemy.OnKnockbackHit();

        Vector2 knockDir =
            ((Vector2)enemy.transform.position - (Vector2)player.position).normalized;

        enemy.transform.position +=
            (Vector3)(knockDir * knockbackForce);

        meleeTimer = 0f;
    }

    void TryRanged(Enemy enemy)
    {
        if (rangedTimer < GetRangedCooldown())
            return;

        Vector2 dir =
            ((Vector2)enemy.transform.position - (Vector2)transform.position).normalized;

        GameObject projectile = Instantiate(
            projectilePrefab,
            transform.position,
            Quaternion.identity
        );

        PuddingProjectile puddingProjectile = projectile.GetComponent<PuddingProjectile>();
        puddingProjectile.damage = GetRangedDamage();
        puddingProjectile.Init(dir);

        rangedTimer = 0f;
    }

    float GetMeleeCooldown()
    {
        float factor = 1f - PlayerBattleData.puddingAttackSpeedLv * 0.1f;
        return Mathf.Max(0.2f, meleeCooldown * factor);
    }

    float GetRangedCooldown()
    {
        float factor = 1f - PlayerBattleData.puddingAttackSpeedLv * 0.1f;
        return Mathf.Max(0.3f, rangedCooldown * factor);
    }

    int GetMeleeDamage()
    {
        return meleeDamage + PlayerBattleData.puddingDamageLv * 2;
    }

    int GetRangedDamage()
    {
        return rangedDamage + PlayerBattleData.puddingDamageLv;
    }

    Enemy FindNearestEnemy()
    {
        return EnemyRegistry.GetNearest(transform.position);
    }
}
