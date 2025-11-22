// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using SonnyRevitExtensions.Extensions.XYZs ;

namespace SonnyRevitExtensions.Extensions.GeometryObjects.Curves ;

public static class CurvePointExtensions
{
    /// <summary>
    ///     Gets all points from tessellating a curve.
    /// </summary>
    /// <param name="curve">The curve.</param>
    /// <returns>A collection of XYZ points from the curve tessellation.</returns>
    public static IEnumerable<XYZ> GetXYZPoints(this Curve curve) => curve.Tessellate() ;

    /// <summary>
    ///     Gets all points from tessellating a collection of curves.
    /// </summary>
    /// <param name="curves">The collection of curves.</param>
    /// <returns>A collection of distinct XYZ points from all curves.</returns>
    public static IEnumerable<XYZ> GetXYZPoints(this IEnumerable<Curve> curves) =>
        curves.SelectMany(x => x.GetXYZPoints())
            .DistinctXYZ() ;
}
