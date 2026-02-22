using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameOverUI : MonoBehaviour
{
    public GameObject panel;
    [Tooltip("可选：显示最终得分的文本。不赋值则不在 GameOver 时显示分数")]
    public TMP_Text scoreText;

    void Awake()
    {
        panel.SetActive(false);
    }

    // ⭐ 给 BattleManager 调用
    public void Show()
    {
        if (scoreText != null && ScoreManager.Instance != null)
        {
            int total = ScoreManager.Instance.TotalScore;
            int kill = ScoreManager.Instance.KillScore;
            float time = ScoreManager.Instance.SurvivalTime;
            int timeScore = Mathf.FloorToInt(time * ScoreManager.Instance.pointsPerSecond);
            scoreText.text = $"得分: {total}\n击杀: {kill}\n生存: {time:F1}秒 (+{timeScore})";
        }
        panel.SetActive(true);
        Time.timeScale = 0f; // 死亡时暂停（推荐）
    }

    // ===== 按钮 =====

    // 🔁 重来一局战斗（保留当前奶茶）
    public void RestartBattle()
    {
        Time.timeScale = 1f;

        // ⚠️ 不 Reset（因为是同一杯奶茶）
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    // 🧋 回到制作界面（必须清空战斗数据）
    public void BackToDrink()
    {
        Time.timeScale = 1f;

        // 1. 卸载所有战斗插件
        PlayerAttack pa = FindObjectOfType<PlayerAttack>();
        if (pa != null)
        {
            pa.DisableAllBattleModules();
        }

        // 2. 清空战斗数据
        PlayerBattleData.Reset();

        // 3. 清空奶茶数据
        if (DrinkRecorder.Instance != null)
        {
            DrinkRecorder.Instance.Reset();
        }

        // 4. 回到制作界面
        SceneManager.LoadScene("BubbleTeaMake");
    }


}
