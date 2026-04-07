using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerHealthParticles : MonoBehaviour
{
    public static PlayerHealthParticles Instance { get; private set; }

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

    readonly List<GameObject> particlePool = new();
    bool isDead = false;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        currentHP = maxHP;
        RefreshAll();
    }

    public void TakeDamage(int damage)
    {
        if (isDead)
            return;

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

    void Die()
    {
        if (isDead)
            return;

        isDead = true;
        Debug.Log("Player Died");

        PlayerMovement move = GetComponent<PlayerMovement>();
        if (move != null)
            move.enabled = false;

        DisableAllAttackModules();
        BattleManager.Instance?.OnPlayerDead();
    }

    void DisableAllAttackModules()
    {
        foreach (MonoBehaviour comp in GetComponents<MonoBehaviour>())
        {
            if (comp is PearlAttack ||
                comp is GrapeAttack ||
                comp is OrangeOrbitAttack)
            {
                comp.enabled = false;
            }
        }
    }

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
        if (bloodParticlePrefab == null || bloodContainer == null)
            return;

        EnsureParticlePool(currentHP);

        for (int i = 0; i < particlePool.Count; i++)
        {
            bool shouldBeVisible = i < currentHP;
            GameObject particle = particlePool[i];

            if (particle.activeSelf != shouldBeVisible)
                particle.SetActive(shouldBeVisible);

            if (shouldBeVisible)
                particle.transform.localPosition = CalculateLocalPos(i);
        }
    }

    Vector3 CalculateLocalPos(int index)
    {
        int x = index % particlesPerRow;
        int y = index / particlesPerRow;

        return new Vector3(
            (x - particlesPerRow / 2f) * spacing,
            y * spacing,
            0f
        );
    }

    void EnsureParticlePool(int targetCount)
    {
        while (particlePool.Count < targetCount)
        {
            GameObject particle = Instantiate(bloodParticlePrefab, bloodContainer);
            particlePool.Add(particle);
        }
    }

    void OnDestroy()
    {
        if (Instance == this)
            Instance = null;
    }
}
