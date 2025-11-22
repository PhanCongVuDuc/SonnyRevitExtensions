// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

namespace SonnyRevitExtensions.Extensions.Utilities ;

/// <summary>
/// Utility class for creating Options objects for dimension operations
/// </summary>
public static class DimensionOptions
{
    /// <summary>
    /// Creates Options object configured for dimension operations
    /// </summary>
    /// <param name="view">The view to use for dimension operations</param>
    /// <returns>Configured Options object with ComputeReferences enabled</returns>
    public static Options Create(View view)
    {
        var options = new Options() ;
        options.ComputeReferences = true ;
        options.View = view ;
        return options ;
    }
}
