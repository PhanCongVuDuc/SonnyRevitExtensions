namespace SonnyRevitExtensions.Extensions.XYZs ;

/// <summary>
///     Extension methods for XYZ vector operations and directions
/// </summary>
public static class XYZVectorExtensions
{
    /// <summary>
    ///     Checks if two vectors are parallel (same or opposite direction)
    /// </summary>
    /// <param name="firstVector">First vector</param>
    /// <param name="secondVector">Second vector</param>
    /// <returns>True if vectors are parallel</returns>
    public static bool AreVectorsParallel(this XYZ firstVector,
        XYZ secondVector) =>
        firstVector.IsAlmostEqualTo(secondVector,
            ToleranceConstants.GeneralTolerance)
        || firstVector.IsAlmostEqualTo(secondVector.Negate(),
            ToleranceConstants.GeneralTolerance) ;

    /// <summary>
    ///     Checks if two vectors have the same direction
    /// </summary>
    /// <param name="firstVector">First vector</param>
    /// <param name="secondVector">Second vector</param>
    /// <returns>True if vectors have same direction</returns>
    public static bool IsSameDirection(this XYZ firstVector,
        XYZ secondVector)
    {
        var dot = firstVector.DotProduct(secondVector) ;
        return dot.Equals(1) ;
    }

    /// <summary>
    ///     Checks if two vectors are perpendicular
    /// </summary>
    /// <param name="firstVector">First vector</param>
    /// <param name="secondVector">Second vector</param>
    /// <returns>True if vectors are perpendicular</returns>
    public static bool IsPerpendicular(this XYZ firstVector,
        XYZ secondVector)
    {
        var dot = firstVector.DotProduct(secondVector) ;
        return dot.Equals(0) ;
    }

    /// <summary>
    ///     Gets a perpendicular vector in 2D (rotated 90 degrees counterclockwise)
    /// </summary>
    /// <param name="inputVector">Input vector</param>
    /// <returns>Perpendicular vector</returns>
    public static XYZ GetPerpendicularVector(this XYZ inputVector) =>
        new(-inputVector.Y,
            inputVector.X,
            inputVector.Z) ;

    /// <summary>
    ///     Checks if two vectors are parallel using cross product
    /// </summary>
    /// <param name="firstVector">First vector</param>
    /// <param name="secondVector">Second vector</param>
    /// <returns>True if vectors are parallel</returns>
    public static bool IsParallel(this XYZ firstVector,
        XYZ secondVector)
    {
        firstVector = firstVector.Normalize() ;
        secondVector = secondVector.Normalize() ;

        return firstVector.IsAlmostEqual3D(secondVector) || firstVector.IsAlmostEqual3D(secondVector.Negate()) ;
    }

    /// <summary>
    ///     Calculates clockwise angle between two vectors in 2D
    /// </summary>
    /// <param name="firstVector">First vector</param>
    /// <param name="secondVector">Second vector</param>
    /// <returns>Clockwise angle in radians</returns>
    public static double GetClockwiseAngleBetweenVectors(this XYZ firstVector,
        XYZ secondVector)
    {
        var dot = secondVector.X * firstVector.X + secondVector.Y * firstVector.Y ;
        var det = secondVector.Y * firstVector.X - secondVector.X * firstVector.Y ;
        return -Math.Atan2(det,
            dot) ;
    }

    /// <summary>
    ///     Checks if two directions are perpendicular in 2D
    /// </summary>
    /// <param name="firstDirection">First direction</param>
    /// <param name="secondDirection">Second direction</param>
    /// <returns>True if directions are perpendicular in 2D</returns>
    public static bool AreDirectionsPerpendicular2D(this XYZ firstDirection,
        XYZ secondDirection) =>
        (ToleranceConstants.AreEqual(Math.Abs(firstDirection.X),
             Math.Abs(secondDirection.Y))
         || ToleranceConstants.AreEqual(Math.Abs(firstDirection.Y),
             Math.Abs(secondDirection.X)))
        && ! ToleranceConstants.AreEqual(Math.Abs(firstDirection.Y),
            Math.Abs(firstDirection.X)) ;

    /// <summary>
    ///     Checks if direction is pointing up in Z
    /// </summary>
    /// <param name="direction">Direction vector</param>
    /// <returns>True if pointing up</returns>
    public static bool IsUpZ(this XYZ direction) =>
        ToleranceConstants.AreEqual(direction.Normalize()
                .Z,
            XYZ.BasisZ.Z) ;

    /// <summary>
    ///     Checks if direction is pointing down in Z
    /// </summary>
    /// <param name="direction">Direction vector</param>
    /// <returns>True if pointing down</returns>
    public static bool IsDownZ(this XYZ direction) =>
        ToleranceConstants.AreEqual(direction.Normalize()
                .Z,
            -XYZ.BasisZ.Z) ;

    /// <summary>
    ///     Checks if two directions are opposite
    /// </summary>
    /// <param name="direction1">First direction</param>
    /// <param name="direction2">Second direction</param>
    /// <returns>True if directions are opposite</returns>
    public static bool IsOppositeDirection(this XYZ direction1,
        XYZ direction2) =>
        Math.Abs(direction1.X - direction2.X) < ToleranceConstants.GeneralTolerance
        && Math.Abs(direction1.Y - -direction2.Y) < ToleranceConstants.GeneralTolerance ;
}
