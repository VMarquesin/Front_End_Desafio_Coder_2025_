namespace OperacaoPato.Backend.Shared.Exceptions
{
    public class ErrorOnValidationException : OperacaoPatoException
    {
        public IList<string> ErrorMessages { get; set; }

        public ErrorOnValidationException(IList<string> errors)
        {
            ErrorMessages = errors;
        }

        public ErrorOnValidationException(string error)
        {
            ErrorMessages = new List<string>
            {
                error
            };
        }

    }
}