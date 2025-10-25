namespace OperacaoPato.Backend.Shared.Exceptions
{
    public class ErrorOnValidationException : OperacaoPatoException
    {
        public IList<string> ErrorMessages { get; set; }

        public ErrorOnValidationException(IList<string> errors)
        {
            ErrorMessages = errors;
        }

        
    }
}