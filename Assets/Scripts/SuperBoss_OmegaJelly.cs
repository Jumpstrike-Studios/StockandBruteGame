using System;
using UnityEngine;
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
    public Vector3 Goal;
    public float STATE_SPIN_TIMER;
    public float STATE_WOBBLE_TIMER;

    public State state;
        public new void Start()
    {
        base.Start();
        state = State.IDLE;
    }

    public void STATE_MOVING()
    {


    }

    public void STATE_IDLE()
    {
        gameObject.transform.localScale = new Vector3(12f+Mathf.Sin(STATE_WOBBLE_TIMER*Mathf.PI/0.9f)*0.3f,12f-Mathf.Sin(STATE_WOBBLE_TIMER*Mathf.PI/0.9f+0.1f)*0.4f,12f);
        STATE_WOBBLE_TIMER += Time.deltaTime;
    }

    // Update is called once per frame
    public new void Update()
    {
        base.Update();
        if (state == State.IDLE) STATE_IDLE(); else STATE_WOBBLE_TIMER=0f;

    }


}