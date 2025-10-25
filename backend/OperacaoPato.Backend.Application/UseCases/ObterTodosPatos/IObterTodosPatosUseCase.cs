using OperacaoPato.Backend.Application.DTOs;

namespace OperacaoPato.Backend.Application.UseCases.ObterTodosPatos
{
    public interface IObterTodosPatosUseCase
    {
        Task<IEnumerable<PatoDto>> HandleAsync(CancellationToken ct = default);
    }
}