using UnityEngine;

public class PuddingControl : MonoBehaviour
{
    [Header("Separation")]
    public float separationRadius = 0.5f;   // 多近算“挤”
    public float separationStrength = 1.2f; // 推开的力度

    // ================= 跟随阈值（滞回） =================
    [Header("Follow Threshold")]
    public float nearRadius = 3.0f;   // 算“跟上了”
    public float farRadius = 8.0f;   // 算“掉队了”

    // ================= 移动 =================
    [Header("Movement")]
    public float followSpeed = 6.5f;        // 追玩家（比玩家快）
    public float guardSpeed = 3.0f;        // 游荡速度
    public float guardRadius = 2.3f;        // 游荡半径
    public float wanderChangeInterval = 1.5f; // 多久换一次游荡方向

    // ================= 近战 =================
    [Header("Melee")]
    public float meleeRange = 3f;
    public int meleeDamage = 4;
    public float meleeCooldown = 0.8f;

    public float meleeChargeDistance = 1.5f;
    public float meleeChargeSpeed = 6f;
    public float knockbackForce = 5f;
    public float meleeFreezeTime = 0.1f;

    // ================= 远程 =================
    [Header("Ranged")]
    public float rangedRange = 15f;
    public int rangedDamage = 2;
    public float rangedCooldown = 1f;
    public GameObject projectilePrefab;

    // ================= 内部状态 =================
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

    Vector2 ComputeSeparation()
    {
        PuddingControl[] all = FindObjectsOfType<PuddingControl>();
        Vector2 force = Vector2.zero;
        int count = 0;

        foreach (var other in all)
        {
            if (other == this) continue;

            Vector2 diff =
                (Vector2)transform.position - (Vector2)other.transform.position;

            float dist = diff.magnitude;
            if (dist > 0f && dist < separationRadius)
            {
                force += diff.normalized * (separationRadius - dist);
                count++;
            }
        }

        if (count > 0)
            force /= count;

        return force;
    }

    // ================= 初始化 =================

    void Start()
    {
        GameObject p = GameObject.FindGameObjectWithTag("Player");
        if (p != null)
            player = p.transform;
        else
            Debug.LogError("❌ 找不到 Player（请确认 Tag = Player）");

        PickNewWanderDir();
    }

    // ================= 主循环 =================
    void Update()
    {


        if (player == null) return;

        // ===== 近战冲刺阶段 =====
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

        // ===== 命中后短暂停顿 =====
        if (freezeTimer > 0f)
        {
            freezeTimer -= Time.deltaTime;
            return;
        }

        // ===== 移动 =====
        Move();

        meleeTimer += Time.deltaTime;
        rangedTimer += Time.deltaTime;

        Enemy enemy = FindNearestEnemy();
        if (enemy == null) return;

        float dist = Vector2.Distance(transform.position, enemy.transform.position);

        // ===== 攻击优先级 =====
        if (dist <= meleeRange)
        {
            TryMelee(enemy);
        }
        else if (dist <= rangedRange)
        {
            TryRanged(enemy);
        }

    }

    // ================= 移动逻辑 =================
    void Move()
    {
        float distToPlayer = Vector2.Distance(transform.position, player.position);

        // --- 滞回判断 ---
        if (isFollowing)
        {
            if (distToPlayer <= nearRadius)
                isFollowing = false;
        }
        else
        {
            if (distToPlayer >= farRadius)
                isFollowing = true;
        }

        // ⭐ 计算分离力（关键）
        Vector2 sep = ComputeSeparation();

        // --- 行为执行 ---
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

            if (dir.sqrMagnitude > 0.01f)
            {
                Vector2 move =
                    dir.normalized * guardSpeed + sep * separationStrength;

                transform.position +=
                    (Vector3)(move * Time.deltaTime);
            }
        }
    }


    void PickNewWanderDir()
    {
        Enemy[] enemies = FindObjectsOfType<Enemy>();

        // 没怪，保持随机
        if (enemies.Length == 0)
        {
            wanderDir = Random.insideUnitCircle.normalized;
            return;
        }

        Vector2 center = player.position;
        Vector2 sumDir = Vector2.zero;
        int count = 0;

        foreach (Enemy e in enemies)
        {
            Vector2 toEnemy = (Vector2)e.transform.position - center;
            float dist = toEnemy.magnitude;

            // 只考虑一定范围内的敌人（避免全图拉偏）
            if (dist < farRadius * 1.2f)
            {
                sumDir += toEnemy.normalized;
                count++;
            }
        }

        // 如果附近没敌人，随机
        if (count == 0)
        {
            wanderDir = Random.insideUnitCircle.normalized;
            return;
        }

        Vector2 enemyBiasDir = sumDir.normalized;

        // ⭐ 70% 概率偏向敌人方向，30% 完全随机
        if (Random.value < 0.7f)
        {
            wanderDir = enemyBiasDir;
        }
        else
        {
            wanderDir = Random.insideUnitCircle.normalized;
        }
    }


    // ================= 近战（拱） =================
    void TryMelee(Enemy enemy)
    {
        if (meleeTimer < GetMeleeCooldown()) return;

        // 拱的方向：布丁 → 敌人
        meleeDir =
            ((Vector2)enemy.transform.position - (Vector2)transform.position).normalized;

        isMeleeCharging = true;
        meleeMoveLeft = meleeChargeDistance;

        enemy.TakeDamage(GetMeleeDamage());
        enemy.OnKnockbackHit(); // ⭐ 告诉敌人：你被击退了

        // ⭐ 击退方向：玩家 → 敌人（三点一线）
        Vector2 knockDir =
            ((Vector2)enemy.transform.position - (Vector2)player.position).normalized;

        enemy.transform.position +=
            (Vector3)(knockDir * knockbackForce);

        meleeTimer = 0f;
    }

    // ================= 远程 =================
    void TryRanged(Enemy enemy)
    {
        if (rangedTimer < GetRangedCooldown()) return;

        Vector2 dir =
            ((Vector2)enemy.transform.position - (Vector2)transform.position).normalized;

        GameObject p = Instantiate(
            projectilePrefab,
            transform.position,
            Quaternion.identity
        );

        var proj = p.GetComponent<PuddingProjectile>();
        proj.damage = GetRangedDamage();
        proj.Init(dir);

        rangedTimer = 0f;
    }

    // ================= 升级数值读取 =================

    // 攻速：每级 -10% 冷却
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

    // 伤害：线性增长
    int GetMeleeDamage()
    {
        return meleeDamage + PlayerBattleData.puddingDamageLv * 2;
    }

    int GetRangedDamage()
    {
        return rangedDamage + PlayerBattleData.puddingDamageLv * 1;
    }


    // ================= 找最近敌人 =================
    Enemy FindNearestEnemy()
    {
        Enemy[] enemies = FindObjectsOfType<Enemy>();
        if (enemies.Length == 0) return null;

        Enemy nearest = null;
        float minDist = float.MaxValue;
        Vector2 pos = transform.position;

        foreach (Enemy e in enemies)
        {
            float d = Vector2.Distance(pos, e.transform.position);
            if (d < minDist)
            {
                minDist = d;
                nearest = e;
            }
        }
        return nearest;
    }
}
