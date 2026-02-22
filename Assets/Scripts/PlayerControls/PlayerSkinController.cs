using UnityEngine;

public class PlayerSkinController : MonoBehaviour
{
    public GameObject milkTeaSkin;   // 奶茶外观
    public GameObject fruitTeaSkin;  // 果茶外观

    void Start()
    {
        ApplySkin();
    }

    public void ApplySkin()
    {
        if (DrinkRecorder.Instance == null ||
            DrinkRecorder.Instance.currentDrink == null)
            return;

        bool isMilkTea =
            DrinkRecorder.Instance.currentDrink.hasMilk;

        milkTeaSkin.SetActive(isMilkTea);
        fruitTeaSkin.SetActive(!isMilkTea);
    }
}