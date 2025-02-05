using UnityEngine;
using System;
using System.Linq;
using System.Collections;
/// <summary>
/// The basis of evil, and MORE EVIL
/// </summary>
public class Enemy_Base : Actor
{

    public float IFrames;
    public int IFrame_Ticker;
    public bool HasAi = true;
    public new void Start()
    {
        base.Start();
        Health = new ActorVitals(BaseHealth);
    }

    // Update is called once per frame
   public new void Update()
    {
        
        if (IFrames > 0)
        IFrames-=Time.deltaTime;
        if(IFrames<=0&&IFrame_Ticker>=0)
        {
        IFrame_Ticker--;
        }
    }

    public override void takeDamage(int damage)
    {
        Health.Health -= damage;
        IFrames=1/60f;
        IFrame_Ticker=40;
        // break the wall
        if (Health.Health <= 0)
        {
           Die();
        }
    }
    
    public override void Die()
    {
        base.Die();
        StopCoroutine(enemyBehavior);
    }

     void OnTriggerEnter2D(Collider2D col)
    {

      if (col.gameObject.CompareTag("Hitbox")&&IFrame_Ticker<=10){
           takeDamage(IFrame_Ticker<=0?100:10);
        }
    }
   
   /// <summary>
   /// The enemies ai
   /// </summary>
    protected IEnumerator enemyBehavior;
    
    protected void RemoveBrain()
    {
        StopCoroutine(enemyBehavior);
    }
   

}