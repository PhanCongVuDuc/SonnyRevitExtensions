using SonnyRevitExtensions.Extensions.XYZs ;

namespace SonnyRevitExtensions.Extensions.GeometryObjects.Curves ;

public static class CurveExtensions
{
  /// <summary>
  /// Gets the angle between two curves in radians.
  /// </summary>
  /// <param name="curve1">The first curve.</param>
  /// <param name="curve2">The second curve.</param>
  /// <returns>The angle between the two curves in radians.</returns>
  public static double GetAngleBetweenCurves( this Curve curve1,
    Curve curve2 )
  {
    var c1Vector = curve1.GetEndPoint( 1 )
      .Subtract( curve1.GetEndPoint( 0 ) ) ;
    var c2Vector = curve2.GetEndPoint( 1 )
      .Subtract( curve2.GetEndPoint( 0 ) ) ;

    return c2Vector.AngleTo( c1Vector ) ;
  }

  /// <summary>
  /// Gets the normalized direction vector of the curve from end point 0 to end point 1.
  /// </summary>
  /// <param name="curve">The curve.</param>
  /// <returns>The normalized direction vector.</returns>
  public static XYZ Direction( this Curve curve )
  {
    var startPoint = curve.GetEndPoint( 0 ) ;
    var endPoint = curve.GetEndPoint( 1 ) ;

    return ( endPoint - startPoint ).Normalize() ;
  }

  /// <summary>
  /// Gets the intersection point of two curves that are currently intersecting.
  /// </summary>
  /// <param name="curve1">The first curve.</param>
  /// <param name="curve2">The second curve.</param>
  /// <returns>The intersection point, or null if the curves do not intersect at exactly one point.</returns>
  public static XYZ? GetIntersection( this Curve curve1,
    Curve curve2 )
  {
    var result = curve1.Intersect( curve2,
      out var results ) ;

    if ( result != SetComparisonResult.Overlap
         || results is not { Size: 1 } ) {
      return null ;
    }

    return results.get_Item( 0 )
      .XYZPoint ;
  }

  /// <summary>
  /// Gets the intersection point of two curves by creating temporary detail curves.
  /// </summary>
  /// <param name="document">The Revit document.</param>
  /// <param name="curve1">The first curve.</param>
  /// <param name="curve2">The second curve.</param>
  /// <returns>The intersection point, or null if the curves do not intersect.</returns>
  public static XYZ? GetIntersectionViaDetailCurves( Document document,
    Curve curve1,
    Curve curve2 )
  {
    using var subTransaction = new SubTransaction( document ) ;
    subTransaction.Start() ;

    var detailCurve1 = document.Create.NewDetailCurve( document.ActiveView,
      curve1 ) ;
    var detailCurve2 = document.Create.NewDetailCurve( document.ActiveView,
      curve2 ) ;

    var result = GetIntersection( detailCurve1.GeometryCurve,
      detailCurve2.GeometryCurve ) ;
    subTransaction.RollBack() ;

    return result ;
  }

  /// <summary>
  /// Gets the intersection point of two curves by extending them first.
  /// </summary>
  /// <param name="curve1">The first curve.</param>
  /// <param name="curve2">The second curve.</param>
  /// <returns>The intersection point of the extended curves, or null if they do not intersect.</returns>
  public static XYZ? GetExtendedIntersection( this Curve curve1,
    Curve curve2 )
  {
    var direction1 = curve1.Direction() ;
    var direction2 = curve2.Direction() ;

    var extensionDistance = 2000.0.FromMillimeters() ;
    var backwardExtensionDistance = -extensionDistance ;

    var point1 = curve1.GetEndPoint( 0 )
      .Add( direction1.Multiply( backwardExtensionDistance ) ) ;
    var point2 = curve1.GetEndPoint( 1 )
      .Add( direction1.Multiply( extensionDistance ) ) ;
    var extendedCurve1 = Line.CreateBound( point1,
      point2 ) ;

    point1 = curve2.GetEndPoint( 0 )
      .Add( direction2.Multiply( backwardExtensionDistance ) ) ;
    point2 = curve2.GetEndPoint( 1 )
      .Add( direction2.Multiply( extensionDistance ) ) ;
    var extendedCurve2 = Line.CreateBound( point1,
      point2 ) ;

    return GetIntersection( extendedCurve1,
      extendedCurve2 ) ;
  }

  /// <summary>
  /// Checks if two curves intersect by creating model lines from them.
  /// </summary>
  /// <param name="document">The Revit document.</param>
  /// <param name="curve1">The first curve.</param>
  /// <param name="curve2">The second curve.</param>
  /// <returns>True if the curves intersect, false otherwise.</returns>
  public static bool IsIntersecting( this Curve curve1,
    Curve curve2,
    Document document )
  {
    var modelLine1 = curve1.CreateModelLine( document ) ;
    var modelLine2 = curve2.CreateModelLine( document ) ;

    if ( modelLine1 == null
         || modelLine2 == null ) {
      return false ;
    }

    var result = modelLine1.GeometryCurve.IsIntersecting( modelLine2.GeometryCurve ) ;

    return result ;
  }

  /// <summary>
  /// Checks if two curves are parallel.
  /// Two curves are parallel when the angle between them is 0 or 180 degrees and the distance between them is greater than 0.
  /// </summary>
  /// <param name="curve1">The first curve.</param>
  /// <param name="curve2">The second curve.</param>
  /// <returns>True if the curves are parallel, false otherwise.</returns>
  public static bool IsParallel( this Curve curve1,
    Curve curve2 )
  {
    var direction1 = curve1.Direction() ;
    var direction2 = curve2.Direction() ;

    var angle = direction1.AngleTo( direction2 ) ;
    if ( Math.Abs( angle ) < ToleranceConstants.GeneralTolerance
         || Math.Abs( angle - Math.PI ) < ToleranceConstants.GeneralTolerance ) {
      return curve1.Distance( curve2.GetEndPoint( 0 ) ) > 0 ;
    }

    return false ;
  }

  /// <summary>
  /// Gets the distance from a point to a curve by extending the curve first.
  /// </summary>
  /// <param name="curve">The curve.</param>
  /// <param name="point">The point.</param>
  /// <returns>The distance from the point to the extended curve.</returns>
  public static double DistanceToPointExtended( this Curve curve,
    XYZ point )
  {
    var direction = curve.Direction() ;

    var extensionDistance = 2000.0.FromMillimeters() ;
    var backwardExtensionDistance = -extensionDistance ;

    var start = curve.GetEndPoint( 0 )
      .Add( direction.Multiply( backwardExtensionDistance ) ) ;
    var end = curve.GetEndPoint( 1 )
      .Add( direction.Multiply( extensionDistance ) ) ;
    var extendedCurve = Line.CreateBound( start,
      end ) ;

    return extendedCurve.Distance( point ) ;
  }

  /// <summary>
  /// Gets the distance from a point to a curve using projection.
  /// </summary>
  /// <param name="curve">The curve.</param>
  /// <param name="point">The point.</param>
  /// <returns>The distance from the point to the curve.</returns>
  public static double Distance( this Curve curve,
    XYZ point )
  {
    var projection = curve.ProjectionOf( point ) ;
    return Math.Abs( ( point - projection ).GetLength() ) ;
  }

  /// <summary>
  /// Gets the projection point of a point onto a curve.
  /// Note: The projection may lie outside the curve bounds.
  /// </summary>
  /// <param name="curve">The curve to project onto.</param>
  /// <param name="point">The point to project.</param>
  /// <returns>The projected point on the curve.</returns>
  public static XYZ ProjectionOf( this Curve curve,
    XYZ point )
  {
    var startPoint = curve.GetEndPoint( 0 ) ;
    var endPoint = curve.GetEndPoint( 1 ) ;
    var vectorFromStartToPoint = point - startPoint ;
    var directionVector = ( endPoint - startPoint ).Normalize() ;

    var scalarProjection = directionVector.DotProduct( vectorFromStartToPoint ) ;
    var projectedPoint = startPoint.Add( directionVector.Multiply( scalarProjection ) ) ;

    return projectedPoint ;
  }

  /// <summary>
  /// Checks if two curves are collinear.
  /// Curves are collinear when they are parallel and the distance between them is 0.
  /// </summary>
  /// <param name="curve1">The first curve.</param>
  /// <param name="curve2">The second curve.</param>
  /// <returns>True if the curves are collinear, false otherwise.</returns>
  public static bool AreCollinear( this Curve curve1,
    Curve curve2 )
  {
    var angle = curve1.Direction()
      .AngleTo( curve2.Direction() ) ;

    if ( Math.Abs( angle ) < ToleranceConstants.GeneralTolerance
         || Math.Abs( angle - Math.PI ) < ToleranceConstants.GeneralTolerance ) {
      return Math.Abs( Distance( curve1,
               curve2.GetEndPoint( 0 ) ) )
             < ToleranceConstants.GeneralTolerance ;
    }

    return false ;
  }

  /// <summary>
  /// Gets the minimum distance from the two end points of a curve to another curve.
  /// </summary>
  /// <param name="curve">The curve whose end points are measured.</param>
  /// <param name="otherCurve">The target curve.</param>
  /// <returns>The minimum distance from the curve's end points to the target curve.</returns>
  public static double GetMinimumDistanceToCurve( this Curve curve,
    Curve otherCurve )
  {
    var e1 = Distance( otherCurve,
      curve.GetEndPoint( 0 ) ) ;
    var e2 = Distance( otherCurve,
      curve.GetEndPoint( 1 ) ) ;

    return e1 > e2 ? e2 : e1 ;
  }

  /// <summary>
  /// Checks if two curves are on the same side relative to an end point of a target curve.
  /// </summary>
  /// <param name="curve1">The first curve.</param>
  /// <param name="curve2">The second curve.</param>
  /// <param name="targetCurve">The target curve used as reference.</param>
  /// <returns>True if both curves are on the same side, false otherwise.</returns>
  public static bool IsSameSide( Curve curve1,
    Curve curve2,
    Curve targetCurve )
  {
    var endPointMinC1 = curve1.GetClosestEndPoint( targetCurve ) ;
    var endPointMinC2 = curve2.GetClosestEndPoint( targetCurve ) ;

    var projectionOfC1 = targetCurve.ProjectionOf( endPointMinC1 ) ;
    var projectionOfC2 = targetCurve.ProjectionOf( endPointMinC2 ) ;

    var c1End0 = projectionOfC1.Subtract( targetCurve.GetEndPoint( 0 ) ) ;
    var c2End0 = projectionOfC2.Subtract( targetCurve.GetEndPoint( 0 ) ) ;

    var c1End1 = projectionOfC1.Subtract( targetCurve.GetEndPoint( 1 ) ) ;
    var c2End1 = projectionOfC2.Subtract( targetCurve.GetEndPoint( 1 ) ) ;

    var equalEnd0 = c1End0.Normalize()
      .IsAlmostEqualTo( c2End0.Normalize() ) ;
    var equalEnd1 = c1End1.Normalize()
      .IsAlmostEqualTo( c2End1.Normalize() ) ;

    return equalEnd0 && equalEnd1 ;
  }

  /// <summary>
  /// Gets the end point of a curve that is closest to a target curve.
  /// </summary>
  /// <param name="curve">The curve.</param>
  /// <param name="otherCurve">The target curve.</param>
  /// <returns>The closest end point.</returns>
  public static XYZ GetClosestEndPoint( this Curve curve,
    Curve otherCurve )
  {
    var e0 = Distance( otherCurve,
      curve.GetEndPoint( 0 ) ) ;
    var e1 = Distance( otherCurve,
      curve.GetEndPoint( 1 ) ) ;

    return e0 > e1 ? curve.GetEndPoint( 1 ) : curve.GetEndPoint( 0 ) ;
  }

  /// <summary>
  /// Gets the curve from a list that has the minimum distance from the two end points of a target curve.
  /// </summary>
  /// <param name="curve">The target curve.</param>
  /// <param name="curves">The list of curves to search.</param>
  /// <returns>The closest curve, or null if the list is empty.</returns>
  public static Curve GetClosestCurve( this Curve curve,
    List<Curve> curves )
  {
    var min = double.MaxValue ;
    var cResult = curves.First() ;
    foreach ( var c in curves ) {
      var d = GetMinimumDistanceToCurve( curve,
        c ) ;
      if ( d < min ) {
        min = d ;
        cResult = c ;
      }
    }

    return cResult ;
  }

  /// <summary>
  /// Checks if a curve is fully contained within another curve.
  /// </summary>
  /// <param name="thisCurve">The curve to check.</param>
  /// <param name="otherCurve">The target curve used for checking.</param>
  /// <returns>True if the curve is fully contained, false otherwise.</returns>
  public static bool IsFullyContained( this Curve thisCurve,
    Curve otherCurve )
  {
    try {
      var start = thisCurve.GetEndPoint( 0 ) ;
      var end = thisCurve.GetEndPoint( 1 ) ;
      return ContainsPoint( otherCurve,
               start )
             && ContainsPoint( otherCurve,
               end ) ;
    }
    catch ( Exception ) {
      return false ;
    }
  }

  /// <summary>
  /// Checks if a curve is fully contained within any of the curves in a list.
  /// </summary>
  /// <param name="curve">The curve to check.</param>
  /// <param name="curves">The list of curves used for checking.</param>
  /// <returns>True if the curve is fully contained in any curve from the list, false otherwise.</returns>
  public static bool IsFullyContained( this Curve curve,
    List<Curve> curves )
  {
    foreach ( var curveTarget in curves ) {
      if ( ! IsFullyContained( curve,
            curveTarget ) )
        continue ;

      return true ;
    }

    return false ;
  }

  /// <summary>
  /// Gets the normal direction vector from the first curve to the second curve.
  /// The two curves must be parallel.
  /// </summary>
  /// <param name="curve">The first curve.</param>
  /// <param name="otherCurve">The second curve.</param>
  /// <returns>The normalized direction vector from the first curve to the second curve.</returns>
  public static XYZ GetNormalBetweenParallelCurves( this Curve curve,
    Curve otherCurve )
  {
    return otherCurve.ProjectionOf( curve.GetEndPoint( 0 ) )
      .Subtract( curve.GetEndPoint( 0 ) )
      .Normalize() ;
  }

  /// <summary>
  /// Sort a list of curves to make them correctly 
  /// ordered and oriented to form a closed loop.
  /// source: https://thebuildingcoder.typepad.com/blog/2013/03/sort-and-orient-curves-to-form-a-contiguous-loop.html
  /// </summary>
  public static void SortCurvesContiguous( this IList<Curve> curves )
  {
    var n = curves.Count ;

    // Walk through each curve (after the first) 
    // to match up the curves in order

    for ( var i = 0 ; i < n ; ++i ) {
      var curve = curves[ i ] ;
      var endPoint = curve.GetEndPoint( 1 ) ;

      // Find curve with start point = end point

      var found = ( i + 1 >= n ) ;

      for ( int j = i + 1 ; j < n ; ++j ) {
        var p = curves[ j ]
          .GetEndPoint( 0 ) ;

        // If there is a match end->start, 
        // this is the next curve

        if ( ToleranceConstants.GeneralTolerance > p.DistanceTo( endPoint ) ) {
          if ( i + 1 != j ) {
            ( curves[ i + 1 ], curves[ j ] ) = ( curves[ j ], curves[ i + 1 ] ) ;
          }

          found = true ;
          break ;
        }

        p = curves[ j ]
          .GetEndPoint( 1 ) ;

        // If there is a match end->end, 
        // reverse the next curve

        if ( ToleranceConstants.GeneralTolerance > p.DistanceTo( endPoint ) ) {
          if ( i + 1 == j ) {
            curves[ i + 1 ] = CreateReversedCurve( curves[ j ] ) ;
          }
          else {
            var tmp = curves[ i + 1 ] ;
            curves[ i + 1 ] = CreateReversedCurve( curves[ j ] ) ;
            curves[ j ] = tmp ;
          }

          found = true ;
          break ;
        }
      }

      if ( ! found ) {
        throw new Exception( "SortCurvesContiguous:" + " non-contiguous input curves" ) ;
      }
    }
  }

  /// <summary>
  /// Create a new curve with the same 
  /// geometry in the reverse direction.
  /// </summary>
  /// <param name="originalCurve">The original curve.</param>
  /// <returns>The reversed curve.</returns>
  /// <throws cref="NotImplementedException">If the 
  /// curve type is not supported by this utility.</throws>
  public static Curve CreateReversedCurve( this Curve originalCurve )
  {
    if ( originalCurve is Line ) {
      return Line.CreateBound( originalCurve.GetEndPoint( 1 ),
        originalCurve.GetEndPoint( 0 ) ) ;
    }

    if ( originalCurve is Arc ) {
      return Arc.Create( originalCurve.GetEndPoint( 1 ),
        originalCurve.GetEndPoint( 0 ),
        originalCurve.Evaluate( 0.5,
          true ) ) ;
    }

    throw new Exception( "CreateReversedCurve - Unreachable" ) ;
  }

  /// <summary>
  /// Creates a model line from two points.
  /// </summary>
  /// <param name="doc">The Revit document.</param>
  /// <param name="startPoint">The start point.</param>
  /// <param name="endPoint">The end point.</param>
  /// <returns>The created model line, or null if the points are too close or creation fails.</returns>
  public static ModelLine? CreateModelLine( Document doc,
    XYZ startPoint,
    XYZ endPoint )
  {
    if ( startPoint.DistanceTo( endPoint ) < ToleranceConstants.GeneralTolerance )
      return null ;

    var v = endPoint - startPoint ;
    var dxy = Math.Abs( v.X ) + Math.Abs( v.Y ) ;
    var w = ( dxy > ToleranceConstants.StandardPrecision ) ? XYZ.BasisZ : XYZ.BasisY ;
    var norm = v.CrossProduct( w )
      .Normalize() ;

    var plane = Plane.CreateByNormalAndOrigin( norm,
      startPoint ) ;
    var sketchPlane = SketchPlane.Create( doc,
      plane ) ;
    var line = Line.CreateBound( startPoint,
      endPoint ) ;

    return (ModelLine) doc.Create.NewModelCurve( line,
      sketchPlane ) ;
  }

  /// <summary>
  /// Creates a model line from a curve.
  /// </summary>
  /// <param name="doc">The Revit document.</param>
  /// <param name="curve">The curve to convert.</param>
  /// <returns>The created model line, or null if the curve is too short or creation fails.</returns>
  public static ModelLine? CreateModelLine( this Curve curve,
    Document doc )
  {
    if ( curve.Length < ToleranceConstants.GeneralTolerance )
      return null ;
    var v = curve.GetEndPoint( 0 ) - curve.GetEndPoint( 1 ) ;
    var dxy = Math.Abs( v.X ) + Math.Abs( v.Y ) ;
    var w = ( dxy > ToleranceConstants.StandardPrecision ) ? XYZ.BasisZ : XYZ.BasisY ;
    var norm = v.CrossProduct( w )
      .Normalize() ;
    try {
      var plane = Plane.CreateByNormalAndOrigin( norm,
        curve.GetEndPoint( 1 ) ) ;
      var sketchPlane = SketchPlane.Create( doc,
        plane ) ;
      return doc.Create.NewModelCurve( curve,
        sketchPlane ) as ModelLine ;
    }
    catch ( Exception ) {
      return null ;
    }
  }

  /// <summary>
  /// Gets points along a curve at specified intervals.
  /// </summary>
  /// <param name="curve">The curve.</param>
  /// <param name="distance">The distance between points in millimeters.</param>
  /// <returns>A list of points along the curve.</returns>
  public static List<XYZ> GetPointsOnCurve( this Curve curve,
    double distance )
  {
    var pointsOnLines = new List<XYZ>() ;
    if ( distance == 0 )
      return pointsOnLines ;

    if ( curve is Line line ) {
      if ( line.Length < ToleranceConstants.GeneralTolerance )
        return pointsOnLines ;

      pointsOnLines.Add( line.GetEndPoint( 0 ) ) ;
      pointsOnLines.Add( line.GetEndPoint( 1 ) ) ;

      var direction = line.Direction() ;
      var r = Math.Round( line.Length / distance.FromMillimeters(),
        0 ) ;

      for ( int i = 1 ; i < r ; i++ ) {
        var nextPoint = line.GetEndPoint( 0 )
          .Add( direction.Multiply( distance.FromMillimeters() * i ) ) ;
        pointsOnLines.Add( nextPoint ) ;
      }
    }
    else if ( curve is Arc arc ) {
      pointsOnLines.AddRange( arc.Tessellate() ) ;
    }
    else if ( curve is NurbSpline spline ) {
      pointsOnLines.AddRange( spline.Tessellate() ) ;
    }

    return pointsOnLines ;
  }

  /// <summary>
  /// Removes duplicate curves from a list.
  /// </summary>
  /// <param name="curves">The list of curves.</param>
  /// <returns>A new list with duplicate curves removed.</returns>
  public static List<Curve> RemoveDuplicateCurves( this List<Curve> curves )
  {
    foreach ( var curve in curves ) {
      var allCurveToCheck = curves.Except( [curve] )
        .ToList() ;
      if ( ! curve.IsFullyContained( allCurveToCheck ) )
        continue ;

      curves = curves.Except( [curve] )
        .ToList() ;
    }

    return curves ;
  }

  /// <summary>
  /// Create a new CurveLoop from a list of points.
  /// </summary>
  public static CurveLoop CreateCurveLoop( this List<XYZ> points )
  {
    var n = points.Count ;
    var curveLoop = new CurveLoop() ;
    for ( var i = 1 ; i < n ; ++i ) {
      curveLoop.Append( Line.CreateBound( points[ i - 1 ],
        points[ i ] ) ) ;
    }

    curveLoop.Append( Line.CreateBound( points[ n ],
      points[ 0 ] ) ) ;
    return curveLoop ;
  }

  /// <summary>
  /// Gets all points from tessellating a curve.
  /// </summary>
  /// <param name="curve">The curve.</param>
  /// <returns>A list of XYZ points from the curve tessellation.</returns>
  public static List<XYZ> GetXYZPoints( this Curve curve )
  {
    var listXyz = new List<XYZ>() ;
    listXyz.AddRange( curve.Tessellate() ) ;
    return listXyz ;
  }

  /// <summary>
  /// Gets all points from tessellating a list of curves.
  /// </summary>
  /// <param name="curves">The list of curves.</param>
  /// <returns>A list of distinct XYZ points from all curves.</returns>
  public static List<XYZ> GetXYZPoints( this List<Curve> curves )
  {
    return curves.SelectMany( x => x.GetXYZPoints() )
      .DistinctXYZ() ;
  }

  /// <summary>
  /// Filters a list of curves to return only Line objects.
  /// </summary>
  /// <param name="curves">The list of curves.</param>
  /// <returns>A list containing only Line objects from the input curves.</returns>
  public static List<Line> GetLines( this List<Curve> curves )
  {
    return curves.OfType<Line>()
      .ToList() ;
  }

  /// <summary>
  /// Gets all intersection points between two curves.
  /// </summary>
  /// <param name="curve">The first curve.</param>
  /// <param name="curveOther">The second curve.</param>
  /// <returns>A list of intersection points, or an empty list if the curves do not intersect.</returns>
  public static List<XYZ> GetIntersectionPoints( this Curve curve,
    Curve curveOther )
  {
    var xyzes = new List<XYZ>() ;

    var setComparisonResult = curve.Intersect( curveOther,
      out IntersectionResultArray intersectionResultArray ) ;

    if ( setComparisonResult == SetComparisonResult.Disjoint
         || intersectionResultArray == null )
      return xyzes ;

    for ( var i = 0 ; i < intersectionResultArray.Size ; i++ ) {
      var intersectionResult = intersectionResultArray.get_Item( i ) ;
      xyzes.Add( intersectionResult.XYZPoint ) ;
    }

    return xyzes ;
  }

  /// <summary>
  /// Checks if two curves have the same direction.
  /// </summary>
  /// <param name="curve">The first curve.</param>
  /// <param name="curveOther">The second curve.</param>
  /// <returns>True if the curves have the same direction, false otherwise.</returns>
  public static bool AreSameDirection( this Curve curve,
    Curve curveOther )
  {
    var setComparisonResult = curve.Intersect( curveOther,
      out _ ) ;

    return setComparisonResult == SetComparisonResult.Equal ;
  }

  /// <summary>
  /// Gets all intersection points between a list of curves and another curve.
  /// </summary>
  /// <param name="curves">The list of curves.</param>
  /// <param name="curveOther">The other curve.</param>
  /// <returns>A list of distinct intersection points.</returns>
  public static List<XYZ> GetIntersectionPoints( this List<Curve> curves,
    Curve curveOther )
  {
    return curves.SelectMany( x => x.GetIntersectionPoints( curveOther ) )
      .DistinctXYZ() ;
  }

  /// <summary>
  /// Gets all intersection points between two lists of curves.
  /// </summary>
  /// <param name="curves">The first list of curves.</param>
  /// <param name="curvesOther">The second list of curves.</param>
  /// <returns>A list of distinct intersection points.</returns>
  public static List<XYZ> GetIntersectionPoints( this List<Curve> curves,
    List<Curve> curvesOther )
  {
    return curves.SelectMany( curvesOther.GetIntersectionPoints )
      .Distinct()
      .ToList() ;
  }

  /// <summary>
  /// Checks if two curves intersect.
  /// </summary>
  /// <param name="curve">The first curve.</param>
  /// <param name="curveOther">The second curve.</param>
  /// <returns>True if the curves intersect, false otherwise.</returns>
  public static bool IsIntersecting( this Curve curve,
    Curve curveOther )
  {
    var intersectCurve = curve.GetIntersectionPoints( curveOther ) ;
    return intersectCurve.Count > 0 ;
  }

  /// <summary>
  /// Checks if any curves from two lists intersect.
  /// </summary>
  /// <param name="curves">The first list of curves.</param>
  /// <param name="curvesOther">The second list of curves.</param>
  /// <returns>True if any curves intersect, false otherwise.</returns>
  public static bool IsIntersecting( this List<Curve> curves,
    List<Curve> curvesOther )
  {
    var intersectCurve = curves.GetIntersectionPoints( curvesOther ) ;
    return intersectCurve.Count > 0 ;
  }

  /// <summary>
  /// Projects a curve to a specified elevation (Z coordinate).
  /// </summary>
  /// <param name="curve">The curve to project.</param>
  /// <param name="elevation">The target elevation.</param>
  /// <returns>The projected curve at the specified elevation.</returns>
  public static Curve ProjectToElevation( this Curve curve,
    double elevation )
  {
    if ( curve is Line line ) {
      var strPoint = new XYZ( line.GetEndPoint( 0 )
          .X,
        line.GetEndPoint( 0 )
          .Y,
        elevation ) ;
      var endPoint = new XYZ( line.GetEndPoint( 1 )
          .X,
        line.GetEndPoint( 1 )
          .Y,
        elevation ) ;
      if ( strPoint.DistanceTo( endPoint ) < 10.0.FromMillimeters() )
        return curve ;
      return Line.CreateBound( strPoint,
        endPoint ) ;
    }

    if ( curve is Arc arc ) {
      var xyzes = arc.GetXYZPoints() ;
      var end0 = new XYZ( xyzes[ 0 ].X,
        xyzes[ 0 ].Y,
        elevation ) ;
      var end1 = new XYZ( xyzes[ ^1 ].X,
        xyzes[ ^1 ].Y,
        elevation ) ;
      var pointOnArc = new XYZ( arc.GetMiddlePoint()
          .X,
        arc.GetMiddlePoint()
          .Y,
        elevation ) ;

      return Arc.Create( end0,
        end1,
        pointOnArc ) ;
    }

    return curve ;
  }

  /// <summary>
  /// Checks if a curve is contained within another curve.
  /// </summary>
  /// <param name="curve">The curve to check.</param>
  /// <param name="curveOther">The target curve.</param>
  /// <returns>True if the curve is contained, false otherwise.</returns>
  public static bool IsContainedIn( this Curve curve,
    Curve curveOther )
  {
    var defaultCurve = curve.ProjectToElevation( curveOther.GetEndPoint( 0 )
      .Z ) ;
    var setComparisonResult = defaultCurve.Intersect( curveOther ) ;

    return setComparisonResult == SetComparisonResult.Equal ;
  }

  /// <summary>
  /// Checks if a point lies on a curve within a specified tolerance.
  /// </summary>
  /// <param name="curve">The curve.</param>
  /// <param name="origin">The point to check.</param>
  /// <param name="tolerance">The tolerance for the distance check.</param>
  /// <returns>True if the point is on the curve within tolerance, false otherwise.</returns>
  public static bool ContainsPoint( this Curve curve,
    XYZ origin,
    double tolerance = ToleranceConstants.StandardPrecision )
  {
    var result = curve.Project( origin ) ;
    var distanceTo = result.XYZPoint.DistanceTo( origin ) ;
    return distanceTo <= tolerance ;
  }

  /// <summary>
  /// Gets the midpoint of a curve.
  /// </summary>
  /// <param name="curve">The curve.</param>
  /// <returns>The midpoint of the curve.</returns>
  public static XYZ GetMiddlePoint( this Curve curve )
  {
    return curve.GetEndPoint( 0 )
      .Add( curve.GetEndPoint( 1 ) )
      .Divide( 2 ) ;
  }

  /// <summary>
  /// Finds the curve from a list that contains a specified point.
  /// </summary>
  /// <param name="curves">The list of curves to search.</param>
  /// <param name="indexResult">Output parameter that receives the index of the found curve, or -1 if not found.</param>
  /// <param name="origin">The point to search for.</param>
  /// <returns>The curve containing the point, or null if not found.</returns>
  public static Curve? FindCurveContainingPoint( this List<Curve> curves,
    ref int indexResult,
    XYZ origin )
  {
    foreach ( var curve in curves ) {
      if ( ! curve.ContainsPoint( origin ) )
        continue ;

      indexResult = curves.IndexOf( curve ) ;
      return curve ;
    }

    indexResult = -1 ;
    return null ;
  }

  /// <summary>
  /// Merges connected curves in a list by joining them together.
  /// </summary>
  /// <param name="curves">The list of curves to merge.</param>
  /// <returns>A new list with connected curves merged.</returns>
  public static List<Curve> MergeConnectedCurves( this List<Curve> curves )
  {
    var newCurves = new List<Curve>() ;
    for ( var i = 0 ; i < curves.Count ; i++ ) {
      var curve = curves[ i ] ;
      var curvejoin = curves.FirstOrDefault( c => c.Intersect( curve ) == SetComparisonResult.Subset ) ;
      if ( curvejoin != null ) {
        curve.Intersect( curvejoin,
          out var resultArray ) ;
        var point1 = resultArray.get_Item( 0 )
          .XYZPoint
          .IsAlmostEqualTo( curve.GetEndPoint( 0 ) )
          ? curve.GetEndPoint( 1 )
          : curve.GetEndPoint( 0 ) ;
        var point2 = resultArray.get_Item( 0 )
          .XYZPoint
          .IsAlmostEqualTo( curvejoin.GetEndPoint( 0 ) )
          ? curvejoin.GetEndPoint( 1 )
          : curvejoin.GetEndPoint( 0 ) ;
        curves[ i ] = Line.CreateBound( point1,
          point2 ) ;
        curves.Remove( curvejoin ) ;
      }
      else {
        newCurves.Add( curve ) ;
        curves.Remove( curve ) ;
      }

      i-- ;
    }

    return newCurves ;
  }
}