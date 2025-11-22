// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using SonnyRevitExtensions.Extensions.XYZs ;

namespace SonnyRevitExtensions.Extensions.GeometryObjects.Solids ;

/// <summary>
///     Extension methods for Solid geometry operations
/// </summary>
public static class SolidFaceExtensions
{
    /// <summary>
    ///     Gets all faces from a collection of solids
    /// </summary>
    /// <param name="solids">Collection of solids</param>
    /// <returns>Collection of faces from all solids</returns>
    public static IEnumerable<Face> GetFaces(this IEnumerable<Solid> solids) => solids.SelectMany(x => x.GetFaces()) ;

    /// <summary>
    ///     Gets all faces from a solid
    /// </summary>
    /// <param name="solid">The solid</param>
    /// <returns>Collection of faces from the solid</returns>
    public static IEnumerable<Face> GetFaces(this Solid solid)
    {
        foreach (Face solidFace in solid.Faces)
        {
            yield return solidFace ;
        }
    }

    /// <summary>
    ///     Gets all planar faces from a collection of solids
    /// </summary>
    /// <param name="solids">Collection of solids</param>
    /// <returns>Collection of planar faces from all solids</returns>
    public static IEnumerable<PlanarFace> GetPlanarFaces(this IEnumerable<Solid> solids) =>
        solids.GetFaces()
            .OfType<PlanarFace>() ;

    /// <summary>
    ///     Gets all planar faces from a solid
    /// </summary>
    /// <param name="solid">The solid</param>
    /// <returns>Collection of planar faces from the solid</returns>
    public static IEnumerable<PlanarFace> GetPlanarFaces(this Solid solid) =>
        solid.GetFaces()
            .OfType<PlanarFace>() ;

    /// <summary>
    ///     Gets planar faces from a solid that match a specific normal direction
    /// </summary>
    /// <param name="solid">The solid</param>
    /// <param name="normalFace">The normal vector to match</param>
    /// <param name="tolerance">Tolerance for normal comparison</param>
    /// <returns>Collection of planar faces with matching normal direction</returns>
    public static IEnumerable<PlanarFace> GetPlanarFaces(this Solid solid,
        XYZ normalFace,
        double tolerance = ToleranceConstants.Tolerance1E9) =>
        solid.GetPlanarFaces()
            .Where(x => x.FaceNormal.IsAlmostEqual3D(normalFace,
                tolerance)) ;
}
