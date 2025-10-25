using FluentValidation;
using OperacaoPato.Backend.Application.DTOs;

namespace OperacaoPato.Backend.Application.Validators
{
    public class PatoDtoValidator : AbstractValidator<PatoDto>
    {
        public PatoDtoValidator()
        {
            RuleFor(x => x.DroneNumeroSerie)
                .NotEmpty().WithMessage("DroneNumeroSerie é obrigatório.");

            // Altura
            RuleFor(x => x.AlturaValor)
                .GreaterThan(0).WithMessage("Altura deve ser maior que zero.");
            RuleFor(x => x.AlturaUnidade)
                .NotEmpty().WithMessage("AlturaUnidade é obrigatória.");

            // Peso
            RuleFor(x => x.PesoValor)
                .GreaterThan(0).WithMessage("Peso deve ser maior que zero.");
            RuleFor(x => x.PesoUnidade)
                .NotEmpty().WithMessage("PesoUnidade é obrigatória.");

            // Localização
            RuleFor(x => x.Pais).NotEmpty();
            RuleFor(x => x.Cidade).NotEmpty();
            RuleFor(x => x.Latitude).InclusiveBetween(-90, 90);
            RuleFor(x => x.Longitude).InclusiveBetween(-180, 180);

            // Status e BPM
            RuleFor(x => x.Status).NotEmpty();
            When(x => x.Status.Equals("Transe", StringComparison.OrdinalIgnoreCase) ||
                      x.Status.Equals("HibernacaoProfunda", StringComparison.OrdinalIgnoreCase), () =>
            {
                RuleFor(x => x.BatimentosPorMinuto)
                    .NotNull().WithMessage("BatimentosPorMinuto é obrigatório para Transe/HibernacaoProfunda.")
                    .GreaterThan(0).WithMessage("BatimentosPorMinuto deve ser positivo.");
            });
            When(x => !(x.Status.Equals("Transe", StringComparison.OrdinalIgnoreCase) ||
                        x.Status.Equals("HibernacaoProfunda", StringComparison.OrdinalIgnoreCase)), () =>
            {
                RuleFor(x => x.BatimentosPorMinuto)
                    .Must(bpm => bpm is null || bpm > 0)
                    .WithMessage("BatimentosPorMinuto deve ser positivo quando informado.");
            });

            // Mutação e poder
            RuleFor(x => x.QuantidadeMutacoes)
                .GreaterThanOrEqualTo(0);

            RuleFor(x => x.PoderNome).NotEmpty().WithMessage("Nome do Poder é obrigatório");

            RuleFor(x => x.PoderDescricao).NotEmpty().WithMessage("Descrição do Poder é obrigatório");
        }
    }
}