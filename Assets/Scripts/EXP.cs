using UnityEngine;

public class ExpOrb : MonoBehaviour
{
    public int value = 1;

    public float attractRange = 2.5f;
    public float attractSpeed = 6f;

    Transform player;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        float dist =
            Vector2.Distance(transform.position, player.position);

        if (dist <= attractRange)
        {
            transform.position = Vector2.MoveTowards(
                transform.position,
                player.position,
                attractSpeed * Time.deltaTime
            );
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player"))
            return;

        PlayerExpController exp =
            other.GetComponent<PlayerExpController>();

        if (exp != null)
        {
            exp.AddExp(value);
            Destroy(gameObject);
        }
    }

}
