using UnityEngine;

public class PlayerTakeDamage : MonoBehaviour
{
    BoxCollider2D PlayerCollider;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        PlayerCollider = GetComponent<BoxCollider2D>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
    }
}
