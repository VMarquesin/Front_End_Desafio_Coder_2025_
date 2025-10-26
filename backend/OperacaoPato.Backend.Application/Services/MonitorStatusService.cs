using OperacaoPato.Backend.Domain.ValueObjects;

namespace OperacaoPato.Backend.Application.Services;

public class MonitorStatusService
{
    private const double LIMITE_CRITICO_BATERIA = 20.0;
    private const double LIMITE_CRITICO_COMBUSTIVEL = 15.0;
    private const double LIMITE_CRITICO_INTEGRIDADE = 30.0;

    public bool PodeOperar(NivelRecurso bateria, NivelRecurso combustivel, NivelRecurso integridade)
    {
        return bateria.PorcentagemAtual > LIMITE_CRITICO_BATERIA &&
               combustivel.PorcentagemAtual > LIMITE_CRITICO_COMBUSTIVEL &&
               integridade.PorcentagemAtual > LIMITE_CRITICO_INTEGRIDADE;
    }

    public double EstimarAutonomiaRestante(NivelRecurso bateria, NivelRecurso combustivel)
    {
        var autonomiaBateria = bateria.PorcentagemAtual * 0.5; // horas por % de bateria
        var autonomiaCombustivel = combustivel.PorcentagemAtual * 0.3; // horas por % de combustível
        
        return Math.Min(autonomiaBateria, autonomiaCombustivel);
    }

    public (bool Critico, string Alerta) VerificarAlertasCriticos(
        NivelRecurso bateria,
        NivelRecurso combustivel,
        NivelRecurso integridade)
    {
        var mensagens = new List<string>();

        if (bateria.PorcentagemAtual <= LIMITE_CRITICO_BATERIA)
            mensagens.Add($"Bateria crítica: {bateria.PorcentagemAtual:F1}%");
        
        if (combustivel.PorcentagemAtual <= LIMITE_CRITICO_COMBUSTIVEL)
            mensagens.Add($"Combustível crítico: {combustivel.PorcentagemAtual:F1}%");
        
        if (integridade.PorcentagemAtual <= LIMITE_CRITICO_INTEGRIDADE)
            mensagens.Add($"Integridade crítica: {integridade.PorcentagemAtual:F1}%");

        return mensagens.Any()
            ? (true, string.Join(". ", mensagens))
            : (false, string.Empty);
    }
}