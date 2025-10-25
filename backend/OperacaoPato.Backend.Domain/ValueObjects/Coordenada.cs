using System;

namespace OperacaoPato.Backend.Domain.ValueObjects
{
    public sealed class Coordenada
    {
        public double Latitude { get; }
        public double Longitude { get; }

        public Coordenada(double latitude, double longitude)
        {
            if (latitude < -90 || latitude > 90) throw new ArgumentOutOfRangeException(nameof(latitude), "Latitude deve estar entre -90 e 90.");
            if (longitude < -180 || longitude > 180) throw new ArgumentOutOfRangeException(nameof(longitude), "Longitude deve estar entre -180 e 180.");

            Latitude = latitude;
            Longitude = longitude;
        }
    }
}