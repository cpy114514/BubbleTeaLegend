using UnityEngine;

public class LiquidParticle : MonoBehaviour
{
    public LiquidType type;

    // ⭐ 扩散半径：调这个就能控制“混得快不快”
    public float spreadRadius = 0.001f;

    private SpriteRenderer sr;
    int spreadLeft = 4;   // ⭐ 每个粒子最多扩散 2 次


    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        UpdateColor();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        LiquidParticle other = collision.collider.GetComponent<LiquidParticle>();
        if (other == null) return;

        TryMixWith(other);
    }

    void TryMixWith(LiquidParticle other)
    {
        if (type == other.type) return;

        // ===== 基础液体混合 =====

        // 奶 + 红茶 → 奶红茶
        if (IsPair(type, other.type, LiquidType.Milk, LiquidType.RedTea))
        {
            SetBoth(LiquidType.MilkRedTea, other);
            return;
        }

        // 奶 + 绿茶 → 奶绿茶
        if (IsPair(type, other.type, LiquidType.Milk, LiquidType.GreenTea))
        {
            SetBoth(LiquidType.MilkGreenTea, other);
            return;
        }

        // 红茶 + 绿茶 → 混合茶
        if (IsPair(type, other.type, LiquidType.RedTea, LiquidType.GreenTea))
        {
            SetBoth(LiquidType.MixedTea, other);
            return;
        }

        // ===== 混合态感染 =====

        if (IsMixed(type) && !IsMixed(other.type))
        {
            other.SetType(type);
            return;
        }

        if (IsMixed(other.type) && !IsMixed(type))
        {
            SetType(other.type);
            return;
        }
    }

    void SetType(LiquidType newType)
    {
        if (type == newType) return;

        type = newType;
        UpdateColor();

        // ⭐ 还有扩散次数，才继续扩散
        if (spreadLeft > 0)
        {
            spreadLeft--;
            SpreadToNearby();
        }
    }



    void SpreadToNearby()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(
            transform.position,
            spreadRadius
        );

        foreach (var hit in hits)
        {
            LiquidParticle lp = hit.GetComponent<LiquidParticle>();
            if (lp != null && lp.type != type)
            {
                if (Random.value < 0.01f)
                    lp.SetType(type);
            }

        }
    }

    void UpdateColor()
    {
        switch (type)
        {
            case LiquidType.Milk:
                sr.color = new Color(1f, 1f, 0.95f);
                break;

            case LiquidType.RedTea:
                sr.color = new Color(0.5f, 0.3f, 0.2f);
                break;

            case LiquidType.GreenTea:
                sr.color = new Color(0.4f, 0.6f, 0.4f);
                break;

            case LiquidType.MilkRedTea:
                sr.color = new Color(0.75f, 0.55f, 0.4f);
                break;

            case LiquidType.MilkGreenTea:
                sr.color = new Color(0.7f, 0.85f, 0.6f);
                break;

            case LiquidType.MixedTea:
                sr.color = new Color(0.45f, 0.45f, 0.35f);
                break;
        }
    }

    bool IsPair(LiquidType a, LiquidType b, LiquidType x, LiquidType y)
    {
        return (a == x && b == y) || (a == y && b == x);
    }

    bool IsMixed(LiquidType t)
    {
        return t == LiquidType.MilkRedTea
            || t == LiquidType.MilkGreenTea
            || t == LiquidType.MixedTea;
    }

    void SetBoth(LiquidType newType, LiquidParticle other)
    {
        SetType(newType);
        other.SetType(newType);
    }
}
