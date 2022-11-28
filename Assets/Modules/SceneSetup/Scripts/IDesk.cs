using System;
using UnityEngine;

public interface IDesk
{
    /// <summary>
    ///     The world position of the point at the center on the top side of the desk's mesh.
    /// </summary>
    public Vector3 CenterTopPosition { get; }
    /// <summary>
    ///     The mesh's bounds.
    /// </summary>
    public Bounds Bounds { get; }
    /// <summary>
    ///     The desks's transform.
    /// </summary>
    public Transform Transform { get; }
    /// <summary>
    ///     Where the bowl should spawn.
    /// </summary>
    /// <returns>A world position</returns>
    public Vector3 GetBowlSpawningLocalPosition();
    public event Action SetupDone;
    /// <summary>
    ///     Make the desk appear.
    /// </summary>
    public void Show();

    /// <summary>
    ///     Make the desk disappear.
    /// </summary>
    public void Hide();
}
