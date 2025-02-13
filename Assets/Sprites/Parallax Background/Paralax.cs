using UnityEngine;

public class Paralax : MonoBehaviour
{
    public GameObject cam;
    private float length, spriteStartPos;
    public float paralax;
  

    // Start is called once before the first execution of Update
    void Start()
    { 
      length = GetComponent<SpriteRenderer>().bounds.size.x;
      spriteStartPos = transform.position.x;
    }
    void FixedUpdate()
    {
       float moving = (cam.transform.position.x * paralax);
       transform.position = new Vector3(spriteStartPos + moving, transform.position.y, transform.position.z);
       float temp = cam.transform.position.x * (1 + paralax);
      

    }
}
