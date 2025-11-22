// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

namespace SonnyRevitExtensions.Extensions ;

public static class ReferenceExtensions
{
    public static bool ConditionCreateDimension(this ReferenceArray referenceArray)
    {
        bool result ;
        if (! referenceArray.IsEmpty
            && referenceArray.Size >= 2)
        {
            result = true ;
        }
        else
        {
            result = false ;
        }

        return result ;
    }
}
