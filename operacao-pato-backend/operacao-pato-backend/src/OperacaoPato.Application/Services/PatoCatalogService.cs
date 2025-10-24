using System;
using FluentValidation;
using OperacaoPato.Application.DTOs;
using OperacaoPato.Application.Interfaces;

namespace OperacaoPato.Application.Services
{
    public class PatoCatalogService : IPatoCatalogService
    {
        private readonly IPatoRepository _repo;
        private readonly IValidator<PatoDto> _validator;

        public PatoCatalogService(IPatoRepository repo, IValidator<PatoDto> validator)
        {
            _repo = repo;
            _validator = validator;
        }

        public PatoDto CatalogarPato(PatoDto dto)
        {
            _validator.ValidateAndThrow(dto);
            throw new NotImplementedException();
        }

        public PatoDto ObterPorId(Guid id)
        {
            throw new NotImplementedException();
        }
    }
}