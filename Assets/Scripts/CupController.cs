using UnityEngine;
using System.Collections;

public class CupController : MonoBehaviour
{
    [Header("Pour Rule")]
    public int maxPourCount = 2;
    int currentPourCount = 0;

    // ⭐ 是否加过奶（给小料台用）
    public bool hasMilk = false;

    // =====================
    // Liquid enter tracking
    // =====================
    int expectedLiquidCount = 0;   // 本次倒多少粒子
    int enteredLiquidCount = 0;    // 已进入杯子的粒子
    bool waitingForLiquid = false;

    public enum CupState
    {
        Pouring,
        Shaking,
        Topping
    }

    [Header("State")]
    public CupState state = CupState.Pouring;

    [Header("Shake")]
    public float shakeDuration = 0.4f;
    public float shakeAmplitude = 0.1f;
    public float shakeFrequency = 20f;

    Rigidbody2D rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // =====================
    // Pour control
    // =====================
    public bool CanPour()
    {
        if (state != CupState.Pouring) return false;
        if (waitingForLiquid) return false;   // ⭐ 上一种还没进完
        if (currentPourCount >= maxPourCount) return false;

        return true;
    }

    // ⭐ 开始倒一种液体时调用
    public void StartLiquidPour(int particleCount)
    {
        expectedLiquidCount = particleCount;
        enteredLiquidCount = 0;
        waitingForLiquid = true;
    }

    // ⭐ 粒子真正进入杯子时调用
    public void OnLiquidEntered()
    {
        if (!waitingForLiquid) return;

        enteredLiquidCount++;

        if (enteredLiquidCount >= expectedLiquidCount)
        {
            waitingForLiquid = false;
            OnLiquidFullyEntered();
        }
    }

    // ⭐ 这一种液体彻底进杯
    void OnLiquidFullyEntered()
    {
        currentPourCount++;

        // 第二种也倒完 → 摇杯
        if (currentPourCount >= maxPourCount)
        {
            state = CupState.Shaking;
            StartCoroutine(ShakeCup());
        }
        // 否则：现在才允许倒第二种
    }

    // =====================
    // Shake only
    // =====================
    IEnumerator ShakeCup()
    {
        Vector2 originalPos = rb.position;
        float t = 0f;

        while (t < shakeDuration)
        {
            float offset = Mathf.Sin(t * shakeFrequency) * shakeAmplitude;
            rb.MovePosition(originalPos + new Vector2(offset, 0f));

            t += Time.fixedDeltaTime;
            yield return new WaitForFixedUpdate();
        }

        rb.MovePosition(originalPos);

        // ⭐ 摇完 → 移动整个 station
        FindObjectOfType<StationMoveController>()
            .MoveAfterShake(hasMilk);


        state = CupState.Topping;
    }

    public void OnPoured()
    {
        currentPourCount++;

        if (currentPourCount >= maxPourCount)
        {
            state = CupState.Shaking;
            StartCoroutine(ShakeCup());
        }
    }

}
