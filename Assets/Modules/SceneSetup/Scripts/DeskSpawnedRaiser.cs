using System;
using UnityEngine;

/// <summary>
///     It's a bit complicated to get a clear list of the desks in the player's zone using the Oculus API,
///     hence this little class to handle our own logic.
/// </summary>
public class DeskSpawnedRaiser : MonoBehaviour
{
    public static event Action DeskSpawned;

    void Awake()
    {
        DeskSpawned?.Invoke();
    }
}
