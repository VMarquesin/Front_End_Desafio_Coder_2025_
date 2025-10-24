using System;

namespace OperacaoPato.Domain.ValueObjects
{
    public class CoordenadaGps
    {
        public double Latitude { get; private set; }
        public double Longitude { get; private set; }

        public CoordenadaGps(double latitude, double longitude)
        {
            Latitude = latitude;
            Longitude = longitude;
        }

        public override string ToString()
        {
            return $"Latitude: {Latitude}, Longitude: {Longitude}";
        }
    }
}