using OperacaoPato.Backend.Application.DTOs; // Para usar PatoDto
using System;
using System.Threading.Tasks;

namespace OperacaoPato.Backend.Application.Interfaces // Ajuste o namespace se necessário
{
    public interface IObterPatoPorIdUseCase
    {
        // Define um método que recebe um Guid e retorna UM PatoDto (ou null se não encontrar)
        Task<PatoDto?> HandleAsync(Guid id); 
    }
}