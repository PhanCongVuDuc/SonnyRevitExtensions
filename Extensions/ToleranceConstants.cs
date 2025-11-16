namespace SonnyRevitExtensions.Extensions ;

/// <summary>
///     Contains tolerance and precision constants used in geometric calculations
/// </summary>
public static class ToleranceConstants
{
    #region Core Tolerance Constants

    /// <summary>
    ///     High precision for critical calculations (1.0e-09)
    ///     Used for: angle calculations, parameter comparisons, vector operations
    /// </summary>
    public const double HighPrecision = 1.0e-09 ;

    /// <summary>
    ///     Standard precision for most geometric operations (0.0001)
    ///     Used for: general floating point comparisons, volume thresholds
    /// </summary>
    public const double StandardPrecision = 0.0001 ;

    /// <summary>
    ///     General tolerance for distance and geometric operations (0.001)
    ///     Used for: point distances, vector comparisons, surface areas
    /// </summary>
    public const double GeneralTolerance = 0.001 ;

    /// <summary>
    ///     Coarse tolerance for intersection and line operations (0.01)
    ///     Used for: line intersections, face detection
    /// </summary>
    public const double CoarseTolerance = 0.01 ;

    /// <summary>
    ///     Very fine tolerance for dot product calculations (0.0000001)
    /// </summary>
    public const double DotProductTolerance = 0.0000001 ;

    #endregion

    #region Helper Methods

    /// <summary>
    ///     Checks if two double values are equal within standard precision
    /// </summary>
    public static bool AreEqual(double value1,
        double value2) =>
        Math.Abs(value1 - value2) < StandardPrecision ;

    /// <summary>
    ///     Checks if two double values are equal within specified tolerance
    /// </summary>
    public static bool AreEqual(double value1,
        double value2,
        double tolerance) =>
        Math.Abs(value1 - value2) < tolerance ;

    /// <summary>
    ///     Checks if a value is zero within standard precision
    /// </summary>
    public static bool IsZero(double value) => Math.Abs(value) < StandardPrecision ;

    /// <summary>
    ///     Checks if a value is zero within specified tolerance
    /// </summary>
    public static bool IsZero(double value,
        double tolerance) =>
        Math.Abs(value) < tolerance ;

    #endregion
}
