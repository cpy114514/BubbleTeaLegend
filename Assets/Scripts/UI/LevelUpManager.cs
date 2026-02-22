using UnityEngine;
using System.Collections.Generic;

// 项目中只保留这一个 enum
public enum UpgradeType
{
    // 🧋 Pearl
    PearlFireRate,
    PearlDamage,
    PearlBounce,

    // 🍇 Grape
    GrapeScatterCount,
    GrapeFireRate,
    GrapeDamage,

    // 🍊 Orange
    OrangeCount,
    OrangeDamage,
    OrangeSpeed,

    // 🥥 Coconut
    CoconutDamage,
    CoconutFireRate,
    CoconutPierce,

    // ===== Lemon =====
    LemonCount,
    LemonFireRate,
    LemonDamage,
    LemonSize,

    // 🍮 Pudding
    PuddingCount,
    PuddingAttackSpeed,
    PuddingDamage,

}

public class LevelUpManager : MonoBehaviour
{
    public static LevelUpManager Instance;

    void Awake()
    {
        Instance = this;
    }

    // =====================
    // 抽 3 个升级
    // =====================
    public List<UpgradeType> Get3Upgrades()
    {
        List<UpgradeType> pool = BuildUpgradePool();
        List<UpgradeType> result = new();

        for (int i = 0; i < 3 && pool.Count > 0; i++)
        {
            int idx = Random.Range(0, pool.Count);
            result.Add(pool[idx]);
            pool.RemoveAt(idx);
        }

        return result;
    }

    // =====================
    // 构建升级池（关键规则）
    // =====================
    List<UpgradeType> BuildUpgradePool()
{
    List<UpgradeType> pool = new List<UpgradeType>();

        // ===== Pearl =====
        if (PlayerBattleData.HasTopping("Pearl"))
        {
            pool.Add(UpgradeType.PearlFireRate);
            pool.Add(UpgradeType.PearlDamage);
            pool.Add(UpgradeType.PearlBounce);
        }

        // ===== Grape =====
        if (PlayerBattleData.HasTopping("Grape"))
        {
            pool.Add(UpgradeType.GrapeScatterCount);
            pool.Add(UpgradeType.GrapeFireRate);
            pool.Add(UpgradeType.GrapeDamage);
        }

        // ===== Orange =====
        if (PlayerBattleData.HasTopping("Orange"))
        {
            pool.Add(UpgradeType.OrangeCount);
            pool.Add(UpgradeType.OrangeDamage);
            pool.Add(UpgradeType.OrangeSpeed);
        }

        // ===== 🥥 Coconut =====
        if (PlayerBattleData.HasTopping("Coconut"))
        {
            pool.Add(UpgradeType.CoconutDamage);
            pool.Add(UpgradeType.CoconutFireRate);
            pool.Add(UpgradeType.CoconutPierce);
        }

        // ===== 🍋 Lemon =====
        if (PlayerBattleData.HasTopping("Lemon"))
        {
            pool.Add(UpgradeType.LemonCount);
            pool.Add(UpgradeType.LemonFireRate);
            pool.Add(UpgradeType.LemonDamage);
            pool.Add(UpgradeType.LemonSize);
        }

        // ===== 🍮 Pudding =====
        if (PlayerBattleData.HasTopping("Pudding"))
        {
            pool.Add(UpgradeType.PuddingCount);
            pool.Add(UpgradeType.PuddingAttackSpeed);
            pool.Add(UpgradeType.PuddingDamage);
        }



        return pool;
    }

    // =====================
    // 应用升级
    // =====================
    public void ApplyUpgrade(UpgradeType type)
    {
        switch (type)
        {
            // ===== Pearl =====
            case UpgradeType.PearlFireRate:
                PlayerBattleData.pearlFireRateLv++;
                break;

            case UpgradeType.PearlDamage:
                PlayerBattleData.pearlDamageLv++;
                break;

            case UpgradeType.PearlBounce:
                PlayerBattleData.pearlBounceLv++;
                break;

            
            // ===== 🍇 Grape =====
            case UpgradeType.GrapeScatterCount:
                PlayerBattleData.grapeScatterCountLv++;
                break;

            case UpgradeType.GrapeFireRate:
                PlayerBattleData.grapeFireRateLv++;
                break;

            case UpgradeType.GrapeDamage:
                PlayerBattleData.grapeDamageLv++;
                break;


            // ===== Orange =====
            case UpgradeType.OrangeCount:
                PlayerBattleData.orangeCountLv++;
                FindObjectOfType<OrangeOrbitAttack>()?.RebuildOranges();
                break;

            case UpgradeType.OrangeDamage:
                PlayerBattleData.orangeDamageLv++;
                break;

            case UpgradeType.OrangeSpeed:
                PlayerBattleData.orangeSpeedLv++;
                break;

            //===== Coconut =====
            case UpgradeType.CoconutDamage:
                PlayerBattleData.coconutDamageLv++;
                break;

            case UpgradeType.CoconutFireRate:
                PlayerBattleData.coconutFireRateLv++;
                break;

            case UpgradeType.CoconutPierce:
                PlayerBattleData.coconutPierceLv++;
                break;
            // ===== Lemon =====
            case UpgradeType.LemonCount:
                PlayerBattleData.lemonCountLv++;
                break;

            case UpgradeType.LemonFireRate:
                PlayerBattleData.lemonFireRateLv++;
                break;

            case UpgradeType.LemonDamage:
                PlayerBattleData.lemonDamageLv++;
                break;

            case UpgradeType.LemonSize:
                PlayerBattleData.lemonSizeLv++;
                break;
            // ===== Pudding =====
            case UpgradeType.PuddingCount:
                PlayerBattleData.puddingCountLv++;
                FindObjectOfType<PuddingAttack>()?.Rebuild(); // ⭐ 关键
                break;


            case UpgradeType.PuddingAttackSpeed:
                PlayerBattleData.puddingAttackSpeedLv++;
                break;

            case UpgradeType.PuddingDamage:
                PlayerBattleData.puddingDamageLv++;
                break;



        }
    }
}
