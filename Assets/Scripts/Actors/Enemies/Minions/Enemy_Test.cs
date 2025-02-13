using UnityEngine;
using System;
using System.Collections;
/// <summary>
/// A dummy enemy
/// </summary>
public class Enemy_Test : Enemy_Base
{
    float touchwallcooldown = 0f;
    new public void Start()
    {
    base.Start();
    Health  = new ActorVitals(100);
    enemyBehavior = AI();
    StartCoroutine(enemyBehavior);
    }

    new void Update()
    {
        base.Update();
        rb.velocity = new Vector3(WalkSpeed, rb.velocity.y,0);
        if(MathF.Abs(WalkSpeed)>3f) WalkSpeed -=Time.deltaTime * Mathf.Sign(WalkSpeed)*10f;
        if(Is_OnWall && touchwallcooldown<=0f) {WalkSpeed *=-1f;
        touchwallcooldown = 0.05f;
        }
        touchwallcooldown-=Time.deltaTime;
    }

    public void Jump(){
    rb.velocity = Vector2.up * 3f;

    }
    private IEnumerator AI(){

        for(;;)
        {
            yield return new WaitForSeconds(2f);
            Jump();
            
        }
        
    }

}