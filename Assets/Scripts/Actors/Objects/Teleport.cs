using UnityEngine;

public class Teleport : MonoBehaviour
{
    public CircleCollider2D circle;
    public GameObject player;
    public Actor_Player actor;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        actor = player.GetComponent<Actor_Player>();
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "Player")
        {
            player.transform.position = new Vector3(-2.75f, 71.2f, 0);
        }
    }
}
