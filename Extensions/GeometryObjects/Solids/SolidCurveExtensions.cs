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
}
