using UnityEngine;
/// <summary>
/// The basis of evil, and MORE EVIL
/// </summary>
public class Enemy_Base : Actor
{
    public int BaseHealth = 300;
    public float IFrames;
    public int IFrame_Ticker;
    public new void Start()
    {
        base.Start();
        Health = new ActorVitals(BaseHealth);
    }

    // Update is called once per frame
   public void Update()
    {
        if (this.IFrames > 0)
        IFrames-=Time.deltaTime;
        if(IFrames<=0&&IFrame_Ticker>=0)
        {
        IFrame_Ticker--;
        }
    }

    public void TakeDamage(int damage)
    {
        Debug.Log(damage);
        Health.Health -= damage;
        IFrames=1/60f;
        IFrame_Ticker=40;
        // break the wall
        if (Health.Health <= 0)
        {
           Die();
        }
    }

     void OnTriggerEnter2D(Collider2D col)
    {

      if (col.gameObject.CompareTag("Hitbox")&&IFrame_Ticker<=10){
           TakeDamage(IFrame_Ticker<=0?100:10);
        }
    }

}