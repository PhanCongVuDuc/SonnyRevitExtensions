using System.Collections ;

namespace SonnyRevitExtensions.Extensions ;

/// <summary>
/// Lazy-evaluated query for filtering Revit elements
/// Uses a different approach than builder pattern - more functional style
/// </summary>
/// <typeparam name="TElement">The type of element to query</typeparam>
internal class ElementQuery<TElement> : IEnumerable<TElement> where TElement : Element
{
    private readonly Document _document ;
    private readonly ElementId[]? _elementIds ;
    private readonly Type? _runtimeType ;
    private readonly View? _view ;

    public ElementQuery(Document document,
        View? view)
    {
        _document = document ;
        _view = view ;
        _elementIds = null ;
        _runtimeType = null ;
    }

    public ElementQuery(Document document,
        View? view,
        Type runtimeType)
    {
        _document = document ;
        _view = view ;
        _elementIds = null ;
        _runtimeType = runtimeType ;
    }

    public ElementQuery(Document document,
        ElementId[] elementIds)
    {
        _document = document ;
        _elementIds = elementIds ;
        _view = null ;
        _runtimeType = null ;
    }

    public ElementQuery(Document document,
        ElementId[] elementIds,
        Type runtimeType)
    {
        _document = document ;
        _elementIds = elementIds ;
        _view = null ;
        _runtimeType = runtimeType ;
    }

    public IEnumerator<TElement> GetEnumerator()
    {
        var collector = CreateCollector() ;

        // Always apply a filter - Revit requires at least one filter
        if (_runtimeType != null
            && _runtimeType != typeof( Element ))
        {
            collector = collector.OfClass(_runtimeType) ;
        }
        else if (typeof( TElement ) != typeof( Element ))
        {
            // If TElement is a specific type (not Element), use OfClass
            collector = collector.OfClass(typeof( TElement )) ;
        }
        else
        {
            // Default filter: get instances (not types)
            collector = collector.WhereElementIsNotElementType() ;
        }

        return collector.Cast<TElement>()
            .GetEnumerator() ;
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator() ;

    private FilteredElementCollector CreateCollector()
    {
        if (_elementIds != null
            && _elementIds.Length > 0)
        {
            return new FilteredElementCollector(_document, _elementIds) ;
        }

        return _view != null
            ? new FilteredElementCollector(_document, _view.Id)
            : new FilteredElementCollector(_document) ;
    }
}
