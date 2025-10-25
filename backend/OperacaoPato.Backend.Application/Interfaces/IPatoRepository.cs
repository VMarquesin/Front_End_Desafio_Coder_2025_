using OperacaoPato.Backend.Domain.Entities;

namespace OperacaoPato.Backend.Application.Interfaces
{
    public interface IPatoRepository
    {
        Task<PatoPrimordial> AdicionarAsync(PatoPrimordial pato);
        Task<IEnumerable<PatoPrimordial>> ObterTodosAsync();
        Task<PatoPrimordial?> ObterPorIdAsync(Guid id);

        // Dependendo da regra, podemos verificar duplicidade por (DroneNumeroSerie, DataColetaUtc) ou outro critério:
        Task<bool> ExisteAsync(string droneNumeroSerie, DateTime dataColetaUtc);
        Task<bool> RemoverAsync(Guid id);
    }
}