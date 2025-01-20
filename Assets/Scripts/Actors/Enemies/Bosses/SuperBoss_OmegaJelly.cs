using System;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;
using Random=UnityEngine.Random;
/// <summary>
/// The basis of evil, and MORE EVIL
/// </summary>
public class SuperBoss_OmegaJelly : Boss_Base
{
    public enum State{
        ASLEEP,
        IDLE,
        MOVING,
        LEAP,
        SPIN,
        BLARF,
        SLAM,
        FIRE,
        OUCH,
        ARG,
    }
    public enum StateStep{
        WINDUP,
        ACTION,
        COOLDOWN
    }
    public Vector3 Goal;
    public float STATE_SPIN_TIMER;
    public float STATE_SPIN_SPEED;
    public float STATE_SPIN_TWIST;
    public float STATE_WOBBLE_TIMER;

    public float STATE_SPIT_TIMER=1;
    public GameObject leftEdge;
    public GameObject rightEdge;
    public GameObject Target;
    public GameObject Enemy;

    public GameObject Level;

    public GameObject Theme;
    private Vector3 Home;
    private Vector3 PreMove;
    public float NextStateIn{get{return timeTillNextState;}}
    public float timeTillNextState{get{return timeTillNextStateA;}set{timeTillNextStateA = value; timeTillNextStateB=value;}}
    private float timeTillNextStateA;
    private float timeTillNextStateB;

    public GameObject SlimeBallLobbing;

    public GameObject trail_root;
    private float TrailTimer;

    float easeOutQuad(float x) {
    return 1 - (1 - x) * (1 - x);
    }
float easeInQuad(float x) {
    return x*x;
    }
    public void UpdateTrail(){
    TrailTimer-=Time.deltaTime;
    if(TrailTimer < 0){
       GameObject trail = Instantiate(trail_root,transform.position,transform.rotation);
        trail.transform.localScale = transform.localScale;
        trail.GetComponent<SpriteRenderer>().sprite =GetComponent<SpriteRenderer>().sprite;
        trail.GetComponent<SpriteRenderer>().flipX = GetComponent<SpriteRenderer>().flipX;
        trail.GetComponent<SpriteRenderer>().color = GetComponent<SpriteRenderer>().color;
        TrailTimer=0.09f;
    }
}


    public State state;
    public StateStep step;

        public new void Start()
    {
        base.Start();

        step= StateStep.WINDUP;
        timeTillNextState = 1f;
        Home= transform.position;
    }

    bool CHOICE_TARGET_PLAYER()
    {
        return Random.Range(0f,1f)>(Health.Health/(float)Health.MaxHealth);
    }

    void CHANGESTATE(State newstate, float newtime)
    {
        state = newstate;
        timeTillNextState = newtime;

    }
    void Spit()
    {
        float Range= Mathf.PI/2f*(CHOICE_TARGET_PLAYER()?0f:1f);
        if(SlimeBallLobbing!=null){
            GameObject slimeBall = Instantiate(SlimeBallLobbing,transform.position+new Vector3(0,0.4f,0),transform.rotation);
            if(slimeBall.GetComponent<SlimeBallPhysics>()!=null){
                slimeBall.GetComponent<SlimeBallPhysics>().Target= new Vector2(Enemy.transform.position.x+Random.Range(-Range,Range),Enemy.transform.position.y);
                slimeBall.GetComponent<SlimeBallPhysics>().hitTime=1.4f*(Enraged?0.7f:1f);
            }
            if(slimeBall.GetComponent<Whisp_Wheel>()!=null){
                slimeBall.GetComponent<Whisp_Wheel>().WalkSpeed=3f;
                slimeBall.GetComponent<Whisp_Wheel>().FireAngle = Random.Range(-Mathf.PI,Mathf.PI);
            }
            STATE_SPIT_TIMER=1f*(Enraged?0.7f:1f);
        }
    }

    public void STATE_SPINNING()
    {
       
        if(step== StateStep.WINDUP){
            STATE_SPIT_TIMER=1f*(Enraged?0.7f:1f);
            STATE_SPIN_TWIST=1-timeTillNextStateA/timeTillNextStateB;
            STATE_SPIN_TIMER+=STATE_SPIN_TWIST*Time.deltaTime;
            Target.transform.position  = CHOICE_TARGET_PLAYER()?new Vector3(Enemy.transform.position.x,Home.y,Home.z) :Vector3.Lerp(leftEdge.transform.position, rightEdge.transform.position,Random.Range(0f,1f));
            PreMove = new Vector3(transform.position.x,Home.y,Home.z);
            gameObject.transform.localScale = new Vector3(1-easeInQuad(1-timeTillNextStateA/timeTillNextStateB)*0.8f,1+easeInQuad(1-timeTillNextStateA/timeTillNextStateB)*0.4f,1)*12f;
            STATE_SPIN_SPEED = (Enraged?1f:0.5f)*(Enemy.transform.position.x>transform.position.x?1f:-1f)*8f;
        }else if (step== StateStep.ACTION){

            STATE_SPIT_TIMER-=Time.deltaTime;
        if(STATE_SPIT_TIMER<=0) Spit();


            STATE_SPIN_TIMER+=STATE_SPIN_TWIST*Time.deltaTime;
            STATE_SPIN_SPEED += 8f*Time.deltaTime*(Enemy.transform.position.x>transform.position.x?1f:-1f)*(Enraged?2.1f:1f);
            float cap = 13f;
            STATE_SPIN_SPEED = Mathf.Clamp(STATE_SPIN_SPEED, -cap,cap);
            PreMove = new Vector3(transform.position.x+STATE_SPIN_SPEED*Time.deltaTime,Home.y,Home.z);
            gameObject.transform.localScale = new Vector3(1+0.4f,1-0.8f,1)*12f;
            transform.position = Vector3.Lerp(PreMove,Target.transform.position,0f)-(new Vector3(0,1.3f,0)*0.8f);

        }else{
            PreMove = new Vector3(transform.position.x+STATE_SPIN_SPEED*Time.deltaTime,Home.y,Home.z);
            STATE_SPIN_TWIST=timeTillNextStateA/timeTillNextStateB;
            STATE_SPIN_TIMER+=STATE_SPIN_TWIST*Time.deltaTime;
        
        }
         gameObject.transform.localScale = new Vector3(gameObject.transform.localScale.x*Mathf.Cos(Mathf.PI*STATE_SPIN_TIMER*8f*(Enraged?1.5f:1f)),gameObject.transform.localScale.y,1f);
         if (step== StateStep.ACTION) UpdateTrail();
    }
     public void STATE_MOVING()
    {

        if(step== StateStep.WINDUP){
            Target.transform.position  = CHOICE_TARGET_PLAYER()?new Vector3(Enemy.transform.position.x,Home.y,Home.z) :Vector3.Lerp(leftEdge.transform.position, rightEdge.transform.position,Random.Range(0f,1f));
            PreMove = new Vector3(transform.position.x,Home.y,Home.z);
            gameObject.transform.localScale = new Vector3(1-easeInQuad(1-timeTillNextStateA/timeTillNextStateB)*0.8f,1+easeInQuad(1-timeTillNextStateA/timeTillNextStateB)*0.4f,1)*12f;
            GetComponent<SpriteRenderer>().flipX = Enemy.transform.position.x>transform.position.x;
        }else if (step== StateStep.ACTION){
            GetComponent<SpriteRenderer>().flipX = Target.transform.position.x>transform.position.x;
            gameObject.transform.localScale = new Vector3(1+(1-easeOutQuad(1-(timeTillNextStateA/timeTillNextStateB)))*0.8f,1-(1-easeOutQuad(1-(timeTillNextStateA/timeTillNextStateB)))*0.8f,1)*12f;
            transform.position = Vector3.Lerp(PreMove,Target.transform.position,easeOutQuad(1-(timeTillNextStateA/timeTillNextStateB)))-(new Vector3(0,1.3f,0)*(1-easeOutQuad(1-(timeTillNextStateA/timeTillNextStateB)))*0.8f);
            UpdateTrail();
        }else{
            GetComponent<SpriteRenderer>().flipX = Target.transform.position.x>transform.position.x;
        }

    }

    public void STATE_IDLE()
    {
         GetComponent<SpriteRenderer>().flipX = Target.transform.position.x>transform.position.x;
        STATE_WOBBLE_TIMER += Time.deltaTime;
        STATE_WOBBLE_TIMER = STATE_WOBBLE_TIMER%2f;
    }
    public void UpdateStep()
    {
        if (state != State.ASLEEP)  timeTillNextStateA-=Time.deltaTime;
        if(step==StateStep.WINDUP)
        { if(timeTillNextStateA<=0) {step = StateStep.ACTION;
        switch(state){
            case State.IDLE: timeTillNextState=1.5f*(Enraged?0.8f:1f); break;

            case State.MOVING: timeTillNextState=1.5f*(Enraged?0.8f:1f); break;

            case State.LEAP: timeTillNextState=1.5f; break;

            case State.SPIN: timeTillNextState=5f; break;

            case State.BLARF: timeTillNextState=1.5f; break;

            case State.SLAM: timeTillNextState=1.5f; break;

            case State.FIRE: timeTillNextState=1.5f; break;

            case State.OUCH: timeTillNextState=1.5f; break;

            case State.ARG: timeTillNextState=1.5f; break;
            default: timeTillNextState=1f; break;
        }
        Debug.Log("ACTION!");
        }
        }else if(step==StateStep.ACTION)
        { if(timeTillNextStateA<=0) {step = StateStep.COOLDOWN;
        switch(state){
            case State.IDLE: timeTillNextState=1.5f*(Enraged?0.5f:1f); break;

            case State.MOVING: timeTillNextState=0f; break;

            case State.LEAP: timeTillNextState=1.5f; break;

            case State.SPIN: timeTillNextState=1.5f; break;

            case State.BLARF: timeTillNextState=1.5f; break;

            case State.SLAM: timeTillNextState=1.5f; break;

            case State.FIRE: timeTillNextState=1.5f; break;

            case State.OUCH: timeTillNextState=1.5f; break;

            case State.ARG: timeTillNextState=1.5f; break;
            default: timeTillNextState=1f; break;
        }
        Debug.Log("COOLING DOWN");
        }
        }else if(step==StateStep.COOLDOWN){
        if(timeTillNextStateA<=0){ step = StateStep.WINDUP; 
        int Nextmove = Random.Range(0,1);
        switch(state){
            case State.IDLE:
            Nextmove = Random.Range(0,2);
            if(Nextmove==0) CHANGESTATE(State.MOVING,1.4f*(Enraged?0.5f:1f)); 
            else CHANGESTATE(State.SPIN,2f*(Enraged?0.6f:1f)); 
            break;
            case State.MOVING: CHANGESTATE(State.IDLE,1f*(Enraged?0.5f:1f)); break;
            
            case State.LEAP: timeTillNextState=1.5f; break;

            case State.SPIN: CHANGESTATE(State.IDLE,1f*(Enraged?0.5f:1f)); break;

            case State.BLARF: timeTillNextState=1.5f; break;

            case State.SLAM: timeTillNextState=1.5f; break;

            case State.FIRE: timeTillNextState=1.5f; break;

            case State.OUCH: timeTillNextState=1.5f; break;

            case State.ARG: timeTillNextState=1.5f; break;
            default: timeTillNextState=1f; break;
        }
        Debug.Log("WINDING UP \""+state.ToString()+"\"");
        }
        }else{
            Debug.LogError("UNKNOWN STATESTEP IN BOSS! TERMINATING BOSS");
            Destroy(gameObject.transform.parent);
        }
    }

    // Update is called once per frame
    public new void Update()
    {
        base.Update();
        if(State.ASLEEP == state){
            if(Vector3.Distance(transform.position,Enemy.transform.position)<14f){
                CHANGESTATE(State.IDLE,1f);
                Debug.Log("BOSS WAKES UPS");
                Level.GetComponent<AudioSource>().Stop();
                Theme.GetComponent<AudioSource>().Play();
            }
            return;

        }
        Theme.GetComponent<AudioSource>().pitch = Enraged?1.1f:1f;
        gameObject.transform.localScale = new Vector3(12f+Mathf.Sin(STATE_WOBBLE_TIMER*Mathf.PI)*0.3f,12f-Mathf.Sin(STATE_WOBBLE_TIMER*Mathf.PI+0.1f)*0.4f,12f);
        
        if (state == State.IDLE) STATE_IDLE(); else if(STATE_WOBBLE_TIMER>0f) STATE_WOBBLE_TIMER-=Time.deltaTime;
        if (state == State.MOVING) STATE_MOVING();
        if (state == State.SPIN) STATE_SPINNING(); else STATE_SPIN_TIMER=0f;

        UpdateStep();
    }


}