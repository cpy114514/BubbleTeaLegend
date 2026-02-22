using UnityEngine;

public class BattleManager : MonoBehaviour
{
    public static BattleManager Instance;
    public GameOverUI gameOverUI;

    [Header("Attack Prefabs")]
    public GameObject pearlProjectilePrefab;
    public GameObject grapeProjectilePrefab;
    public GameObject orangePrefab;
    public GameObject coconutProjectilePrefab;
    public GameObject lemonPrefab;
    public GameObject puddingPrefab;   // ⭐ 新增

    bool battleEnded = false;

    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        if (FindObjectOfType<ScoreManager>() == null)
            gameObject.AddComponent<ScoreManager>();

        Debug.Log("⚔ Battle Manager Awake");
    }

    void Start()
    {
        ScoreManager.Instance?.ResetScore();

        PlayerBattleData.ApplyFromRecorder();

        var player = GameObject.FindGameObjectWithTag("Player");

        // ===== Pearl =====
        if (PlayerBattleData.HasTopping("Pearl"))
        {
            var atk = player.AddComponent<PearlAttack>();
            atk.pearlPrefab = pearlProjectilePrefab;   // ⭐ 注入
        }

        // ===== Grape =====
        if (PlayerBattleData.HasTopping("Grape"))
        {
            var atk = player.AddComponent<GrapeAttack>();
            atk.projectilePrefab = grapeProjectilePrefab; // ⭐ 注入
        }

        // ===== Orange =====
        if (PlayerBattleData.HasTopping("Orange"))
        {
            var atk = player.AddComponent<OrangeOrbitAttack>();
            atk.orangePrefab = orangePrefab;
            atk.Init();   // ⭐⭐⭐ 这一行就是“缺失的真凶”
        }

        // ===== Coconut =====
        if (PlayerBattleData.HasTopping("Coconut"))
        {
            var atk = player.AddComponent<CoconutAttack>();
            atk.coconutPrefab = coconutProjectilePrefab;   // ⭐ 注入椰果子弹 prefab
        }

        // ===== Lemon =====
        if (PlayerBattleData.HasTopping("Lemon"))
        {
            var atk = player.AddComponent<LemonAttack>();
            atk.lemonPrefab = lemonPrefab;   // ⭐ 注入 prefab
        }

        
        // ===== 🍮 Pudding =====
        if (PlayerBattleData.HasTopping("Pudding"))
        {
            var atk = player.AddComponent<PuddingAttack>(); // ⭐ 关键：挂到 Player
            atk.puddingPrefab = puddingPrefab;              // Inspector 拖
        }







    }

    // =====================
    // Player Death
    // =====================
    public void OnPlayerDead()
    {
        if (battleEnded) return;
        battleEnded = true;

        if (ScoreManager.Instance != null)
            ScoreManager.Instance.OnBattleEnd();

        Debug.Log("💀 Battle Over");

        EnemySpawner spawner = FindObjectOfType<EnemySpawner>();
        if (spawner != null)
            spawner.enabled = false;

        EnemyMoveAI[] enemies = FindObjectsOfType<EnemyMoveAI>();
        foreach (var e in enemies)
            e.isDead = true;

        if (gameOverUI != null)
            gameOverUI.Show();

        Time.timeScale = 0f;
    }
}
