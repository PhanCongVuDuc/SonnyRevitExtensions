namespace SonnyRevitExtensions.Extensions.Lines ;

public static partial class LineExtensions
{
  /// <summary>
  /// Determines whether the specified point lies on this line.
  /// </summary>
  /// <param name="line">The line to check.</param>
  /// <param name="xyz">The point to check.</param>
  /// <returns>True if the point lies on the line; otherwise, false.</returns>
  public static bool ContainsPoint( this Line line,
    XYZ xyz )
  {
    var intersectionResult = line.Project( xyz ) ;
    return intersectionResult.XYZPoint.IsAlmostEqualTo( xyz ) ;
  }

  /// <summary>
  /// Creates a new line offset from this line by the specified distance in the given direction.
  /// </summary>
  /// <param name="line">The line to offset.</param>
  /// <param name="direct">The direction vector for the offset.</param>
  /// <param name="offset">The offset distance.</param>
  /// <returns>A new offset line.</returns>
  public static Line Offset( this Line line,
    XYZ direct,
    double offset )
  {
    var point1 = line.GetEndPoint( 0 ) + direct * offset ;
    var point2 = line.GetEndPoint( 1 ) + direct * offset ;
    return Line.CreateBound( point1,
      point2 ) ;
  }

  /// <summary>
  /// Gets the midpoint of this line.
  /// </summary>
  /// <param name="line">The line.</param>
  /// <returns>The midpoint XYZ coordinate.</returns>
  public static XYZ GetMidpoint( this Line line )
  {
    return line.GetEndPoint( 0 )
      .Add( line.GetEndPoint( 1 ) )
      .Divide( 2 ) ;
  }
}