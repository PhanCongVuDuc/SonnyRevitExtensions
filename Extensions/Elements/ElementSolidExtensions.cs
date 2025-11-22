// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using SonnyRevitExtensions.Extensions.GeometryObjects.GeometryElements ;

namespace SonnyRevitExtensions.Extensions.Elements ;

/// <summary>
///     Extension methods for Element geometry operations
/// </summary>
public static class ElementSolidExtensions
{
    /// <summary>
    ///     Gets all valid solids from an element's geometry
    /// </summary>
    /// <param name="element">The element to extract solids from</param>
    /// <param name="options">Geometry options for retrieving geometry. If null, default options will be used</param>
    /// <returns>Collection of solids with volume greater than tolerance (1e-9)</returns>
    public static IEnumerable<Solid> GetSolids(this Element element,
        Options? options = null)
    {
        var geometry = element.GetGeometryElement(options) ;
        if (geometry is null)
        {
            yield break ;
        }

        foreach (var geometryObject in geometry)
        {
            if (geometryObject is Solid item)
            {
                if (item.Volume > ToleranceConstants.Tolerance1E9)
                {
                    yield return item ;
                }
            }
            else if (geometryObject is GeometryInstance geometryInstance)
            {
                var geometryElement = geometryInstance.GetInstanceGeometry() ;
                foreach (var o in geometryElement)
                {
                    if (o is Solid solid
                        && solid.Volume > ToleranceConstants.Tolerance1E9)
                    {
                        yield return solid ;
                    }
                }
            }
        }
    }

    /// <summary>
    ///     Gets all valid solids from a collection of elements with specified geometry options
    /// </summary>
    /// <param name="elements">Collection of elements to extract solids from</param>
    /// <param name="options">Geometry options for retrieving geometry. If null, default options will be used</param>
    /// <returns>Collection of solids from all elements</returns>
    public static IEnumerable<Solid> GetSolids(this IEnumerable<Element> elements,
        Options? options = null) =>
        elements.SelectMany(x => x.GetSolids(options)) ;
}
