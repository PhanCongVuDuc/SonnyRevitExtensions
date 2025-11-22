using SonnyRevitExtensions.Extensions.XYZs ;

namespace SonnyRevitExtensions.Extensions.GeometryObjects.Faces.PlanarFaces ;

/// <summary>
///     Extension methods for PlanarFace operations
/// </summary>
public static class PlanarFaceExtensions
{
    /// <summary>
    ///     Gets all distinct points from multiple planar faces
    /// </summary>
    /// <param name="planarFaces">The collection of planar faces</param>
    /// <returns>Collection of distinct XYZ points from all planar faces</returns>
    public static IEnumerable<XYZ> GetPoints(this IEnumerable<PlanarFace> planarFaces) =>
        planarFaces.SelectMany(x => x.GetPoints())
            .DistinctXYZ() ;

    /// <summary>
    ///     Checks if two planar faces are the same (coplanar and not parallel)
    /// </summary>
    /// <param name="planarFace1">The first planar face</param>
    /// <param name="planarFace2">The second planar face</param>
    /// <returns>True if the faces are the same, false otherwise</returns>
    public static bool IsSamePlanarFace(this PlanarFace planarFace1,
        PlanarFace planarFace2)
    {
        var faceIntersectionFaceResult = planarFace1.Intersect(planarFace2,
            out _) ;

        if (faceIntersectionFaceResult == FaceIntersectionFaceResult.Intersecting)
        {
            return false ;
        }

        var isParallelPlanarFace = planarFace1.IsParallelPlanarFace(planarFace2) ;

        // Check if two planar faces have the same normal direction
        if (! (planarFace1.FaceNormal.IsAlmostEqualTo(planarFace2.FaceNormal)
               || planarFace1.FaceNormal.IsAlmostEqualTo(planarFace2.FaceNormal.Negate())))
        {
            return false ;
        }

        return ! isParallelPlanarFace ;
    }

    /// <summary>
    ///     Checks if two planar faces are parallel
    /// </summary>
    /// <param name="planarFace1">The first planar face</param>
    /// <param name="planarFace2">The second planar face</param>
    /// <returns>True if the faces are parallel, false otherwise</returns>
    public static bool IsParallelPlanarFace(this PlanarFace planarFace1,
        PlanarFace planarFace2)
    {
        var faceIntersectionFaceResult = planarFace1.Intersect(planarFace2,
            out _) ;

        if (faceIntersectionFaceResult == FaceIntersectionFaceResult.Intersecting)
        {
            return false ;
        }

        // Check if two planar faces have the same normal direction
        if (! (planarFace1.FaceNormal.IsAlmostEqualTo(planarFace2.FaceNormal)
               || planarFace1.FaceNormal.IsAlmostEqualTo(planarFace2.FaceNormal.Negate())))
        {
            return false ;
        }

        var lengthOfPointToPlane = planarFace1.Origin.GetLengthOfPointToPlane(planarFace2.FaceNormal,
            planarFace2.Origin) ;

        return lengthOfPointToPlane >= ToleranceConstants.Tolerance1E9 ;
    }

    /// <summary>
    ///     Gets the boundary points of a planar face
    /// </summary>
    /// <param name="planarFace">The planar face</param>
    /// <returns>Collection of boundary points</returns>
    public static IEnumerable<XYZ> GetBoundaryPoints(this PlanarFace planarFace)
    {
        var curveLoop = planarFace.GetEdgesAsCurveLoops()
            .FirstOrDefault() ;
        if (curveLoop == null)
        {
            yield break ;
        }

        var curveLoopIterator = curveLoop.GetCurveLoopIterator() ;
        while (curveLoopIterator.MoveNext())
        {
            var curve = curveLoopIterator.Current ;
            if (curve is null)
            {
                continue ;
            }

            yield return curve.GetEndPoint(0) ;
        }
    }

    /// <summary>
    ///     Remove coplanar faces from the collection, keeping only one face per plane
    /// </summary>
    /// <param name="planarFaces">The collection of planar faces</param>
    /// <returns>A new collection with coplanar faces removed</returns>
    public static IEnumerable<PlanarFace> RemoveCoplanarFaces(this IEnumerable<PlanarFace> planarFaces)
    {
        var planarFacesList = planarFaces.ToList() ;
        if (planarFacesList.Count <= 1)
        {
            foreach (var face in planarFacesList)
            {
                yield return face ;
            }

            yield break ;
        }

        var uniqueFaces = new List<PlanarFace>() ;

        foreach (var face in planarFacesList)
        {
            var isCoplanar = false ;

            foreach (var uniqueFace in uniqueFaces)
            {
                // Check if faces are coplanar by comparing their planes
                if (AreFacesCoplanar(face,
                        uniqueFace))
                {
                    isCoplanar = true ;
                    break ;
                }
            }

            if (! isCoplanar)
            {
                uniqueFaces.Add(face) ;
                yield return face ;
            }
        }
    }

    /// <summary>
    ///     Check if two planar faces are coplanar (on the same plane)
    /// </summary>
    public static bool AreFacesCoplanar(this PlanarFace face1,
        PlanarFace face2)
    {
        // Get plane origins and normals
        var origin1 = face1.Origin ;
        var normal1 = face1.FaceNormal ;
        var origin2 = face2.Origin ;
        var normal2 = face2.FaceNormal ;

        return AreFacesCoplanar(normal1,
            origin1,
            normal2,
            origin2) ;
    }

    public static bool AreFacesCoplanar(XYZ normal1,
        XYZ origin1,
        XYZ normal2,
        XYZ origin2)
    {
        // Check if normals are parallel (same direction or opposite)
        if (! normal1.IsParallel(normal2))
        {
            return false ;
        }

        // Check if origin2 lies on the plane defined by face1
        var vectorBetweenOrigins = origin2 - origin1 ;
        var distance = Math.Abs(vectorBetweenOrigins.DotProduct(normal1)) ;

        return distance < ToleranceConstants.Tolerance1E4 ;
    }
}
