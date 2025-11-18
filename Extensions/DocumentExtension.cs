namespace PentaOceanApp.RevitExtensions ;

public static class DocumentExtension
{
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

    public static TElement? GetElementById<TElement>(this Document document,
        ElementId elementId) where TElement : Element =>
        ElementId.InvalidElementId == elementId ? null : document.GetElement(elementId) as TElement ;
}
