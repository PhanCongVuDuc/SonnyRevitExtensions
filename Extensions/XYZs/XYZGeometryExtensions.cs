namespace SonnyRevitExtensions.Extensions.XYZs ;

/// <summary>
/// Extension methods for XYZ geometric calculations
/// </summary>
public static partial class XYZGeometryExtensions
{
  /// <summary>
  /// Checks if a point is inside a polygon using ray casting algorithm
  /// </summary>
  /// <param name="point">Point to test</param>
  /// <param name="polygonVertices">Vertices of the polygon</param>
  /// <returns>True if point is inside polygon</returns>
  public static bool IsPointInsidePolygon( this XYZ point,
    List<XYZ> polygonVertices )
  {
    var angle = 0.0 ;
    for ( var i = 0 ; i < polygonVertices.Count ; i++ ) {
      var j = i == polygonVertices.Count - 1 ? 0 : i + 1 ;
      var a = new XYZ( polygonVertices[ i ].X,
        polygonVertices[ i ].Y,
        0 ) ;
      var b = new XYZ( point.X,
        point.Y,
        0 ) ;
      var c = new XYZ( polygonVertices[ j ].X,
        polygonVertices[ j ].Y,
        0 ) ;

      var ba = Line.CreateBound( b,
          a )
        .Direction ;
      var bc = Line.CreateBound( b,
          c )
        .Direction ;
      angle += ba.AngleTo( bc ) ;
    }

    return Math.Abs( angle - 2 * Math.PI ) < ToleranceConstants.HighPrecision ;
  }

  /// <summary>
  /// Point-in-polygon test with vertex array
  /// </summary>
  /// <param name="nvert">Number of vertices</param>
  /// <param name="vertx">X coordinates of vertices</param>
  /// <param name="verty">Y coordinates of vertices</param>
  /// <param name="testx">X coordinate of test point</param>
  /// <param name="testy">Y coordinate of test point</param>
  /// <returns>True if point is inside polygon</returns>
  public static bool PointInPolygon( int nvert,
    double[] vertx,
    double[] verty,
    double testx,
    double testy )
  {
    int i, j ;
    var c = false ;
    for ( i = 0, j = nvert - 1 ; i < nvert ; j = i++ ) {
      if ( verty[ i ] > testy != ( verty[ j ] > testy )
           && ( testx
                < ( vertx[ j ] - vertx[ i ] ) * ( testy - verty[ i ] ) / ( verty[ j ] - verty[ i ] ) + vertx[ i ] ) )
        c = ! c ;
    }

    return c ;
  }

  /// <summary>
  /// Calculates the centroid of a list of points
  /// </summary>
  /// <param name="sources">List of points</param>
  /// <returns>Centroid point</returns>
  public static XYZ GetCentroid( this List<XYZ> sources )
  {
    var centroid = sources[ 0 ] ;
    for ( var i = 1 ; i < sources.Count ; i++ ) {
      centroid = centroid.Add( sources[ i ] ) ;
    }

    return centroid.Divide( sources.Count ) ;
  }

  /// <summary>
  /// Checks if three points form a clockwise orientation
  /// </summary>
  /// <param name="firstPoint">First point</param>
  /// <param name="secondPoint">Second point</param>
  /// <param name="thirdPoint">Third point</param>
  /// <returns>True if clockwise</returns>
  public static bool IsClockwise( XYZ firstPoint,
    XYZ secondPoint,
    XYZ thirdPoint )
  {
    var v12 = secondPoint - firstPoint ;
    var v13 = thirdPoint - firstPoint ;
    return v12.CrossProduct( v13 )
             .Z
           < 0 ;
  }

  /// <summary>
  /// Calculates intersection point of two lines defined by points and directions
  /// </summary>
  /// <param name="firstLinePoint">Point on first line</param>
  /// <param name="firstLineDirection">Direction of first line</param>
  /// <param name="secondLinePoint">Point on second line</param>
  /// <param name="secondLineDirection">Direction of second line</param>
  /// <returns>Intersection point or null if lines are parallel</returns>
  public static XYZ? CalculateIntersection( XYZ firstLinePoint,
    XYZ firstLineDirection,
    XYZ secondLinePoint,
    XYZ secondLineDirection )
  {
    // Ensure we're working with 2D: zero out the Z component
    firstLinePoint = new XYZ( firstLinePoint.X,
      firstLinePoint.Y,
      0 ) ;
    secondLinePoint = new XYZ( secondLinePoint.X,
      secondLinePoint.Y,
      0 ) ;
    firstLineDirection = new XYZ( firstLineDirection.X,
      firstLineDirection.Y,
      0 ).Normalize() ;
    secondLineDirection = new XYZ( secondLineDirection.X,
      secondLineDirection.Y,
      0 ).Normalize() ;

    // Calculate determinants
    var denominator = firstLineDirection.X * secondLineDirection.Y - firstLineDirection.Y * secondLineDirection.X ;

    if ( Math.Abs( denominator ) < ToleranceConstants.HighPrecision ) {
      return null ;
    }

    var t = ( ( secondLinePoint.X - firstLinePoint.X ) * secondLineDirection.Y
              - ( secondLinePoint.Y - firstLinePoint.Y ) * secondLineDirection.X )
            / denominator ;
    var intersectionPoint = new XYZ( firstLinePoint.X + t * firstLineDirection.X,
      firstLinePoint.Y + t * firstLineDirection.Y,
      0 ) ;

    return intersectionPoint ;
  }

  /// <summary>
  /// Checks if angle between two vectors is acute (less than 90 degrees) in 2D
  /// </summary>
  /// <param name="first">First vector</param>
  /// <param name="second">Second vector</param>
  /// <param name="third">Third vector</param>
  /// <returns>True if angle is acute</returns>
  public static bool IsAcuteAngle2D( XYZ first,
    XYZ second,
    XYZ third )
  {
    var vector1 = first - second ;
    var vector2 = third - second ;

    // Convert to 2D
    vector1 = new XYZ( vector1.X,
      vector1.Y,
      0 ) ;
    vector2 = new XYZ( vector2.X,
      vector2.Y,
      0 ) ;

    var dotProduct = vector1.DotProduct( vector2 ) ;
    return dotProduct > 0 ;
  }

  /// <summary>
  /// Checks if three points are collinear in 2D
  /// </summary>
  /// <param name="lineStartPoint">First point</param>
  /// <param name="lineEndPoint">Second point</param>
  /// <param name="testPoint">Third point</param>
  /// <param name="tolerance">Tolerance for comparison</param>
  /// <returns>True if points are collinear</returns>
  public static bool IsPointOnLineSegment2D( XYZ lineStartPoint,
    XYZ lineEndPoint,
    XYZ testPoint,
    double tolerance = ToleranceConstants.GeneralTolerance )
  {
    var distanceLine = lineStartPoint.DistanceTo2D( lineEndPoint ) ;
    var distanceToFrom = lineStartPoint.DistanceTo2D( testPoint ) ;
    var distanceToEnd = lineEndPoint.DistanceTo2D( testPoint ) ;
    return Math.Abs( distanceLine - distanceToFrom - distanceToEnd ) < tolerance ;
  }

  /// <summary>
  /// Checks if three points are collinear in 3D
  /// </summary>
  /// <param name="lineStartPoint">First point</param>
  /// <param name="lineEndPoint">Second point</param>
  /// <param name="testPoint">Third point</param>
  /// <param name="tolerance">Tolerance for comparison</param>
  /// <returns>True if points are collinear</returns>
  public static bool IsPointOnLineSegment3D( XYZ lineStartPoint,
    XYZ lineEndPoint,
    XYZ testPoint,
    double tolerance = ToleranceConstants.GeneralTolerance )
  {
    var distanceLine = lineStartPoint.DistanceTo( lineEndPoint ) ;
    var distanceToFrom = lineStartPoint.DistanceTo( testPoint ) ;
    var distanceToEnd = lineEndPoint.DistanceTo( testPoint ) ;
    return Math.Abs( distanceLine - distanceToFrom - distanceToEnd ) < tolerance ;
  }

  /// <summary>
  /// Calculates the perpendicular distance from a point to a plane
  /// </summary>
  /// <param name="normalOfPlane">Normal vector of the plane</param>
  /// <param name="originOfPlane">Origin point on the plane</param>
  /// <param name="anyPoint">Point to calculate distance from</param>
  /// <returns>The perpendicular distance from the point to the plane</returns>
  public static double GetDistanceToPlane( this XYZ normalOfPlane,
    XYZ originOfPlane,
    XYZ anyPoint )
  {
    var plane = Plane.CreateByNormalAndOrigin( normalOfPlane,
      originOfPlane ) ;
    var planeConstant = plane.Normal.DotProduct( -plane.Origin ) ;
    var numerator = plane.Normal.DotProduct( anyPoint ) + planeConstant ;

    var normalLength = Math.Sqrt( plane.Normal.DotProduct( plane.Normal ) ) ;
    return Math.Abs( numerator / normalLength ) ;
  }
}