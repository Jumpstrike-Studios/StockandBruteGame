using UnityEngine;
/// <summary>
/// The basis of evil, and MORE EVIL
/// </summary>
public class Projectile_Base : Actor
{
    public float Size;
    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
        Destroy(gameObject);
        }
    }
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
        Destroy(gameObject);
        }
    }
}