using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections;

/// <summary>
/// 全局计分系统（跨场景常驻）
/// </summary>
public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance { get; private set; }

    [Header("生存时间计分")]
    public float pointsPerSecond = 5f;

    [Header("分数显示")]
    [Tooltip("拖入分数文本，或留空则自动查找名为 ScoreText 的对象")]
    public TMP_Text scoreText;

    int killScore;
    float survivalTime;
    bool battleEnded;

    // ================================
    // 对外属性
    // ================================
    public int TotalScore =>
        killScore + Mathf.FloorToInt(survivalTime * pointsPerSecond);

    public int KillScore => killScore;
    public float SurvivalTime => survivalTime;

    // ================================
    // 初始化
    // ================================
    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
        TryBindScoreText();

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    // ================================
    // 场景切换时自动重新绑定 UI
    // ================================
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        StartCoroutine(RebindScoreText());
    }

    IEnumerator RebindScoreText()
    {
        yield return null;
        TryBindScoreText();
    }

    void TryBindScoreText()
    {
        if (scoreText != null) return;

        var go = GameObject.Find("ScoreText");
        if (go != null)
            scoreText = go.GetComponent<TMP_Text>();
    }

    // ================================
    // 每帧更新
    // ================================
    void Update()
    {
        if (!battleEnded)
            survivalTime += Time.deltaTime;

        RefreshScoreDisplay();
    }

    // ================================
    // 外部接口
    // ================================
    public void AddKillScore(int score)
    {
        if (battleEnded) return;
        killScore += score;
    }

    public void OnBattleEnd()
    {
        battleEnded = true;
        RefreshScoreDisplay();
    }

    public void ResetScore()
    {
        killScore = 0;
        survivalTime = 0f;
        battleEnded = false;
        RefreshScoreDisplay();
    }

    // ================================
    // 内部刷新
    // ================================
    void RefreshScoreDisplay()
    {
        if (scoreText != null)
            scoreText.text = TotalScore.ToString();
    }
}
