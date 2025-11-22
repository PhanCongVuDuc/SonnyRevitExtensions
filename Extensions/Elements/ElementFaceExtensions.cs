// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using SonnyRevitExtensions.Extensions.GeometryObjects.Solids ;

namespace SonnyRevitExtensions.Extensions.Elements ;

/// <summary>
///     Extension methods for Element face operations
/// </summary>
public static class ElementFaceExtensions
{
    /// <summary>
    ///     Gets all faces from an element's geometry
    /// </summary>
    /// <param name="element">The element to extract faces from</param>
    /// <param name="options">Geometry options for retrieving geometry. If null, default options will be used</param>
    /// <returns>Collection of faces from the element</returns>
    public static IEnumerable<Face> GetFaces(this Element element,
        Options? options = null) =>
        element.GetSolids(options)
            .GetFaces() ;

    /// <summary>
    ///     Gets all planar faces from an element's geometry
    /// </summary>
    /// <param name="element">The element to extract planar faces from</param>
    /// <param name="options">Geometry options for retrieving geometry. If null, default options will be used</param>
    /// <returns>Collection of planar faces from the element</returns>
    public static IEnumerable<PlanarFace> GetPlanarFaces(this Element element,
        Options? options = null) =>
        element.GetSolids(options)
            .GetPlanarFaces() ;

    /// <summary>
    ///     Gets all cylindrical faces from a collection of solids
    /// </summary>
    /// <param name="solids">Collection of solids to extract cylindrical faces from</param>
    /// <returns>Collection of cylindrical faces from all solids</returns>
    public static IEnumerable<CylindricalFace> GetCylindricalFaces(this IEnumerable<Solid> solids) =>
        solids.GetFaces()
            .OfType<CylindricalFace>() ;

    /// <summary>
    ///     Gets all cylindrical faces from an element's geometry
    /// </summary>
    /// <param name="element">The element to extract cylindrical faces from</param>
    /// <param name="options">Geometry options for retrieving geometry. If null, default options will be used</param>
    /// <returns>Collection of cylindrical faces from the element</returns>
    public static IEnumerable<CylindricalFace> GetCylindricalFaces(this Element element,
        Options? options = null) =>
        element.GetSolids(options)
            .GetCylindricalFaces() ;
}
