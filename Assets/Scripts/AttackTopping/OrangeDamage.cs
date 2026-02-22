using UnityEngine;

public class OrangeDamage : MonoBehaviour
{
    public int baseDamage = 2;
    public float hitCooldown = 0.03f;

    float lastHitTime;

    void OnTriggerStay2D(Collider2D other)
    {
        if (!other.CompareTag("Enemy"))
            return;

        if (Time.time - lastHitTime < hitCooldown)
            return;

        Enemy enemy = other.GetComponent<Enemy>();
        if (enemy == null)
            return;

        int dmg = baseDamage + PlayerBattleData.orangeDamageLv * 2;
        enemy.TakeDamage(dmg);

        lastHitTime = Time.time;
    }
}
