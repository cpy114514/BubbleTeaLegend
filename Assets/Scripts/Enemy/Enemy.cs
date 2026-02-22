using UnityEngine;
using System.Collections;
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
    [Tooltip("??????????? Inspector ?????????")]
    public int scoreValue = 10;

    [Header("Hit Feedback")]
    public float hitDarkenTime = 0.12f;
    public Color hitColor = new Color(0.5f, 0.5f, 0.5f, 1f);

    SpriteRenderer sr;
    Color originalColor;

    protected virtual void Start()
    {
        currentHP = maxHP;
    }
    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        if (sr != null)
            originalColor = sr.color;
    }

    // =====================
    // Knockback???????
    // =====================
    public void OnKnockbackHit()
    {
        if (sr == null) return;

        StopAllCoroutines();
        StartCoroutine(HitFlash());
    }
    IEnumerator HitFlash()
    {
        sr.color = hitColor;
        yield return new WaitForSeconds(hitDarkenTime);
        sr.color = originalColor;
    }


    public void TakeDamage(int damage)
    {
        currentHP -= damage;

        if (currentHP <= 0)
            Die();
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
        if (expOrbPrefab == null) return;

        GameObject orb = Instantiate(
            expOrbPrefab,
            transform.position,
            Quaternion.identity
        );

        orb.GetComponent<ExpOrb>().value = expValue;
    }
}
