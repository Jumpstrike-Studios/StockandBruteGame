using Unity.VisualScripting;
using UnityEngine;



// YES, I KNOW THIS SCRIPT IS NOT DONE. I AM WORKING ON IT. PLEASE DO NOT DELETE IT. -Gryphon
public class StockMovement : MonoBehaviour
{

    public Actor_Player actorplayer;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

        // Initalizes distance to ground
        float distToGround = 0;

        // Checks the shift is pressed
        if (Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.RightShift))
        {
            if (Input.GetKeyDown(KeyCode.D))
            {

            }
        }
    }
}
