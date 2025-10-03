namespace IAVH.BioTablero.CM.Application.Utils;

using System;
using System.Linq;

using NetTopologySuite.Geometries;

/// <summary>
/// Geometry utilities for area calculations.
/// </summary>
public static class GeometryUtils
{
    /// <summary>
    /// Calculate area of a geometry in square kilometers.
    /// </summary>
    /// <param name="geometry">Geometry to calculate area for.</param>
    /// <returns>Area in square kilometers.</returns>
    public static double CalculateAreaInSquareKilometers(Geometry geometry)
    {
        if (geometry == null || geometry.IsEmpty)
        {
            return 0;
        }

        // Use a projection that preserves area (e.g., UTM or Albers Equal Area)
        // For Colombia, we can use UTM Zone 18N (EPSG:32618) or a Colombian projection
        // For simplicity, we'll use a basic calculation with the SRID 4326 (WGS84)
        // Note: This is an approximation. For accurate calculations, use a proper projection.
        var area = geometry.Area;

        // Convert from square degrees to square kilometers
        // This is a rough approximation - for production use, consider using a proper projection
        var latitude = geometry.Centroid.Y;
        var degreesToKm = 111.32 * Math.Cos(latitude * Math.PI / 180);
        var areaInSquareKm = area * degreesToKm * degreesToKm;

        return Math.Round(areaInSquareKm, 6);
    }

    /// <summary>
    /// Calculate area of a polygon in square kilometers.
    /// </summary>
    /// <param name="polygon">Polygon to calculate area for.</param>
    /// <returns>Area in square kilometers.</returns>
    public static double CalculatePolygonAreaInSquareKilometers(Polygon polygon)
    {
        if (polygon == null || polygon.IsEmpty)
        {
            return 0;
        }

        return CalculateAreaInSquareKilometers(polygon);
    }

    /// <summary>
    /// Calculate area of multiple geometries combined.
    /// </summary>
    /// <param name="geometries">Collection of geometries.</param>
    /// <returns>Total area in square kilometers.</returns>
    public static double CalculateTotalAreaInSquareKilometers(params Geometry[] geometries)
    {
        if (geometries == null || geometries.Length == 0)
        {
            return 0;
        }

        var totalArea = 0.0;
        foreach (var geometry in geometries)
        {
            if (geometry != null && !geometry.IsEmpty)
            {
                totalArea += CalculateAreaInSquareKilometers(geometry);
            }
        }

        return Math.Round(totalArea, 6);
    }
}
