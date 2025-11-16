# SonnyRevitExtensions

Open-source library providing extension methods for Autodesk Revit API to simplify geometric operations.

## About

This library was originally written in 2021 as part of the AlphaBIM project. This was during my early days of coding, so
please bear with me if there are bugs or issues with the methods.

Starting from November 13, 2025, I decided to open-source the code I have written, including these extension methods,
rather than letting it sit unused for years. While these methods have been in use for some time, they can be improved
and refined through community contributions and feedback.

## Features

- Extension method style API for Revit classes
- **XYZ Extensions** (`Extensions/XYZs/`)
    - `XYZComparisonExtensions`, `XYZDistanceExtensions`, `XYZGeometryExtensions`
    - `XYZTransformExtensions`, `XYZUtilityExtensions`, `XYZVectorExtensions`
- **Curve Extensions** (`Extensions/GeometryObjects/Curves/`)
    - `CurveExtensions`, `LineExtensions`
- **Face Extensions** (`Extensions/GeometryObjects/Faces/`)
    - `FaceExtensions`, `PlanarFaceExtensions`, `CylindricalFaceExtensions`
- **Other Extensions**
    - `ToleranceConstants` - Predefined tolerance values for geometric comparisons
    - `LinqExtensions` - Additional LINQ operations
- Supports Revit 2021-2026 (.NET Framework 4.8 / .NET 8.0)

## Installation

Build the project and reference `SonnyRevitExtensions.dll` in your Revit add-in project.

## Quick Start

### XYZ Extensions

```csharp
using SonnyRevitExtensions.Extensions.XYZs;
using SonnyRevitExtensions.Extensions;

var point1 = new XYZ(0, 0, 0);
var point2 = new XYZ(10, 10, 5);

// 2D distance
double dist2D = point1.DistanceTo2D(point2);

// Compare points
bool equal = point1.IsAlmostEqual2D(point2, ToleranceConstants.GeneralTolerance);

// Vector operations
var vec1 = new XYZ(1, 0, 0);
var vec2 = new XYZ(2, 0, 0);
bool parallel = vec1.AreVectorsParallel(vec2);
```

### Curve Extensions

```csharp
using SonnyRevitExtensions.Extensions.GeometryObjects.Curves;

var curve1 = Line.CreateBound(new XYZ(0, 0, 0), new XYZ(10, 0, 0));
var curve2 = Line.CreateBound(new XYZ(5, -5, 0), new XYZ(5, 5, 0));

// Get intersection point
XYZ? intersection = curve1.GetIntersection(curve2);

// Check if parallel
bool isParallel = curve1.IsParallel(curve2);

// Get distance from point to curve
double distance = curve1.Distance(point);
```

### Face Extensions

```csharp
using SonnyRevitExtensions.Extensions.GeometryObjects.Faces;

// Check if point is inside face
bool isInside = face.IsPointInside(point);

// Get intersection points between two faces
List<XYZ> intersectionPoints = face1.GetIntersectionPoints(face2);

// Get normal vector
XYZ normal = face.GetNormal();

// For PlanarFace: check if parallel
bool isParallel = planarFace1.IsParallelPlanarFace(planarFace2);

// For CylindricalFace: get radius
double? radius = cylindricalFace.GetRadius();
```

## Available Extensions

### XYZ Extensions (`SonnyRevitExtensions.Extensions.XYZs`)

- `XYZComparisonExtensions` - Point comparison operations
- `XYZDistanceExtensions` - Distance calculations
- `XYZGeometryExtensions` - Geometric operations (centroid, intersection, point-in-polygon)
- `XYZTransformExtensions` - Transformation operations
- `XYZUtilityExtensions` - Utility methods
- `XYZVectorExtensions` - Vector operations

### Curve Extensions (`SonnyRevitExtensions.Extensions.GeometryObjects.Curves`)

- `CurveExtensions` - Operations for Curve class
- `LineExtensions` - Operations for Line class

### Face Extensions (`SonnyRevitExtensions.Extensions.GeometryObjects.Faces`)

- `FaceExtensions` - Operations for Face class
- `PlanarFaceExtensions` - Operations for PlanarFace class
- `CylindricalFaceExtensions` - Operations for CylindricalFace class

### Other Extensions

- `ToleranceConstants` - Predefined tolerance constants for geometric comparisons
- `LinqExtensions` - Additional LINQ operations

## Planned Extensions

The goal is to open-source all extension methods that have been developed during work on the project. Future additions
may include:

- Solid extensions
- Edge extensions
- Additional geometric operations

## Contributing

Contributions welcome! Follow existing code style, add XML documentation, and ensure compatibility across all Revit
versions.
