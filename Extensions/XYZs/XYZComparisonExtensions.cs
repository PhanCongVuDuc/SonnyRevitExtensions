namespace SonnyRevitExtensions.Extensions.XYZs ;

/// <summary>
///     Extension methods for XYZ comparison and equality operations
/// </summary>
public static class XYZComparisonExtensions
{
    /// <summary>
    ///     Checks if two points are almost equal in 2D (ignoring Z)
    /// </summary>
    /// <param name="firstPoint">First point</param>
    /// <param name="secondPoint">Second point</param>
    /// <param name="tolerance">Tolerance for comparison</param>
    /// <returns>True if points are almost equal in 2D</returns>
    public static bool IsAlmostEqual2D(this XYZ firstPoint,
        XYZ? secondPoint,
        double tolerance = ToleranceConstants.GeneralTolerance) =>
        secondPoint is not null
        && ToleranceConstants.AreEqual(firstPoint.X,
            secondPoint.X,
            tolerance)
        && ToleranceConstants.AreEqual(firstPoint.Y,
            secondPoint.Y,
            tolerance) ;

    /// <summary>
    ///     Checks if two points are almost equal in 3D
    /// </summary>
    /// <param name="firstPoint">First point</param>
    /// <param name="secondPoint">Second point</param>
    /// <param name="tolerance">Tolerance for comparison</param>
    /// <returns>True if points are almost equal in 3D</returns>
    public static bool IsAlmostEqual3D(this XYZ firstPoint,
        XYZ? secondPoint,
        double tolerance = ToleranceConstants.GeneralTolerance) =>
        secondPoint is not null
        && IsAlmostEqual2D(firstPoint,
            secondPoint,
            tolerance)
        && ToleranceConstants.AreEqual(firstPoint.Z,
            secondPoint.Z,
            tolerance) ;

    /// <summary>
    ///     Checks if a point is on a line within tolerance
    /// </summary>
    /// <param name="lineStartPoint">Start point of line</param>
    /// <param name="lineEndPoint">End point of line</param>
    /// <param name="direction">Point to test</param>
    /// <returns>True if point is on line</returns>
    public static bool IsPointOnLine(XYZ lineStartPoint,
        XYZ lineEndPoint,
        XYZ direction)
    {
        const double tolerance = ToleranceConstants.CoarseTolerance ;

        // Check if testDirection is valid
        if (direction.GetLength() < ToleranceConstants.HighPrecision)
        {
            return false ;
        }

        // Vector from lineStartPoint to lineEndPoint
        XYZ? vector = lineEndPoint - lineStartPoint ;

        // If lineEndPoint == lineStartPoint, it's on the line
        if (vector.GetLength() < tolerance)
        {
            return true ;
        }

        // Normalize direction
        XYZ? normalizedDir = direction.Normalize() ;

        // Cross product to check if vectors are parallel
        XYZ? crossProduct = vector.CrossProduct(normalizedDir) ;
        double distance = crossProduct.GetLength() ;

        return distance < tolerance ;
    }

    /// <summary>
    ///     Removes duplicate points
    /// </summary>
    /// <param name="xyzes">Collection of points</param>
    /// <returns>List of distinct points</returns>
    public static List<XYZ> DistinctXYZ(this IEnumerable<XYZ> xyzes) =>
        xyzes.DistinctBy(x => (x.X, x.Y, x.Z))
            .ToList() ;
}
