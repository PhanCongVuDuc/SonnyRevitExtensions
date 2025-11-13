namespace SonnyRevitExtensions.Extensions.XYZs ;

/// <summary>
/// Extension methods for XYZ distance and proximity calculations
/// </summary>
public static class XYZDistanceExtensions
{
  /// <summary>
  /// Gets the minimum distance from a point to a list of points
  /// </summary>
  /// <param name="point">Target point</param>
  /// <param name="points">List of points to check</param>
  /// <returns>Minimum distance</returns>
  public static double GetMinimumDistanceToPoints( this XYZ point,
    List<XYZ> points )
  {
    return points.Min( p => p.DistanceTo( point ) ) ;
  }

  /// <summary>
  /// Calculates 2D distance between two points (ignoring Z coordinate)
  /// </summary>
  /// <param name="firstPoint">First point</param>
  /// <param name="secondPoint">Second point</param>
  /// <returns>2D distance</returns>
  public static double DistanceTo2D( this XYZ firstPoint,
    XYZ secondPoint ) =>
    Math.Sqrt( Math.Pow( firstPoint.X - secondPoint.X,
                 2 )
               + Math.Pow( firstPoint.Y - secondPoint.Y,
                 2 ) ) ;

  /// <summary>
  /// Calculates distance from a point to a line
  /// </summary>
  /// <param name="point">Point to measure from</param>
  /// <param name="line">Line to measure to</param>
  /// <returns>Distance from point to line</returns>
  public static double DistancePointToLine( this XYZ point,
    Line line )
  {
    return line.Distance( point ) ;
  }

  // Todo: will fix soon
  /// <summary>
  /// Calculates distance from a point to a line using plane distance calculation
  /// </summary>
  /// <param name="point">Point to measure from</param>
  /// <param name="line">Line to measure to</param>
  /// <param name="direction">Direction vector for plane calculation</param>
  /// <returns>Distance from point to line</returns>
  public static double DistancePointToLine( this XYZ point,
    Line line,
    XYZ direction )
  {
    return SonnyRevitExtensions.Extensions.Geometries.GeometryExtensions.GetLengthOfPointToPlane( direction,
      line.GetEndPoint( 0 ),
      point ) ;
  }

  /// <summary>
  /// Calculates the distance from any point to a plane
  /// </summary>
  /// <param name="planeNormal">Normal vector of the plane</param>
  /// <param name="planeOrigin">Origin point on the plane</param>
  /// <param name="targetPoint">Any point to calculate distance from</param>
  /// <returns>Distance from point to plane</returns>
  public static double GetLengthOfPointToPlane( this XYZ targetPoint,
    XYZ planeNormal,
    XYZ planeOrigin )
  {
    // Create plane
    var plane = Plane.CreateByNormalAndOrigin( planeNormal,
      planeOrigin ) ;
    var d = plane.Normal.DotProduct( -plane.Origin ) ;

    // Calculate numerator
    var numerator = Math.Abs( plane.Normal.DotProduct( targetPoint ) + d ) ;

    // Calculate denominator
    var denominator = Math.Sqrt( plane.Normal.DotProduct( plane.Normal ) ) ;

    return numerator / denominator ;
  }
}