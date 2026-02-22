using UnityEngine;

public class PlayerExpController : MonoBehaviour
{
    public int level = 1;
    public int currentExp = 0;
    public int expToNext = 2;

    public System.Action<int> OnExpGained; // ⭐ 新增

    public void AddExp(int amount)
    {
        currentExp += amount;

        Debug.Log($"AddExp +{amount} = {currentExp}/{expToNext}");

        OnExpGained?.Invoke(amount); // ⭐ 通知粒子系统

        while (currentExp >= expToNext)
        {
            LevelUp();
        }

    }

    void LevelUp()
    {
        currentExp -= expToNext;
        level++;
        expToNext *= 2;

        Debug.Log($"LEVEL UP → Lv {level}, Next {expToNext}");

        ClearCupDrops();   // ⭐ 新增

        LevelUpPanel.Instance.Show();
    }

    void ClearCupDrops()
    {
        GameObject[] drops = GameObject.FindGameObjectsWithTag("ExpDrop");

        foreach (var d in drops)
            Destroy(d);
    }

}
