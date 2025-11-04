using SixLabors.ImageSharp.PixelFormats;

namespace Api.Shared.DTOs.Filters
{
    public class StaticCordsModels
    {
        public StaticCordsModels(double lat1, double lat2, double lng1, double lng2, string unit)
        {
            Lat1 = lat1;
            Lat2 = lat2;
            Lng1 = lng1;
            Lng2 = lng2;
            Unit = unit;
        }
        public double Lat1 { get; set; } = 0;
        public double Lat2 { get; set; } = 0;
        public double Lng1 { get; set; } = 0;
        public double Lng2 { get; set; } = 0;
        public string Unit { get; set; } = string.Empty;

        // Funcion que hace el calculo para obtener la distancia ente 2 coordenadas
        public static double DistanceCalculator(StaticCordsModels staticCordsModels)
        {
            // Radio de la Tierra en kilómetros
            double radioTierra = EarthRadius(staticCordsModels.Unit);

            // Convertir las coordenadas de grados a radianes
            double latitud1Rad = ToRadians(staticCordsModels.Lat1);
            double longitud1Rad = ToRadians(staticCordsModels.Lng1);
            double latitud2Rad = ToRadians(staticCordsModels.Lat2);
            double longitud2Rad = ToRadians(staticCordsModels.Lng2);

            // Calcula la diferencia en radianes entre las latitudes y longitudes de dos puntos.
            double latitudDelta = latitud2Rad - latitud1Rad;
            double longitudDelta = longitud2Rad - longitud1Rad;

            // Calcula el término 'a' de la fórmula de Haversine. Este término es una parte de la fórmula que involucra las diferencias en latitud y longitud entre los dos puntos.
            double a = Math.Sin(latitudDelta / 2) * Math.Sin(latitudDelta / 2) +
                       Math.Cos(latitud1Rad) * Math.Cos(latitud2Rad) *
                       Math.Sin(longitudDelta / 2) * Math.Sin(longitudDelta / 2);

            // Calcula el ángulo central (central angle) utilizando la función arcotangente (Atan2). Este ángulo central es necesario para calcular la distancia entre los dos puntos.
            double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));

            // Devuelve la distancia en la unidad de medida que pasaste
            return Math.Round(radioTierra * c);
        }

        // Transforma las cordenadas en grados radianes
        public static double ToRadians(double grados) => grados * Math.PI / 180.0;

        // Obtiene de acuerdo al parametro Unit la distanca del radio de la tierra en su respectiva unidad
        public static double EarthRadius(string unit) => unit.ToLower() switch
        {
            "kilometers" => 6371,
            "meters" => 6371000,
            _ => throw new ArgumentException("Unidad de medida no válida", nameof(unit)),
        };

    }
}
