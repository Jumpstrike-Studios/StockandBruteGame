using UnityEngine;

public class GrowingPlatform : MonoBehaviour
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
        transform.localScale += new Vector3(speed, 0) * dt;
    }
}