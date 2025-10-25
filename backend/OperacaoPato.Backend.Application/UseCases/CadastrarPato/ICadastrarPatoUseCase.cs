using OperacaoPato.Backend.Application.DTOs;

namespace OperacaoPato.Backend.Application.UseCases.CadastrarPato
{
    public interface ICadastrarPatoUseCase
    {
        Task<PatoDto> HandleAsync(PatoDto input, CancellationToken ct = default);
    }
}