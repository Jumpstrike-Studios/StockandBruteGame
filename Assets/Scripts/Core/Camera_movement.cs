using System;
using Unity.VisualScripting;
using UnityEngine;

public class Camera_movement : MonoBehaviour
{

    public GameObject player;

    public GameObject GameoverHud;
    public float cameraSpeed;
    [Range(0f, 1f)]
    public float cameraPadding;
    [Range(0f, 1f)]
    public float cameraMargin;
    public Vector2 cameraOffset;

    //camera is 16 units wide.
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Actor_Player.OnGameOver += Show;
    }
    public void Show(bool real)
    {
        if (GameoverHud.IsDestroyed())return;
        GameoverHud.SetActive(true);
    }
    // Update is called once per frame
    void Update()
    {   
        //get the distance between the player and the camera, and use it as a multiplier
        //float CameraWorkout = Mathf.Clamp(Vector2.Distance((Vector2)transform.position,(Vector2)player.transform.position)/(8f*(1f-cameraPadding))-cameraMargin,0f,1f);
        //Vector2 new_position = Vector2.Lerp((Vector2)transform.position, (Vector2)player.transform.position, (cameraSpeed*CameraWorkout) * Time.deltaTime);
        if(player.GetComponent<Actor_Player>().Duo_Dead)return;
        Vector2 new_position = new Vector2(Mathf.Clamp(transform.position.x,player.transform.position.x-5,player.transform.position.x+5),Mathf.Clamp(transform.position.y,player.transform.position.y-1.3f,player.transform.position.y+1.8f))+cameraOffset;
        transform.position = new Vector3(new_position.x, new_position.y, -1f);
    }
}
