using UnityEngine;
using UnityEngine.Splines;

public class GameLevel : MonoBehaviour
{
    public SplineContainer SplineContainer;
    /// <summary>
    ///     The minimum percentage of dominos in the spline that should be destroyed during spawning.
    /// </summary>
    public float MinimumPercentageToDestroy;
    /// <summary>
    ///     The maximum percentage of dominos in the spline that should be destroyed during spawning.
    /// </summary>
    public float MaxPercentageToDestroy;
}
