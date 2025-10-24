namespace OperacaoPato.Application.Interfaces
{
    public interface IPatoRepository
    {
        void AddPatoPrimordial(PatoPrimordial pato);
        void UpdatePatoPrimordial(PatoPrimordial pato);
        PatoPrimordial GetPatoPrimordialById(Guid id);
        IEnumerable<PatoPrimordial> GetAllPatosPrimordiais();
    }
}