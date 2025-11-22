// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using SonnyRevitExtensions.Extensions ;
using SonnyRevitExtensions.Extensions.XYZs ;

namespace SonnyRevitExtensions.RevitWrapper ;

public class ViewWrapperBase(View view) : ElementWrapperBase(view)
{
    private bool? _isViewPlan ;

    private List<Element>? _elements ;

    private List<GridWrapperBase>? _gridWrappers ;

    public View View { get ; } = view ;

    public bool IsViewPlan =>
        _isViewPlan ??= View.ViewDirection.IsAlmostEqual3D(XYZ.BasisZ)
                        || View.ViewDirection.IsAlmostEqual3D(XYZ.BasisZ.Negate()) ;

    public List<Element> Elements =>
        _elements ??= View.Document
            .GetAllElements<Element>(View)
            .ToList() ;

    public List<GridWrapperBase> GridWrappers =>
        _gridWrappers ??= Elements.OfType<Grid>()
            .Select(x => new GridWrapperBase(x))
            .ToList() ;
}
