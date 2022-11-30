using UnityEngine;

public interface IBowl
{
    /// <summary>
    ///     TODO This will rather be handled in level specific objects in the future.
    /// </summary>
    public int NumberOfDominosToSpawn { get; }

    /// <summary>
    ///     Get the world world position of where the next domino should be spawned.
    /// </summary>
    /// <returns></returns>
    public Vector3 GetDominoSpawningPosition();
}
