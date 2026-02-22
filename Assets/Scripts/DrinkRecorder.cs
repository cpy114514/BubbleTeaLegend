using UnityEngine;
using UnityEngine.UIElements;

public class DrinkRecorder : MonoBehaviour
{
    public GameObject toppingButton; // ⭐ 要控制的按钮

    public static DrinkRecorder Instance;

    public DrinkData currentDrink;

    void Update()
    {
        UpdateToppingButton();
    }

    public static void DestroyInstance()
    {
        if (Instance != null)
        {
            Destroy(Instance.gameObject);
            Instance = null;
        }
    }

    void UpdateToppingButton()
    {
        if (toppingButton == null) return;

        if (currentDrink != null && currentDrink.toppings.Count > 0)
            toppingButton.SetActive(true);
        else
            toppingButton.SetActive(false);
    }


    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        // ❌ 不要在 Awake 里 StartNewDrink
        // StartNewDrink();
    }

    public void Reset()
    {
        currentDrink = null;
    }

    // ⭐ 只在“进入制作界面”时调用
    public void StartNewDrink()
    {
        currentDrink = new DrinkData();

        // ⭐⭐⭐ 关键：新一局，清空战斗数据
        PlayerBattleData.Reset();
        UpdateToppingButton(); // ⭐ 新一局，按钮默认隐藏
        Debug.Log("🧋 New Drink + New Battle Data");
    }

}
