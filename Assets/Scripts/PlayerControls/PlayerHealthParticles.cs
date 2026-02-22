using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class PlayerHealthParticles : MonoBehaviour
{
    [Header("Health")]
    public int maxHP = 100;
    public int currentHP;

    [Header("Particles (Visual Only)")]
    public GameObject bloodParticlePrefab;
    public Transform bloodContainer;

    [Header("Layout")]
    public int particlesPerRow = 10;
    public float spacing = 0.12f;

    [Header("UI")]
    public TextMeshProUGUI hpText;

    List<GameObject> particles = new List<GameObject>();
    bool isDead = false;

    void Start()
    {
        currentHP = maxHP;
        RefreshAll();
    }

    // =====================
    // Public API
    // =====================
    public void TakeDamage(int damage)
    {
        if (isDead) return;

        currentHP = Mathf.Max(0, currentHP - damage);
        RefreshAll();

        if (currentHP <= 0)
            Die();
    }

    public void Heal(int amount)
    {
        currentHP = Mathf.Min(maxHP, currentHP + amount);
        RefreshAll();
    }

    // =====================
    // Death
    // =====================
    void Die()
    {
        if (isDead) return;
        isDead = true;

        Debug.Log("☠ Player Died");

        // 1️⃣ 禁止移动
        PlayerMovement move = GetComponent<PlayerMovement>();
        if (move != null)
            move.enabled = false;

        // 2️⃣ 禁止所有攻击模块（新架构安全写法）
        DisableAllAttackModules();

        // 3️⃣ 通知战斗系统
        BattleManager.Instance?.OnPlayerDead();
    }

    void DisableAllAttackModules()
    {
        foreach (var comp in GetComponents<MonoBehaviour>())
        {
            if (comp is PearlAttack ||
                comp is GrapeAttack ||
                comp is OrangeOrbitAttack)
            {
                comp.enabled = false;
            }
        }
    }

    // =====================
    // Visual Refresh
    // =====================
    void RefreshAll()
    {
        RefreshParticles();
        RefreshText();
    }

    void RefreshText()
    {
        if (hpText != null)
            hpText.text = $"{currentHP} / {maxHP}";
    }

    void RefreshParticles()
    {
        // 清空旧粒子
        foreach (var p in particles)
            Destroy(p);

        particles.Clear();

        // 生成新粒子
        for (int i = 0; i < currentHP; i++)
        {
            if (bloodParticlePrefab == null || bloodContainer == null)
                return;

            GameObject p = Instantiate(bloodParticlePrefab, bloodContainer);
            p.transform.localPosition = CalculateLocalPos(i);
            particles.Add(p);
        }
    }

    Vector3 CalculateLocalPos(int index)
    {
        int x = index % particlesPerRow;
        int y = index / particlesPerRow;

        return new Vector3(
            (x - particlesPerRow / 2f) * spacing,
            y * spacing,
            0
        );
    }
}
