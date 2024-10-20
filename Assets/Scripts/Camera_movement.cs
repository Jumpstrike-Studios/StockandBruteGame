using System;
using UnityEngine;

public class Camera_movement : MonoBehaviour
{

    public GameObject player;
    public float cameraSpeed;
    [Range(0f, 1f)]
    public float cameraPadding;
    [Range(0f, 1f)]
    public float cameraMargin;

    //camera is 16 units wide.
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {   
        //get the distance between the player and the camera, and use it as a multiplier
        float CameraWorkout = Mathf.Clamp(Vector2.Distance((Vector2)transform.position,(Vector2)player.transform.position)/(8f*(1f-cameraPadding))-cameraMargin,0f,1f);
        Vector2 new_position = Vector2.Lerp((Vector2)transform.position, (Vector2)player.transform.position, (cameraSpeed*CameraWorkout) * Time.deltaTime);
        transform.position = new Vector3(new_position.x, new_position.y, -1f);
    }
}
