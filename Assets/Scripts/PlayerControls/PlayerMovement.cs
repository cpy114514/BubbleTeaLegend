using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;

    Rigidbody2D rb;
    Vector2 input;
    Animator anim;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        input.x = Input.GetAxisRaw("Horizontal");
        input.y = Input.GetAxisRaw("Vertical");

        // ⭐ 用“未归一化”的 input 算动画速度
        anim.SetFloat("Speed", input.magnitude);

        // ⭐ 再归一化用于移动（斜着不更快）
        input = input.normalized;
    }

    void FixedUpdate()
    {
        rb.velocity = input * moveSpeed;
    }
}
