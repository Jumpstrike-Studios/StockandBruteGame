using UnityEngine;

public class Camera_movement : MonoBehaviour
{

    public GameObject player;
    public float camera_speed;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 new_position = Vector2.Lerp(new Vector2(transform.position.x, transform.position.y), new Vector2(player.transform.position.x, player.transform.position.y), camera_speed * Time.deltaTime);
        transform.position = new Vector3(new_position.x, new_position.y, -1f);
    }
}
