namespace SonnyRevitExtensions.Extensions.XYZs ;

/// <summary>
/// Extension methods for XYZ coordinate transformations
/// </summary>
public static class XYZTransformExtensions
{
  /// <summary>
  /// Creates a copy of the XYZ point
  /// </summary>
  /// <param name="xyz">Point to clone</param>
  /// <returns>Cloned point</returns>
  public static XYZ Clone( this XYZ xyz ) =>
    new(xyz.X,
      xyz.Y,
      xyz.Z) ;

  /// <summary>
  /// Calculates 2D angle between two points
  /// </summary>
  /// <param name="firstPoint">First point</param>
  /// <param name="secondPoint">Second point</param>
  /// <returns>Angle in radians</returns>
  public static double AngleTo2D( this XYZ firstPoint,
    XYZ secondPoint )
  {
    return firstPoint.SetZ( 0 )
      .AngleTo( secondPoint.SetZ( 0 ) ) ;
  }
}