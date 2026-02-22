using UnityEngine;
using System.Collections;

public class StationMoveController : MonoBehaviour
{
    public Transform stationRoot;

    [Header("Move Offset (Inspector decides direction & distance)")]
    public Vector3 milkMoveOffset;   // ⭐ 有奶时怎么移动
    public Vector3 teaMoveOffset;    // ⭐ 没奶时怎么移动

    public float moveDuration = 0.6f;

    bool moved = false;
    Coroutine moving;

    /// <summary>
    /// 摇完后调用，根据 hasMilk 选择偏移
    /// </summary>
    public void MoveAfterShake(bool hasMilk)
    {
        if (moved) return;

        Vector3 offset = hasMilk ? milkMoveOffset : teaMoveOffset;
        Vector3 target = stationRoot.position + offset;

        StartMove(target);
        moved = true;
    }

    void StartMove(Vector3 target)
    {
        if (moving != null)
            StopCoroutine(moving);

        moving = StartCoroutine(MoveRoutine(target));
    }

    IEnumerator MoveRoutine(Vector3 target)
    {
        Vector3 start = stationRoot.position;
        float t = 0f;

        while (t < 1f)
        {
            t += Time.deltaTime / moveDuration;
            stationRoot.position = Vector3.Lerp(start, target, t);
            yield return null;
        }

        stationRoot.position = target;
    }
}
