using System.Collections.Generic;

[System.Serializable]
public class DrinkData
{
    // ===== 基础 =====
    public bool hasMilk;

    // 倒过哪些茶
    public bool hasRedTea;
    public bool hasGreenTea;

    // 小料：名字 → 数量
    public Dictionary<string, int> toppings =
        new Dictionary<string, int>();

    // ===== 工具方法 =====
    public void AddTopping(string name, int count)
    {
        if (toppings.ContainsKey(name))
            toppings[name] += count;
        else
            toppings[name] = count;
    }
}
