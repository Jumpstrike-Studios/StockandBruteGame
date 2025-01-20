using System;
using Unity.VisualScripting;
using UnityEngine;

public class SlimeBallPhysics : Projectile_Base
{
    // Gameobject of the player
    public Vector2 Target;

    // Defines the time that it takes for the slime ball to hit the player
    public float hitTime;
    private float Rotate;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GravityScale = 10f;
        // Calculates the exact velocity at which the slimeball needs to move in order to hit the player
        Velocity = new Vector2((Target.x - transform.position.x) / hitTime,
            (Target.y - transform.position.y + 0.5f * GravityScale * hitTime * hitTime) / hitTime);

    }


    // Update is called once per frame
    void Update()
    {
        Rotate=270*Time.deltaTime*Mathf.Sign(Velocity.x);
        // Changes position based off of velocity
        transform.position += new Vector3(Velocity.x, Velocity.y, 0f) * Time.deltaTime;
        transform.Rotate(0,0,Rotate);

        //Accelerates the slime with gravity
        Velocity -= new Vector2(0f, GravityScale) * Time.deltaTime;
    }
}
