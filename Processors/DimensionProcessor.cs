using SonnyRevitExtensions.Extensions ;
using SonnyRevitExtensions.Utilities ;

namespace SonnyRevitExtensions.Processors ;

/// <summary>
///     Processor class for handling dimension operations with encapsulated state
/// </summary>
public class DimensionProcessor
{
    private readonly Dimension _dimension ;
    private readonly Line _dimensionLine ;
    private readonly DimensionType? _dimensionType ;
    private readonly Document _document ;
    private readonly double _minimumValue ;
    private readonly ReferenceArray _references ;
    private readonly DimensionSegmentArray _segments ;
    private readonly View _view ;

    /// <summary>
    ///     Initializes a new instance of DimensionProcessor
    /// </summary>
    /// <param name="dimension">The dimension to process</param>
    /// <param name="dimensionLine">The line along which the dimension is placed</param>
    /// <param name="view"></param>
    /// <param name="minimumValue">The minimum value threshold for dimension segments</param>
    /// <param name="dimensionType"></param>
    public DimensionProcessor(Dimension dimension,
        Line dimensionLine,
        View view,
        double minimumValue,
        DimensionType? dimensionType = null)
    {
        _dimension = dimension ;
        _dimensionLine = dimensionLine ;
        _view = view ;
        _minimumValue = minimumValue ;
        _dimensionType = dimensionType ;

        _document = dimension.Document ;
        _segments = dimension.Segments ;
        _references = dimension.References ;
    }

    /// <summary>
    ///     Processes the dimension by removing segments smaller than minimum value
    /// </summary>
    /// <returns>The modified dimension or null if no valid dimension can be created</returns>
    public Dimension? Process()
    {
        // Handle simple dimension with no segments
        if (IsSimpleDimensionWithZeroValue())
        {
            _document.Delete(_dimension.Id) ;
            return null ;
        }

        // Check if any segments have zero or near-zero values
        if (! HasZeroValueSegments())
        {
            return _dimension ;
        }

        // Process dimension segments and create new dimension
        return ProcessDimensionSegments() ;
    }

    /// <summary>
    ///     Checks if the dimension is a simple dimension (no segments) with zero value
    /// </summary>
    private bool IsSimpleDimensionWithZeroValue() =>
        _segments.Size == 0
        && _references.Size == 2
        && _dimension.Value != null
        && Math.Abs((double)_dimension.Value) < ToleranceConstants.Tolerance1E4 ;

    /// <summary>
    ///     Checks if any dimension segments have zero or near-zero values
    /// </summary>
    private bool HasZeroValueSegments()
    {
        foreach (DimensionSegment segment in _segments)
        {
            if (segment.Value != null
                && Math.Abs((double)segment.Value) < ToleranceConstants.Tolerance1E4)
            {
                return true ;
            }
        }

        return false ;
    }

    /// <summary>
    ///     Processes dimension segments and creates a new dimension with valid segments
    /// </summary>
    private Dimension? ProcessDimensionSegments()
    {
        var validReferences = FilterValidReferences() ;

        // Add the last reference if the last segment is valid
        if (ShouldIncludeLastReference())
        {
            validReferences.Add(_references.get_Item(_references.Size - 1)) ;
        }

        // Delete the original dimension
        _document.Delete(_dimension.Id) ;

        // Create new dimension with valid references
        return CreateNewDimension(validReferences) ;
    }

    /// <summary>
    ///     Filters references based on segment values and minimum threshold
    /// </summary>
    private List<Reference> FilterValidReferences()
    {
        var validReferences = new List<Reference>() ;

        // Always add the first reference
        if (_references.Size > 0)
        {
            validReferences.Add(_references.get_Item(0)) ;
        }

        // Add references for segments that meet the minimum value requirement
        for (var i = 0; i < _segments.Size; i++)
        {
            var segment = _segments.get_Item(i) ;
            if (segment.Value >= _minimumValue)
            {
                validReferences.Add(_references.get_Item(i + 1)) ;
            }
        }

        return validReferences ;
    }

    /// <summary>
    ///     Determines if the last reference should be included based on the last segment value
    /// </summary>
    private bool ShouldIncludeLastReference()
    {
        if (_segments.Size == 0)
        {
            return false ;
        }

        var lastSegment = _segments.get_Item(_segments.Size - 1) ;
        return lastSegment.Value > _minimumValue ;
    }

    /// <summary>
    ///     Creates a new dimension if there are enough valid references
    /// </summary>
    private Dimension? CreateNewDimension(List<Reference> validReferences)
    {
        if (validReferences.Count < 2)
        {
            return null ;
        }

        var referenceArray = new ReferenceArray() ;
        validReferences.ForEach(reference => referenceArray.Append(reference)) ;

        return referenceArray.ConditionCreateDimension()
            ? CreateElement.CreateDimension(_view,
                _dimensionLine,
                referenceArray,
                _dimensionType)
            : null ;
    }
}
