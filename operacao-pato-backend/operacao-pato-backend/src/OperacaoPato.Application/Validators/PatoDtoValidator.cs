using FluentValidation;
using OperacaoPato.Application.DTOs;

namespace OperacaoPato.Application.Validators
{
    public class PatoDtoValidator : AbstractValidator<PatoDto>
    {
        public PatoDtoValidator()
        {
            RuleFor(x => x.NumeroSerieDrone)
                .NotEmpty().WithMessage("Número de série do drone é obrigatório.");

            RuleFor(x => x.MarcaDrone)
                .NotEmpty().WithMessage("Marca do drone é obrigatória.");

            RuleFor(x => x.FabricanteDrone)
                .NotEmpty().WithMessage("Fabricante do drone é obrigatório.");

            RuleFor(x => x.PaisOrigemDrone)
                .NotEmpty().WithMessage("País de origem do drone é obrigatório.");

            RuleFor(x => x.Altura)
                .GreaterThan(0).WithMessage("Altura deve ser maior que zero.");

            RuleFor(x => x.Peso)
                .GreaterThan(0).WithMessage("Peso deve ser maior que zero.");

            RuleFor(x => x.Longitude)
                .NotNull().WithMessage("Localização é obrigatória.");

            RuleFor(x => x.EstadoHibernacao)
                .IsInEnum().WithMessage("Estado de hibernação inválido.");

            RuleFor(x => x.QuantidadeMutacoes)
                .GreaterThanOrEqualTo(0).WithMessage("Quantidade de mutações não pode ser negativa.");

            When(x => x.BatimentosCardiacos.HasValue, () =>
            {
                RuleFor(x => x.BatimentosCardiacos.Value)
                    .InclusiveBetween(1, 300).WithMessage("Batimentos cardíacos devem estar entre 1 e 300.");
            });

            When(x => !string.IsNullOrEmpty(x.NomeSuperPoder) || !string.IsNullOrEmpty(x.DescricaoSuperPoder), () =>
            {
                RuleFor(x => x.NomeSuperPoder)
                    .NotEmpty().WithMessage("Nome do superpoder é obrigatório quando houver informações do superpoder.");
                RuleFor(x => x.DescricaoSuperPoder)
                    .NotEmpty().WithMessage("Descrição do superpoder é obrigatória quando houver informações do superpoder.");
            });
        }
    }
}