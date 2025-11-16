using SonnyRevitExtensions.Extensions.XYZs ;

namespace SonnyRevitExtensions.Extensions.GeometryObjects.Faces ;

/// <summary>
///     Extension methods for Face operations
/// </summary>
public static class FaceExtensions
{
    /// <summary>
    ///     Gets all points from tessellating curves of multiple faces
    /// </summary>
    /// <param name="faces">The list of faces</param>
    /// <returns>List of XYZ points from all faces</returns>
    public static List<XYZ> GetPoints(this List<Face> faces) =>
        faces.SelectMany(x => x.GetPoints())
            .ToList() ;

    /// <summary>
    ///     Gets all lines from curves of multiple faces
    /// </summary>
    /// <param name="faces">The list of faces</param>
    /// <returns>List of lines extracted from face curves</returns>
    public static List<Line> GetLines(this List<Face> faces) =>
        faces.GetCurves()
            .OfType<Line>()
            .ToList() ;

    /// <summary>
    ///     Checks if a point is inside the face
    /// </summary>
    /// <param name="face">The face to check</param>
    /// <param name="point">The point to check</param>
    /// <returns>True if the point is inside the face, false otherwise</returns>
    public static bool IsPointInside(this Face face,
        XYZ point)
    {
        IntersectionResult? intersectionResult = face.Project(point) ;

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
    /// <returns>List of intersection points that are inside both faces</returns>
    public static List<XYZ> GetIntersectionPoints(this Face face1,
        Face face2)
    {
        FaceIntersectionFaceResult faceIntersectionFaceResult = face1.Intersect(face2,
            out Curve curve) ;

        if (faceIntersectionFaceResult == FaceIntersectionFaceResult.NonIntersecting)
        {
            return [] ;
        }

        IList<XYZ>? list = curve.Tessellate() ;

        List<XYZ> listXyzes = new() ;
        foreach (XYZ xyz in list)
        {
            bool pointInsideInFace1 = face1.IsPointInside(xyz) ;
            bool pointInsideInFace2 = face2.IsPointInside(xyz) ;

            if (pointInsideInFace1 && pointInsideInFace2)
            {
                listXyzes.Add(xyz) ;
            }
        }

        return listXyzes.DistinctXYZ()
            .ToList() ;
    }

    /// <summary>
    ///     Gets intersection points between a face and a list of faces
    /// </summary>
    /// <param name="face">The face</param>
    /// <param name="faces">The list of faces to intersect with</param>
    /// <returns>List of intersection points</returns>
    public static List<XYZ> GetIntersectionPoints(this Face face,
        List<Face> faces) =>
        faces.SelectMany(x => x.GetIntersectionPoints(face))
            .ToList() ;

    /// <summary>
    ///     Gets intersection points between two lists of faces
    /// </summary>
    /// <param name="faces1">The first list of faces</param>
    /// <param name="faces2">The second list of faces</param>
    /// <returns>List of intersection points</returns>
    public static List<XYZ> GetIntersectionPoints(this List<Face> faces1,
        List<Face> faces2) =>
        faces1.SelectMany(x => x.GetIntersectionPoints(faces2))
            .ToList() ;

    /// <summary>
    ///     Gets intersection points between a face and a line
    /// </summary>
    /// <param name="face">The face</param>
    /// <param name="line">The line</param>
    /// <returns>List of intersection points</returns>
    public static List<XYZ> GetIntersectionPoints(this Face face,
        Line line)
    {
        SetComparisonResult comparisonResult = face.Intersect(line,
            out IntersectionResultArray results) ;

        List<XYZ> listXyzes = new() ;
        if (comparisonResult == SetComparisonResult.Overlap)
        {
            for (int i = 0; i < results.Size; i++)
            {
                IntersectionResult? intersectionResult = results.get_Item(i) ;
                XYZ? intersectionResultXyzPoint = intersectionResult.XYZPoint ;
                listXyzes.Add(intersectionResultXyzPoint) ;
            }
        }

        return listXyzes ;
    }

    /// <summary>
    ///     Gets intersection points between a list of faces and a line
    /// </summary>
    /// <param name="faces">The list of faces</param>
    /// <param name="line">The line</param>
    /// <returns>List of intersection points</returns>
    public static List<XYZ> GetIntersectionPoints(this List<Face> faces,
        Line line) =>
        faces.SelectMany(x => x.GetIntersectionPoints(line))
            .ToList() ;

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

        BoundingBoxUV? boundingBox = face.GetBoundingBox() ;
        UV? uv = (boundingBox.Max + boundingBox.Min) / 2.0 ;

        XYZ? computeNormal = face.ComputeNormal(uv) ;

        return computeNormal ;
    }

    /// <summary>
    ///     Gets the origin point inside the face
    /// </summary>
    /// <param name="face">The face</param>
    /// <returns>The origin point if found inside the face, null otherwise</returns>
    public static XYZ? GetOriginInFace(this Face face)
    {
        BoundingBoxUV? boundingBox = face.GetBoundingBox() ;
        UV? boundingBoxMax = boundingBox.Max ;
        UV? boundingBoxMin = boundingBox.Min ;

        UV? xyz = boundingBoxMax + boundingBoxMin ;
        UV? uv = xyz / 2 ;

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

            for (int i = 1; i < 99; i++)
            {
                double temp = i / 100d ;

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
        BoundingBoxUV? boundingBox = face.GetBoundingBox() ;
        UV? boundingBoxMax = boundingBox.Max ;
        UV? boundingBoxMin = boundingBox.Min ;

        UV? xyz = boundingBoxMax + boundingBoxMin ;
        UV? uv = xyz / 2 ;

        return face.Evaluate(uv) ;
    }

    /// <summary>
    ///     Gets all points from tessellating curves of the face
    /// </summary>
    /// <param name="face">The face</param>
    /// <returns>List of XYZ points from tessellating all curves of the face</returns>
    public static List<XYZ> GetPoints(this Face face)
    {
        List<XYZ> listXyzes = new() ;
        foreach (Curve curve in face.GetCurves())
        {
            listXyzes.AddRange(curve.Tessellate()) ;
        }

        return listXyzes ;
    }

    /// <summary>
    ///     Gets all curves from multiple faces
    /// </summary>
    /// <param name="faces">The list of faces</param>
    /// <returns>List of curves from all faces</returns>
    public static List<Curve> GetCurves(this List<Face> faces) =>
        faces.SelectMany(x => x.GetCurves())
            .ToList() ;

    /// <summary>
    ///     Gets all curves from the face edges
    /// </summary>
    /// <param name="face">The face</param>
    /// <returns>List of curves extracted from face edge loops</returns>
    public static List<Curve> GetCurves(this Face face)
    {
        List<Curve> listCurves = new() ;

        EdgeArrayArray? edgeArrayArray = face.EdgeLoops ;
        foreach (EdgeArray edgeArray in edgeArrayArray)
        {
            foreach (Edge edge in edgeArray)
            {
                listCurves.Add(edge.AsCurve()) ;
            }
        }

        return listCurves ;
    }

    /// <summary>
    ///     Gets all edges from the face
    /// </summary>
    /// <param name="face">The face</param>
    /// <returns>List of edges from all edge loops of the face</returns>
    public static List<Edge> GetEdges(this Face face)
    {
        List<Edge> listEdges = new() ;

        EdgeArrayArray? edgeArrayArray = face.EdgeLoops ;
        foreach (EdgeArray edgeArray in edgeArrayArray)
        {
            foreach (Edge edge in edgeArray)
            {
                listEdges.Add(edge) ;
            }
        }

        return listEdges ;
    }

    /// <summary>
    ///     Gets an edge from the face that matches the specified direction
    /// </summary>
    /// <param name="face">The face</param>
    /// <param name="direction">The direction vector to match</param>
    /// <returns>The edge with matching direction if found, null otherwise</returns>
    public static Edge? GetEdge(this Face face,
        XYZ direction)
    {
        List<Edge> listEdges = face.GetEdges() ;
        foreach (Edge listEdge in listEdges)
        {
            Curve? curve = listEdge.AsCurve() ;
            if (curve is Line line)
            {
                if (line.Direction.IsAlmostEqualTo(direction))
                {
                    return listEdge ;
                }
            }
        }

        return null ;
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
        SetComparisonResult result = face.Intersect(curve,
            out IntersectionResultArray? results) ;

        if (result != SetComparisonResult.Overlap
            || results is not { Size: 1 })
        {
            return null ;
        }

        return results.get_Item(0)
            .XYZPoint ;
    }
}
