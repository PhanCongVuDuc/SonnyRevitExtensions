// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

namespace SonnyRevitExtensions.Extensions.GeometryObjects.Faces ;

/// <summary>
///     Extension methods for extracting curves and edges from Face geometry
/// </summary>
public static class FaceCurveExtensions
{
    /// <summary>
    ///     Gets all lines from curves of the face
    /// </summary>
    /// <param name="face">The face</param>
    /// <returns>Collection of lines extracted from face curves</returns>
    public static IEnumerable<Line> GetLines(this Face face) =>
        face.GetCurves()
            .OfType<Line>() ;

    /// <summary>
    ///     Gets all lines from curves of multiple faces
    /// </summary>
    /// <param name="faces">The collection of faces</param>
    /// <returns>Collection of lines extracted from face curves</returns>
    public static IEnumerable<Line> GetLines(this IEnumerable<Face> faces) => faces.SelectMany(x => x.GetLines()) ;

    /// <summary>
    ///     Gets all curves from multiple faces
    /// </summary>
    /// <param name="faces">The collection of faces</param>
    /// <returns>Collection of curves from all faces</returns>
    public static IEnumerable<Curve> GetCurves(this IEnumerable<Face> faces) => faces.SelectMany(x => x.GetCurves()) ;

    /// <summary>
    ///     Gets all curves from the face edges
    /// </summary>
    /// <param name="face">The face</param>
    /// <returns>Collection of curves extracted from face edge loops</returns>
    public static IEnumerable<Curve> GetCurves(this Face face)
    {
        var edgeArrayArray = face.EdgeLoops ;
        foreach (EdgeArray edgeArray in edgeArrayArray)
        {
            foreach (Edge edge in edgeArray)
            {
                yield return edge.AsCurve() ;
            }
        }
    }

    /// <summary>
    ///     Gets all arcs from curves of the face
    /// </summary>
    /// <param name="face">The face</param>
    /// <returns>Collection of arcs extracted from face curves</returns>
    public static IEnumerable<Arc> GetArcs(this Face face) =>
        face.GetCurves()
            .OfType<Arc>() ;

    /// <summary>
    ///     Gets all arcs from curves of multiple faces
    /// </summary>
    /// <param name="faces">The collection of faces</param>
    /// <returns>Collection of arcs extracted from face curves</returns>
    public static IEnumerable<Arc> GetArcs(this IEnumerable<Face> faces) => faces.SelectMany(x => x.GetArcs()) ;

    /// <summary>
    ///     Gets all edges from the face
    /// </summary>
    /// <param name="face">The face</param>
    /// <returns>Collection of edges from all edge loops of the face</returns>
    public static IEnumerable<Edge> GetEdges(this Face face)
    {
        var edgeArrayArray = face.EdgeLoops ;
        foreach (EdgeArray edgeArray in edgeArrayArray)
        {
            foreach (Edge edge in edgeArray)
            {
                yield return edge ;
            }
        }
    }

    /// <summary>
    ///     Gets all edges from multiple faces
    /// </summary>
    /// <param name="faces">The collection of faces</param>
    /// <returns>Collection of edges from all faces</returns>
    public static IEnumerable<Edge> GetEdges(this IEnumerable<Face> faces) => faces.SelectMany(x => x.GetEdges()) ;

    /// <summary>
    ///     Gets an edge from the face that matches the specified direction
    /// </summary>
    /// <param name="face">The face</param>
    /// <param name="direction">The direction vector to match</param>
    /// <returns>The edge with matching direction if found, null otherwise</returns>
    public static Edge? GetEdge(this Face face,
        XYZ direction)
    {
        var listEdges = face.GetEdges() ;
        foreach (var listEdge in listEdges)
        {
            var curve = listEdge.AsCurve() ;
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
}
