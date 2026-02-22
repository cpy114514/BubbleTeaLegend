using UnityEngine;
using System.Collections.Generic;

public class OrangeOrbitAttack : MonoBehaviour
{
    public GameObject orangePrefab;
    public int baseCount = 1;
    public float radius = 5f;
    public float rotateSpeed = 120f;

    List<GameObject> oranges = new List<GameObject>();

    void Start()
    {
        // ❌ 不在这里 Rebuild
    }

    // ⭐ 由 BattleManager 主动调用
    public void Init()
    {
        RebuildOranges();
    }

    public void RebuildOranges()
    {
        // 清空旧的
        foreach (var o in oranges)
            Destroy(o);
        oranges.Clear();

        if (orangePrefab == null)
        {
            Debug.LogError("Orange prefab missing");
            return;
        }

        int count = baseCount + PlayerBattleData.orangeCountLv;
        if (count <= 0) return;

        for (int i = 0; i < count; i++)
        {
            GameObject o = Instantiate(
                orangePrefab,
                transform
            );
            oranges.Add(o);
        }
    }

    void Update()
    {
        float angleStep = 360f / oranges.Count;
        for (int i = 0; i < oranges.Count; i++)
        {
            float angle =
                Time.time * rotateSpeed + angleStep * i;

            Vector3 pos = new Vector3(
                Mathf.Cos(angle * Mathf.Deg2Rad),
                Mathf.Sin(angle * Mathf.Deg2Rad),
                0
            ) * radius;

            oranges[i].transform.localPosition = pos;
        }
    }
}
