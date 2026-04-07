using UnityEngine;
using TMPro;

public class DamageText : MonoBehaviour
{
    public float moveSpeed = 2f;
    public float lifeTime = 0.8f;

    TextMeshPro text;
    float timer;

    void Awake()
    {
        text = GetComponent<TextMeshPro>();
    }

    public void SetDamage(int damage)
    {
        text.text = damage.ToString();
    }

    void Update()
    {
        timer += Time.deltaTime;

        // ÉÏÆ® + Î¢Ëæ»ú×óÓÒ
        transform.position +=
            (Vector3.up + Vector3.right * Random.Range(-0.3f, 0.3f))
            * moveSpeed * Time.deltaTime;

        // ½¥Òþ
        float alpha = Mathf.Lerp(1f, 0f, timer / lifeTime);
        text.color = new Color(text.color.r, text.color.g, text.color.b, alpha);

        if (timer >= lifeTime)
            Destroy(gameObject);
    }
}