// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using SonnyRevitExtensions.Extensions.GeometryObjects.Solids ;
using SonnyRevitExtensions.Extensions.XYZs ;

namespace SonnyRevitExtensions.Extensions.Elements ;

/// <summary>
///     Extension methods for extracting XYZ points from Element geometry
/// </summary>
public static class ElementPointExtensions
{
    /// <summary>
    ///     Gets all distinct XYZ points from an element's geometry
    /// </summary>
    /// <param name="element">The element to extract points from</param>
    /// <param name="options">Geometry options for retrieving geometry. If null, default options will be used</param>
    /// <returns>Collection of distinct XYZ points from tessellating curves of the element's solids</returns>
    public static IEnumerable<XYZ> GetXyzs(this Element element,
        Options? options = null) =>
        element.GetSolids(options)
            .GetXyzes()
            .DistinctXYZ() ;
}
