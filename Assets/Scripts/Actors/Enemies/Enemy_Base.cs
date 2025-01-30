using UnityEngine;
using System;
using System.Linq;
using System.Collections.Generic;
/// <summary>
/// The basis of evil, and MORE EVIL
/// </summary>
public class Enemy_Base : Actor
{

    public float IFrames;
    public int IFrame_Ticker;

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

        if(Actions.Count==0) return;
        Actions[^1].StateTimer.Reset();
        Actions[^1].Function.Invoke(this);
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
    /// <summary>
    /// The wait timer for the next part of a state
    /// </summary>
    public struct YieldState{
        public int YieldTimer_Now;
        public List<float> YieldTimer_Before;
    public void Reset() {YieldTimer_Now =1;
    }  
    public void FullReset() {YieldTimer_Now =1;
    YieldTimer_Before.Clear();
    }  
    public void Next() {YieldTimer_Now +=1;}    
    }
    
    public struct AI_State{
        public YieldState StateTimer;
        public Action<Enemy_Base> Function;
    }
    public bool[] Yield(float wait){return Sleep(wait);}
    public bool[] Sleep(float wait){
        
        YieldState YS = Actions[^1].StateTimer;
        if (YS.YieldTimer_Before.Count==0||(YS.YieldTimer_Before[YS.YieldTimer_Now]<=0 && YS.YieldTimer_Before.Count==YS.YieldTimer_Now)) Actions[Actions.Count-1].StateTimer.YieldTimer_Before.Add(wait);
        
        //first, check if the timer has surpassed this point in time

       if(YS.YieldTimer_Before[YS.YieldTimer_Now]-Time.deltaTime<=0&&!(YS.YieldTimer_Before[YS.YieldTimer_Now]<=0)){
        Actions[^1].StateTimer.Next();
        return new bool[2]{false,YS.YieldTimer_Before.Count==YS.YieldTimer_Now+1};}
        YS.YieldTimer_Before[YS.YieldTimer_Now]-= Time.deltaTime;
       return new bool[2]{true,false};

    }

     void OnTriggerEnter2D(Collider2D col)
    {

      if (col.gameObject.CompareTag("Hitbox")&&IFrame_Ticker<=10){
           takeDamage(IFrame_Ticker<=0?100:10);
        }
    }

   
   /// <summary>
   /// The nested set of actions
   /// </summary>
    public List<AI_State> Actions= new List<AI_State>();
   

    public void BeginState(Action<Enemy_Base> newstate){
        if (newstate is null) throw new Exception("Null State prevention ENGAGED!");
        if(Actions.Count>=1) {
        foreach (AI_State item in Actions)
        {
           if (item.Function==newstate)throw new Exception("Nested State prevention ENGAGED!");
           
        }}
        Debug.Log("NEW STATE STARTED");
        var F = new AI_State
        {
            Function = newstate,
            StateTimer = new YieldState{YieldTimer_Before=new List<float>{}}
        };
        Actions.Add(F);
    }
    /// <summary>
    /// ends the current active state (required with any state)
    /// </summary>
       public void EndState(){
        
       Actions.RemoveAt(Actions.Count-1);

    }


}