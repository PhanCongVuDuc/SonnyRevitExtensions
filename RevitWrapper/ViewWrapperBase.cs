// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using SonnyRevitExtensions.Extensions.XYZs ;

namespace SonnyRevitExtensions.RevitWrapper ;

public class ViewWrapperBase(View view) : ElementWrapperBase(view)
{
    private bool? _isViewPlan ;

    public View View { get ; } = view ;

    public bool IsViewPlan =>
        _isViewPlan ??= View.ViewDirection.IsAlmostEqual3D(XYZ.BasisZ)
                        || View.ViewDirection.IsAlmostEqual3D(XYZ.BasisZ.Negate()) ;
}
