using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// 经验条 UI，显示当前经验 / 升级所需，以及还需多少经验。
/// </summary>
public class ExpBarUI : MonoBehaviour
{
    [Header("引用")]
    [Tooltip("填充条（普通 Image 即可，脚本会自动设为 Filled 横向填充）")]
    public Image fillImage;
    [Tooltip("可选：显示等级，如 Lv 3")]
    public TMP_Text levelText;
    [Tooltip("可选：显示「当前/所需」或「还需 X 经验」的文本")]
    public TMP_Text expText;

    [Header("显示格式")]
    [Tooltip("true=显示「5/16」，false=显示「还需 11 经验升级」")]
    public bool showFraction = true;

    PlayerExpController expController;

    void Start()
    {
        var player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
            expController = player.GetComponent<PlayerExpController>();

        if (expController == null)
            Debug.LogWarning("ExpBarUI: 找不到 Player 或 PlayerExpController");

        if (fillImage != null)
        {
            fillImage.type = Image.Type.Filled;
            fillImage.fillMethod = Image.FillMethod.Horizontal;
            fillImage.fillOrigin = (int)Image.OriginHorizontal.Left;
        }
    }

    void LateUpdate()
    {
        Refresh();
    }

    void Refresh()
    {
        if (expController == null) return;

        int cur = expController.currentExp;
        int need = expController.expToNext;
        int lv = expController.level;

        if (fillImage != null)
        {
            fillImage.fillAmount = need > 0 ? Mathf.Clamp01((float)cur / need) : 1f;
        }

        if (levelText != null)
            levelText.text = $"Lv {lv}";

        if (expText != null)
        {
            if (showFraction)
                expText.text = $"{cur} / {need}";
            else
                expText.text = need > 0 ? $"还需 {need - cur} 经验升级" : "已满级";
        }
    }
}
