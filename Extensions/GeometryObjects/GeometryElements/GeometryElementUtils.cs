// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

namespace SonnyRevitExtensions.Extensions.GeometryObjects.GeometryElements ;

/// <summary>
///     Utility methods for GeometryElement operations
/// </summary>
public static class GeometryElementUtils
{
    /// <summary>
    ///     Gets the geometry element from an element with optional geometry options
    /// </summary>
    /// <param name="element">The element to get geometry from</param>
    /// <param name="options">Geometry options for retrieving geometry. If null, default options will be used</param>
    /// <returns>The geometry element, or null if the element has no geometry</returns>
    public static GeometryElement? GetGeometryElement(this Element element,
        Options? options = null)
    {
        return element.get_Geometry( options ?? new Options() ) ;
    }
}
