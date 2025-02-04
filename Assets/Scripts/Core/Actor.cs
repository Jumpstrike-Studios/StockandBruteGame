using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using Unity.VisualScripting;
using UnityEditor.UI;
using UnityEngine;
using UnityEngine.UIElements;
/// <summary>
/// The basis of everything that moves and/or is interactable
/// </summary>
public class Actor : MonoBehaviour
{
    [Serializable]
    public enum Collision_State{
        /// <summary>
        /// The Actor is touching terrain below them. 
        /// </summary>
        OnGround,
        /// <summary>
        /// The Actor is touching terrain beside them. 
        /// </summary>
        AgainstWall,
        /// <summary>
        /// The Actor is touching terrain above them. 
        /// </summary>
        BelowCeiling,
        /// <summary>
        /// The Actor is touching nothing. 
        /// </summary>
         Airborne,
    }
    
    [Serializable]
    public enum TouchWall{
        None=0,
        Left=-1,
        Right=1

    }

    [Serializable]
    public struct ActorVitals
    {
        [ReadOnly]
        [Tooltip("The actor's maximum health.")]
        public int MaxHealth;

        [Tooltip("The actor's starting and current health.")]
        public int Health;
        [ReadOnly]
        [Tooltip("Set to true if the actor's max health is -1")]
        public bool Invincible;
    
    
    /// <summary>
    /// When the actor's health reaches 0, the actor will be removed from the scene
    /// </summary>
    [Tooltip("The actor is removed from the world upon reaching 0 health")]
    public bool RemoveOnDeath;
    public ActorVitals(int BaseHealth)
    {
        MaxHealth = BaseHealth;
        Health = MaxHealth;
        RemoveOnDeath=true;
        Invincible = MaxHealth<0;
    }
    }
    
    [Header("General")]
    
    /// <summary>
    /// The health of the actor. This value sets both the maximum hp and the invincibility flag
    /// </summary>
    [Tooltip("The vitals of the actor")]
    public ActorVitals Health;


    public float WalkSpeed = 5f;

    public Vector2 ContactNormal {get; private set;}

    public GameObject AfterImage;
    public float GravityScale=1f;
    [DoNotSerialize]
    protected Vector2 Velocity;

    [Header("Collision")]
    [ReadOnly]
    [SerializeField]
    [Tooltip("Determines if the actor is standing on the ground")]
    private bool OnGround;
    public bool Is_OnGround{get{return OnGround;} protected set{OnGround = value;}}

    [ReadOnly]
    [SerializeField]
    [Tooltip("Determines if the actor is touching a wall")]
    private bool OnWall;
    public bool Is_OnWall{get{return OnWall;} protected set{OnWall = value;}}
    
    [ReadOnly]
    [SerializeField]
    [Tooltip("Determines if the actor is touching a wall")]
    private bool OnCeiling;
    public bool Is_OnCeiling{get{return OnCeiling;} protected set{OnCeiling = value;}}
    

    [Space]

    [ReadOnly]
    [SerializeField]
    [Tooltip("Determines if the actor is capable to perform a special wall-related action")]
    private bool EligibleForWallAction;
    public bool Is_EligibleForWallAction{get{return EligibleForWallAction;} protected set{EligibleForWallAction = value;}}


    [ReadOnly]
    [SerializeField]
    [Tooltip("The current wall the actor is touching, if any")]
    private TouchWall OnWhatWall;
    public TouchWall Is_OnWhatWall { get { return OnWhatWall; } protected set { OnWhatWall = value; } }
    
    [ReadOnly]
    [SerializeField]
    [Tooltip("The prior current wall the actor is touching, if any")]
    private TouchWall PreviousWall;
    public TouchWall My_PreviousWall { get { return PreviousWall; } protected set { PreviousWall = value; } }



    [ReadOnly]
    [SerializeField]
    [Tooltip("The current state of collision for the actor")]
    private Collision_State C_State;
    public Collision_State CollisionState {get {return C_State;} protected set {
        if (value != C_State) OnStateChange?.Invoke(C_State,value);
        Previous = C_State;
        C_State = value;
    }}


    [ReadOnly]
    [SerializeField]
    [Tooltip("The prior state of collision for the actor")]
    protected Collision_State Previous;


    public int BaseHealth = 300;

    protected Rigidbody2D rb;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void Start()
    {
        
        rb = GetComponent<Rigidbody2D>();
    }

    public delegate void StateChange_Dele(Collision_State From, Collision_State To);
    public static event StateChange_Dele OnStateChange;

    // Update is called once per frame
    public virtual void Update()
    {
    
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        UpdateCollision_State(collision);
    }
    public void OnCollisionStay2D(Collision2D collision){
        UpdateCollision_State(collision);
    }
    public void OnCollisionExit2D(Collision2D collision){
        UpdateCollision_State(collision);
    }
     public void UpdateCollision_State(Collision2D collision)
    {
        Is_OnGround=false;
        Is_OnWall=false;
        Is_OnCeiling=false;
        for (int i = 0; i < collision.contactCount; i++)
        {
            ContactNormal = collision.GetContact(i).normal; 
            if(ContactNormal.y> 0.2f){ 
            Debug.DrawLine(transform.position,(Vector2)transform.position-ContactNormal, Color.red);
            Is_OnGround |= ContactNormal.y > 0.2f; // shorthanded for " x = x || y; "
            }
            else if(MathF.Abs(ContactNormal.x)>0.5){
            Debug.DrawLine(transform.position,(Vector2)transform.position-ContactNormal, Color.green);
            Is_OnWall |= MathF.Abs(ContactNormal.x)>0.5;
            OnWhatWall = (TouchWall)Math.Sign(ContactNormal.x);
            }
            else if(ContactNormal.y < -0.2f){
            Debug.DrawLine(transform.position,(Vector2)transform.position-ContactNormal, Color.blue);
            Is_OnCeiling |= ContactNormal.y < -0.2f;
            }
        }
        Is_OnCeiling &= !Is_OnGround;
        Is_OnWall &= !Is_OnCeiling; // shorthanded for " x = x && y; "
        
        /*Prioritize Ground > Ceiling > Wall > Air
        * If touching both wall and ceiling, ceiling.
        * Wall and ground? Ground.
        * etc
        */
        
        if (!Is_OnWall){
            OnWhatWall = TouchWall.None;
            EligibleForWallAction = false;
        }else{
        EligibleForWallAction = PreviousWall ==0||PreviousWall!=OnWhatWall;
        CollisionState = Collision_State.AgainstWall;
        }
        if(Is_OnGround){//the player is on the ground
            EligibleForWallAction = false;
            PreviousWall = TouchWall.None;
            CollisionState = Collision_State.OnGround;
        }else{ //the player is in the air
            if(Is_OnCeiling) CollisionState = Collision_State.BelowCeiling;
            else CollisionState = Collision_State.Airborne;
        }
       
    }

    public virtual void takeDamage(int amount){}

    public virtual void Die()
    {
        
        if(Health.RemoveOnDeath)
        {
            Destroy(gameObject);
        }
    }
}
