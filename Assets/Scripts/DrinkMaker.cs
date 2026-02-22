using UnityEngine;

public class DrinkMaker : MonoBehaviour
{
    void Awake()
    {
        // ⭐ 进入制作界面时，初始化 Recorder 里的 drink
        DrinkRecorder.Instance.StartNewDrink();
    }

    // ====== 按钮调用的方法 ======

    public void AddMilk()
    {
        DrinkRecorder.Instance.currentDrink.hasMilk = true;
        Debug.Log("Add Milk");
    }

    public void AddRedTea()
    {
        DrinkRecorder.Instance.currentDrink.hasRedTea = true;
        Debug.Log("Add Red Tea");
    }

    public void AddGreenTea()
    {
        DrinkRecorder.Instance.currentDrink.hasGreenTea = true;
        Debug.Log("Add Green Tea");
    }

    public void AddPearl()
    {
        DrinkRecorder.Instance.currentDrink.AddTopping("Pearl", 1);
        Debug.Log("Add Pearl");
    }

    // 👉 如果你有 Orange 按钮，也必须是：
    public void AddOrange()
    {
        DrinkRecorder.Instance.currentDrink.AddTopping("Orange", 1);
        Debug.Log("Add Orange");
    }

    public void AddGrape()
    {
        DrinkRecorder.Instance.currentDrink.AddTopping("Grape", 1);
        Debug.Log("Add Grape");
    }
}
