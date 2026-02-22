using System.Collections.Generic;
using UnityEngine;

public static class PlayerBattleData
{
    // =====================
    // Topping Presence
    // =====================
    static HashSet<string> toppings = new HashSet<string>();

    // =====================
    // Pearl Upgrades
    // =====================
    public static int pearlFireRateLv;
    public static int pearlDamageLv;
    public static int pearlBounceLv;

    // =====================
    // Grape Upgrades
    // =====================
    public static int grapeFireRateLv;
    public static int grapeDamageLv;
    public static int grapeScatterCountLv;   // ⭐ 正确名字

    // =====================
    // 🍊 Orange Upgrades
    // =====================
    public static int orangeCountLv;
    public static int orangeDamageLv;
    public static int orangeSpeedLv;

    // 🥥 Coconut upgrades
    public static int coconutDamageLv;
    public static int coconutFireRateLv;
    public static int coconutPierceLv;

    // 🍋 Lemon
    public static int lemonCountLv;
    public static int lemonFireRateLv;
    public static int lemonDamageLv;
    public static int lemonSizeLv;

    // 🍮 Pudding
    // BattleData.cs（示意）
    public static int puddingCountLv;        // 小布丁数量等级
    public static int puddingAttackSpeedLv;  // 攻速等级（近战 + 远程）
    public static int puddingDamageLv;        // 伤害等级（近战 + 远程）




    // =====================
    // Core
    // =====================
    public static void Reset()
    {
        toppings.Clear();

        pearlFireRateLv = 0;
        pearlDamageLv = 0;
        pearlBounceLv = 0;

        grapeFireRateLv = 0;
        grapeDamageLv = 0;
        grapeScatterCountLv = 0;

        orangeCountLv = 0;
        orangeDamageLv = 0;
        orangeSpeedLv = 0;

        coconutDamageLv = 0;
        coconutFireRateLv = 0;
        coconutPierceLv = 0;

        lemonCountLv = 0;
        lemonFireRateLv = 0;
        lemonDamageLv = 0;
        lemonSizeLv = 0;

        puddingCountLv = 0;
        puddingAttackSpeedLv = 0;
        puddingDamageLv = 0;
    }

    public static bool HasTopping(string name)
    {
        return toppings.Contains(name);
    }

    public static void ApplyFromRecorder()
    {
        Reset();

        if (DrinkRecorder.Instance == null)
        {
            Debug.LogError("DrinkRecorder missing!");
            return;
        }

        var drink = DrinkRecorder.Instance.currentDrink;
        if (drink == null)
        {
            Debug.LogError("Drink is NULL");
            return;
        }

        Debug.Log("Drink toppings count = " +
            (drink.toppings == null ? "NULL" : drink.toppings.Count.ToString()));

        if (drink.toppings == null) return;

        foreach (var kv in drink.toppings)
        {
            Debug.Log($"Topping read: {kv.Key} = {kv.Value}");
            if (kv.Value > 0)
                toppings.Add(kv.Key);
        }

        Debug.Log("Battle toppings: " + string.Join(",", toppings));
    }

}
