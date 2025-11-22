// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using SonnyRevitExtensions.Extensions.GeometryObjects.Curves ;

namespace SonnyRevitExtensions.RevitWrapper ;

public class ElementWrapperBase(Element element)
{
    private readonly Dictionary<string, BoundingBoxXYZ> _boundingBoxXyz = new() ;

    private readonly Dictionary<string, XYZ?> _centerPoint = new() ;
    public Element Element { get ; } = element ;

    public BoundingBoxXYZ GetBoundingBoxXyz(ViewWrapperBase viewWrapper)
    {
        if (_boundingBoxXyz.TryGetValue(viewWrapper.View.UniqueId,
                out var boundingBoxXyzValue))
        {
            return boundingBoxXyzValue ;
        }

        var boundingBoxXyz = Element.get_BoundingBox(viewWrapper.View) ;
        _boundingBoxXyz.Add(viewWrapper.View.UniqueId,
            boundingBoxXyz) ;
        return boundingBoxXyz ;
    }

    public XYZ? GetCenterPoint(ViewWrapperBase viewWrapper)
    {
        if (_centerPoint.TryGetValue(viewWrapper.View.UniqueId,
                out var pointValue))
        {
            return pointValue ;
        }

        XYZ? result = null ;

        if (viewWrapper.IsViewPlan)
        {
            if (Element.Location is LocationPoint locationPoint)
            {
                result = locationPoint.Point ;
            }
            else if (Element.Location is LocationCurve locationCurve)
            {
                result = locationCurve.Curve.GetMiddlePoint() ;
            }
        }

        if (result == null
            && GetBoundingBoxXyz(viewWrapper) is { } boundingBoxXyz)
        {
            result = boundingBoxXyz.ComputeCentroid() ;
        }

        _centerPoint.Add(viewWrapper.View.UniqueId,
            result) ;

        return result ;
    }
}
