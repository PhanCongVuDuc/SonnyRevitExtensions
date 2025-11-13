# SonnyRevitExtensions

Open-source library providing extension methods for Autodesk Revit API to simplify geometric operations.

## About

This library was originally written in 2021 as part of the AlphaBIM project. This was during my early days of coding, so please bear with me if there are bugs or issues with the methods.

Starting from November 13, 2025, I decided to open-source the code I have written, including these extension methods, rather than letting it sit unused for years. While these methods have been in use for some time, they can be improved and refined through community contributions and feedback.

## Features

- Extension method style API for Revit classes
- Comprehensive XYZ extensions (distance, vector, comparison, geometry operations)
- Tolerance constants for consistent geometric comparisons
- Supports Revit 2021-2026 (.NET Framework 4.8 / .NET 8.0)
- Planned: Solid, Curve, Line, Face extensions

## Installation

Build the project and reference `SonnyRevitExtensions.dll` in your Revit add-in project.

## Quick Start

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

## Planned Extensions

The goal is to open-source all extension methods that have been developed during work on project

## Contributing

Contributions welcome! Follow existing code style, add XML documentation, and ensure compatibility across all Revit versions.
