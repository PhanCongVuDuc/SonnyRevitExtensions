using SonnyRevitExtensions.Extensions.XYZs ;

namespace SonnyRevitExtensions.Extensions.Faces.PlanarFaces ;

/// <summary>
/// Extension methods for PlanarFace operations
/// </summary>
public static class PlanarFaceExtensions
{
  /// <summary>
  /// Gets all distinct points from multiple planar faces
  /// </summary>
  /// <param name="planarFaces">The list of planar faces</param>
  /// <returns>List of distinct XYZ points from all planar faces</returns>
  public static List<XYZ> GetPoints( this List<PlanarFace> planarFaces )
  {
    return planarFaces.SelectMany( x => x.GetPoints() )
      .DistinctXYZ()
      .ToList() ;
  }

  /// <summary>
  /// Checks if two planar faces are the same (coplanar and not parallel)
  /// </summary>
  /// <param name="planarFace1">The first planar face</param>
  /// <param name="planarFace2">The second planar face</param>
  /// <returns>True if the faces are the same, false otherwise</returns>
  public static bool IsSamePlanarFace( this PlanarFace planarFace1,
    PlanarFace planarFace2 )
  {
    FaceIntersectionFaceResult faceIntersectionFaceResult = planarFace1.Intersect( planarFace2,
      out _ ) ;

    if ( faceIntersectionFaceResult == FaceIntersectionFaceResult.Intersecting ) {
      return false ;
    }

    var isParallelPlanarFace = planarFace1.IsParallelPlanarFace( planarFace2 ) ;

    // Check if two planar faces have the same normal direction
    if ( ! ( planarFace1.FaceNormal.IsAlmostEqualTo( planarFace2.FaceNormal )
             || planarFace1.FaceNormal.IsAlmostEqualTo( planarFace2.FaceNormal.Negate() ) ) ) {
      return false ;
    }

    return ! isParallelPlanarFace ;
  }

  /// <summary>
  /// Checks if two planar faces are parallel
  /// </summary>
  /// <param name="planarFace1">The first planar face</param>
  /// <param name="planarFace2">The second planar face</param>
  /// <returns>True if the faces are parallel, false otherwise</returns>
  public static bool IsParallelPlanarFace( this PlanarFace planarFace1,
    PlanarFace planarFace2 )
  {
    FaceIntersectionFaceResult faceIntersectionFaceResult = planarFace1.Intersect( planarFace2,
      out _ ) ;

    if ( faceIntersectionFaceResult == FaceIntersectionFaceResult.Intersecting ) {
      return false ;
    }

    // Check if two planar faces have the same normal direction
    if ( ! ( planarFace1.FaceNormal.IsAlmostEqualTo( planarFace2.FaceNormal )
             || planarFace1.FaceNormal.IsAlmostEqualTo( planarFace2.FaceNormal.Negate() ) ) ) {
      return false ;
    }

    var lengthOfPointToPlane = planarFace1.Origin.GetLengthOfPointToPlane( planarFace2.FaceNormal,
      planarFace2.Origin ) ;

    return lengthOfPointToPlane >= ToleranceConstants.HighPrecision ;
  }

  /// <summary>
  /// Gets the boundary points of a planar face
  /// </summary>
  /// <param name="planarFace">The planar face</param>
  /// <returns>List of boundary points</returns>
  public static List<XYZ> GetBoundaryPoints( this PlanarFace planarFace )
  {
    var xyzs = new List<XYZ>() ;

    var curveLoop = planarFace.GetEdgesAsCurveLoops()
      .FirstOrDefault() ;
    if ( curveLoop == null )
      return xyzs ;

    var curveLoopIterator = curveLoop.GetCurveLoopIterator() ;
    while ( curveLoopIterator.MoveNext() ) {
      var curve = curveLoopIterator.Current ;
      if ( curve is null )
        continue ;

      xyzs.Add( curve.GetEndPoint( 0 ) ) ;
    }

    return xyzs ;
  }
}