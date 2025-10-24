using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;

namespace OperacaoPato.API.Filters
{
    public class ApiExceptionFilter : IExceptionFilter
    {
        private readonly ILogger<ApiExceptionFilter> _logger;

        public ApiExceptionFilter(ILogger<ApiExceptionFilter> logger)
        {
            _logger = logger;
        }

        public void OnException(ExceptionContext context)
        {
            var ex = context.Exception;
            _logger.LogError(ex, "Erro não tratado na API");

            ProblemDetails problem;
            int status;

            switch (ex)
            {
                case ArgumentOutOfRangeException aro:
                    status = 400;
                    problem = new ProblemDetails
                    {
                        Title = "Valor fora do intervalo",
                        Detail = aro.Message,
                        Status = status
                    };
                    break;

                case ArgumentException aex:
                    status = 400;
                    problem = new ProblemDetails
                    {
                        Title = "Argumento inválido",
                        Detail = aex.Message,
                        Status = status
                    };
                    break;

                case FormatException fex:
                    status = 400;
                    problem = new ProblemDetails
                    {
                        Title = "Formato inválido",
                        Detail = fex.Message,
                        Status = status
                    };
                    break;

                case KeyNotFoundException knf:
                    status = 404;
                    problem = new ProblemDetails
                    {
                        Title = "Recurso não encontrado",
                        Detail = knf.Message,
                        Status = status
                    };
                    break;

                case InvalidOperationException ioe:
                    status = 409;
                    problem = new ProblemDetails
                    {
                        Title = "Operação inválida",
                        Detail = ioe.Message,
                        Status = status
                    };
                    break;

                default:
                    status = 500;
                    problem = new ProblemDetails
                    {
                        Title = "Erro interno",
                        Detail = "Ocorreu um erro inesperado. Verifique os logs para mais detalhes.",
                        Status = status
                    };
                    break;
            }

            context.Result = new ObjectResult(problem) { StatusCode = status };
            context.ExceptionHandled = true;
        }
    }
}