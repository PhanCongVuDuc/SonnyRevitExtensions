using SonnyRevitExtensions.Processors ;

namespace SonnyRevitExtensions.Extensions ;

public static class DimensionExtension
{
    /// <summary>
    ///     Removes dimension segments that are smaller than the specified minimum value
    ///     and recreates the dimension with the remaining valid segments.
    /// </summary>
    /// <param name="dimension">The dimension to process</param>
    /// <param name="dimensionLine">The line along which the dimension is placed</param>
    /// <param name="view"></param>
    /// <param name="minimumValue">The minimum value threshold for dimension segments</param>
    /// <param name="dimensionType"></param>
    /// <returns>The modified dimension or null if no valid dimension can be created</returns>
    public static Dimension? RemoveDimension(this Dimension dimension,
        Line dimensionLine,
        View view,
        double minimumValue,
        DimensionType? dimensionType = null)
    {
        var processor = new DimensionProcessor(dimension,
            dimensionLine,
            view,
            minimumValue,
            dimensionType) ;
        return processor.Process() ;
    }
}
