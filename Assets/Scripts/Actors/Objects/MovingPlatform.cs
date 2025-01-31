using UnityEngine;
using UnityEngine.UIElements;

public class MovingPlatform : MonoBehaviour
{
    public float curCountdown = 5;
    public float absCountdown = 5;
    public float speed = 1;

    void Update()
    {
        float dt = Time.deltaTime;
        curCountdown -= dt;
        if (curCountdown <= 0)
        {
            curCountdown = absCountdown;
            speed *= -1;
        }
        transform.position += new Vector3(speed, 0) * dt;
    }
}