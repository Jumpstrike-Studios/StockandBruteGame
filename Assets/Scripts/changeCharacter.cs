using UnityEngine;

public class changeCharacter : MonoBehaviour
{

    public SpriteRenderer spriteRenderer;
    public Sprite Stock_Sprite;
    public Sprite Brute_Sprite;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("q"))
        {
            if (spriteRenderer.sprite == Brute_Sprite)
            {
                spriteRenderer.sprite = Stock_Sprite;
            }
            else
            {
                spriteRenderer.sprite = Brute_Sprite;
            }
        }


    }
}
