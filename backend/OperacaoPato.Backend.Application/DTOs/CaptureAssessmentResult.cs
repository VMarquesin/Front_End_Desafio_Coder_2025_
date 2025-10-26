using System;

namespace OperacaoPato.Backend.Application.DTOs
{
    public class CaptureAssessmentResult
    {
        public Guid PatoId { get; set; }
        public double OperationalCostScore { get; set; }
        public double CaptureRiskScore { get; set; }
        public double ScientificValueScore { get; set; }
        public string RequiredForceLevel { get; set; } = string.Empty;
        public double EstimatedTransportKm { get; set; }
        public double EstimatedTransportCost { get; set; }
        public double EstimatedSequencingCost { get; set; }
        public string Recommendation { get; set; } = string.Empty;
        public string Notes { get; set; } = string.Empty;
    }
}
