using UnityEngine;

public class RemoveSlimeBallOnGround : MonoBehaviour
{

    public CircleCollider2D SlimeBallCollider;
    public Rigidbody2D rb;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        SlimeBallCollider = GetComponent<CircleCollider2D>();
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = Vector2.right;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        this.gameObject.SetActive(false);
    }
}
