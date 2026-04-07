using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class EnemyMoveAI : MonoBehaviour
{
    readonly Collider2D[] separationHits = new Collider2D[32];

    Rigidbody2D rb;
    Transform player;

    public bool isDead = false;

    [Header("Move")]
    public float moveSpeed = 2.5f;
    public float stopDistance = 0.6f;
    public float chaseDistance = 6f;

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

    [Header("Melee")]
    public bool enableMeleeAttack = true;
    public float meleeRange = 1.3f;
    public int meleeDamage = 5;
    public float meleeCooldown = 1f;

    [Header("Ranged")]
    public bool enableRangedAttack = false;
    public GameObject bulletPrefab;
    public float bulletSpeed = 6f;
    public float rangedRange = 5f;

    public float shootPauseTime = 0.4f;
    public float moveAfterShootTime = 0.5f;

    float lastAttackTime = -999f;
    float rangedTimer;
    bool isShootPause = false;

    public void ApplyKnockback(Vector2 force)
    {
        Rigidbody2D self = GetComponent<Rigidbody2D>();
        if (self == null)
            return;

        self.velocity = Vector2.zero;
        self.AddForce(force, ForceMode2D.Impulse);
    }

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject != null)
            player = playerObject.transform;

        rangedTimer = Random.Range(0.2f, 0.6f);
    }

    void FixedUpdate()
    {
        if (isDead || player == null)
            return;

        float dist = Vector2.Distance(transform.position, player.position);

        Vector2 moveDir = GetMoveDirection(dist);
        rb.velocity = moveDir * moveSpeed;

        if (enableMeleeAttack &&
            dist <= meleeRange &&
            Time.time - lastAttackTime >= meleeCooldown)
        {
            DoMelee();
        }

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
            else if (rangedTimer <= 0f)
            {
                isShootPause = false;
                rangedTimer = moveAfterShootTime;
            }
        }
    }

    Vector2 GetMoveDirection(float dist)
    {
        Vector2 toPlayer =
            ((Vector2)player.position - (Vector2)transform.position).normalized;

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
        int hitCount = Physics2D.OverlapCircleNonAlloc(
            transform.position,
            separationRadius,
            separationHits
        );

        for (int i = 0; i < hitCount; i++)
        {
            Collider2D hit = separationHits[i];
            if (hit == null || hit.gameObject == gameObject)
                continue;

            EnemyMoveAI other = hit.GetComponent<EnemyMoveAI>();
            if (other == null)
                continue;

            Vector2 away =
                (Vector2)(transform.position - other.transform.position);

            if (away.magnitude > 0.001f)
                sep += away.normalized / away.magnitude;
        }

        return sep * separationStrength;
    }

    void DoMelee()
    {
        PlayerHealthParticles hp = PlayerHealthParticles.Instance;
        if (hp == null)
            return;

        hp.TakeDamage(meleeDamage);
        lastAttackTime = Time.time;
    }

    void DoRanged()
    {
        if (bulletPrefab == null)
            return;

        Vector2 dir =
            ((Vector2)player.position - (Vector2)transform.position).normalized;

        Vector2 spawnPos =
            (Vector2)transform.position + dir * 0.6f;

        GameObject bullet =
            Instantiate(bulletPrefab, spawnPos, Quaternion.identity);

        Rigidbody2D bulletRb = bullet.GetComponent<Rigidbody2D>();
        if (bulletRb != null)
            bulletRb.velocity = dir * bulletSpeed;

        lastAttackTime = Time.time;
    }

    public void Die()
    {
        isDead = true;
        rb.velocity = Vector2.zero;
        Destroy(gameObject);
    }
}
