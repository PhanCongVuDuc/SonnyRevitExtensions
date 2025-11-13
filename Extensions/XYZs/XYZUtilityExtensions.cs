namespace SonnyRevitExtensions.Extensions.XYZs ;

/// <summary>
/// Extension methods for XYZ utility operations (string conversion, clipboard, etc.)
/// </summary>
public static class XYZUtilityExtensions
{
  /// <summary>
  /// Converts XYZ to string representation
  /// </summary>
  /// <param name="xyz">Point to convert</param>
  /// <returns>String representation</returns>
  public static string ToString( this XYZ xyz )
  {
    return $"{Math.Round( xyz.X, 9 )}_{Math.Round( xyz.Y, 9 )}_{Math.Round( xyz.Z, 9 )}" ;
  }

  /// <summary>
  /// Gets XY coordinates as string
  /// </summary>
  /// <param name="xyz">Point to convert</param>
  /// <returns>XY coordinates as string</returns>
  public static string ToOXYString( this XYZ xyz )
  {
    return $"{Math.Round( xyz.X, 6 )}, {Math.Round( xyz.Y, 6 )}" ;
  }
}