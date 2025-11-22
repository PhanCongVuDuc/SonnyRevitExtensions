// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

namespace SonnyRevitExtensions.Extensions.GeometryObjects.Faces ;

public static class FacePointExtensions
{
    /// <summary>
    ///     Gets all points from tessellating curves of the face
    /// </summary>
    /// <param name="face">The face</param>
    /// <returns>Collection of XYZ points from tessellating all curves of the face</returns>
    public static IEnumerable<XYZ> GetPoints(this Face face)
    {
        foreach (var curve in face.GetCurves())
        {
            foreach (var point in curve.Tessellate())
            {
                yield return point ;
            }
        }
    }

    /// <summary>
    ///     Gets all points from tessellating curves of multiple faces
    /// </summary>
    /// <param name="faces">The collection of faces</param>
    /// <returns>Collection of XYZ points from all faces</returns>
    public static IEnumerable<XYZ> GetPoints(this IEnumerable<Face> faces) => faces.SelectMany(x => x.GetPoints()) ;
}
