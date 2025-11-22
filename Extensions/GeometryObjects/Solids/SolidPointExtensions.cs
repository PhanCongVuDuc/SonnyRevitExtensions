// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using SonnyRevitExtensions.Extensions.GeometryObjects.Curves ;

namespace SonnyRevitExtensions.Extensions.GeometryObjects.Solids ;

/// <summary>
///     Extension methods for extracting XYZ points from Solid geometry
/// </summary>
public static class SolidPointExtensions
{
    /// <summary>
    ///     Gets all XYZ points from tessellating curves of a solid
    /// </summary>
    /// <param name="solid">The solid to extract points from</param>
    /// <returns>Collection of distinct XYZ points from all curves of the solid's faces</returns>
    public static IEnumerable<XYZ> GetXyzes(this Solid solid) =>
        solid.GetCurves()
            .GetXYZPoints() ;

    /// <summary>
    ///     Gets all XYZ points from tessellating curves of a collection of solids
    /// </summary>
    /// <param name="solids">Collection of solids to extract points from</param>
    /// <returns>Collection of distinct XYZ points from all curves of all solids</returns>
    public static IEnumerable<XYZ> GetXyzes(this IEnumerable<Solid> solids) =>
        solids.SelectMany(x => x.GetXyzes()) ;
}
