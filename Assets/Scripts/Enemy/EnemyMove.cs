using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class EnemyMoveAI : MonoBehaviour
{
    Rigidbody2D rb;
    Transform player;

    public bool isDead = false;

    // =====================
    // Move
    // =====================
    [Header("Move")]
    public float moveSpeed = 2.5f;
    public float stopDistance = 0.6f;
    public float chaseDistance = 6f;

    // 防止挤在一起
    [Header("Separation")]
    public float separationRadius = 0.8f;
    public float separationStrength = 1.2f;

    public enum MoveStyle
    {
        Chase,
        Orbit,
        Kite
    }

    public MoveStyle moveStyle = MoveStyle.Chase;
    public float orbitDistance = 1.6f;
    public float kiteDistance = 4f;

    // =====================
    // Melee
    // =====================
    [Header("Melee")]
    public bool enableMeleeAttack = true;
    public float meleeRange = 1.3f;
    public int meleeDamage = 5;
    public float meleeCooldown = 1f;

    // =====================
    // Ranged
    // =====================
    [Header("Ranged")]
    public bool enableRangedAttack = false;
    public GameObject bulletPrefab;
    public float bulletSpeed = 6f;
    public float rangedRange = 5f;

    // 元气骑士节奏
    public float shootPauseTime = 0.4f;
    public float moveAfterShootTime = 0.5f;

    float lastAttackTime = -999f;
    float rangedTimer;
    bool isShootPause = false;

    public void ApplyKnockback(Vector2 force)
    {
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb == null) return;

        rb.velocity = Vector2.zero;
        rb.AddForce(force, ForceMode2D.Impulse);
    }

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        GameObject p = GameObject.FindGameObjectWithTag("Player");
        if (p != null) player = p.transform;

        rangedTimer = Random.Range(0.2f, 0.6f);
    }

    void FixedUpdate()
    {
        if (isDead || player == null) return;

        float dist = Vector2.Distance(transform.position, player.position);

        // ========= 移动 =========
        Vector2 moveDir = GetMoveDirection(dist);
        rb.velocity = moveDir * moveSpeed;

        // ========= 近战 =========
        if (enableMeleeAttack &&
            dist <= meleeRange &&
            Time.time - lastAttackTime >= meleeCooldown)
        {
            DoMelee();
        }

        // ========= 远程（元气骑士风格） =========
        if (enableRangedAttack && dist <= rangedRange)
        {
            rangedTimer -= Time.fixedDeltaTime;

            if (!isShootPause)
            {
                rb.velocity = Vector2.zero;

                if (rangedTimer <= 0f)
                {
                    DoRanged();
                    isShootPause = true;
                    rangedTimer = shootPauseTime;
                }
            }
            else
            {
                if (rangedTimer <= 0f)
                {
                    isShootPause = false;
                    rangedTimer = moveAfterShootTime;
                }
            }
        }
    }

    // =====================
    // Movement Logic
    // =====================
    Vector2 GetMoveDirection(float dist)
    {
        Vector2 toPlayer =
            (player.position - transform.position).normalized;

        Vector2 baseDir = Vector2.zero;

        switch (moveStyle)
        {
            case MoveStyle.Chase:
                if (dist > stopDistance && dist < chaseDistance)
                    baseDir = toPlayer;
                break;

            case MoveStyle.Orbit:
                if (dist > orbitDistance)
                    baseDir = toPlayer;
                else
                    baseDir = Vector2.Perpendicular(toPlayer);
                break;

            case MoveStyle.Kite:
                if (dist < kiteDistance)
                    baseDir = -toPlayer;
                else
                    baseDir = toPlayer;
                break;
        }

        Vector2 sep = GetSeparation();
        if (baseDir == Vector2.zero && sep == Vector2.zero)
            return Vector2.zero;

        return (baseDir + sep).normalized;
    }

    Vector2 GetSeparation()
    {
        Vector2 sep = Vector2.zero;
        Collider2D[] hits =
            Physics2D.OverlapCircleAll(transform.position, separationRadius);

        foreach (var h in hits)
        {
            if (h.gameObject == gameObject) continue;
            EnemyMoveAI other = h.GetComponent<EnemyMoveAI>();
            if (other == null) continue;

            Vector2 away =
                (Vector2)(transform.position - other.transform.position);

            if (away.magnitude > 0.001f)
                sep += away.normalized / away.magnitude;
        }

        return sep * separationStrength;
    }

    // =====================
    // Attacks
    // =====================
    void DoMelee()
    {
        PlayerHealthParticles hp =
            FindObjectOfType<PlayerHealthParticles>();

        if (hp != null)
        {
            hp.TakeDamage(meleeDamage);
            lastAttackTime = Time.time;
        }
    }

    void DoRanged()
    {
        if (bulletPrefab == null) return;

        Vector2 dir =
            (player.position - transform.position).normalized;

        Vector2 spawnPos =
            (Vector2)transform.position + dir * 0.6f;

        GameObject bullet =
            Instantiate(bulletPrefab, spawnPos, Quaternion.identity);

        Rigidbody2D brb = bullet.GetComponent<Rigidbody2D>();
        if (brb != null)
            brb.velocity = dir * bulletSpeed;

        lastAttackTime = Time.time;
    }

    public void Die()
    {
        isDead = true;
        rb.velocity = Vector2.zero;
        Destroy(gameObject);
    }
}
