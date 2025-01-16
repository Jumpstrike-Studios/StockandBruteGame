using UnityEngine;

/// <summary>
/// For actors that move on the ground or surface
/// </summary>
public interface IWalkingActor
{
    /// <summary>
    /// The Horizontal movement speed
    /// </summary>
    public float WalkSpeed { get; set;}

}