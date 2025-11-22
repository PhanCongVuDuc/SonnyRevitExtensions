namespace SonnyRevitExtensions.Extensions ;

/// <summary>
/// Extension methods for Revit Document providing element query capabilities
/// </summary>
public static class DocumentExtension
{
    /// <summary>
    /// Gets an element by its unique ID
    /// </summary>
    /// <typeparam name="TElement">The type of element to retrieve</typeparam>
    /// <param name="document">The document</param>
    /// <param name="uniqueId">The unique ID of the element</param>
    /// <returns>The element, or null if not found</returns>
    public static TElement? GetElementById<TElement>(this Document document,
        string uniqueId) where TElement : Element
    {
        if (string.IsNullOrEmpty(uniqueId))
        {
            return null ;
        }

        try
        {
            return document.GetElement(uniqueId) as TElement ;
        }
        catch
        {
            return null ;
        }
    }

    /// <summary>
    /// Gets an element by its element ID
    /// </summary>
    /// <typeparam name="TElement">The type of element to retrieve</typeparam>
    /// <param name="document">The document</param>
    /// <param name="elementId">The element ID</param>
    /// <returns>The element, or null if not found or invalid</returns>
    public static TElement? GetElementById<TElement>(this Document document,
        ElementId elementId) where TElement : Element =>
        ElementId.InvalidElementId == elementId ? null : document.GetElement(elementId) as TElement ;

    /// <summary>
    /// Gets all elements of the specified type in the document
    /// </summary>
    /// <typeparam name="TElement">The type of element to retrieve</typeparam>
    /// <param name="document">The document</param>
    /// <returns>An enumerable collection of elements</returns>
    public static IEnumerable<TElement> GetAllElements<TElement>(this Document document) where TElement : Element =>
        document.GetAllElements<TElement>((View?)null) ;

    /// <summary>
    /// Gets all elements of the specified type visible in the view
    /// </summary>
    /// <typeparam name="TElement">The type of element to retrieve</typeparam>
    /// <param name="document">The document</param>
    /// <param name="view">The view to filter elements, or null for all elements</param>
    /// <returns>An enumerable collection of elements</returns>
    public static IEnumerable<TElement> GetAllElements<TElement>(this Document document,
        View? view) where TElement : Element =>
        new ElementQuery<TElement>(document, view) ;

    /// <summary>
    /// Gets all elements of the specified type from a collection of element IDs
    /// </summary>
    /// <typeparam name="TElement">The type of element to retrieve</typeparam>
    /// <param name="document">The document</param>
    /// <param name="elementIds">The collection of element IDs</param>
    /// <returns>An enumerable collection of elements</returns>
    public static IEnumerable<TElement> GetAllElements<TElement>(this Document document,
        IEnumerable<ElementId> elementIds) where TElement : Element
    {
        var idArray = elementIds.ToArray() ;
        return idArray.Length == 0
            ? []
            : new ElementQuery<TElement>(document, idArray) ;
    }

    /// <summary>
    /// Gets all elements of the specified type matching the given runtime type
    /// </summary>
    /// <typeparam name="TElement">The type of element to retrieve</typeparam>
    /// <param name="document">The document</param>
    /// <param name="type">The runtime type to filter by</param>
    /// <returns>An enumerable collection of elements</returns>
    public static IEnumerable<TElement> GetAllElements<TElement>(this Document document,
        Type type) where TElement : Element =>
        document.GetAllElements<TElement>(type, (View?)null) ;

    /// <summary>
    /// Gets all elements of the specified type matching the given runtime type visible in the view
    /// </summary>
    /// <typeparam name="TElement">The type of element to retrieve</typeparam>
    /// <param name="document">The document</param>
    /// <param name="type">The runtime type to filter by</param>
    /// <param name="view">The view to filter elements, or null for all elements</param>
    /// <returns>An enumerable collection of elements</returns>
    public static IEnumerable<TElement> GetAllElements<TElement>(this Document document,
        Type type,
        View? view) where TElement : Element
    {
        if (! typeof( TElement ).IsAssignableFrom(type))
        {
            throw new ArgumentException($"Type {type.Name} is not assignable to {typeof( TElement ).Name}",
                nameof( type )) ;
        }

        return new ElementQuery<TElement>(document, view, type) ;
    }

    /// <summary>
    /// Gets all elements of the specified type matching the given runtime type from element IDs
    /// </summary>
    /// <typeparam name="TElement">The type of element to retrieve</typeparam>
    /// <param name="document">The document</param>
    /// <param name="type">The runtime type to filter by</param>
    /// <param name="elementIds">The collection of element IDs</param>
    /// <returns>An enumerable collection of elements</returns>
    public static IEnumerable<TElement> GetAllElements<TElement>(this Document document,
        Type type,
        IEnumerable<ElementId> elementIds) where TElement : Element
    {
        if (! typeof( TElement ).IsAssignableFrom(type))
        {
            throw new ArgumentException($"Type {type.Name} is not assignable to {typeof( TElement ).Name}",
                nameof( type )) ;
        }

        var idArray = elementIds.ToArray() ;
        return idArray.Length == 0
            ? []
            : new ElementQuery<TElement>(document, idArray, type) ;
    }

    /// <summary>
    /// Gets all instances of elements in the document
    /// </summary>
    /// <param name="document">The document</param>
    /// <returns>A collection of all element instances</returns>
    public static IEnumerable<Element> GetInstances(this Document document)
    {
        return new FilteredElementCollector(document)
            .WhereElementIsNotElementType() ;
    }

    /// <summary>
    /// Gets all instances of elements in the document by category
    /// </summary>
    /// <param name="document">The document</param>
    /// <param name="category">The category to filter by</param>
    /// <returns>A collection of element instances</returns>
    public static IEnumerable<Element> GetInstances(this Document document,
        BuiltInCategory category)
    {
        return new FilteredElementCollector(document)
            .WhereElementIsNotElementType()
            .OfCategory(category) ;
    }

    /// <summary>
    /// Gets all instances of elements in the document visible in the view
    /// </summary>
    /// <param name="document">The document</param>
    /// <param name="viewId">The view ID</param>
    /// <returns>A collection of element instances</returns>
    public static IEnumerable<Element> GetInstances(this Document document,
        ElementId viewId)
    {
        return new FilteredElementCollector(document, viewId)
            .WhereElementIsNotElementType() ;
    }

    /// <summary>
    /// Gets all instances of elements in the document visible in the view by category
    /// </summary>
    /// <param name="document">The document</param>
    /// <param name="viewId">The view ID</param>
    /// <param name="category">The category to filter by</param>
    /// <returns>A collection of element instances</returns>
    public static IEnumerable<Element> GetInstances(this Document document,
        ElementId viewId,
        BuiltInCategory category)
    {
        return new FilteredElementCollector(document, viewId)
            .WhereElementIsNotElementType()
            .OfCategory(category) ;
    }

    /// <summary>
    /// Gets all instances of elements of type T in the document
    /// </summary>
    /// <typeparam name="T">Type inherited from Element</typeparam>
    /// <param name="document">The document</param>
    /// <returns>An enumerable collection of element instances</returns>
    public static IEnumerable<T> EnumerateInstances<T>(this Document document) where T : Element
    {
        return new FilteredElementCollector(document)
            .WhereElementIsNotElementType()
            .OfClass(typeof( T ))
            .Cast<T>() ;
    }

    /// <summary>
    /// Gets all instances of elements of type T in the document by category
    /// </summary>
    /// <typeparam name="T">Type inherited from Element</typeparam>
    /// <param name="document">The document</param>
    /// <param name="category">The category to filter by</param>
    /// <returns>An enumerable collection of element instances</returns>
    public static IEnumerable<T> EnumerateInstances<T>(this Document document,
        BuiltInCategory category) where T : Element
    {
        return new FilteredElementCollector(document)
            .WhereElementIsNotElementType()
            .OfCategory(category)
            .OfClass(typeof( T ))
            .Cast<T>() ;
    }

    /// <summary>
    /// Gets all instances of elements of type T visible in the view
    /// </summary>
    /// <typeparam name="T">Type inherited from Element</typeparam>
    /// <param name="document">The document</param>
    /// <param name="viewId">The view ID</param>
    /// <returns>An enumerable collection of element instances</returns>
    public static IEnumerable<T> EnumerateInstances<T>(this Document document,
        ElementId viewId) where T : Element
    {
        return new FilteredElementCollector(document, viewId)
            .WhereElementIsNotElementType()
            .OfClass(typeof( T ))
            .Cast<T>() ;
    }

    /// <summary>
    /// Gets all instances of elements of type T visible in the view by category
    /// </summary>
    /// <typeparam name="T">Type inherited from Element</typeparam>
    /// <param name="document">The document</param>
    /// <param name="viewId">The view ID</param>
    /// <param name="category">The category to filter by</param>
    /// <returns>An enumerable collection of element instances</returns>
    public static IEnumerable<T> EnumerateInstances<T>(this Document document,
        ElementId viewId,
        BuiltInCategory category) where T : Element
    {
        return new FilteredElementCollector(document, viewId)
            .WhereElementIsNotElementType()
            .OfCategory(category)
            .OfClass(typeof( T ))
            .Cast<T>() ;
    }
}
