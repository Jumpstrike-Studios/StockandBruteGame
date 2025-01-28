using UnityEngine;
using System;
using System.Linq;
using System.Collections.Generic;
/// <summary>
/// The basis of evil, and MORE EVIL
/// </summary>
public class Enemy_Test : Enemy_Base
{
    new public void Start()
    {
    base.Start();
    Health  = new ActorVitals(500);
    BeginState(STATE_BASE);
    }

    new void Update()
    {
        base.Update();
        rb.velocity = new Vector3(WalkSpeed, rb.velocity.y,0);
        if(MathF.Abs(WalkSpeed)>3f) WalkSpeed -=Time.deltaTime * Mathf.Sign(WalkSpeed)*10f;
    }

    public void Jump(){
    rb.velocity = Vector2.up * 3f;

    }
    /// <summary>
    /// This is a state. ask Aaron how it works.
    /// </summary>
    public Action<Enemy_Base> STATE_BASE = (Enemy_Base cpu) => { 


   };

}