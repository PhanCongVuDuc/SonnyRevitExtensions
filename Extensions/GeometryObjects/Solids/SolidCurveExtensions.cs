// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using SonnyRevitExtensions.Extensions.GeometryObjects.Faces ;

namespace SonnyRevitExtensions.Extensions.GeometryObjects.Solids ;

/// <summary>
///     Extension methods for extracting curves from Solid geometry
/// </summary>
public static class SolidCurveExtensions
{
    /// <summary>
    ///     Gets all curves from a collection of solids
    /// </summary>
    /// <param name="solids">Collection of solids to extract curves from</param>
    /// <returns>Collection of curves from all faces of all solids</returns>
    public static IEnumerable<Curve> GetCurves(this IEnumerable<Solid> solids) =>
        solids.SelectMany(x => x.GetCurves()) ;

    /// <summary>
    ///     Gets all curves from a solid's faces
    /// </summary>
    /// <param name="solid">The solid to extract curves from</param>
    /// <returns>Collection of curves extracted from all edge loops of the solid's faces</returns>
    public static IEnumerable<Curve> GetCurves(this Solid solid) =>
        solid.GetFaces()
            .SelectMany(x => x.GetCurves()) ;

    /// <summary>
    ///     Gets all lines from a solid's faces
    /// </summary>
    /// <param name="solid">The solid to extract lines from</param>
    /// <returns>Collection of lines extracted from all curves of the solid's faces</returns>
    public static IEnumerable<Line> GetLines(this Solid solid) =>
        solid.GetFaces()
            .GetLines() ;

    /// <summary>
    ///     Gets all lines from a collection of solids
    /// </summary>
    /// <param name="solids">Collection of solids to extract lines from</param>
    /// <returns>Collection of lines from all faces of all solids</returns>
    public static IEnumerable<Line> GetLines(this IEnumerable<Solid> solids) =>
        solids.SelectMany(x => x.GetLines()) ;

    /// <summary>
    ///     Gets all arcs from a solid's faces
    /// </summary>
    /// <param name="solid">The solid to extract arcs from</param>
    /// <returns>Collection of arcs extracted from all curves of the solid's faces</returns>
    public static IEnumerable<Arc> GetArcs(this Solid solid) =>
        solid.GetFaces()
            .GetArcs() ;

    /// <summary>
    ///     Gets all arcs from a collection of solids
    /// </summary>
    /// <param name="solids">Collection of solids to extract arcs from</param>
    /// <returns>Collection of arcs from all faces of all solids</returns>
    public static IEnumerable<Arc> GetArcs(this IEnumerable<Solid> solids) =>
        solids.SelectMany(x => x.GetArcs()) ;

    /// <summary>
    ///     Gets all edges from a solid's faces
    /// </summary>
    /// <param name="solid">The solid to extract edges from</param>
    /// <returns>Collection of edges from all edge loops of the solid's faces</returns>
    public static IEnumerable<Edge> GetEdges(this Solid solid) =>
        solid.GetFaces()
            .GetEdges() ;

    /// <summary>
    ///     Gets all edges from a collection of solids
    /// </summary>
    /// <param name="solids">Collection of solids to extract edges from</param>
    /// <returns>Collection of edges from all faces of all solids</returns>
    public static IEnumerable<Edge> GetEdges(this IEnumerable<Solid> solids) =>
        solids.SelectMany(x => x.GetEdges()) ;
}
