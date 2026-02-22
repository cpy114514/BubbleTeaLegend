using UnityEngine;

public class ToppingButton : MonoBehaviour
{
    public ToppingSpawner spawner;
    public GameObject toppingPrefab;

    [Header("Topping Settings")]
    public int spawnCount = 12;   // ⭐ 不同小料填不同值

    bool used = false; // 可选：限制只能加一次

    public void OnClick()
    {
        if (used) return;

        spawner.SpawnTopping(toppingPrefab, spawnCount);

        // ⭐ 记录进配方
        DrinkRecorder.Instance.currentDrink
            .AddTopping(toppingPrefab.name, spawnCount);

        used = true;
    }

}
