using Unity.VisualScripting;
using UnityEngine;

public class SlimeBallPhysics : MonoBehaviour
{
    // Gameobject of the player
    public GameObject Player;

    public float gravityScale;

    public Vector2 slimeVelocity;

    // Defines the time that it takes for the slime ball to hit the player
    public float hitTime;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

        // Calculates the exact velocity at which the slimeball needs to move in order to hit the player
        slimeVelocity = new Vector2((Player.transform.position.x - transform.position.x) / hitTime,
            (Player.transform.position.y - transform.position.y + 0.5f * gravityScale * hitTime * hitTime) / hitTime);

    }


    // Update is called once per frame
    void Update()
    {
        // Changes position based off of velocity
        transform.position += new Vector3(slimeVelocity.x, slimeVelocity.y, 0f) * Time.deltaTime;

        //Accelerates the slime with gravity
        slimeVelocity -= new Vector2(0f, gravityScale) * Time.deltaTime;
    }
}
