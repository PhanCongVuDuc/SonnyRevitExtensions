namespace SonnyRevitExtensions.Extensions.GeometryObjects.Faces.CylindricalFaces ;

/// <summary>
/// Extension methods for CylindricalFace operations
/// </summary>
public static class CylindricalFaceExtensions
{
  /// <summary>
  /// Gets the radius of a cylindrical face
  /// </summary>
  /// <param name="cylindricalFace">The cylindrical face</param>
  /// <returns>The radius if found from an arc edge, null otherwise</returns>
  public static double? GetRadius( this CylindricalFace cylindricalFace )
  {
    var curveLoop = cylindricalFace.GetEdgesAsCurveLoops()
      .FirstOrDefault() ;
    if ( curveLoop == null )
      return null ;

    var curveLoopIterator = curveLoop.GetCurveLoopIterator() ;
    while ( curveLoopIterator.MoveNext() ) {
      var curve = curveLoopIterator.Current ;
      if ( curve is not Arc arc )
        continue ;

      return arc.Radius ;
    }

    return null ;
  }
}