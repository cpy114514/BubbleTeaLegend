using UnityEngine;

public class PearlAttack : MonoBehaviour
{
    public GameObject pearlPrefab;
    public float baseInterval = 1f;
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
        if (target == null) return;

        Vector2 dir =
            (target.transform.position - transform.position).normalized;

        GameObject b = Instantiate(
            pearlPrefab,
            transform.position,
            Quaternion.identity
        );

        var proj = b.GetComponent<PearlProjectile>();
        proj.speed = bulletSpeed;
        proj.damage = baseDamage + PlayerBattleData.pearlDamageLv * 2;
        proj.bounceCount = PlayerBattleData.pearlBounceLv;
        proj.Init(dir);
    }


    Enemy FindNearestEnemy()
    {
        Enemy[] enemies = FindObjectsOfType<Enemy>();
        float min = float.MaxValue;
        Enemy best = null;

        foreach (var e in enemies)
        {
            float d = Vector2.Distance(transform.position, e.transform.position);
            if (d > attackRange) continue;

            if (d < min)
            {
                min = d;
                best = e;
            }
        }
        return best;
    }
}
