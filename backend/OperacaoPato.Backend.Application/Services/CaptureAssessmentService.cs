using System;
using OperacaoPato.Backend.Application.DTOs;
using OperacaoPato.Backend.Domain.Entities;
using OperacaoPato.Backend.Domain.Enums;
using OperacaoPato.Backend.Domain.ValueObjects;

namespace OperacaoPato.Backend.Application.Services
{
    public class CaptureAssessmentService
    {
        private readonly (double lat, double lon) _baseCoords;
        private const double BaseTransportRatePerKm = 10.0;
        private const double BaseSequencingCost = 5000.0;

        public CaptureAssessmentService(double baseLatitude = -23.55052, double baseLongitude = -46.633308)
        {
            _baseCoords = (baseLatitude, baseLongitude);
        }

        public CaptureAssessmentResult Assess(PatoPrimordial pato)
        {
            var result = new CaptureAssessmentResult { PatoId = pato.Id };

            // Distance
            double distanceKm;
            try
            {
                distanceKm = HaversineKm(_baseCoords.lat, _baseCoords.lon, pato.Localizacao.Coordenada.Latitude, pato.Localizacao.Coordenada.Longitude);
            }
            catch
            {
                distanceKm = 10000; // penalize
                result.Notes += "Missing or invalid coordinates; distance assumed very large. ";
            }
            result.EstimatedTransportKm = Math.Round(distanceKm, 2);

            // Size
            var altura = pato.ObterAlturaEm(UnidadeComprimento.Metro).Valor;
            var alturaScore = ComputeHeightScore(altura);

            double pesoGramas = pato.Peso.EmGramas();
            var pesoScore = ComputeWeightScore(pesoGramas);

            var sizeScore = Math.Max(alturaScore, pesoScore);

            // Distance score
            var distScore = ComputeDistanceScore(distanceKm);

            // Status risk
            var statusRisk = ComputeStatusRisk(pato.Status, pato.BatimentosPorMinuto);

            // Power severity
            var powerSeverity = EstimatePowerSeverity(pato.Poder);

            // Mutations
            var mutationValue = ComputeMutationValue(pato.QuantidadeMutacoes);

            // Composite
            var operationalCost = Clamp(sizeScore * 0.5 + distScore * 0.4 + powerSeverity * 0.1, 0, 100);
            var captureRisk = Clamp(statusRisk * 0.5 + powerSeverity * 0.3 + sizeScore * 0.2, 0, 100);
            var scientificValue = Clamp(mutationValue * 0.7 + (100 - powerSeverity) * 0.3, 0, 100);

            var forceLevel = MapForceLevel(captureRisk, powerSeverity);

            var transportCost = BaseTransportRatePerKm * distanceKm * (1 + sizeScore / 100.0);
            var sequencingCost = BaseSequencingCost * (1 + pato.QuantidadeMutacoes / 10.0);

            var recommendation = MakeRecommendation(scientificValue, captureRisk, operationalCost);

            result.OperationalCostScore = Math.Round(operationalCost, 1);
            result.CaptureRiskScore = Math.Round(captureRisk, 1);
            result.ScientificValueScore = Math.Round(scientificValue, 1);
            result.RequiredForceLevel = forceLevel;
            result.EstimatedTransportCost = Math.Round(transportCost, 2);
            result.EstimatedSequencingCost = Math.Round(sequencingCost, 2);
            result.Recommendation = recommendation;

            return result;
        }

        private static double HaversineKm(double lat1, double lon1, double lat2, double lon2)
        {
            double R = 6371.0;
            double dLat = ToRad(lat2 - lat1);
            double dLon = ToRad(lon2 - lon1);
            double a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                       Math.Cos(ToRad(lat1)) * Math.Cos(ToRad(lat2)) *
                       Math.Sin(dLon / 2) * Math.Sin(dLon / 2);
            double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
            return R * c;
        }

        private static double ToRad(double deg) => deg * Math.PI / 180.0;

        private static double ComputeHeightScore(double meters)
        {
            if (meters <= 0.5) return 0;
            if (meters <= 2) return Lerp(0, 40, (meters - 0.5) / (2 - 0.5));
            if (meters <= 10) return Lerp(40, 80, (meters - 2) / (10 - 2));
            return Lerp(90, 100, Math.Min(1.0, (meters - 10) / 10.0));
        }

        private static double ComputeWeightScore(double grams)
        {
            if (grams <= 1000) return 0;
            if (grams <= 20000) return Lerp(0, 40, (grams - 1000) / (20000 - 1000));
            if (grams <= 100000) return Lerp(40, 80, (grams - 20000) / (100000 - 20000));
            return Lerp(90, 100, Math.Min(1.0, (grams - 100000) / 100000.0));
        }

        private static double ComputeDistanceScore(double km)
        {
            if (km <= 10) return Lerp(0, 10, km / 10.0);
            if (km <= 100) return Lerp(10, 50, (km - 10) / 90.0);
            if (km <= 1000) return Lerp(50, 90, (km - 100) / 900.0);
            return Lerp(90, 100, Math.Min(1.0, (km - 1000) / 4000.0));
        }

        private static double ComputeStatusRisk(StatusHibernacao status, int? bpm)
        {
            return status switch
            {
                StatusHibernacao.Desperto => 90,
                StatusHibernacao.Transe => ComputeTranseRisk(bpm),
                StatusHibernacao.HibernacaoProfunda => 10,
                _ => 50
            };
        }

        private static double ComputeTranseRisk(int? bpm)
        {
            if (!bpm.HasValue) return 70;
            if (bpm.Value <= 40) return 35;
            if (bpm.Value <= 60) return 45;
            if (bpm.Value <= 80) return 55;
            if (bpm.Value <= 100) return 65;
            return 85;
        }

        private static double EstimatePowerSeverity(SuperPoder poder)
        {
            if (poder == null) return 30;
            var name = poder.Nome?.ToLowerInvariant() ?? "";
            var cls = poder.Classificacao?.ToLowerInvariant() ?? "";
            double score = 20;

            if (cls.Contains("ofens") || cls.Contains("alto") || cls.Contains("risco") || name.Contains("raio") || name.Contains("explos"))
                score = 90;
            else if (cls.Contains("defens") || name.Contains("camuflagem")) score = 40;
            else if (cls.Contains("mobilidade") || name.Contains("voo")) score = 60;
            else if (cls.Contains("raro") || name.Contains("supers")) score = 75;
            else score = 30;

            score = Clamp(score + Math.Min(10, name.Length / 5.0), 0, 100);
            return score;
        }

        private static double ComputeMutationValue(int quantidade)
        {
            if (quantidade <= 0) return 0;
            if (quantidade <= 5) return Lerp(10, 60, (quantidade - 1) / 4.0);
            if (quantidade <= 20) return Lerp(60, 100, (quantidade - 5) / 15.0);
            return 100;
        }

        private static string MapForceLevel(double captureRisk, double powerSeverity)
        {
            double combined = (captureRisk * 0.6 + powerSeverity * 0.4);
            if (combined < 25) return "Recon";
            if (combined < 50) return "Team";
            if (combined < 75) return "Armored";
            return "Heavy";
        }

        private static string MakeRecommendation(double scientificValue, double captureRisk, double operationalCost)
        {
            if (scientificValue > 70 && captureRisk < 50 && operationalCost < 60) return "Go";
            if (scientificValue > 50 && captureRisk < 70 && operationalCost < 80) return "Consider";
            return "Defer/Observe";
        }

        private static double Lerp(double a, double b, double t) => a + (b - a) * Clamp(t, 0, 1);
        private static double Clamp(double v, double a, double b) => Math.Max(a, Math.Min(b, v));
    }
}
