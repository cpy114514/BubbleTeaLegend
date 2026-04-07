using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    Rigidbody2D rb;

    [Header("HP")]
    public int maxHP = 10;
    int currentHP;

    [Header("Exp Drop")]
    public GameObject expOrbPrefab;
    public int expValue = 1;

    [Header("Score")]
    public int scoreValue = 10;

    [Header("Hit Feedback")]
    public float hitDarkenTime = 0.12f;
    public Color hitColor = new(0.5f, 0.5f, 0.5f, 1f);

    [Header("Damage Text")]
    public GameObject damageTextPrefab;

    SpriteRenderer sr;
    Color originalColor;

    protected virtual void Start()
    {
        currentHP = maxHP;
    }

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        sr = GetComponent<SpriteRenderer>();
        if (sr != null)
            originalColor = sr.color;
    }

    void OnEnable()
    {
        EnemyRegistry.Register(this);
    }

    void OnDisable()
    {
        EnemyRegistry.Unregister(this);
    }

    public void OnKnockbackHit()
    {
        PlayHitFlash();
    }

    public void TakeDamage(int damage)
    {
        currentHP -= damage;

        PlayHitFlash();
        ShowDamageText(damage);

        if (currentHP <= 0)
            Die();
    }

    void PlayHitFlash()
    {
        if (sr == null)
            return;

        StopAllCoroutines();
        StartCoroutine(HitFlash());
    }

    IEnumerator HitFlash()
    {
        sr.color = hitColor;
        yield return new WaitForSeconds(hitDarkenTime);
        sr.color = originalColor;
    }

    void ShowDamageText(int damage)
    {
        if (damageTextPrefab == null)
            return;

        Vector3 spawnPos = transform.position + Vector3.up * 0.8f;
        GameObject obj = Instantiate(
            damageTextPrefab,
            spawnPos,
            Quaternion.identity
        );

        obj.GetComponent<DamageText>().SetDamage(damage);
    }

    void Die()
    {
        if (ScoreManager.Instance != null)
            ScoreManager.Instance.AddKillScore(scoreValue);

        DropExp();
        Destroy(gameObject);
    }

    void DropExp()
    {
        if (expOrbPrefab == null)
            return;

        GameObject orb = Instantiate(
            expOrbPrefab,
            transform.position,
            Quaternion.identity
        );

        orb.GetComponent<ExpOrb>().value = expValue;
    }
}

public static class EnemyRegistry
{
    static readonly System.Collections.Generic.List<Enemy> ActiveEnemyList = new();

    public static System.Collections.Generic.IReadOnlyList<Enemy> ActiveEnemies => ActiveEnemyList;

    public static void Register(Enemy enemy)
    {
        if (enemy == null || ActiveEnemyList.Contains(enemy))
            return;

        ActiveEnemyList.Add(enemy);
    }

    public static void Unregister(Enemy enemy)
    {
        if (enemy == null)
            return;

        int index = ActiveEnemyList.IndexOf(enemy);
        if (index < 0)
            return;

        int lastIndex = ActiveEnemyList.Count - 1;
        ActiveEnemyList[index] = ActiveEnemyList[lastIndex];
        ActiveEnemyList.RemoveAt(lastIndex);
    }

    public static Enemy GetNearest(
        Vector2 origin,
        float maxDistance = Mathf.Infinity,
        Enemy exclude = null
    )
    {
        float bestDistanceSqr = maxDistance == Mathf.Infinity
            ? float.PositiveInfinity
            : maxDistance * maxDistance;
        Enemy bestEnemy = null;

        for (int i = 0; i < ActiveEnemyList.Count; i++)
        {
            Enemy enemy = ActiveEnemyList[i];
            if (enemy == null || enemy == exclude)
                continue;

            Vector2 delta = (Vector2)enemy.transform.position - origin;
            float distanceSqr = delta.sqrMagnitude;
            if (distanceSqr >= bestDistanceSqr)
                continue;

            bestDistanceSqr = distanceSqr;
            bestEnemy = enemy;
        }

        return bestEnemy;
    }
}
