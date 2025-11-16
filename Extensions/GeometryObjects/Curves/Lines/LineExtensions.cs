namespace SonnyRevitExtensions.Extensions.GeometryObjects.Curves.Lines ;

public static class LineExtensions
{
    /// <summary>
    ///     Determines whether the specified point lies on this line.
    /// </summary>
    /// <param name="line">The line to check.</param>
    /// <param name="xyz">The point to check.</param>
    /// <returns>True if the point lies on the line; otherwise, false.</returns>
    public static bool ContainsPoint(this Line line,
        XYZ xyz)
    {
        var intersectionResult = line.Project(xyz) ;
        return intersectionResult.XYZPoint.IsAlmostEqualTo(xyz) ;
    }

    /// <summary>
    ///     Creates a new line offset from this line by the specified distance in the given direction.
    /// </summary>
    /// <param name="line">The line to offset.</param>
    /// <param name="direct">The direction vector for the offset.</param>
    /// <param name="offset">The offset distance.</param>
    /// <returns>A new offset line.</returns>
    public static Line Offset(this Line line,
        XYZ direct,
        double offset)
    {
        var point1 = line.GetEndPoint(0) + direct * offset ;
        var point2 = line.GetEndPoint(1) + direct * offset ;
        return Line.CreateBound(point1,
            point2) ;
    }

    /// <summary>
    ///     Gets the midpoint of this line.
    /// </summary>
    /// <param name="line">The line.</param>
    /// <returns>The midpoint XYZ coordinate.</returns>
    public static XYZ GetMidpoint(this Line line) =>
        line.GetEndPoint(0)
            .Add(line.GetEndPoint(1))
            .Divide(2) ;

    /// <summary>
    ///     Return true if the given point is very close
    ///     to this line, within a very narrow ellipse
    ///     whose focal points are the line start and end.
    ///     The tolerance is defined as (1 - e) using the
    ///     eccentricity e. e = 0 means we have a circle;
    ///     The closer e is to 1, the more elongated the
    ///     shape of the ellipse.
    ///     https://en.wikipedia.org/wiki/Ellipse#Eccentricity
    /// </summary>
    public static bool Contains(this Line line,
        XYZ point)
    {
        var a = line.GetEndPoint(0) ; // line start point
        var b = line.GetEndPoint(1) ; // line end point
        var f = a.DistanceTo(b) ; // distance between focal points
        var da = a.DistanceTo(point) ;
        var db = point.DistanceTo(b) ;
        // da + db is always greater or equal f
        return (da + db - f) * f < ToleranceConstants.GeneralTolerance ;
    }
}
