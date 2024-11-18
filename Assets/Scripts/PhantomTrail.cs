using UnityEngine;

public class PhantomTrail : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public float StartAlpha=0.8f;
    public float LifeTime=1.0f;

    private float MaxLife;
    private bool antivoid=true;

    void Start()
    {
        StartAlpha=0.8f;
        LifeTime=1.0f;
        MaxLife=LifeTime;
    }

    // Update is called once per frame
    void Update()
    {
        if(antivoid)
        {
            antivoid=false;
            return;
        }
        if (LifeTime < 0)
        {
            Destroy(gameObject);
        }else
        {
            gameObject.GetComponent<SpriteRenderer>().color = new Color(1,1,1,LifeTime*StartAlpha);
        }
 
        LifeTime-=1*Time.deltaTime;

    }
}
