using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;

public class LevelUpPanel : MonoBehaviour
{
    public static LevelUpPanel Instance;

    public GameObject panel;
    public Button[] buttons;

    // TextMeshPro - Text (UI)
    public TextMeshProUGUI[] buttonTexts;

    List<UpgradeType> currentOptions;

    void Awake()
    {
        Instance = this;
        panel.SetActive(false);
    }

    // =====================
    // 显示 3 个升级选项
    // =====================
    public void Show()
    {
        Debug.Log("LEVEL UP PANEL SHOW");

        Time.timeScale = 0f;
        panel.SetActive(true);

        currentOptions = LevelUpManager.Instance.Get3Upgrades();

        for (int i = 0; i < buttons.Length; i++)
        {
            if (i < currentOptions.Count)
            {
                buttons[i].gameObject.SetActive(true);
                buttonTexts[i].text = GetUpgradeText(currentOptions[i]);
            }
            else
            {
                buttons[i].gameObject.SetActive(false);
            }
            if (currentOptions == null || currentOptions.Count == 0)
            {
                Debug.LogWarning("No upgrades available!");
                panel.SetActive(false);
                Time.timeScale = 1f;
                return;
            }
        }
    }

    // =====================
    // 玩家选择
    // =====================
    void SelectUpgrade(int index)
    {
        Debug.Log("Upgrade selected index = " + index);

        UpgradeType type = currentOptions[index];
        LevelUpManager.Instance.ApplyUpgrade(type);

        panel.SetActive(false);
        Time.timeScale = 1f;
    }

    // 这三个函数在 Button OnClick 里绑定
    public void Select0() => SelectUpgrade(0);
    public void Select1() => SelectUpgrade(1);
    public void Select2() => SelectUpgrade(2);

    // =====================
    // 文案（英文，和新 enum 对齐）
    // =====================
    string GetUpgradeText(UpgradeType type)
    {
        switch (type)
        {
            // ===== Pearl =====
            case UpgradeType.PearlFireRate:
                return $"Pearl Fire Rate (Lv {PlayerBattleData.pearlFireRateLv} → {PlayerBattleData.pearlFireRateLv + 1})";

            case UpgradeType.PearlDamage:
                return $"Pearl Damage (Lv {PlayerBattleData.pearlDamageLv} → {PlayerBattleData.pearlDamageLv + 1})";

            case UpgradeType.PearlBounce:
                return $"Pearl Bounce +1 (Lv {PlayerBattleData.pearlBounceLv} → {PlayerBattleData.pearlBounceLv + 1})";

            // ===== Grape =====
            case UpgradeType.GrapeScatterCount:
                return $"Grape Scatter +1 (Lv {PlayerBattleData.grapeScatterCountLv} → {PlayerBattleData.grapeScatterCountLv + 1})";
            case UpgradeType.GrapeDamage:
                return $"Grape Damage +1 (Lv {PlayerBattleData.grapeDamageLv} → {PlayerBattleData.grapeDamageLv + 1})";
            case UpgradeType.GrapeFireRate:
                return $"Grape Fire Rate (Lv {PlayerBattleData.grapeFireRateLv} → {PlayerBattleData.grapeFireRateLv + 1})";

            // ===== Orange =====
            case UpgradeType.OrangeCount:
                return $"Orange Count +1 (Lv {PlayerBattleData.orangeCountLv} → {PlayerBattleData.orangeCountLv + 1})";

            case UpgradeType.OrangeDamage:
                return $"Orange Damage (Lv {PlayerBattleData.orangeDamageLv} → {PlayerBattleData.orangeDamageLv + 1})";

            case UpgradeType.OrangeSpeed:
                return $"Orange Speed (Lv {PlayerBattleData.orangeSpeedLv} → {PlayerBattleData.orangeSpeedLv + 1})";

            // ===== 🥥 Coconut =====
            case UpgradeType.CoconutDamage:
                return $"Coconut Damage (Lv {PlayerBattleData.coconutDamageLv} → {PlayerBattleData.coconutDamageLv + 1})";

            case UpgradeType.CoconutFireRate:
                return $"Coconut Fire Rate (Lv {PlayerBattleData.coconutFireRateLv} → {PlayerBattleData.coconutFireRateLv + 1})";

            case UpgradeType.CoconutPierce:
                return $"Coconut Pierce +1 (Lv {PlayerBattleData.coconutPierceLv} → {PlayerBattleData.coconutPierceLv + 1})";

            // ===== 🍋 Lemon =====
            case UpgradeType.LemonCount:
                return $"Lemon Count +1 (Lv {PlayerBattleData.lemonCountLv} → {PlayerBattleData.lemonCountLv + 1})";

            case UpgradeType.LemonFireRate:
                return $"Lemon Fire Rate (Lv {PlayerBattleData.lemonFireRateLv} → {PlayerBattleData.lemonFireRateLv + 1})";

            case UpgradeType.LemonDamage:
                return $"Lemon Damage (Lv {PlayerBattleData.lemonDamageLv} → {PlayerBattleData.lemonDamageLv + 1})";

            case UpgradeType.LemonSize:
                return $"Lemon Size (Lv {PlayerBattleData.lemonSizeLv} → {PlayerBattleData.lemonSizeLv + 1})";
            // ===== 🍮 Pudding =====
            
            case UpgradeType.PuddingCount:
                return $"Pudding Count +1 (Lv {PlayerBattleData.puddingCountLv} → {PlayerBattleData.puddingCountLv + 1})";

            case UpgradeType.PuddingAttackSpeed:
                return $"Pudding Attack Speed (Lv {PlayerBattleData.puddingAttackSpeedLv} → {PlayerBattleData.puddingAttackSpeedLv + 1})";

            case UpgradeType.PuddingDamage:
                return $"Pudding Damage (Lv {PlayerBattleData.puddingDamageLv} → {PlayerBattleData.puddingDamageLv + 1})";



        }

        // ⭐ 永远不要省这个
        return "Unknown Upgrade";
    }

}
