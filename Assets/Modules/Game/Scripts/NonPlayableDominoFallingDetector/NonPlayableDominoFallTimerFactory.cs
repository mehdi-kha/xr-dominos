using Zenject;

/// <summary>
///     This factory is required in order for the Zenject injection to work properly when instantiating NonPlayableDominoFallingDetectorFactory at runtime.
/// </summary>
class NonPlayableDominoFallTimerFactory : PlaceholderFactory<DominoFallTimerManager>
{
}
