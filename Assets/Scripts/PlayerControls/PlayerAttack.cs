using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    void Update()
    {
        // 这里只保留升级调试用
        if (Input.GetKeyDown(KeyCode.L))
        {
            if (LevelUpPanel.Instance != null)
            {
                LevelUpPanel.Instance.Show();
            }
        }
    }

    // =====================
    // ⭐ 卸载所有战斗插件（关键）
    // =====================
    public void DisableAllBattleModules()
    {
        // ===== 主武器 =====
        PearlAttack pearl = GetComponent<PearlAttack>();
        if (pearl != null)
        {
            pearl.enabled = false;
        }

        GrapeAttack grape = GetComponent<GrapeAttack>();
        if (grape != null)
        {
            grape.enabled = false;
        }

        // ===== Toppings =====
        OrangeOrbitAttack orange = GetComponent<OrangeOrbitAttack>();
        if (orange != null)
        {
            orange.enabled = false;
        }

        CoconutAttack coconut = GetComponent<CoconutAttack>();
        if (coconut != null)
        {
            coconut.enabled = false;
        }


        LemonAttack lemon = GetComponent<LemonAttack>();
        if (lemon != null)
        {
            lemon.enabled = false;
        }


        
    }
}
