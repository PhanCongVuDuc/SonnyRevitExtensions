// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using SonnyRevitExtensions.Extensions.GeometryObjects.Faces ;
using SonnyRevitExtensions.Extensions.GeometryObjects.GeometryElements ;
using SonnyRevitExtensions.Extensions.GeometryObjects.Solids ;

namespace SonnyRevitExtensions.Extensions.Elements ;

/// <summary>
///     Extension methods for Element curve operations
/// </summary>
public static class ElementCurveExtensions
{
    /// <summary>
    ///     Gets all curves from an element's geometry
    /// </summary>
    /// <param name="element">The element to extract curves from</param>
    /// <param name="options">Geometry options for retrieving geometry. If null, default options will be used</param>
    /// <returns>Collection of curves from the element</returns>
    public static IEnumerable<Curve> GetCurves(this Element element,
        Options? options = null)
    {
        // Get curves from solids
        foreach (var curve in element.GetSolids(options)
                     .GetFaces()
                     .GetCurves())
        {
            yield return curve ;
        }

        // Get curves directly from geometry
        var geometryElement = element.GetGeometryElement(options) ;
        if (geometryElement == null)
        {
            yield break ;
        }

        foreach (var geometryObject in geometryElement)
        {
            if (geometryObject is Curve curve)
            {
                yield return curve ;
            }
            else if (geometryObject is GeometryInstance geometryInstance)
            {
                var instanceGeometry = geometryInstance.GetInstanceGeometry() ;
                foreach (var geoObject in instanceGeometry)
                {
                    if (geoObject is not Curve instanceCurve)
                    {
                        continue ;
                    }

                    yield return instanceCurve ;
                }
            }
        }
    }

    /// <summary>
    ///     Gets all lines from a collection of elements
    /// </summary>
    /// <param name="elements">Collection of elements to extract lines from</param>
    /// <param name="options">Geometry options for retrieving geometry. If null, default options will be used</param>
    /// <returns>Collection of lines from all elements</returns>
    public static IEnumerable<Line> GetLines(this IEnumerable<Element> elements,
        Options? options = null) =>
        elements.SelectMany(x => x.GetLines(options)) ;

    /// <summary>
    ///     Gets all lines from an element's geometry
    /// </summary>
    /// <param name="element">The element to extract lines from</param>
    /// <param name="options">Geometry options for retrieving geometry. If null, default options will be used</param>
    /// <returns>Collection of lines from the element</returns>
    public static IEnumerable<Line> GetLines(this Element element,
        Options? options = null)
    {
        var geometry = element.GetGeometryElement(options) ;
        if (geometry != null)
        {
            foreach (var geometryObject in geometry)
            {
                if (geometryObject is not Line line)
                {
                    continue ;
                }

                yield return line ;
            }
        }

        // Get lines from solids
        foreach (var line in element.GetSolids(options)
                     .GetFaces()
                     .GetLines())
        {
            yield return line ;
        }
    }

    /// <summary>
    ///     Gets all arcs from an element's geometry
    /// </summary>
    /// <param name="element">The element to extract arcs from</param>
    /// <param name="options">Geometry options for retrieving geometry. If null, default options will be used</param>
    /// <returns>Collection of arcs from the element</returns>
    public static IEnumerable<Arc> GetArcs(this Element element,
        Options? options = null)
    {
        // Get arcs from solids

        foreach (var arc in element.GetSolids(options)
                     .GetFaces()
                     .GetCurves()
                     .OfType<Arc>())
        {
            yield return arc ;
        }

        // Get arcs directly from geometry
        var geometryElement = element.GetGeometryElement(options) ;
        if (geometryElement == null)
        {
            yield break ;
        }

        foreach (var geometryObject in geometryElement)
        {
            if (geometryObject is Arc arc)
            {
                yield return arc ;
            }
            else if (geometryObject is GeometryInstance geometryInstance)
            {
                var instanceGeometry = geometryInstance.GetInstanceGeometry() ;
                foreach (var geoObject in instanceGeometry)
                {
                    if (geoObject is not Arc instanceArc)
                    {
                        continue ;
                    }

                    yield return instanceArc ;
                }
            }
        }
    }

    /// <summary>
    ///     Gets all poly lines from an element's geometry
    /// </summary>
    /// <param name="element">The element to extract poly lines from</param>
    /// <param name="options">Geometry options for retrieving geometry. If null, default options will be used</param>
    /// <returns>Collection of poly lines from the element</returns>
    public static IEnumerable<PolyLine> GetPolyLines(this Element element,
        Options? options = null)
    {
        var geometryElement = element.GetGeometryElement(options) ;
        if (geometryElement == null)
        {
            yield break ;
        }

        foreach (var geometryObject in geometryElement)
        {
            if (geometryObject is PolyLine polyLine)
            {
                yield return polyLine ;
            }
            else if (geometryObject is GeometryInstance geometryInstance)
            {
                var instanceGeometry = geometryInstance.GetInstanceGeometry() ;
                foreach (var geoObject in instanceGeometry)
                {
                    if (geoObject is not PolyLine instancePolyLine)
                    {
                        continue ;
                    }

                    yield return instancePolyLine ;
                }
            }
        }
    }

    /// <summary>
    ///     Gets all edges from an element's geometry
    /// </summary>
    /// <param name="element">The element to extract edges from</param>
    /// <param name="options">Geometry options for retrieving geometry. If null, default options will be used</param>
    /// <returns>Collection of edges from the element</returns>
    public static IEnumerable<Edge> GetEdges(this Element element,
        Options? options = null) =>
        element.GetSolids(options)
            .SelectMany(solid => solid.GetFaces()
                .SelectMany(face => face.GetEdges())) ;
}
