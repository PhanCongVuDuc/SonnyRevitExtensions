// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

using SonnyRevitExtensions.Extensions.Utilities ;

namespace SonnyRevitExtensions.Extensions.Views ;

public static class ViewExtensions
{
    /// <summary>
    /// Creates Options object configured for dimension operations
    /// </summary>
    /// <param name="view">The view to use for dimension operations</param>
    /// <returns>Configured Options object with ComputeReferences enabled</returns>
    public static Options CreateDimensionOptions(this View view)
        => DimensionOptions.Create(view);
}
