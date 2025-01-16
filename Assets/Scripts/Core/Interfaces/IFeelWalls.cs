using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// used for actors that touch walls
/// </summary>
public interface IFeelWalls
{
    /// <summary>
    /// The Horizontal movement speed
    /// </summary>
    public Vector2 ContactNormal {get;}

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
    public enum TouchWall{
        None=0,
        Left=-1,
        Right=1

    }
    [Header("Collision")]

    [ReadOnly]
    [SerializeField]
    [Tooltip("Determines if the actor is standing on the ground")]
    protected bool OnGround{get; set;}
    public bool Is_OnGround{get{return OnGround;} protected set{OnGround = value;}}

    [ReadOnly]
    [SerializeField]
    [Tooltip("Determines if the actor is touching a wall")]
    protected bool OnWall{get; set;}
    public bool Is_OnWall{get{return OnWall;} protected set{OnWall = value;}}
    
    [ReadOnly]
    [SerializeField]
    [Tooltip("Determines if the actor is touching a ceiling")]
    protected bool OnCeiling{get; set;}
    public bool Is_OnCeiling{get{return OnCeiling;} protected set{OnCeiling = value;}}


    [ReadOnly]
    [SerializeField]
    [Tooltip("Determines if the actor is capable to perform a special wall-related action")]
    protected bool EligibleForWallAction {get; set;}
    public bool Is_EligibleForWallAction{get{return EligibleForWallAction;} protected set{EligibleForWallAction = value;}}


    [ReadOnly]
    [SerializeField]
    [Tooltip("The current wall the actor is touching, if any")]
    protected TouchWall OnWhatWall {get; set;}
    public TouchWall Is_OnWhatWall { get { return OnWhatWall; } protected set { OnWhatWall = value; } }
    
    [ReadOnly]
    [SerializeField]
    [Tooltip("The prior current wall the actor is touching, if any")]
    protected TouchWall PreviousWall {get; set;}
    public TouchWall My_PreviousWall { get { return PreviousWall; } protected set { PreviousWall = value; } }


    [ReadOnly]
    [SerializeField]
    [Tooltip("The current state of collision for the actor")]
    protected Collision_State C_State {get; set;}
    public Collision_State Collision {get {return C_State;} protected set {
        if (value != C_State) OnStateChange?.Invoke(Previous,value);
        C_State = value;
    }}


    [ReadOnly]
    [SerializeField]
    [Tooltip("The prior state of collision for the actor")]
    protected Collision_State Previous {get; set;}

    
    public delegate void StateChange_Dele(Collision_State From, Collision_State To);
    public static event StateChange_Dele OnStateChange;
    
}