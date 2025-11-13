namespace SonnyRevitExtensions.Extensions.Geometries ;

public class GeometryExtensions
{
  public static double GetLengthOfPointToPlane( XYZ normalOfPlane,
    XYZ originOfPlane,
    XYZ anyPoint )
  {
    // plane
    Plane plane = Plane.CreateByNormalAndOrigin( normalOfPlane,
      originOfPlane ) ;
    double d = plane.Normal.DotProduct( -plane.Origin ) ;
    // tu
    //double tu = Math.Abs(plane.Normal.DotProduct(AnyPoint) + d);
    double tu = plane.Normal.DotProduct( anyPoint ) + d ;

    // mau
    double mau = Math.Sqrt( plane.Normal.DotProduct( plane.Normal ) ) ;
    return Math.Abs( tu / mau ) ;
  }
}