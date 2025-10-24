using System.Collections.Generic;

namespace OperacaoPato.Domain.Exceptions
{
    public class PatoDomainException : DomainException
    {
        public PatoDomainException(string message) : base(message) { }

        public PatoDomainException(string message, IDictionary<string, string[]> errors) : base(message, errors) { }
    }
}