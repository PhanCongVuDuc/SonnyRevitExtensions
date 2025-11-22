using SonnyRevitExtensions.Extensions ;

namespace SonnyRevitExtensions.Utilities ;

public class CreateElement
{
    public static Dimension CreateDimension(View view,
        Line line,
        ReferenceArray references,
        DimensionType? dimensionType = null,
        double? minimumValue = null)
    {
        if (dimensionType != null)
        {
            var newDimension = view.Document.Create.NewDimension(view,
                line,
                references,
                dimensionType) ;

            if (minimumValue != null)
            {
                var removeDimension = newDimension.RemoveDimension(line,
                    view,
                    (double)minimumValue,
                    dimensionType) ;

                return removeDimension
                       ?? view.Document.Create.NewDimension(view,
                           line,
                           references,
                           dimensionType) ;
            }

            return newDimension ;
        }

        var dimension2 = view.Document.Create.NewDimension(view,
            line,
            references) ;

        if (minimumValue != null)
        {
            var removeDimension2 = dimension2.RemoveDimension(line,
                view,
                (double)minimumValue!,
                dimensionType) ;
            return removeDimension2
                   ?? view.Document.Create.NewDimension(view,
                       line,
                       references) ;
        }

        return dimension2 ;
    }
}
