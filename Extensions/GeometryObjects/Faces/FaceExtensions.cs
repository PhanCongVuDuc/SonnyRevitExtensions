using SonnyRevitExtensions.Extensions.XYZs ;

namespace SonnyRevitExtensions.Extensions.GeometryObjects.Faces ;

/// <summary>
///     Extension methods for Face operations
/// </summary>
public static class FaceExtensions
{
    /// <summary>
    ///     Checks if a point is inside the face
    /// </summary>
    /// <param name="face">The face to check</param>
    /// <param name="point">The point to check</param>
    /// <returns>True if the point is inside the face, false otherwise</returns>
    public static bool IsPointInside(this Face face,
        XYZ point)
    {
        var intersectionResult = face.Project(point) ;

        // intersectionResult== null lí do là điểm XYZ không nằm trong không gian face
        if (intersectionResult == null)
        {
            return false ;
        }

        return Math.Abs(intersectionResult.Distance) < 1e-3 ;
    }

    /// <summary>
    ///     Gets intersection points between two faces
    /// </summary>
    /// <param name="face1">The first face</param>
    /// <param name="face2">The second face</param>
    /// <returns>Collection of intersection points that are inside both faces</returns>
    public static IEnumerable<XYZ> GetIntersectionPoints(this Face face1,
        Face face2)
    {
        var faceIntersectionFaceResult = face1.Intersect(face2,
            out var curve) ;

        if (faceIntersectionFaceResult == FaceIntersectionFaceResult.NonIntersecting)
        {
            yield break ;
        }

        IList<XYZ>? list = curve.Tessellate() ;

        foreach (var xyz in list)
        {
            var pointInsideInFace1 = face1.IsPointInside(xyz) ;
            var pointInsideInFace2 = face2.IsPointInside(xyz) ;

            if (pointInsideInFace1 && pointInsideInFace2)
            {
                yield return xyz ;
            }
        }
    }

    /// <summary>
    ///     Gets intersection points between a face and a collection of faces
    /// </summary>
    /// <param name="face">The face</param>
    /// <param name="faces">The collection of faces to intersect with</param>
    /// <returns>Collection of intersection points</returns>
    public static IEnumerable<XYZ> GetIntersectionPoints(this Face face,
        IEnumerable<Face> faces) =>
        faces.SelectMany(x => x.GetIntersectionPoints(face)) ;

    /// <summary>
    ///     Gets intersection points between two collections of faces
    /// </summary>
    /// <param name="faces1">The first collection of faces</param>
    /// <param name="faces2">The second collection of faces</param>
    /// <returns>Collection of intersection points</returns>
    public static IEnumerable<XYZ> GetIntersectionPoints(this IEnumerable<Face> faces1,
        IEnumerable<Face> faces2) =>
        faces1.SelectMany(x => x.GetIntersectionPoints(faces2)) ;

    /// <summary>
    ///     Gets intersection points between a face and a line
    /// </summary>
    /// <param name="face">The face</param>
    /// <param name="line">The line</param>
    /// <returns>Collection of intersection points</returns>
    public static IEnumerable<XYZ> GetIntersectionPoints(this Face face,
        Line line)
    {
        var comparisonResult = face.Intersect(line,
            out var results) ;

        if (comparisonResult != SetComparisonResult.Overlap)
        {
            yield break ;
        }

        for (var i = 0; i < results.Size; i++)
        {
            var intersectionResult = results.get_Item(i) ;
            yield return intersectionResult.XYZPoint ;
        }
    }

    /// <summary>
    ///     Gets intersection points between a collection of faces and a line
    /// </summary>
    /// <param name="faces">The collection of faces</param>
    /// <param name="line">The line</param>
    /// <returns>Collection of intersection points</returns>
    public static IEnumerable<XYZ> GetIntersectionPoints(this IEnumerable<Face> faces,
        Line line) =>
        faces.SelectMany(x => x.GetIntersectionPoints(line)) ;

    /// <summary>
    ///     Gets the normal vector of the face
    /// </summary>
    /// <param name="face">The face</param>
    /// <returns>The normal vector. For PlanarFace, returns FaceNormal. For other faces, computes normal at the center point.</returns>
    public static XYZ GetNormal(this Face face)
    {
        if (face is PlanarFace planar)
        {
            return planar.FaceNormal ;
        }

        var boundingBox = face.GetBoundingBox() ;
        var uv = (boundingBox.Max + boundingBox.Min) / 2.0 ;

        var computeNormal = face.ComputeNormal(uv) ;

        return computeNormal ;
    }

    /// <summary>
    ///     Gets the origin point inside the face
    /// </summary>
    /// <param name="face">The face</param>
    /// <returns>The origin point if found inside the face, null otherwise</returns>
    public static XYZ? GetOriginInFace(this Face face)
    {
        var boundingBox = face.GetBoundingBox() ;
        var boundingBoxMax = boundingBox.Max ;
        var boundingBoxMin = boundingBox.Min ;

        var xyz = boundingBoxMax + boundingBoxMin ;
        var uv = xyz / 2 ;

        XYZ? result = null ;
        if (face.IsInside(uv))
        {
            result = face.Evaluate(uv) ;
        }
        else
        {
            UV? uvTempFar ;
            if (boundingBoxMax.DistanceTo(UV.Zero) > boundingBoxMin.DistanceTo(UV.Zero))
            {
                uvTempFar = boundingBoxMax ;
            }
            else
            {
                uvTempFar = boundingBoxMin ;
            }

            for (var i = 1; i < 99; i++)
            {
                var temp = i / 100d ;

                UV tempUv = new(uvTempFar.U * temp,
                    uvTempFar.V * temp) ;
                if (face.IsInside(tempUv))
                {
                    result = face.Evaluate(tempUv) ;
                    break ;
                }
            }
        }

        return result ;
    }

    /// <summary>
    ///     Gets the origin point of the face (center of bounding box)
    /// </summary>
    /// <param name="face">The face</param>
    /// <returns>The origin point at the center of the face's bounding box</returns>
    public static XYZ GetOrigin(this Face face)
    {
        var boundingBox = face.GetBoundingBox() ;
        var boundingBoxMax = boundingBox.Max ;
        var boundingBoxMin = boundingBox.Min ;

        var xyz = boundingBoxMax + boundingBoxMin ;
        var uv = xyz / 2 ;

        return face.Evaluate(uv) ;
    }

    /// <summary>
    ///     Gets the intersection point between a face and a curve
    /// </summary>
    /// <param name="face">The face</param>
    /// <param name="curve">The curve</param>
    /// <returns>The intersection point if there is exactly one intersection, null otherwise</returns>
    public static XYZ? GetIntersection(this Face face,
        Curve curve)
    {
        var result = face.Intersect(curve,
            out var results) ;

        if (result != SetComparisonResult.Overlap
            || results is not { Size: 1 })
        {
            return null ;
        }

        return results.get_Item(0)
            .XYZPoint ;
    }
}
