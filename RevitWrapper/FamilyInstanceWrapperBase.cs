// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

namespace SonnyRevitExtensions.RevitWrapper ;

public class FamilyInstanceWrapperBase(FamilyInstance familyInstance) : ElementWrapperBase(familyInstance)
{
    public FamilyInstance FamilyInstance { get ; } = familyInstance ;
}
