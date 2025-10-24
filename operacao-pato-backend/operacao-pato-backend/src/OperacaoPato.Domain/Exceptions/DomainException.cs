using System;
using System.Collections.Generic;

namespace OperacaoPato.Domain.Exceptions
{
    public class DomainException : Exception
    {
        public IDictionary<string, string[]> Errors { get; }

        public DomainException(string message) : base(message)
        {
            Errors = new Dictionary<string, string[]>();
        }

        public DomainException(string message, IDictionary<string, string[]> errors) : base(message)
        {
            Errors = errors ?? new Dictionary<string, string[]>();
        }
    }
}