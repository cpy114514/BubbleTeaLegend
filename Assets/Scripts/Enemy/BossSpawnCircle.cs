using UnityEngine;

public class CircleAroundBoss : MonoBehaviour
{
    Transform boss;
    Transform player;

    public float circleSpeed = 360f;
    public float circleTime = 2f;

    float timer;

    public void Init(Transform bossTransform)
    {
        boss = bossTransform;
    }

    void Start()
    {
        GameObject p =
            GameObject.FindGameObjectWithTag("Player");

        if (p != null)
            player = p.transform;
    }

    void Update()
    {
        if (boss == null) return;

        timer += Time.deltaTime;

        // µÚÒ»½×¶Î£ºÈÆÈ¦
        if (timer <= circleTime)
        {
            transform.RotateAround(
                boss.position,
                Vector3.forward,
                circleSpeed * Time.deltaTime
            );
        }
        else
        {
            // µÚ¶þ½×¶Î£º³åÏòÍæ¼Ò
            if (player == null) return;

            Vector2 dir =
                (player.position - transform.position).normalized;

            transform.position +=
                (Vector3)(dir * 3f * Time.deltaTime);

            Destroy(this); // É¾³ýÈÆÈ¦½Å±¾
        }
    }
}