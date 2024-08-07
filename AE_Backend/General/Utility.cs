namespace AE_Backend.General
{
    public class Utility
    {
        public decimal CalculateDistance(decimal lat1, decimal lon1, decimal lat2, decimal lon2)
        {
            const double EarthRadiusKm = 6371; // Earth's radius in kilometers

            // Convert latitude and longitude from degrees to radians
            double lat1Rad = ToRadians((double)lat1);
            double lon1Rad = ToRadians((double)lon1);
            double lat2Rad = ToRadians((double)lat2);
            double lon2Rad = ToRadians((double)lon2);

            // Haversine formula
            double dlon = lon2Rad - lon1Rad;
            double dlat = lat2Rad - lat1Rad;
            double a = Math.Pow(Math.Sin(dlat / 2), 2) + Math.Cos(lat1Rad) * Math.Cos(lat2Rad) * Math.Pow(Math.Sin(dlon / 2), 2);
            double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
            double distanceKm = EarthRadiusKm * c;

            // Convert the result back to decimal
            decimal distanceDecimal = (decimal)distanceKm;

            return distanceDecimal;
        }

        private double ToRadians(double degrees)
        {
            return degrees * Math.PI / 180;
        }
    }
}
